using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PRO_ReceiptsInvMgr.Core.Helper
{

    /// <summary>  
    /// 此类专门是针对table-list相关的数据进行服务一个类  
    /// </summary>  
    public static class DataTableHelper
    {
        /// <summary>
        /// IEnumerable<T> to Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName = null)
        {
            string tbName = tableName;
            if (string.IsNullOrWhiteSpace(tbName))
            {
                tbName = typeof(T).Name;
            }
            DataTable table = new DataTable(tbName);

            //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("Value");
                table.Columns.Add(dc);
                foreach (T item in data)
                {
                    DataRow dr = table.NewRow();
                    dr[0] = item;
                    table.Rows.Add(dr);
                }
            }
            else
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name,
                    Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        try
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            row[prop.Name] = DBNull.Value;
                        }
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
        /// <summary>
        /// T to Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataRow<T>(this T data, string tableName = null)
        {
            string tbName = tableName;
            if (string.IsNullOrWhiteSpace(tbName))
            {
                tbName = typeof(T).Name;
            }
            DataTable table = new DataTable(tbName);

            //special handling for value types and string
            if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
            {

                DataColumn dc = new DataColumn("Value");
                table.Columns.Add(dc);

                DataRow dr = table.NewRow();
                dr[0] = data;
                table.Rows.Add(dr);
            }
            else
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name,
                    Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(data) ?? DBNull.Value;
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
                        row[prop.Name] = DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable ConvertTo<T>(this IList<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static IList<T> ConvertTo<T>(this IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null)
            {
                return new List<T>();
            }

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        //Convert DataRow into T Object
        public static T CreateItem<T>(DataRow row)
        {
            string columnName;
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    //Get property with same columnName
                    PropertyInfo prop = obj.GetType().GetProperty(columnName);
                    try
                    {
                        if (prop == null)
                        {
                            continue;
                        }

                        var rowValue = row[columnName];

                        if (!prop.PropertyType.IsGenericType)
                        {
                            if (prop.PropertyType == typeof(DateTime))
                            {
                                try
                                {
                                    rowValue = Convert.ChangeType(rowValue, prop.PropertyType);
                                }
                                catch
                                {
                                    rowValue = ValueParse.GetDateTimeFromOADateNullAble(rowValue.ToString());
                                }
                            }
                            else
                            {
                                rowValue = Convert.ChangeType(rowValue, prop.PropertyType);
                            }
                        }

                        //Get value for the column
                        object value = (row[columnName] is DBNull)
                        ? null : rowValue;
                        //Set property value
                        if (prop.CanWrite)    //判断其是否可写
                        {
                            prop.SetValue(obj, value, null);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logging.Log4NetHelper.Error(typeof(DataTableHelper), ex);
                    }
                }
            }
            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
    }
}