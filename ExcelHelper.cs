using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Zdd.Utility
{
    /// <summary>
    /// Description of ExcelHelper.
    /// </summary>
    public static class ExcelHelper
    {
        private static string connectionString = string.Empty;

        /// <summary>
        /// 获取或设置ConnectionString
        /// </summary>
        /// <value></value>
        public static string ConnectionString
        {
            get { return connectionString; }
        }

        /// <summary>
        /// Executes the specified select command text.
        /// </summary>
        /// <param name="selectCommandText">The select command text.</param>
        /// <param name="fileName">Name of the file.</param>
        public static DataTable Execute(string selectCommandText, string fileName)
        {
            ArgumentValidator.NotNullValidator(fileName, "fileName");
            ArgumentValidator.NotNullValidator(selectCommandText, "selectCommandText");

            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", fileName);

            DataTable dt = new DataTable();
            OleDbConnection conn = new OleDbConnection(connectionString);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommandText, conn);
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }
    }
}
