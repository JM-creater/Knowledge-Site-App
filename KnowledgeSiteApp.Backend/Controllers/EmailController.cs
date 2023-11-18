using KnowledgeSiteApp.Backend.Attributes;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        [ApiKey]
        public async Task<IActionResult> SendEmail(string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("eldora4@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("eldora4@ethereal.email"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("eldora4@ethereal.email", "Uf5kuknb5hXHZugcKH");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
