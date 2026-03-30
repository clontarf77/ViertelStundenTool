using System.Data;

namespace ViertelStdTool.Converter
{
    public interface ICsvDataTableConverter
    {
        /// <summary>
        /// Convert CSV to DataTable.
        /// </summary>
        /// <param name="pathCSV"></param>
        /// <param name="seperator"></param>
        /// <param name="startAtRow">optional</param>
        /// <returns>dataTable</returns>
        DataTable ConvertCsvToDataTable(string pathCSV, char seperator, uint startAtRow = 0);

        /// <summary>
        /// Convert DataTable to CSV.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="pathCSV"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        void ConvertDataTableToCsv(DataTable dataTable, string pathCSV, char seperator);

        /// <summary>
        /// Compare 2 CSV files.
        /// </summary>
        /// <param name="pathCSV1"></param>
        /// <param name="pathCSV2"></param>
        /// <returns></returns>
        bool CompareCSVs(string pathCSV1, string pathCSV2);
    }
}