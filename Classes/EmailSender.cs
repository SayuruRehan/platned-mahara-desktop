using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedMahara.Classes
{
    #region Task 91 - Email sending handeling
    public class EmailSender
    {
        private static string fromEmail = "no-reply-mahara-temp@edvicon.org"; 
        private static string password = "}vZT1<NR9K9tY+f6";    
        private static string smtpServer = "smtp.office365.com";     
        private static int smtpPort = 587;

        // Asynchronous Method
        public static Task<bool> MailSenderAsync(string recipientEmail, string MailSubject, string MailBody, bool IsBodyHtml)
        {
            return SendEmailAsync(recipientEmail, MailSubject, MailBody, IsBodyHtml);
        }

        public static Task<bool> MailSenderAsync(string MailSubject, string MailBody, bool IsBodyHtml)
        {
            return SendEmailAsync(GlobalData.UserEmail, MailSubject, MailBody, IsBodyHtml);
        }

        private static async Task<bool> SendEmailAsync(string recipientEmail, string MailSubject, string MailBody, bool IsBodyHtml)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(recipientEmail);
                    mail.Subject = MailSubject;
                    mail.Body = MailBody;
                    mail.IsBodyHtml = IsBodyHtml;

                    using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(fromEmail, password);
                        smtp.EnableSsl = true; // Ensures secure communication
                        await smtp.SendMailAsync(mail);
                    }
                }
                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false; // Failed to send email
            }
        }

        // Synchronous Method
        public static bool MailSender(string recipientEmail, string MailSubject, string MailBody, bool IsBodyHtml)
        {
            return SendEmail(recipientEmail, MailSubject, MailBody, IsBodyHtml);
        }

        public static bool MailSender(string MailSubject, string MailBody, bool IsBodyHtml)
        {
            return SendEmail(GlobalData.UserEmail, MailSubject, MailBody, IsBodyHtml);
        }

        private static bool SendEmail(string recipientEmail, string MailSubject, string MailBody, bool IsBodyHtml)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(recipientEmail);
                    mail.Subject = MailSubject;
                    mail.Body = MailBody;
                    mail.IsBodyHtml = IsBodyHtml;

                    using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(fromEmail, password);
                        smtp.EnableSsl = true; // Ensures secure communication
                        smtp.Send(mail); // Synchronous send
                    }
                }
                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false; // Failed to send email
            }
        }
    }
    #endregion 

}
