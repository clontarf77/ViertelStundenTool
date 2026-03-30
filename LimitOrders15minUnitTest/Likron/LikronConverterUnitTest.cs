using ViertelStdTool.Converter;
using ViertelStdTool.Likron;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace UnitTestLikronConverter
{
    [TestClass]
    public class LikronConverterUnitTest
    {
        #region Generate Order Data CSV BUY for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikron()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit, 
                validityEnd = ValidityEnd.source
            };
                   
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";
                      
            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue( filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikronEmptyValidityEnd()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.empty
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE_ValEndEmpty.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyNextDayForLikron()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy_NextDay.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_NextDay.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - limit 50 percent.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// Limit set to 50 percent.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikron50Percent()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                loadLimit = "50",
                limitType = LimitType.Percent,
                validityEnd = ValidityEnd.source
            };
            
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE_50Percent.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - limit 110 percent - In this case limit is ignored.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// Limit set to 110 percent. In this case Limit is ignored.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikron110Percent()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                loadLimit = "110",
                limitType = LimitType.Percent,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - limit 10 MWh.
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// Limit set to 10 MWh.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikronLimit10MWh()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                loadLimit = "10",
                limitType = LimitType.MWh,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE_Limit10MWh.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion   

        #region Generate Order Data CSV SELL for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV SELL for Likron.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataSellForLikron()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Sell.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };            
            
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.            
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Sell_RWE.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (likronData.pathOrderLikron.Length > 0)
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (likronData.pathOrderLikronSmaller30Min.Length > 0)
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty");
            Assert.IsTrue(filesAreEqual, "Template Sell Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV where Sell and Buy are balanced for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV Sell and Buy are balanced.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBalancedForLikron()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Balanced.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };
            
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Balanced_RWE.csv";
                        
            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty");
            Assert.IsTrue(filesAreEqual, "Template for Nalanced Order and generated Order for Likron do not match.");
            Assert.IsTrue(likronData.sumQuantityIsZero, "Sum of quantity should b e zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion   

        #region Generate Order Data CSV - wrong Path/not existing Limit file - no limit.
        /// <summary>
        /// Generate Order Data CSV - wrong Path/not existing Limit file
        /// No Limits set.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataNoLimitFile()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\WrongPath\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };
                        
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";
            string retVal = string.Empty;
          
            IGenerateOrderData likron = new GenerateOrderData();
            retVal = likron.GenerateCsvForLikron(ref likronData);

            Assert.IsFalse(retVal.Equals("OK"), "Result should not be OK.");
        }
        #endregion

        #region Generate Order Data CSV - wrong Path/not existing Load file - no limit.
        /// <summary>
        /// Generate Order Data CSV - wrong Path/not existing Limit file
        /// No Limits set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.IO.DirectoryNotFoundException))]
        public void GenerateOderDataNoLoadFile()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\WrongPath\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";
                        
            string retVal = string.Empty;

            IGenerateOrderData likron = new GenerateOrderData();
            retVal = likron.GenerateCsvForLikron(ref likronData);           
        }
        #endregion

        #region Generate Order Data CSV - wrong path output directory - no limit.
        /// <summary>
        /// Generate Order Data CSV - wrong path output directory
        /// No Limits set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.UnauthorizedAccessException))]
        public void GenerateOderDataNoOutPutDirectory()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);
            
            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = "C:\\WrongPath",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };
                                  
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";
                 
            string retVal = string.Empty;

            IGenerateOrderData likron = new GenerateOrderData();
            retVal = likron.GenerateCsvForLikron(ref likronData);
        }
        #endregion

        #region Generate Order Data CSV - try to read data from load file with wrong table name - no limit.
        /// <summary>
        /// Generate Order Data CSV - try to read data from load file with wrong table name
        /// No Limits set.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataWrongTableNameLoadFile()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "WrongTableName",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };
            
            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";
                       
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsFalse(retVal.Equals("OK"), "Result should not be OK.");
        }
        #endregion

        #region Generate Order Data CSV BUY for Likron - limit type not set - limits for Percent and MWh set
        /// <summary>
        /// Generate Order Data CSV BUY for Likron.
        /// Limits set but no limit type. Limits should be ignored thus default is LimitType-->NoLimit.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataBuyForLikronNoLimitType()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_Buy.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                loadLimit = "10",
                //limitType = LimitType.NoLimit - not set this should be default
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_Buy_RWE.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion       

        #region Generate Order Data CSV for Likron with 3 different contracts - limits not set 
        /// <summary>
        /// Generate Order Data CSV 3 different contracts for Likron.
        /// Limits set but no limit type. Limits should be ignored thus default is LimitType-->NoLimit.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderData3ContractsLikronNoLimits()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_3Contracts.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_3Contracts.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                // According to the time this test is executed there will be 1 or 2 files. Code is written the way that test works.
                // For proper test check the files manually.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV for Likron with 3 different contracts - limits not set 
        /// <summary>
        /// This is the original file from emsys with additional information in contract and product which has to be filtered.
        /// Generate Order Data CSV 3 different contracts for Likron.
        /// Limits set but no limit type. Limits should be ignored thus default is LimitType-->NoLimit.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderData3ContractsLikronNoLimits_Org()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_3Contracts_org.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_3Contracts_org.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;
            string likronFilePath = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikron(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();

                // This is workaround which is needed to bring correct results even if test is started at a not fitting time.
                // According to the time this test is executed there will be 1 or 2 files. Code is written the way that test works.
                // For proper test check the files manually.
                if (File.Exists(likronData.pathOrderLikron))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
                    likronFilePath = likronData.pathOrderLikron;
                }
                if (File.Exists(likronData.pathOrderLikronSmaller30Min))
                {
                    filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikronSmaller30Min);
                    likronFilePath = likronData.pathOrderLikronSmaller30Min;
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronFilePath.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV TuD for Likron - no limit.
        /// <summary>
        /// Generate Order Data CSV TuD for Likron.
        /// No Limits set.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTudForLikron()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_TuD.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",                
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template TuD Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV TuD for Likron - limit 60 percent.
        /// <summary>
        /// Generate Order Data CSV TuD for Likron.
        /// Limit set to 60 percent.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTudForLikron60Percent()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_TuD.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                loadLimit = "60",
                limitType = LimitType.Percent,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD_60Percent.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template TuD Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV TuD for Likron - limit 85 percent.
        /// <summary>
        /// Generate Order Data CSV TuD for Likron.
        /// Limit set to 85 percent.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTudForLikron85Percent()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_TuD.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                loadLimit = "85",
                limitType = LimitType.Percent,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD_85Percent.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template TuD Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV TuD for Likron - limit 9 MWh.
        /// <summary>
        /// Generate Order Data CSV TuD for Likron.
        /// Limit set to 9 MWh.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTudForLikronLimit9MWh()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_TuD.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                loadLimit = "9",
                limitType = LimitType.MWh,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD_Limit9MWh.csv";
                                                                            
            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template TuD Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion       

        #region Generate Order Data CSV Tud for Likron with 3 different contracts - limits not set 
        /// <summary>
        /// Generate Order Data CSV TuD 3 different contracts for Likron.
        /// Limits set but no limit type. Limits should be ignored thus default is LimitType-->NoLimit.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTud3ContractsLikronNoLimits()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_3Contracts.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD_3Contracts.csv";

            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion

        #region Generate Order Data CSV for Likron with 3 different contracts - limits not set 
        /// <summary>
        /// This is the original file from emsys with additional information in contract and product which has to be filtered.
        /// Generate Order Data CSV TuD 3 different contracts for Likron.
        /// Limits set but no limit type. Limits should be ignored thus default is LimitType-->NoLimit.
        /// This test verifies that the generated order file for likron does fit to the given template.
        /// HINT: Generated File cannot be imported to likron because order is in the past.
        /// </summary>
        [TestMethod]
        public void GenerateOderDataTud3ContractsLikronNoLimits_Org()
        {
            string result = string.Empty;
            string pathExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LikronConverterUnitTest)).CodeBase);

            OrderDataGenerationData likronData = new OrderDataGenerationData()
            {
                pathLoadCSV = pathExecutable.Remove(0, 6) + @"\Likron\TestData\FromVPPToComTrader_3Contracts_org.csv",
                pathOrderLikron = pathExecutable.Remove(0, 6) + @"\Likron\TestData\",
                pathLimitExcel = pathExecutable.Remove(0, 6) + @"\Converter\TestData\Limit_VSK_15Min.xlsx",
                tableName = "Limit_VSK_15Min",
                sumQuantityIsZero = false,
                limitType = LimitType.NoLimit,
                validityEnd = ValidityEnd.source
            };

            // This is the result to this path the generated likron order will be loaded. The name will be set in called function.
            string pathOrderLikronTemplate = likronData.pathOrderLikron + @"Deal_Import_Template_TuD_3Contracts_org.csv"; // TODO adapt contract

            bool filesAreEqual = false;
            string retVal = string.Empty;

            try
            {
                IGenerateOrderData likron = new GenerateOrderData();
                retVal = likron.GenerateCsvForLikronTuD(ref likronData);

                ICsvDataTableConverter converter = new CsvDataTableConverter();
                filesAreEqual = converter.CompareCSVs(pathOrderLikronTemplate, likronData.pathOrderLikron);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(likronData.pathOrderLikron.Length > 0, "Path of order file for likron is empty.");
            Assert.IsTrue(filesAreEqual, "Template Buy Order and generated Order for Likron do not match.");
            Assert.IsFalse(likronData.sumQuantityIsZero, "Sum of quantity must not be be zero");
            Assert.IsTrue(retVal.Equals("OK"), "File generation for Likron was not successful");
        }
        #endregion
    }
}
