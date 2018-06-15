using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace BusinessLayer.Services
{
    public class MailService
    {
        /// <summary>
        /// Send recovery email for when a used has lost his/her password
        /// </summary>
        /// <param name="email"></param>
        public void SendRecoveryMail(string email, string recoveryLink)
        {
            string[] emails = new string[]
            {
                email
            };
            string subject = "Wolfpack - Password reset";
            string message = "You have requested a password reset.<br>"
                + "Please go to this link reset your password: <a href='" + recoveryLink + "'>Reset Password</a>";
            SendMail(emails, subject, message, true);
        }

        /// <summary>
        /// Send register email for when a groupadmin added a non-existing user by email
        /// </summary>
        /// <param name="email"></param>
        public void SendRegisterMail(string email, string registerLink)
        {
            string[] emails = new string[]
            {
                email
            };
            string subject = "Wolfpack - Group invitation";
            string message = "You have been invited to join a WolfPack group.<br>"
                + "Please go to this link: <a href='" + registerLink + "'>Wolfpack Register</a>, to register on our website and automatically get added to the group this invitation is from.";
            SendMail(emails, subject, message, true);
        }

        /// <summary>
        /// Send login attempt email to let a user know that someone is trying to login and failed multiple times
        /// </summary>
        /// <param name="email"></param>
        /// <param name="unlockLink"></param>
        public void SendLoginAttemptMail(string email, string unlockLink)
        {
            string[] emails = new string[]
            {
                email
            };
            string subject = "Wolfpack - Too many login attempts";
            string message = "You or someone else has failed to login too many times on your Wolfpack account and therefor has been locked for 1 hour.<br>" +
                "If this was you click here to unlock your account <a href='" + unlockLink + "'>Unlock account</a>";
            SendMail(emails, subject, message, true);
        }

        /// <summary>
        /// Send custom mail to multiple emails
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void SendMailCustom(string[] emails, string subject, string message)
        {
            SendMail(emails, subject, message);
        }

        /// <summary>
        /// Send custom mail to a single person
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void SendMailCustom(string email, string subject, string message)
        {
            string[] emails = new string[]
            {
                email
            };
            SendMail(emails, subject, message);
        }

        /// <summary>
        /// Send mail to emails provided. Depending on type the email will be different.
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void SendMail(string[] emails, string subject = "", string message = "", bool isHtml = false)
        {
            if (subject != "" && message != "")
            {
                using (MailMessage mm = new MailMessage())
                {
                    mm.Subject = subject;
                    mm.Body = message;

                    if (isHtml) mm.IsBodyHtml = true;
                    else mm.IsBodyHtml = false;

                    foreach (string email in emails)
                    {
                        mm.To.Add(email);
                    }
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        try
                        {
                            smtp.Send(mm);
                        }
                        catch (SmtpException e)
                        {
                            Debug.WriteLine("Error {0}", e.StatusCode);
                        }
                    }
                }
            }
        }
    }
}
