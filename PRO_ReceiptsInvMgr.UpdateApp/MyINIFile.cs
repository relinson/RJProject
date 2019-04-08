using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PRO_ReceiptsInvMgr.UpdateApp
{
    /// <summary>
    /// MyINIFile ��ժҪ˵����
    /// </summary>
    public class MyIniFile
    {
        public string Path
        {
            get;set;
        }

        public MyIniFile(string INIPath)
        {
            Path = INIPath;
        }

        #region ����

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        #endregion


        #region  дINI

        /// <summary>
        /// дINI�ļ�
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);
        }
        #endregion

        #region ɾ��ini����

        /// <summary>
        /// ɾ��ini�ļ������ж���
        /// </summary>
        public void ClearAllSection()
        {
            IniWriteValue(null, null, null);
        }
        /// <summary>
        /// ɾ��ini�ļ���personal�����µ����м�
        /// </summary>
        /// <param name="Section"></param>
        public void ClearSection(string Section)
        {
            IniWriteValue(Section, null, null);
        }
        #endregion

        #region ��ȡINI
        /// <summary>
        /// ��ȡINI�ļ�
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", temp, 255, this.Path);
            return temp.ToString();
        }
        public byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[255];
            GetPrivateProfileString(section, key, "", temp, 255, this.Path);
            return temp;
        }
        /// <summary>
        /// ��ȡini�ļ������ж�����
        /// </summary>    
        public string[] IniReadValues()
        {
            byte[] allSection = IniReadValues(null, null);
            return ByteToString(allSection);

        }
        /// <summary>
        /// ת��byte[]����Ϊstring[]�������� 
        /// </summary>
        /// <param name="sectionByte"></param>
        /// <returns></returns>
        private string[] ByteToString(byte[] sectionByte)
        {                  
            ASCIIEncoding ascii = new ASCIIEncoding();           
            //��������key��string����
            string sections = ascii.GetString(sectionByte);
            //��ȡkey������
            string[] sectionList = sections.Split(new char[1] { '\0' });
            return sectionList;
        }

        /// <summary>
        /// ��ȡini�ļ���ĳ���������м���
        /// </summary>    
        public string[] IniReadValues(string Section)
        {
            byte[] sectionByte = IniReadValues(Section, null);
            return ByteToString(sectionByte);
        }

        #endregion

    }
}
