namespace ComplainMgt.Services.Services
{
    public interface IEmailNotificationService
    {
        void SendUserUpdateNotification(string userEmail, string updatedBy, string Type, string complainDescription, string complainStatus);
        void SendAdminUpdateNotification(string adminEmail, string updatedUserEmail, string Type, string complainDescription, string complainStatus);
        void SendAdminComplainAddedNotification(string adminEmail, string AddedByUserEmail, string Type, string complainDescription, string complainStatus);
    }
}
