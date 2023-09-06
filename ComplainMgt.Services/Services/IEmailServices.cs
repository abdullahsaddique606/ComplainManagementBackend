using ComplainMgt.Services.Model;

namespace ComplainMgt.Services.Services;
public interface IEmailServices
{
    void SendEmail(Message message);

}
