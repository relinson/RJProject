using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PRO_ReceiptsInvMgr.Core.Helper
{

    /// <summary>  
    /// 此类专门是针对枚举相关的数据进行服务一个类  
    /// </summary>  
    public static class EnumHelper
    {  
        /// <summary>  
        /// 获取一个枚举集合类。  
        /// </summary>  
        public static List<EnumberEntity> EnumToList<T>()
        {
            List<EnumberEntity> list = new List<EnumberEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                EnumberEntity m = new EnumberEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Desction = da.Description;
                }
                m.EnumValue = Convert.ToInt32(e);
                m.EnumName = e.ToString();
                list.Add(m);
            }
            return list;
        }
        /// <summary>
        /// 获取枚举的描述名称
        /// </summary>
        /// <param name="enumName">枚举名</param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumName)
        {
            string description = string.Empty;
            object[] objArr = enumName.GetType().GetField(enumName.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (objArr != null && objArr.Length > 0)
            {
                DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                description = da.Description;
            }
            return description;
        }

      
        /// <summary>
		/// 根据Description获取枚举
		/// 说明：
		/// 单元测试-->通过
		/// </summary>
		/// <typeparam name="T">枚举类型</typeparam>
		/// <param name="description">枚举描述</param>
		/// <returns>枚举</returns>
		public static T GetEnumName<T>(this string description)
		{
			Type _type = typeof(T);
			foreach (FieldInfo field in _type.GetFields())
			{
                DescriptionAttribute[] _curDesc=null;
                if (field != null)
                {
                    _curDesc = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (_curDesc != null && _curDesc.Length > 0)
                    {
                        if (_curDesc[0].Description == description)
                        {
                            return (T)field.GetValue(null);
                        }
                    }
                    else
                    {
                        if (field.Name == description)
                        {
                            return (T)field.GetValue(null);
                        }
                    }
                }
				
			}
			throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "description");
		}

        /// <summary>
        /// 根据Name获取枚举
        /// 说明：
        /// 单元测试-->通过
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举</param>
        /// <returns>枚举</returns>
        public static T GetEnumByName<T>(this string value)
        {
            Type _type = typeof(T);
            try
            {
                return (T)System.Enum.Parse(_type, value);
            }
            catch
            {
                return default(T);
            }
        }
    }
    public class EnumberEntity
    {
        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Desction { set; get; }

        /// <summary>  
        /// 枚举名称  
        /// </summary>  
        public string EnumName { set; get; }

        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public int EnumValue { set; get; }
    }
}