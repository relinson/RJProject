using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
    public class DownloadHelper : IDisposable
    {
        /// <summary>
        /// 默认存储路径
        /// </summary>
        public static string DefaultDirectory
        {
            get; set;
        }
        /// <summary>
        /// 链接地址
        /// </summary>
        private string url;
        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;
        /// <summary>
        /// 总大小
        /// </summary>
        private long totalSize;
        /// <summary>
        /// 文件路径
        /// </summary>
        private string filePath;

        /// <summary>
        /// 当前进度
        /// </summary>
        private long currentSize;
        /// <summary>
        /// 是否完成
        /// </summary>
        private bool isFinished;
        /// <summary>
        /// 文件流对象，用于生成文件
        /// </summary>
        private FileStream fs;
        /// <summary>
        /// 
        /// </summary>
        private int bufferSize = 1024;
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
        /// <summary>
        /// 总大小
        /// </summary>
        public long TotalSize
        {
            get { return totalSize; }
        }
        /// <summary>
        /// 当前文件大小
        /// </summary>
        public long CurrentSize
        {
            get { return this.currentSize; }
        }
        /// <summary>
        /// 当前进度的百分数
        /// </summary>
        public float CurrentProgress
        {
            get
            {
                if (this.totalSize != 0)
                {
                    return (float)this.currentSize * 100 / (float)this.totalSize;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 目录地址
        /// </summary>
        public string Directory
        {
            get; set;
        }
        /// <summary>
        /// 生成文件路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
        }

        /// <summary>
        /// 文件
        /// </summary>
        public FileStream OperateFile
        {
            get { return fs; }
        }


        /// <summary>
        /// 一次请求大小
        /// 根据带宽及文件大小确定合理的值
        /// 如果带宽：1M,那下载速率约为100KB/秒，那step可设置为12500
        /// </summary>
        public long Step
        {
            get; set;
        }
        /// <summary>
        /// 缓冲池大小
        /// </summary>
        public int BufferSize
        {
            get { return this.bufferSize; }
            set
            {
                if (value > 0)
                {
                    this.bufferSize = value;
                }
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url"></param>
        public DownloadHelper(string url, string directory, string fname = "")
        {
            Init(url, directory, fname);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="directory"></param>
        private void Init(string url, string directory, string fname)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            this.url = url;
            this.Directory = directory;

            if (string.IsNullOrEmpty(fname))
            {
                if (url.Contains("?"))
                {
                    string flname = url.Substring(0, url.IndexOf("?"));
                    this.fileName = HttpUtility.UrlDecode(Path.GetFileName(flname));
                }
                else
                {
                    this.fileName = HttpUtility.UrlDecode(Path.GetFileName(url));
                }
            }
            else
            {
                this.fileName = fname;
            }
            string fullName = Path.Combine(this.Directory, fileName);
            this.filePath = fullName;

            if (File.Exists(filePath))
            {
                this.fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                this.currentSize = this.fs.Length;
            }
            else
            {
                this.fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }


        /// <summary>
        /// 下载
        /// </summary>
        public void Download()
        {
            //从0计数，需要减一
            long from = this.currentSize;
            if (from < 0)
            {
                from = 0;
            }

            long to = this.currentSize + this.Step - 1;
            if (to >= this.totalSize && this.totalSize > 0)
            {
                to = this.totalSize - 1;
            }
            this.Download(from, to);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url">下载路径</param>
        /// <param name="range">下载长度</param>
        public void Download(long from, long to)
        {
            if (this.totalSize == 0)
            {
                GetTotalSize();
            }
            if (from >= this.totalSize || this.currentSize >= this.totalSize)
            {
                this.isFinished = true;
                Dispose();
                return;
            }

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                request.AddRange("bytes", from, to);
                request.KeepAlive = false;
                response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    byte[] buffer = this.Buffer;
                    using (Stream stream = response.GetResponseStream())
                    {
                        int readTotalSize = 0;
                        int size = stream.Read(buffer, 0, buffer.Length);
                        while (size > 0)
                        {
                            //只将读出的字节写入文件
                            fs.Write(buffer, 0, size);
                            readTotalSize += size;
                            size = stream.Read(buffer, 0, buffer.Length);
                        }

                        //更新当前进度
                        this.currentSize += readTotalSize;

                        //如果返回的response头中Content-Range值为空，说明服务器不支持Range属性，不支持断点续传,返回的是所有数据
                        if (response.Headers["Content-Range"] == null)
                        {
                            this.isFinished = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(DownloadHelper), ex);
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
        /// <summary>
        /// 获取文件总大小
        /// </summary>
        public void GetTotalSize()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(this.url);
                request.Timeout = 10000;
                request.KeepAlive = false;
                request.Proxy = null;
                response = (HttpWebResponse)request.GetResponse();
                this.totalSize = response.ContentLength;
            }
            catch (Exception ex)
            {
                this.Dispose();
                Logging.Log4NetHelper.Error(typeof(DownloadHelper), "GetTotalSize Error", ex);
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
        /// <summary>
        /// 缓存
        /// </summary>
        /// <returns></returns>
        private byte[] Buffer
        {
            get
            {
                if (this.bufferSize <= 0)
                {
                    this.bufferSize = 1024;
                }
                return new byte[this.bufferSize];
            }
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.fs != null)
            {
                this.fs.Close();
            }
        }
    }
}