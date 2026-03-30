using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Aligne.Importer.Generate.Xml;
using ViertelStdToolLib.BookTradesAligne;
using ViertelStdToolLib.Comtrader;

namespace UnitTestBookTradesAligneComtraderExport
{
    [TestClass]
    public class BookTradesAligneComtraderExportUnitTest
    {
        #region Get Deal export from Comtrader export file.
        /// <summary>
        /// Get Deal export from Comtrader export file.
        /// </summary>
        [TestMethod]
        public void ExportDealsComtraderExportFile()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export.csv";
                                  
            const uint startRow = 2;
            string errorInformation = string.Empty;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
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

        #region Generate XML for Aligne. -- One Trade
        /// <summary>
        /// Generate XML for Aligne. This test is for one trade. Parameter of test trade is given as static struct.
        /// No read from Comtrader-Export here.
        /// </summary>
        [TestMethod]
        public void GenerateAligneXmlOneTrade()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);            
            string pathTestData = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData";
                  
            // Generate List of structs.
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            ParameterXmlGeneration parameter = new ParameterXmlGeneration()
            {
                // Just in this test. Dynamic in real code.
                path = pathTestData,
                fileNameGeneratedXml = "GeneratedXml_17695912",
                buySell = BuySell.BUY, // Export from Comtrader Export is a sell so this is a buy.
                controlArea = ControlArea.AMP, // TSO is AMP in Export
                trader = "RFSR",
                memo1 = "VSK", // Text fromExport File
                memo2 = "17695912", // TradeNo/TradeID from Comtrader Export File
                tradeDate = "22.06.2020", // Date from Comtrader Export file
                contract = "10Q2", // Contract from Comtrader Export '20200622 10:15-20200622 10:30'
                quantity = "1", // Qty from Comtrader Export
                price = "11.2" // Prc from comtrader Export
            };

            parameterList.Add(parameter);

