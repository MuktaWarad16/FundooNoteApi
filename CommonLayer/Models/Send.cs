//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Mail;
//using System.Text;

//namespace CommonLayer.Models
//{
//    public class Send
//    {
//        public string SendEmail(string ToEmail, string Token)
//        {
//            string FromEmail = "waradmukta16@gmail.com";
//            MailMessage message = new MailMessage(FromEmail, ToEmail);
//            string MailBody = "The token for the reset password: " + Token;
//            message.Subject = "token for reset password: " + Token;

//            message.Body = MailBody.ToString();
//            message.BodyEncoding = Encoding.UTF8;
//            message.IsBodyHtml = true;

//            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
//            NetworkCredential credential = new NetworkCredential("waradmukta16@gmail.com", "lisg pdpr wyxv dwzx");

//            smtpClient.EnableSsl = true;
//            smtpClient.UseDefaultCredentials = false;
//            smtpClient.Credentials = credential;
//            smtpClient.Send(message);
//            return ToEmail;
//        }
//    }
//}
