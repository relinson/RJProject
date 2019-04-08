using System;
using System.Windows.Media;

namespace Util.Controls
{
    /// <summary>
    /// 本地视频缩略图创建服务
    /// 使用工具ffmpeg提取帧图片
    /// </summary>
    internal class VedioThumbnailProvider : IThumbnailProvider
    {

        #region VedioThumbnailProvider-构造函数（初始化）

        /// <summary>
        ///  VedioThumbnailProvider-构造函数（初始化）
        /// </summary>
        public VedioThumbnailProvider()
        {
        }

        #endregion

        /// <summary>
        /// 创建缩略图。fileName:文件路径；width:图片宽度；height:高度
        /// </summary>
        public ImageSource GenereateThumbnail(object fileSource, double width, double height)
        {
            try
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}