            try
            {                
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();                
                xmlGenerator.GenerateXmlForVsk(parameterList);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            
            Assert.IsTrue(File.Exists(parameterList[0].path + @"\" + parameterList[0].fileNameGeneratedXml + ".xml"), "File does not exist.");
        }
        #endregion

        #region Generate XML for Aligne. -- Several Trades
        /// <summary>
        /// Generate XML for Aligne. This test is for several trades. Parameter of test trades are given as static struct.
        /// No read from Comtrader-Export here.
        /// </summary>
        [TestMethod]
        public void GenerateAligneXmlSeveralTrades()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathTestData = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData";
                       
            // Generate List of structs.
            List<ParameterXmlGeneration> parameterList = new List<ParameterXmlGeneration>();

            ParameterXmlGeneration parameter1 = new ParameterXmlGeneration()
            {
                // Just in this test. Dynamic in real code.
                path = pathTestData,
                fileNameGeneratedXml = "GeneratedXml_17695912",
                buySell = BuySell.BUY, // Export from Comtrader Export is a sell so this is a buy.
                controlArea = ControlArea.AMP, // TSO is AMP in Export
                trader = "RFSR",
                memo1 = "17695912", // Text from Export File
                memo2 = "17695912", // TradeNo/TradeID from Comtrader Export File
                tradeDate = "22.06.2020", // Date from Comtrader Export file
                contract = "10Q2", // Contract from Comtrader Export '20200622 10:15-20200622 10:30'
                quantity = "1", // Qty from Comtrader Export
                price = "11.2" // Prc from comtrader Export
            };

            ParameterXmlGeneration parameter2 = new ParameterXmlGeneration()
            {
                // Just in this test. Dynamic in real code.
                path = pathTestData,
                fileNameGeneratedXml = "GeneratedXml_17695913",
                buySell = BuySell.BUY, // Export from Comtrader Export is a sell so this is a buy.
                controlArea = ControlArea.AMP, // TSO is AMP in Export
                trader = "RFSR",
                memo1 = "VSK", // Text from Export File
                memo2 = "17695913", // TradeNo/TradeID from Comtrader Export File
                tradeDate = "22.06.2020", // Date from Comtrader Export file
                contract = "10Q2", // Contract from Comtrader Export '20200622 10:15-20200622 10:30'
                quantity = "2", // Qty from Comtrader Export
                price = "11.3" // Prc from comtrader Export
            };

            parameterList.Add(parameter1);
            parameterList.Add(parameter2);

            try
            {                
                IGenerateImporterXml xmlGenerator = new GenerateImporterXml();                
                xmlGenerator.GenerateXmlForVsk(parameterList);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
                      
            Assert.IsTrue(File.Exists(parameterList[0].path + @"\" + parameterList[0].fileNameGeneratedXml + ".xml"), "File does not exist.");
            Assert.IsTrue(File.Exists(parameterList[1].path + @"\" + parameterList[1].fileNameGeneratedXml + ".xml"), "File does not exist.");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade1ComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade1.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;
            
            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"                
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;
                                
                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17695912"), "TradeNo/TradeID from imported file was not read.");
            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade2ComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade2.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"                
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17703425"), "TradeNo/TradeID from imported file was not read.");
            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade3ComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade3.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };            

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",                
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"               
            };

            List<OutputData> outputDataList = new List<OutputData>();
           
            try
            {
                IReadExport comtrader = new ReadExport();                
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17704570"), "TradeNo/TradeID from imported file was not read.");
            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade4ComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade4.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };           

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",                
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"               
            };

            List<OutputData> outputDataList = new List<OutputData>();
           
            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;
                                
                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17704571"), "TradeNo/TradeID from imported file was not read.");
            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTrade5ComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade5.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",                
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"               
            };

            List<OutputData> outputDataList = new List<OutputData>();
           
            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;
                                                                                                 
                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17704572"), "TradeNo/TradeID from imported file was not read.");
            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion       

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. --  Several Trades
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- Several Trades
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradesComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "22.06.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };          

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "22.06.2020",                
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"                
            };

            List<OutputData> outputDataList  = new List<OutputData>();
            
            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("17695912"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(outputDataList[1].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[1].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[1].tradeNo.Equals("17703425"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(outputDataList[2].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[2].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[2].tradeNo.Equals("17704570"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(outputDataList[3].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[3].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[3].tradeNo.Equals("17704571"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(outputDataList[4].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[4].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[4].tradeNo.Equals("17704572"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion        

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade  -- T -- (D+1)
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade -- T -- (D+1)
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradeTComtraderExportGenerateXmlImportAligne()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade_T.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "15.12.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "15.12.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"                
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;
                                
                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("1143163090"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade  -- AMP
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade -- AMP)
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradeComtraderExportGenerateXmlImportAligne_AMP()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade_AMP.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "16.12.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "16.12.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("1143224326"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade  -- 50HZ
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade -- 50HZ)
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradeComtraderExportGenerateXmlImportAligne_50HZ()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade_50HZ.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "16.12.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "16.12.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("1143224328"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade  -- ENBW
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade -- ENBW)
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradeComtraderExportGenerateXmlImportAligne_ENBW()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade_ENBW.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "16.12.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "16.12.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("1143224324"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion

        #region Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade  -- TENET
        /// <summary>
        /// Get Deal export from Comtrader export file, generate XML and import to Aligne. -- One Trade -- TENNET)
        /// </summary>
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ExportTradeComtraderExportGenerateXmlImportAligne_TENNET()
        {
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(BookTradesAligneComtraderExportUnitTest)).CodeBase);
            string pathComtraderExportTEST = pathExecutable.Remove(0, 6) + @"\BookTradesInAligne\TestData\eex_deals_export_trade_TENNET.csv";
            const uint startRow = 2;
            string errorInformation = string.Empty;
            DataTable vskTradesResult = new DataTable(); // Empty for that test.
            bool importDone = false;

            ResultTable resultTable = new ResultTable()
            {
                numberOfRows = 0,
                table = new DataTable()
            };

            InputFilter filter = new InputFilter()
            {
                date = "16.12.2020",
                traderId = TraderId.TRD001,
                text = TextMemo2.VSK
            };

            InputDataComtraderExport input = new InputDataComtraderExport()
            {
                date = "16.12.2020",
                trader = "RFSR",
                password = "VV0QDC7cFs08XiYkMLHIEg==",
                pwEnCrypton = PwEnCrypton.PW_MANAGER,
                server = "MA-TR-KHST41"
            };

            List<OutputData> outputDataList = new List<OutputData>();

            try
            {
                IReadExport comtrader = new ReadExport();
                IBookTradesAligne bookTrades = new BookTradesAligne();

                errorInformation = comtrader.ExportDealsComtrader(pathComtraderExportTEST, filter, ref resultTable, startRow);
                input.resultComtraderExport = resultTable;

                bookTrades.DetectTradesComTraderExportAndBookToAligne(input, ref outputDataList, ref importDone, vskTradesResult, false);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }
            Assert.IsTrue(outputDataList[0].result.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(outputDataList[0].zKey.Length == 8, "Zkey from imported file was not read.");
            Assert.IsTrue(outputDataList[0].tradeNo.Equals("1143224329"), "TradeNo/TradeID from imported file was not read.");

            Assert.IsTrue(importDone, "Nothing was imported,");
        }
        #endregion
    }
}





