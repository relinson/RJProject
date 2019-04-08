using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace PRO_ReceiptsInvMgr.Core.Helper
{
   public  class ValidCode
    {
        #region Private Fields

        private  readonly int _len;

        private readonly Single _jianju = (float)18.0;

        private readonly Single _height = (float)24.0;

        private string _checkCode;

        #endregion

        #region Public Property

        public string CheckCode
        {

            get
            {

                return _checkCode;

            }

        }

        #endregion

        #region Constructors

        /// <summary> 

        /// public constructors 

        /// </summary> 

        /// <param name="len"> 验证码长度 </param> 

        /// <param name="ctype"> 验证码类型：字母、数字、字母+ 数字 </param> 

        public ValidCode(int len, CodeType ctype)
        {

            this._len = len;
        }

        #endregion

        #region Public Field

        public enum CodeType { Words, Numbers, Characters, Alphas }

        #endregion

        #region Private Methods



        private string GenerateNumbers()
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < _len; i++)
            {
                int t = rand.Next(10); 
                sb.Append(allCharArray[t]);
            }
            return sb.ToString();
        }
         
        #endregion

        #region Public Methods

        public Stream CreateCheckCodeImage()
        {
            string checkCode  = GenerateNumbers();
            this._checkCode = checkCode;
            MemoryStream ms = null;
            if (checkCode == null || checkCode.Trim() == String.Empty)
            {
                return null;
            }
            Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * _jianju)), (int)_height);

            Graphics g = Graphics.FromImage(image);

            try
            {
                Random random = new Random();

                g.Clear(Color.White);
                // 画图片的背景噪音线 
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
                }
                //定义颜色
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Coral, Color.Brown, Color.DarkCyan, Color.Purple };

                Font font = new System.Drawing.Font("宋体", 14, System.Drawing.FontStyle.Bold);
                //输出不同字体和颜色的验证码字符
                for (int i = 0; i < checkCode.Length; i++)
                {
                    int cindex = random.Next(7);
                    Brush b = new System.Drawing.SolidBrush(c[cindex]);
                    int ii = 4;
                    if ((i + 1) % 2 == 0)
                    {
                        ii = 2;
                    }
                    g.DrawString(checkCode.Substring(i, 1), font, b, 3 + (i * 12), ii);
                }
              
                // 画图片的边框线 

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);



                ms = new System.IO.MemoryStream();

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            }

            finally
            {

                g.Dispose();

                image.Dispose();

            }

            return ms;

        }

        #endregion

    }
}

