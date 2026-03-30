using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using ViertelStdToolLib.Comtrader;

namespace UnitTestComtrader
{
    [TestClass]
    public class ComtraderUnitTest
    {
        #region Get Deal export from Comtrader - PROD.
        /// <summary>
        /// Get Deal export from Comtrader  - PROD.
        /// </summary>
        [TestMethod]        
        public void ExportDealsComtraderPROD()
        {
            const string pathComtraderExportPROD = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            const uint startRow = 2;            
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = string.Empty,
                traderId = TraderId.ALL,
                text = TextMemo2.ALL           
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportPROD, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(resultTable.table.Columns.Count > 0, "Table does not have columns.");
            Assert.IsTrue(errorInformation.Equals("OK"), "Method returns an error.");
        }
        #endregion      

        #region Get Deal export from Comtrader -TEST.
        /// <summary>
        /// Get Deal export from Comtrader  - TEST.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderTEST()
        {
            const string pathComtraderExportTEST = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = string.Empty,
                traderId = TraderId.ALL,
                text = TextMemo2.ALL
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(resultTable.table.Columns.Count > 0, "Table does not have columns.");
            Assert.IsTrue(errorInformation.Equals("OK"), "Method returns an error.");
        }
        #endregion       

        #region Get Deal export from Comtrader -TEST.
        /// <summary>
        /// Get Deal export from Comtrader - just VSK trades - TEST.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderVskTEST()
        {
            const string pathComtraderExportTEST = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = string.Empty,
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(resultTable.table.Columns.Count > 0, "Table does not have columns.");
            Assert.IsTrue(errorInformation.Equals("OK"), "Method returns an error.");
        }
        #endregion       

        #region Get Deal export from Comtrader using wrong path of comtrader export - PROD.
        /// <summary>
        /// Get Deal export from Comtrader using wrong path of comtrader export - PROD.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderWrongPathPROD()
        {
            const string pathComtraderExportPROD = "T:\\24-7-Trading\\24-7-Trading\\VTO\\001_Strom\\001a_Handel\\001b_Export\\WrongPath.csv";
            const uint startRow = 2;           
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = string.Empty,
                traderId = TraderId.ALL,
                text = TextMemo2.ALL
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportPROD, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsFalse(errorInformation.Equals("OK"), "Method does not return an error.");
        }
        #endregion

        #region Get Deal export from Comtrader using wrong start row - PROD.
        /// <summary>
        /// Get Deal export from Comtrader using wrong start row - PROD.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderWrongStartRowPROD()
        {
            const string pathComtraderExportPROD = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            // Use wrong start row for eex_trade_export.csv.           
            const uint startRow = 0;           
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = string.Empty,
                traderId = TraderId.ALL,
                text = TextMemo2.ALL
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportPROD, filter, ref resultTable, startRow); 
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsFalse(errorInformation.Equals("OK"), "Method does not return an error.");
        }
        #endregion     

        #region Get Deal export from Comtrader - show deals from today - PROD.
        /// <summary>
        /// Get Deal export from Comtrader - show deals from today  - PROD.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderFromTodayPROD()
        {
            const string pathComtraderExportPROD = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            // Use wrong start row for eex_trade_export.csv.           
            const uint startRow = 2;           
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = DateTime.Now.ToShortDateString(),
                traderId = TraderId.ALL,
                text = TextMemo2.ALL
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportPROD, filter, ref resultTable, startRow); 
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(errorInformation.Equals("OK"), "Method does return an error.");
        }
        #endregion

        #region Get Deal export from Comtrader - show deals from today - TEST.
        /// <summary>
        /// Get Deal export from Comtrader - show deals from today - TEST.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderFromTodayTEST()
        {
            const string pathComtraderExportTEST = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = DateTime.Now.ToShortDateString(),
                traderId = TraderId.ALL,
                text = TextMemo2.ALL
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(resultTable.table.Columns.Count > 0, "Table does not have columns.");
            Assert.IsTrue(errorInformation.Equals("OK"), "Method returns an error.");
        }
        #endregion             

        #region Get Deal export from Comtrader - show all VSK trades from today - PROD.
        /// <summary>
        /// Get Deal export from Comtrader - show deals from today  - PROD.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderFromVSKTodayPROD()
        {
            const string pathComtraderExportPROD = "T:\\24-7-Trading\\Intraday-Trading\\001_Strom_Handel_Export\\eex_deals_export.csv";
            // Use wrong start row for eex_trade_export.csv.           
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = DateTime.Now.ToShortDateString(),
                traderId = TraderId.TRD010, // Trader for Likron PROD
                text = TextMemo2.VSK
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportPROD, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(errorInformation.Equals("OK"), "Method does return an error.");
        }
        #endregion

        #region Get Deal export from Comtrader - show all VSK trades from today - TEST.
        /// <summary>
        /// Get Deal export from Comtrader - show deals from today  - TEST.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderFromVSKTodayTEST()
        {
            const string pathComtraderExportTEST = "T:\\24-7-Trading\\24-7-Trading\\VTO\\001_Strom\\001a_Handel\\001b_Export\\test\\eex_deals_export.csv";
            // Use wrong start row for eex_trade_export.csv.           
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = DateTime.Now.ToShortDateString(),
                traderId = TraderId.TRD001, // Trader Likron Test
                text = TextMemo2.VSK
            };

            try
            {
                IReadExport comtrader = new ReadExport();
                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(errorInformation.Equals("OK"), "Method does return an error.");
        }
        #endregion     
    }
}


