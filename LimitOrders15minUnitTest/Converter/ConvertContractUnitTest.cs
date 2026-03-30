using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ViertelStdToolLib.Converter;

namespace UnitTestConvertContract
{
    [TestClass]
    public class ConvertContractUnitTest
    {
        #region Convert contract '00Q1' data to contract delivery start time
        /// <summary>
        /// Convert contract '00Q1' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_00Q1()
        {
            string contract = "00Q1";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals("00:00"), "Columns are available in the generated table.");            
        }
        #endregion      

        #region Convert contract '15Q4' data to contract delivery start time
        /// <summary>
        /// Convert contract '15Q4' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_15Q4()
        {
            string contract = "15Q4";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals("15:45"), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract '00Q1_AMP' data to contract delivery start time
        /// <summary>
        /// Convert contract '00Q1_AMP' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_00Q1_AMP()
        {
            string contract = "00Q1_AMP";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals("00:00"), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract 'T00Q1' data to contract delivery start time
        /// <summary>
        /// Convert contract 'T00Q1' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_T00Q1()
        {
            string contract = "T00Q1";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals("00:00"), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract '15q2' data to contract delivery start time
        /// <summary>
        /// Convert contract '15q2' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_15q2()
        {
            string contract = "15q2";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals("15:15"), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract '99Q10' data to contract delivery start time
        /// <summary>
        /// Convert contract '99Q10' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_99Q10()
        {
            string contract = "99Q10";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals(""), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract '99Q4' data to contract delivery start time
        /// <summary>
        /// Convert contract '99Q4' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_99Q4()
        {
            string contract = "99Q4";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals(""), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract '15Q99' data to contract delivery start time
        /// <summary>
        /// Convert contract '15Q99' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_15Q99()
        {
            string contract = "15Q99";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals(""), "Columns are available in the generated table.");
        }
        #endregion      

        #region Convert contract 'AbsoluteWrong' data to contract delivery start time
        /// <summary>
        /// Convert contract 'AbsoluteWrong' data to contract delivery start time.
        /// </summary>
        [TestMethod]
        public void GetDeliveryStartTimeFromContract_AbsoluteWrong()
        {
            string contract = "AbsoluteWrong";
            string deliveryStartTime = string.Empty;

            try
            {
                IContractValidityConverter converter = new ContractValidityConverter();
                converter.ConvertContractToValidityStartTime(contract, ref deliveryStartTime);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(deliveryStartTime.Equals(""), "Columns are available in the generated table.");
        }
        #endregion      
    }
}


