using ViertelStdTool.CredentialsHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestCredentialsReader
{
    [TestClass]
    public class CredentialsReaderUnitTest
    {
        #region Read credentials from TEST XML
        /// <summary>
        /// Read credentials from TEST XML
        /// </summary>
        [TestMethod]
        public void ReadCredentialsFromTestXML()
        {
            // Tool runs in TEST mode.
            string fileName = "UserSettingsDEV";

            ICredentialsReader credentialsReader;
            CredentialsStructureAligne readCredentials = new CredentialsStructureAligne();
            CredentialsStructureAligne templateCredentials = new CredentialsStructureAligne
            {
                adUser = "U16228",
                aligneUser = "RFSR",
                // Not used for compare.
                //password = "topSecret"
            };

            try
            {
                // Read config
                credentialsReader = new CredentialsReader(fileName);
                readCredentials = credentialsReader.Credentials;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(CompareCredentials(readCredentials, templateCredentials), "Read credentials are not equal to template.");
        }
        #endregion

        #region Read credentials from PROD XML
        /// <summary>
        /// Read configuration from PROD XML
        /// </summary>        
        [TestMethod]
        public void ReadCredentialsFromProdXML()
        {
            // Tool runs in PROD mode.
            string fileName = "UserSettingsPROD";

            ICredentialsReader credentialsReader;
            CredentialsStructureAligne readCredentials = new CredentialsStructureAligne();
            CredentialsStructureAligne templateCredentials = new CredentialsStructureAligne
            {
                adUser = "U16228",
                aligneUser = "RFSR",
                // Not used for compare.
                //password = "topSecret"
            };

            try
            {
                // Read config
                credentialsReader = new CredentialsReader(fileName);
                readCredentials = credentialsReader.Credentials;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception should not be thrown" + e.ToString());
            }

            Assert.IsTrue(CompareCredentials(readCredentials, templateCredentials), "Read credentials are not equal to template.");
        }
        #endregion

        #region Read credentials from not existing XML
        /// <summary>
        /// Read configuration from not existing XML
        /// </summary>        
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ReadCredentialsFromNotExistingXML()
        {
            // Tool runs in PROD mode.
            string fileName = "UserSettingsVoid";

            ICredentialsReader credentialsReader;
            CredentialsStructureAligne readCredentials = new CredentialsStructureAligne();
            CredentialsStructureAligne templateCredentials = new CredentialsStructureAligne
            {
                adUser = "U16228",
                aligneUser = "RFSR",
                // Not used for compare.
                //password = "topSecret"
            };

           
            // Read config
            credentialsReader = new CredentialsReader(fileName);
            readCredentials = credentialsReader.Credentials;
           
            Assert.IsNull(readCredentials.adUser, "Value should be null.");
            Assert.IsNull(readCredentials.aligneUser, "Value should be null.");
            Assert.IsNull(readCredentials.password, "Value should be null.");
        }
        #endregion

        #region Read credentials from corrupted XML
        /// <summary>
        ///Read credentials from corrupted XML
        /// </summary>        
        [TestMethod]
        [Ignore]  // Corupted File has to be added to user's private document folder.
        [ExpectedException(typeof(System.Xml.XmlException))]
        public void ReadCredentialsFromCorruptedXML()
        {
            // Tool runs in PROD mode.
            string fileName = "UserSettingsCorrupted";

            ICredentialsReader credentialsReader;
            CredentialsStructureAligne readCredentials = new CredentialsStructureAligne();
            CredentialsStructureAligne templateCredentials = new CredentialsStructureAligne
            {
                adUser = "U16228",
                aligneUser = "RFSR",
                // Not used for compare.
                //password = "topSecret"
            };
                        
            // Read config
            credentialsReader = new CredentialsReader(fileName);
            readCredentials = credentialsReader.Credentials;
           
            Assert.IsNull(readCredentials.adUser, "Value should be null.");
            Assert.IsNull(readCredentials.aligneUser, "Value should be null.");
            Assert.IsNull(readCredentials.password, "Value should be null.");
        }
        #endregion

        #region Read credentials from incomplete XML
        /// <summary>
        ///Read credentials from incomplete XML
        /// </summary>        
        [TestMethod]
        [Ignore]  // Incomplete File has to be added to user's private document folder.
        [ExpectedException(typeof(NullReferenceException))]
        public void ReadCredentialsFromIncompleteXML()
        {
            // Tool runs in PROD mode.
            string fileName = "UserSettingsIncomplete";

            ICredentialsReader credentialsReader;
            CredentialsStructureAligne readCredentials = new CredentialsStructureAligne();
            CredentialsStructureAligne templateCredentials = new CredentialsStructureAligne
            {
                adUser = "U16228",
                aligneUser = "RFSR",
                // Not used for compare.
                //password = "topSecret"
            };
           
            // Read config
            credentialsReader = new CredentialsReader(fileName);
            readCredentials = credentialsReader.Credentials;           
           
            Assert.IsNull(readCredentials.adUser, "Value should be null.");
            Assert.IsNull(readCredentials.aligneUser, "Value should be null.");
            Assert.IsNull(readCredentials.password, "Value should be null.");
        }
        #endregion

        #region Compare Credentials
        /// <summary>
        ///  Compare Configs
        /// </summary>
        /// <param name="credentials1"></param>
        /// <param name="credentials2"></param>
        /// <returns>bool 1 if equal</returns>
        private bool CompareCredentials(CredentialsStructureAligne credentials1, CredentialsStructureAligne credentials2)
        {
            bool result = true;

            // Active Directory USer
            if (!credentials1.adUser.Equals(credentials2.adUser))
            {
                result = false;
            }
            // Aligne Uer
            if (!credentials1.aligneUser.Equals(credentials2.aligneUser))
            {
                result = false;
            }           
            // Not used for compare.
            //if (!credentials1.password.Equals(credentials2.password))
            //{
            //    result = false;
            //}            

            return result;
        }
        #endregion
    }
}

