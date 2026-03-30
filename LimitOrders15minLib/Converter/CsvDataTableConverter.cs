using System.Data;
using System.IO;

namespace ViertelStdTool.Converter
{
    public class CsvDataTableConverter : ICsvDataTableConverter
    {
        #region Convert CSV to DataTable
        /// <summary>
        /// Convert CSV to DataTable.
        /// </summary>
        /// <param name="pathCSV"></param>
        /// <param name="seperator"></param>
        /// <returns>dataTable</returns>
        public DataTable ConvertCsvToDataTable(string pathCSV, char seperator, uint startAtRow = 0)
        {
            DataTable dataTable = new DataTable();
            FileStream csvDataStream = new FileStream(pathCSV, FileMode.Open);

            using (StreamReader streamReader = new StreamReader(csvDataStream, System.Text.Encoding.Default))
            {
                string strLine = string.Empty;

                for (int i = 0; i <= startAtRow; i++)
                {
                    strLine = streamReader.ReadLine();
                }
                
                string[] strArray = strLine.Split(seperator);

                foreach (string value in strArray)
                {
                    // Remove '"' and spaces from string.   
                    dataTable.Columns.Add(value.Replace("\"", string.Empty).Trim());
                }

                DataRow dr = dataTable.NewRow();

                while (!streamReader.EndOfStream)
                {
                    // Remove '"' and spaces from string.   
                    strLine = streamReader.ReadLine().Replace("\"", string.Empty).Trim();
                    strArray = strLine.Split(seperator);
                    dataTable.Rows.Add(strArray);
                }
            }
            return dataTable;
        }
        #endregion

        #region Convert DataTable to CSV
        /// <summary>
        /// Convert DataTable to CSV.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="pathCSV"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public void ConvertDataTableToCsv(DataTable dataTable, string pathCSV, char seperator)
        {
            using (StreamWriter streamWriter = new StreamWriter(pathCSV, false, System.Text.Encoding.Default))
            {
                int numberOfColumns = dataTable.Columns.Count;

                for (int i = 0; i < numberOfColumns; i++)
                {
                    streamWriter.Write(dataTable.Columns[i]);
                    if (i < numberOfColumns - 1)
                    {
                        streamWriter.Write(seperator);
                    }
                }
                streamWriter.Write(streamWriter.NewLine);

                foreach (DataRow dr in dataTable.Rows)
                {
                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        streamWriter.Write(dr[i].ToString());

                        if (i < numberOfColumns - 1)
                        {
                            streamWriter.Write(seperator);
                        }
                    }
                    streamWriter.Write(streamWriter.NewLine);
                }
            }
        }
        #endregion

        #region Compare 2 CSV files
        /// <summary>
        /// Compare 2 CSV files.
        /// </summary>
        /// <param name="pathCSV1"></param>
        /// <param name="pathCSV2"></param>
        /// <returns></returns>
        public bool CompareCSVs(string pathCSV1, string pathCSV2)
        {
            bool filesAreEqual = false;
            string string1 = string.Empty;
            string string2 = string.Empty;

            //Compare CSVs
            // Read both files from file system and write each data to a string.
            // If generated strings are equal test is fine.
            FileStream csvDataStreamTemplate = new FileStream(pathCSV1, FileMode.Open);
            FileStream csvDataStreamResult = new FileStream(pathCSV2, FileMode.Open);

            using (StreamReader streamReaderTemplate = new StreamReader(csvDataStreamTemplate, System.Text.Encoding.Default))
            {
                while (!streamReaderTemplate.EndOfStream)
                {
                    // Write template data to string.
                    string1 += streamReaderTemplate.ReadLine();
                }
            }

            using (StreamReader streamReaderResult = new StreamReader(csvDataStreamResult, System.Text.Encoding.Default))
            {
                while (!streamReaderResult.EndOfStream)
                {
                    // Write result data to string.
                    string2 += streamReaderResult.ReadLine();
                }
            }

            // Compare the generated strings.
            if (string1.Equals(string2))
            {
                // Template CSV and CSV which was converted to DataTable and back to CSV are equal.
                filesAreEqual = true;
            }

            return filesAreEqual;
        }
        #endregion
    }
}
