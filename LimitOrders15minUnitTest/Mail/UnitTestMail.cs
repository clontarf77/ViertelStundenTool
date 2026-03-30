using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;
using ViertelStdTool.Mail;

namespace UnitTestMail
{
    [TestClass]
    public class UnitTestMail
    {
        #region Send a mail.
        [TestMethod]
        public void SendMail()
        {
            try
            {
                // Parameter 
                string sSMTPHost = "smtp-ma.konzern.mvvcorp.de";
                string sender = "ViertelStdTool@mvv.de";
                List<string> receiver = new List<string>() { "ralf.schloesser@mvv.de" };
                string title = "ViertelStdTool - " +Assembly.GetExecutingAssembly().GetName().Version.ToString() + " - Emergency-Backup-Software is used";
                string body = "The software currently in use is the <b> Emergency-Backup-Software </b> for the  <b> ViertelstundenTool </b>. <br>" +
                              "If this email has been received, something has gone wrong with the <b> ViertelstundenTool </b> Client-Server-Solution provided by Soluvia. <br><br>" +
                              "<b> Emergency-Backup-Software </b> was started at: <b>" + DateTime.Now.ToString() + "</b>";
                             
                ISendMail sendMail = new SendMail();
                sendMail.SendMailviaSMTP(sSMTPHost, sender, receiver, title, body);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
        }
        #endregion

        #region Send error mail.
        [TestMethod]
        public void SendErrorMail()
        {
            try
            {
                // Parameter 
                string sSMTPHost = "smtp-ma.konzern.mvvcorp.de";
                string sender = "ViertelStdTool@mvv.de";
                List<string> receiver = new List<string>() { "ralf.schloesser@mvv.de" };
                string title = "ViertelStdTool - Test-Mail";
                string body = "This is just a test.";

                ISendMail sendMail = new SendMail();
                sendMail.SendErrorMailviaSMTP(sSMTPHost, sender, receiver, title, body);
            }
            catch (Exception e)
            {
                // If exception is catched the test is not ok.
                Assert.Fail("Exception should not be thrown here" + e.Message);
            }
        }
        #endregion
    }
}

