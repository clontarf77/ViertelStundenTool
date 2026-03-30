using ViertelStdTool.Log;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ViertelStdTool.Mail
{
    public class SendMail : ISendMail
    {
        private INLogger logger = new NLogger();

        #region Send error mail via SMTP
        /// <summary>
        /// Send error mail via SMTP.
        /// </summary>
        /// <param name="sSMTPHost"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        public void SendErrorMailviaSMTP(string sSMTPHost, string sender, List<string> receiver, string title, string body)
        { 
            try
            {
                MailMessage message = new MailMessage();
                
                foreach (string elementReceiver in receiver)
                {
                    message.To.Add(elementReceiver);
                    logger.WriteInfo("Send mail to " + elementReceiver);
                }

                message.From = new MailAddress(sender);

                message.Subject = title;
                message.Body = body + "<br>";
                message.Body += "Please see <b>logfile.log</b>, which is located in same folder as the executable.";
                message.IsBodyHtml = true;

                SmtpClient smtpc = new SmtpClient
                {
                    Host = sSMTPHost
                };
                smtpc.Send(message);              
            }
            catch (Exception)
            {
                //logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");               
            }
        }
        #endregion       

        #region Send mail via SMTP
        /// <summary>
        /// Send mail via SMTP.
        /// </summary>
        /// <param name="sSMTPHost"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        public void SendMailviaSMTP(string sSMTPHost, string sender, List<string> receiver, string title, string body)
        {
            try
            {
                MailMessage message = new MailMessage();

                foreach (string elementReceiver in receiver)
                {
                    message.To.Add(elementReceiver);
                    logger.WriteInfo("Send mail to " + elementReceiver);
                }

                message.From = new MailAddress(sender);

                message.Subject = title;
                message.Body = body + "<br>";
                message.IsBodyHtml = true;

                SmtpClient smtpc = new SmtpClient
                {
                    Host = sSMTPHost
                };
                smtpc.Send(message);
            }
            catch (Exception)
            {
                //logger.WriteError("Exception in : " + (MethodBase.GetCurrentMethod().Name) + "(): " + exception.Message.ToString() + ".");               
            }
        }
        #endregion       
    }
}

