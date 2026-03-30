using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ViertelStdTool.AligneImporter.Rest;
using ViertelStdToolLib.Aligne.Importer.DealStructure;
using ViertelStdToolLib.Aligne.Importer.DealStructure.Rest;

namespace UnitTestAligneImporterRest
{
    [TestClass]
    public class AligneImporterUnitTestRest
    {                                       
        #region Import Rest new Deal (Buy) with 15min Shapes to DEV3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealBuyDev3()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34); 

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal (Buy) with 15min Shapes to PREPROD3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealBuyPreProd3()
        {
            const string server = "MA-TR-KHST35";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl                
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal (Sell) with 15min Shapes to DEV3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        //[Ignore]
        public void ImportRestNewDealSellDev3()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "S",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.            
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal (Sell) with 15min Shapes to PREPROD3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
         public void ImportRestNewDealSellPreProd3()
        {
            const string server = "MA-TR-KHST35";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "S",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl                
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.    
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion
                             
        #region Import Rest new Deal with 30min Shapes to DEV3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealDev3HalfHourShape()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._30min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "N", // 30 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:30 --> 8 * 2 + 1 = 17 --> start at 0 --> 16
            trade.SetShapeData("0", "5", 16);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal  with hour Shapes to DEV3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealDev3HourShape()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType.hour)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "H", // hour shape                              // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:00 --> 8 * 1 = 8 --> start at 0 --> 7
            trade.SetShapeData("0", "5", 7);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal  with hour Shapes to DEV3 with user RFSR use password from PasswordManager.
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealPreProd3HourShape_NT()
        {
            const string server = "MA-TR-KHST35";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType.hour)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "H", // hour shape                              // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "ITA_EVO_EPM_15min",                            // memo1
                Memo2 = "1919-9Q 221,70 Euro 0,2 MW NT",                // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for hour min.--> // 8:00 --> 8 * 1 = 8 --> start at 0 --> 7
            trade.SetShapeData("0", "5", 7);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
                                    // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("success"), "Upload.exe was not execuded.");
            Assert.IsTrue(zKey.Length == 8, "Zkey from imported file was not read.");
        }
        #endregion

        #region Import Rest new Deal with 15min Shapes -- use wrong server
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealWrongServer()
        {
            const string server = "MA-TR-KHST44";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("Die Verbindung mit dem Remoteserver kann nicht hergestellt werden."), "Upload.exe does not return correct error message");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import Rest new Deal with 15min Shapes -- use wrong user
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealWrongUser()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "AAA"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("Login failed"), "Upload.exe does not return correct error message.");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import Rest new Deal with 15min Shapes -- use wrong password
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        //[Ignore]
        public void ImportRestNewDealWrongPassword()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "D",                                            // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "wrong";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(retValue.Equals("Login failed for TRM REST. Password not valid."), "Upload.exe does not return correct error message.");
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
        }
        #endregion

        #region Import Rest new Deal with 15min Shapes to DEV3 with user RFSR use wrong JSON (Parameter)
        [TestMethod]
        // Set ignore deal should not be imported by mistake.
        [Ignore]
        public void ImportRestNewDealWrongJSON()
        {
            const string server = "MA-TR-KHST45";

            RootObjectRequestTrade trade = new RootObjectRequestTrade(ShapeType._15min)
            {
                BuySell = "B",                                          // buysell
                Status = "DN",                                          // status
                // Filled with POWER SHAPES                             // blktpl
                Mkt1 = "AMP",                                           // mkt1
                Comp1 = "AMP",                                          // comp1     
                Mkt2 = "FX",                                            // mkt2
                Comp2 = "EUR",                                          // comp2
                // Automatically set                                    // firmtype
                Hflag = "F", // 15 min shape                            // hflag
                Unit = "W",                                             // unit
                Book = "INTR",                                          // book
                Cpty = "KVPC",                                          // cpty
                Contract = "DLV",                                       // contract
                Currency = "EUR",                                       // currency
                Tgroup1 = "00001",                                      // tgroup1
                Tgroup2 = "00005",                                      // tgroup2
                Tgroup3 = "00000",                                      // tgroup3
                Util1 = "C",     // ------ wrong                        // util1
                Util2 = "A",                                            // util2
                Util3 = "A",                                            // util3
                Util4 = "A",                                            // util4
                Util5 = "A",                                            // util5
                Memo1 = "DealImportTestVSKviaRest",                     // memo1
                Memo2 = "UnitTest",                                     // memo2
                // Use login data if not changed.                       // trader
                // Set according template                               // a5tpl               
                LEBALGROUP = "AMP-MVVTRD",                              // module name="POWERNOMS" -- lebalgrp
                CPBALGROUP = "AMP-11XMVV---------5",                    // module name="POWERNOMS" -- cpbalgrp

                // is 160 but should be 250
                //TpowDays = "SMTWtFsH", // ???
                // Check ammount again               
            };

            // TODO method needed to convert data.
            // Add shape data for 15 min.--> // 8:45 --> 8 * 4 + 3 = 35 --> start at 0 --> 34
            trade.SetShapeData("0", "5", 34);

            string user = "A16228"; // This is TRM User as in A3/A5 not RFSR anymore.
            // Use encrypted password from PasswordManager.
            string password = "h96GdYuq9Ci0LXMQnoQqzg==";
            string retValue = string.Empty;
            string zKey = string.Empty;

            try
            {
                IAligneImporterRest aligneRest = new AligneImporterRest(server, user, password);
                retValue = aligneRest.ImportDeal(trade, PwEnCrypton.PW_MANAGER, ref zKey);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
            Assert.IsTrue(zKey.Length == 0, "Zkey from imported file was not empty.");
            Assert.IsTrue(retValue.Equals("Invalid fields {tpowUtil1=C}"), "Upload.exe was not execuded.");            
        }
        #endregion

        // Possible improvement.
        // Implement test for one login with several imports and then logout.
    }
}


