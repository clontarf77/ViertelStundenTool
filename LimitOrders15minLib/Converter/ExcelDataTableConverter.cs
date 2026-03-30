using ExcelDataReader;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace ViertelStdTool.Converter
{
    public class ExcelDataTableConverter : IExcelDataTableConverter
    {
        #region Convert one Excel-Sheet (Wiht given table name) to DataTable.
        /// <summary>
        /// Convert one Excel-Sheet (Wiht given table name) to DataTable.
        /// </summary>
        /// <param name="pathExcel"></param>
        /// <param name="tableName"></param>
        /// <returns>dataTable</returns>
        public DataTable ConvertExcelToDataTable(string pathExcel, string tableName)
        {

            DataTable resultDataTable = new DataTable();

            // Just do this if file is avialble.
            if (File.Exists(pathExcel))
            {
                FileStream stream = File.Open(pathExcel, FileMode.Open, FileAccess.Read);
                // Read from *.xlsx      
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet resultDataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,                        
                    }

                });

                if (resultDataSet.Tables.Contains(tableName))
                {
                    resultDataTable = resultDataSet.Tables[tableName];
                }

                // Close stream.
                stream.Close();
            }
            return resultDataTable;
        }
        #endregion      

        #region Convert one Excel-Sheet (Wiht given table name) to DataTable for OLEDB 12 - obsolete for office 365.
        /// <summary>
        /// Convert one Excel-Sheet (Wiht given table name) to DataTable for OLEDB 12  - obsolete for office 365.
        /// </summary>
        /// <param name="pathExcel"></param>
        /// <param name="tableName"></param>
        /// <returns>dataTable</returns>
        public DataTable ConvertExcelToDataTableObsolete(string pathExcel, string tableName)
        {
            DataTable dataTable = new DataTable();
            DataTable sheets = new DataTable();

            OleDbConnection con = new OleDbConnection
            {
                ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + pathExcel + ";"
            };
            con.ConnectionString += @"Extended Properties=""Excel 12.0 Xml;HDR=Yes""";

            con.Open();

            sheets = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            foreach (DataRow sheet in sheets.Rows)
            {
                string tableNameSheet = sheet["Table_Name"].ToString();

                if (tableNameSheet.Equals(tableName + "$"))
                {
                    string sql = "SELECT * FROM [" + tableNameSheet + "]";
                    OleDbDataAdapter adap = new OleDbDataAdapter(sql, con);
                    adap.Fill(dataTable);
                }
            }
            return dataTable;
        }
        #endregion      
    }
}
