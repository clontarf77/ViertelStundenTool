using System.Data;

namespace ViertelStdTool.Converter
{
    public interface IExcelDataTableConverter
    {
        /// <summary>
        /// Convert one Excel-Sheet (Wiht given table name) to DataTable.
        /// </summary>
        /// <param name="pathExcel"></param>
        /// <param name="tableName"></param>
        /// <returns>dataTable</returns>
        DataTable ConvertExcelToDataTable(string pathExcel, string tableName);       
    }
}