using Microsoft.Extensions.Options;
using MimeKit;
using PetHouse.Application.Contracts.Mail;
using PetHouse.Application.Interfaces.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace PetHouse.Application.Services;

public class MailService : IMailService
{
   private readonly SenderSettings _senderSettings;

   public MailService(IOptions<SenderSettings> senderSettings)
   {
      _senderSettings = senderSettings.Value;
   }

   // Method for sending an email with a verification or password recovery code.
   public async Task SendEmail(string recieverEmail, string code, EmailSendType sendType)
   {
      var email = new MimeMessage();

      // Set up the sender's email address.
      email.From.Add(new MailboxAddress("PetHouse mail system", _senderSettings.SenderEmail));
      email.To.Add(new MailboxAddress("", recieverEmail));

      // Set the subject of the email.
      email.Subject = "Verification Code";

      // Determine the email template based on the email type (Verification or Password Recovery).
      var templatePath = Path.Combine(
         Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "PetHouse.Application")),
         "Templates",
         sendType == EmailSendType.VerificationCode ? "EmailTemplate.html" : "PasswordRecoveringTemplate.html");

      // Read the template file and replace the placeholder with the verification code.
      var htmlContent = await File.ReadAllTextAsync(templatePath);
      htmlContent = htmlContent.Replace("{{code}}", code);

      // Set the email body to the modified HTML content.
      email.Body = new TextPart("html")
      {
         Text = htmlContent
      };

      // Set up the SMTP client for sending the email.
      using var smtp = new SmtpClient();
      try
      {
         // Connect to the SMTP server (Gmail in this case) using TLS.
         await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

         // Authenticate using the sender's email and password.
         await smtp.AuthenticateAsync(_senderSettings.SenderEmail, _senderSettings.SenderPassword);

         // Send the email.
         await smtp.SendAsync(email);
      }
      finally
      {
         await smtp.DisconnectAsync(true);
      }
   }
}