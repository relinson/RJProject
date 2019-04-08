using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace PRO_ReceiptsInvMgr.Core.Utilites
{
    /// <summary>
    /// DataImporter
    /// </summary>
    /// <remarks></remarks>
    public class DataImporter
    {
        protected DataImporter() { }

        /// <summary>
        /// Imports the excel to data set.
        /// </summary>
        /// <param name="excelFullFileName">Name of the excel full file.</param>
        /// <param name="targetTable">The target table.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataSet ImportExcelToDataSet(string excelFullFileName, string targetTable)
        {
            OleDbConnection oledbConnection = GetOledbConnection(excelFullFileName);

            string commandText = "select * from [Sheet1$]";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandText, oledbConnection);

            DataSet ds = new DataSet();

            try
            {
                oledbConnection.Open();
                dataAdapter.TableMappings.Add("Table", targetTable);
                dataAdapter.Fill(ds);
            }
            finally
            {
                if (oledbConnection.State == ConnectionState.Open)
                {
                    oledbConnection.Close();
                }
            }

            return ds;
        }
         

        /// <summary>
        /// Gets the oledb connection.
        /// </summary>
        /// <param name="excelFullFileName">Name of the excel full file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static OleDbConnection GetOledbConnection(string excelFullFileName)
        {
            string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + excelFullFileName + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";

            return new OleDbConnection(excelConnectionString);
        }
    }
}
