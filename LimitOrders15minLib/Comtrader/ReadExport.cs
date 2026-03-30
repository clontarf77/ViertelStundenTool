using System;
using System.Data;
using System.IO;
using System.Reflection;
using ViertelStdTool.Converter;
using ViertelStdTool.Log;

namespace ViertelStdToolLib.Comtrader
{
    public class ReadExport : IReadExport
    {
        // Logger
        private readonly INLogger logger = new NLogger();

        // Path Comtrader Deal Export      
        private const char seperator = ';';
        private readonly ICsvDataTableConverter converter = new CsvDataTableConverter();

        #region Export deals from Comtrader Export as DataTable.
        /// <summary>
        /// Export deals from Comtrader Export as DataTable.
        /// </summary>
        /// <param name="pathComtraderExport"></param>
        /// <param name="filter"></param>
        /// <param name="resultTable"></param>       
        /// <param name="startAtRow">optional</param>
        /// <returns></returns>
        public string ExportDealsComtrader(string pathComtraderExport, InputFilter filter, ref ResultTable resultTable, uint startAtRow = 0)
        {
            string pathLocalExport = string.Empty;
            string errorInformation = string.Empty;
         
            errorInformation = CopyComtraderFileToLocalFolder(pathComtraderExport, ref pathLocalExport);

            if (errorInformation.Equals("OK"))
            {
                errorInformation = ConvertReadFileToDataTable(pathLocalExport, ref resultTable.table, startAtRow);

                if (errorInformation.Equals("OK"))
                {
                    resultTable.table = FilterResultTable(filter, resultTable.table);
                }
            }            

            resultTable.numberOfRows = resultTable.table.Rows.Count;

            return errorInformation;
        }
        #endregion

        #region Copy comtrader export file to local folder to be able to work on it.
        /// <summary>
        ///  Copy comtrader export file to local folder to be able to work on it.
        /// </summary>
        /// <param name="pathComtraderExport"></param>
        /// <param name="pathLocalExport"></param>
        /// <returns>Return OK if file could be copied.</returns>
        private string CopyComtraderFileToLocalFolder(string pathComtraderExport, ref string pathLocalExport)
        {
            // Set default.
            string fileCopied = "NOK";

            Directory.CreateDirectory("Comtrader");

            // Set local path.
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ReadExport)).CodeBase);

            #region Workaround if started with long path. If not substituted path will exceed 260 chars and exception will be thrown.
            if (pathExecutable.StartsWith(@"file:\konzern\dfs\firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"file:\konzern\dfs\firmen", "T:");
                logger.WriteInfo("Replaced path 'file:\\konzern\\dfs\\firmen' with 'T:'");
            }
            else if (pathExecutable.StartsWith("file:\\"))
            {
                pathExecutable = pathExecutable.Remove(0, 6);
            }
            else if (pathExecutable.StartsWith(@"\\konzern\dfs\Firmen"))
            {
                pathExecutable = pathExecutable.Replace(@"\\konzern\dfs\Firmen", "T:");
                logger.WriteInfo("Replaced path '\\\\konzern\\dfs\\Firmen' with 'T:'");
            }
            #endregion
            pathLocalExport = pathExecutable + @"\Comtrader\eex_deals_export.csv";

            try
            {
                // Copy file to local directory.
                if (File.Exists(pathLocalExport))
                {
                    File.Delete(pathLocalExport);
                }
                File.Copy(pathComtraderExport, pathLocalExport);
                fileCopied = "OK";
            }
            catch(Exception e)
            {
                fileCopied = "NOK - " + e.ToString();
            }            

            return fileCopied;
        }
        #endregion

        #region Convert read file to dataTable.
        /// <summary>
        /// Convert read file to dataTable.
        /// </summary>        
        /// <param name="pathLocalExport"></param>
        /// <param name="startAtRow">optional</param>
        /// <returns>Return OK if dataTable could be generated.</returns>
        private string ConvertReadFileToDataTable(string pathLocalExport, ref DataTable tradesComTrader, uint startAtRow = 0)
        {
            // Set default.
            string tableGenerated = "NOK";

            try
            {
                // First two rows have to be ignored of comtrader export.
                tradesComTrader = converter.ConvertCsvToDataTable(pathLocalExport, seperator, startAtRow);
                tableGenerated = "OK";
            }
            catch (Exception e)
            {
                tableGenerated = "NOK - " + e.ToString();
            }

            return tableGenerated;
        }
        #endregion

        #region Filter DataTable.
        /// <summary>
        /// Filter DataTable.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="resultTable"></param>        
        /// <returns></returns>
        private DataTable FilterResultTable(InputFilter filter, DataTable resultTable)
        {
            string query = string.Empty;
            string columnName = string.Empty;
            bool partOfFilterAlreadySet = false;

            DateTime date;
            DateTime dateTommorow;
            string startDateTime = string.Empty;
            string startDateTimeTommorow = string.Empty;

            // Clean up colums names to be used from DataTable select.
            int numberOfColumns = resultTable.Columns.Count;

            for (int i = 0; i < numberOfColumns; i++)
            {
                columnName = resultTable.Columns[i].ColumnName;
                columnName = columnName.Replace(" ", "");
                columnName = columnName.Replace("/", "_");
                resultTable.Columns[i].ColumnName = columnName;
            }

            DataTable filteredTable = resultTable.Clone();

            if (filter.text != TextMemo2.ALL)
            {
                query += "Text = '" + filter.text + "'";
                partOfFilterAlreadySet = true;
            }            
            if (filter.traderId != TraderId.ALL)
            {
                if(partOfFilterAlreadySet)
                {
                    query += " AND ";
                } 
                query += "TraderId = '" + filter.traderId + "'";
            }
            if (filter.date.Length > 0)
            {
                if (partOfFilterAlreadySet)
                {
                    query += " AND ";
                }

                date = Convert.ToDateTime(filter.date);
                dateTommorow = date.AddDays(1);
                startDateTime = date.ToString();
                startDateTimeTommorow = dateTommorow.AddSeconds(-1).ToString();

                query += "Date_Time > '" + startDateTime + "' and Date_Time < '" + startDateTimeTommorow + "'";
            }
                        
            DataRow[] resultRows = resultTable.Select(query);

            if (resultRows.Length > 0)
            {
                // This goes wrong if table has no rows.
                filteredTable = resultRows.CopyToDataTable();
            }

            return filteredTable;
        }
        #endregion
    }
}
