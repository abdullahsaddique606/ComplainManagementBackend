using ComplainMgt.Services.Model;
using MailKit.Net.Smtp;
using MimeKit;



namespace ComplainMgt.Services.Services;

public class EmailServices : IEmailServices
{
    private readonly EmailConfiguration _emailConfig;
    public EmailServices(EmailConfiguration emailConfig)
    => _emailConfig = emailConfig;

    public void SendEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private void Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            client.Send(mailMessage);
        }
        catch
        {
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Email", _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
        return emailMessage;

    }


}

