using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using ViertelStdTool.Converter;

namespace UnitTestConverter
{
    [TestClass]
    public class ConverterUnitTest
    {
        #region Convert CSV-File to DataTable and back - compare template CSV and result CSV
        /// <summary>
        /// Convert CSV-File to DataTable and back - compare template CSV and result CSV
        /// </summary>
        [TestMethod]
        public void CsvDataTableConverter()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConverterUnitTest)).CodeBase);
            string pathCsvTemplate = pathExecutable.Remove(0, 6) + @"\Converter\TestData\FromVPPToComTrader_2019.08.20_12.47_089_v2.csv";
            char seperator = ';';       
            DataTable table;
            string localPathResult = pathExecutable.Remove(0, 6) + @"\Converter\TestData\converterTest.csv";
            bool filesAreEqual = false;         

            try
            {
                ICsvDataTableConverter converter = new CsvDataTableConverter();
                table = converter.ConvertCsvToDataTable(pathCsvTemplate, seperator);
                converter.ConvertDataTableToCsv(table, localPathResult, seperator);

                filesAreEqual = converter.CompareCSVs(pathCsvTemplate, localPathResult);

            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(filesAreEqual, "Files are not equal after conversion.");
        }
        #endregion      

        #region Convert Excel-File to DataTable
        /// <summary>
        /// Convert Excel-File to DataTable
        /// </summary>
        [TestMethod]
        public void ExcelDataTableConverter()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConverterUnitTest)).CodeBase);
            string pathExeTemplate = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx";
            string tableName = "Limit_VSK_15Min";
            DataTable table = new DataTable();                      

            try
            {
                IExcelDataTableConverter converter = new ExcelDataTableConverter();
                table = converter.ConvertExcelToDataTable(pathExeTemplate, tableName);               

            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(table.Columns.Count > 0, "No columns available in the generated table.");
            Assert.IsTrue(table.Rows.Count > 0, "No rows available in the generated table.");
        }
        #endregion

        #region Convert Excel-File to DataTable use wrong table name
        /// <summary>
        /// Convert Excel-File to DataTable use wrong table name
        /// </summary>
        [TestMethod]
        public void ExcelDataTableConverterWrongTableName()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConverterUnitTest)).CodeBase);
            string pathExceTemplate = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx";
            string tableName = "ThisTableDoesNotExist";
            DataTable table = new DataTable();

            try
            {
                IExcelDataTableConverter converter = new ExcelDataTableConverter();
                table = converter.ConvertExcelToDataTable(pathExceTemplate, tableName);

            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(table.Columns.Count == 0, "Columns are available in the generated table.");
            Assert.IsTrue(table.Rows.Count == 0, "Rows are available in the generated table.");
        }
        #endregion

        #region Convert Excel-File to DataTable use wrong excel path
        /// <summary>
        /// Convert Excel-File to DataTable use wrong excel path
        /// </summary>
        [TestMethod]
        public void ExcelDataTableConverterWrongExcelPath()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ConverterUnitTest)).CodeBase);
            string pathExceTemplate = pathExecutable.Remove(0, 6) + @"\Converter\TestData\ThisFileDoesNotExists.xlsx";
            string tableName = "ThisTableDoesNotExist";
            DataTable table = new DataTable();

            try
            {
                IExcelDataTableConverter converter = new ExcelDataTableConverter();
                table = converter.ConvertExcelToDataTable(pathExceTemplate, tableName);

            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(table.Columns.Count == 0, "Columns are available in the generated table.");
            Assert.IsTrue(table.Rows.Count == 0, "Rows are available in the generated table.");
        }
        #endregion            

    }
}

