using System;
using System.Xml;
using ViertelStdTool.Log;

namespace ViertelStdTool.CredentialsHandler
{
    public class CredentialsReader : ICredentialsReader
    {
        private INLogger logger = new NLogger();

        #region Constructor to read in the credentials.
        /// <summary>
        /// Constructor to read in the credentials.
        /// </summary>       
        public CredentialsReader(string filename)
        {
            string privateDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Create the XmlDocument.
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(privateDocumentPath + @"\Aligne\" + filename + ".xml");

                XmlNode node = doc.SelectSingleNode("credentials/ADuser");

                Credentials.adUser = doc.SelectSingleNode("credentials/ADuser").InnerText;
                Credentials.aligneUser = doc.SelectSingleNode("credentials/aligneUser").InnerText;
                Credentials.password = doc.SelectSingleNode("credentials/password").InnerText;

                // Do the logging.
                logger.WriteInfo("Init: Credentials read from file: " + filename + ".xml.");
            }
            catch(Exception exception)
            {
                logger.WriteError("Credentials could not be read. Error: " + exception.Message.ToString());
                throw exception;
            }
        }
        #endregion

        #region Getter for credentials structure.
        /// <summary>
        /// Getter for credentials structure.
        /// </summary>
        public CredentialsStructureAligne Credentials { get; } = new CredentialsStructureAligne();
        #endregion
    }
}
