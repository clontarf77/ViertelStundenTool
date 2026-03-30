using System.Collections.Generic;

namespace ViertelStdTool.Mail
{
    public interface ISendMail
    {
        /// <summary>
        /// Send error mail via SMTP.
        /// </summary>
        /// <param name="sSMTPHost"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        void SendErrorMailviaSMTP(string sSMTPHost, string sender, List<string> receiver, string title, string body);

        /// <summary>
        /// Send mail via SMTP.
        /// </summary>
        /// <param name="sSMTPHost"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        void SendMailviaSMTP(string sSMTPHost, string sender, List<string> receiver, string title, string body);
    }
}
