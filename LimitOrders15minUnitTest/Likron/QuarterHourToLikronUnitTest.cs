using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using ViertelStdTool.ConfigurationHandler;
using ViertelStdTool.Likron;

namespace UnitTestQuarterHourToLikron
{
    [TestClass]
    public class QuarterHourToLikronUnitTest
    {
        private string resultInit = string.Empty;
        private string resultProcess = string.Empty;
        const string fileStartsWith = "FromVPPToComTrader";
        private readonly IConfigurationReader configReader = new ConfigurationReader();
        private ConfigurationStructure config = new ConfigurationStructure();
        private readonly string pathExecutable = (Path.GetDirectoryName(Assembly.GetAssembly(typeof(QuarterHourToLikronUnitTest)).CodeBase)).Remove(0, 6);
        private ResultsQuarterHourToLikron results = new ResultsQuarterHourToLikron();
        
        #region QuarterlyHourToLikron
        [TestMethod]
        public void QuarterlyHourToLikron()
        {        
            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                config = configReader.Config;
                // Set own path.
                config.ownPath = pathExecutable + "\\Likron\\";
                resultInit = toLikron.Init(config);
                resultProcess = toLikron.WriteQuarterHourOrderToLikron(LimitType.NoLimit, "50", "0","0", fileStartsWith, ref results);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultInit.Equals("OK"), "Init was not successful");
            Assert.IsTrue(resultProcess.StartsWith("OK"), "Init was not successful");
        }
        #endregion

        #region QuarterlyHourToLikronTuD
        [TestMethod]
        public void QuarterlyHourToLikronTuD()
        {
            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                config = configReader.Config;
                // Set own path.
                config.ownPath = pathExecutable + "\\Likron\\";
                resultInit = toLikron.Init(config);
                resultProcess = toLikron.WriteQuarterHourOrderToLikronTuD(LimitType.NoLimit, "50", "0", "0", fileStartsWith, ref results);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultInit.Equals("OK"), "Init was not successful");
            Assert.IsTrue(resultProcess.StartsWith("OK"), "Init was not successful");
        }
        #endregion

        #region Check if we are in TuD range for product 15Q4
        [TestMethod]
        public void TudRange15Q4()
        {
            string contract =  "15Q4";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);   
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.Equals("OK"), "Init was not successful");
            Assert.IsTrue(additionalRangeInfo.Length > 0, "Additional range info not set.");
        }
        #endregion

        #region Test GetContractRange for Product 00Q1
        [TestMethod]
        public void TudRange00Q1()
        {
            string contract = "00Q1";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.Equals("OK"), "Init was not successful");
            Assert.IsTrue(additionalRangeInfo.Length > 0, "Additional range info not set.");
        }
        #endregion

        #region Test GetContractRange for Product 00Q1
        [TestMethod]
        public void TudRange00Q1_AMP()
        {
            string contract = "00Q1_AMP";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.Equals("OK"), "Init was not successful");
            Assert.IsTrue(additionalRangeInfo.Length > 0, "Additional range info not set.");
        }
        #endregion

        #region Test GetContractRange for Product T00Q1
        [TestMethod]
        public void TudRangeT00Q1()
        {
            string contract = "T00Q1";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.Equals("OK"), "Init was not successful");
            Assert.IsTrue(additionalRangeInfo.Length > 0, "Additional range info not set.");
        }
        #endregion

        #region Check if we are in TuD range for product 15q2
        [TestMethod]
        public void TudRange15q2()
        {
            string contract = "15q2";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.Equals("OK"), "Init was not successful");
            Assert.IsTrue(additionalRangeInfo.Length > 0, "Additional range info not set.");
        }
        #endregion

        #region Check if we are in TuD range for wrong contract 
        [TestMethod]
        public void TudRangeWrongContract()
        {
            string contract = "AbsoluteWrong";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
           
            Assert.IsTrue(resultProcess.StartsWith("NOK"), "Init was successful");
            Assert.IsTrue(additionalRangeInfo.Length == 0, "Additional range info set.");
        }
        #endregion

        #region Check if we are in TuD range for wrong hour and quarter
        [TestMethod]
        public void TudRangeWrongHourAndQuarter()
        {
            string contract = "99Q10";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.StartsWith("NOK"), "Init was successful");
            Assert.IsTrue(additionalRangeInfo.Length == 0, "Additional range info set.");
        }
        #endregion

        #region Check if we are in TuD range for wrong hour
        [TestMethod]
        public void TudRangeWrongHour()
        {
            string contract = "99Q4";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.StartsWith("NOK"), "Init was successful");
            Assert.IsTrue(additionalRangeInfo.Length == 0, "Additional range info set.");
        }
        #endregion

        #region Check if we are in TuD range for wrong quarter
        [TestMethod]
        public void TudRangeWrongQuarter()
        {
            string contract = "15Q99";
            Range contractRange = Range.normal;
            string additionalRangeInfo = string.Empty;

            try
            {
                IQuarterHourToLikron toLikron = new QuarterHourToLikron();
                resultProcess = toLikron.GetContractRange(contract, ref contractRange, ref additionalRangeInfo);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }

            Assert.IsTrue(resultProcess.StartsWith("NOK"), "Init was successful");
            Assert.IsTrue(additionalRangeInfo.Length == 0, "Additional range info set.");
        }
        #endregion
    }
}

