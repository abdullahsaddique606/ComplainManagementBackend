using ComplainMgt.Services.Model;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace ComplainMgt.Services.Services;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailServices _emailServices;

    public EmailNotificationService(IEmailServices emailServices)
    {
        _emailServices = emailServices;
    }

    public void SendUserUpdateNotification(string userEmail, string updatedBy, string Type, string complainDescription, string complainStatus)
    {
        string subject = "Complain Update Notification";
        string message = $"Hello User,\n\nComplain information has been updated by admin {updatedBy}.\n Type: {Type}\n Description: {complainDescription}\nStatus:{complainStatus}";

        var emailMessage = new Message(
            new List<string> { userEmail },
            subject,
            message
        );

        _emailServices.SendEmail(emailMessage);
    }

    public void SendAdminUpdateNotification(string adminEmail, string updatedUserEmail, string Type, string complainDescription, string complainStatus)
    {
        string subject = "Update Notification";
        string message = $"Hello Admin,\n\nComplain information has been updated by {updatedUserEmail}.\n Type: {Type}\n Description: {complainDescription}\nStatus:{complainStatus}";

        var emailMessage = new Message(
            new List<string> { adminEmail },
            subject,
            message
        );

        _emailServices.SendEmail(emailMessage);
    }
    public void SendAdminComplainAddedNotification(string adminEmail, string AddedByUserEmail, string Type, string complainDescription, string complainStatus)
    {
        string subject = "New Complain Added Notification";
        string message = $"Hello Admin,\n\nComplain has been Added by {AddedByUserEmail}.\nComplain Detail is below:\n Type: {Type}\n Description: {complainDescription}\nStatus:{complainStatus}";

        var emailMessage = new Message(
            new List<string> { adminEmail },
            subject,
            message
        );

        _emailServices.SendEmail(emailMessage);

    }
}
