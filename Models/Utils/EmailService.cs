using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Models.Utils
{
    public interface IEmailService
    {
        Task<bool> sendEmailForUpdatePassword(string email, string token, string link,string name);
        Task<bool> sendEmailForActivateAccount(string email, string token, string link, string name);
    }

    public class EmailService : IEmailService
    {
        private IConfiguration _config;
        public EmailService(IConfiguration _config)
        {
            this._config = _config;
        }
        public async Task<bool> sendEmailForUpdatePassword(string email, string token,string link, string name)
        {
            try
            {

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("ECOM",
                _config["email:address"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(email, email);
                message.To.Add(to);

                message.Subject = "Forget Password";
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1 class='align-center'>Hello "+name+ ",</h1></br>";
                bodyBuilder.HtmlBody = "<a href='"+link+"auth/resetPassword?token="+token+"'>Please click here to reset password:</a>" +
                    "<br></br><p>Thanks!</p>";
                message.Body = bodyBuilder.ToMessageBody();

                return await sendEmail(message).ConfigureAwait(false);

            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> sendEmailForActivateAccount(string email, string token, string link, string name)
        {
            try
            {

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("ECOM",
                _config["email:address"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(email, email);
                message.To.Add(to);

                message.Subject = "Activate Account";
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1 class='align-center'>Hello " + name + ",</h1></br>";
                bodyBuilder.HtmlBody = "<a href='" + link + "auth/activateAccount?email="+email+"&token=" + token + "'>Please click here to activate account:</a>" +
                    "<br></br><p>Thanks!</p>";
                message.Body = bodyBuilder.ToMessageBody();

                return await sendEmail(message).ConfigureAwait(false);

            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> sendEmail(MimeMessage message)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync(_config["email:address"], _config["email:password"]).ConfigureAwait(false);

                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                client.Dispose();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
