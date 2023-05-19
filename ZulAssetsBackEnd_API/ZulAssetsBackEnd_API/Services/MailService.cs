using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using ZulAssetsBackEnd_API.Model;
using ZulAssetsBackEnd_API.Settings;
using static ZulAssetsBackEnd_API.BAL.ResponseParameters;

namespace ZulAssetsBackEnd_API.Services
{
    public class MailService : IMailService
    {

        #region Declaration

        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _config;

        public MailService(IOptions<MailSettings> options, IConfiguration configuration)
        {
            _mailSettings = options.Value;
            _config = configuration;
        }


        #endregion

        #region ForgetPasswordEmail

        public async Task ForgetPasswordEmail(MailRequest mailRequest)
        {
            var fromAddress = _config.GetSection("Mail").Value;
            var fromPassword = _config.GetSection("Password").Value;
            var host = _config.GetSection("Host").Value;
            var ToAddress = _config.GetSection("EmailTo").Value;
            var email = new MimeMessage();
            var builder = new BodyBuilder();
            Message msg = new Message();
            email.From.Add(new MailboxAddress(_config.GetSection("DisplayName").Value, fromAddress));
            email.To.Add(new MailboxAddress("", ToAddress));
            try
            {
                email.Subject = "ZulAssets - Account recovery instruction";
                builder.HtmlBody = "Dear Admin! <br> Please change password for " + mailRequest.AppliedUserName + "<br> <br> Thank You <br> ZulAssets Management System" ;
            }
            catch
            {
                msg.message = "Please contact IT department for your password information.";
            }
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(host, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(fromAddress, fromPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        #endregion

    }
}
