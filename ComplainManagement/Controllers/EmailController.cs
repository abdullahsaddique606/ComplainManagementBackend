/*using ComplainMgt.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComplainManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IEmailNotificationService _emailNotificationService;

    public EmailController(IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
    }

    [HttpPost("user-update")]
    public IActionResult UserUpdate(UserUpdateRequest request)
    {
        // Logic to update user data

        // Send notification to the user
        _emailNotificationService.SendUserUpdateNotification(request.UserEmail, "Admin", "Updated Content");

        return Ok("User data updated successfully.");
    }

    [HttpPost("admin-update")]
    public IActionResult AdminUpdate(AdminUpdateRequest request)
    {
        // Logic to update user data as an admin

        // Send notification to the admin
        _emailNotificationService.SendAdminUpdateNotification("abdullahsaddique6061@gmail.com", request.UserEmail, "Updated Content");

        return Ok("User data updated successfully by admin.");
    }
}
*/