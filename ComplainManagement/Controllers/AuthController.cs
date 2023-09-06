using ComplainManagement.Model.Authentication.Login;
using ComplainManagement.Model.Authentication.Signup;
using ComplainManagement.Model.BusinessClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComplainManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    
    private readonly IAuthBusiness _userManagementService;

    public AuthController(IAuthBusiness userManagementService)
    {
        _userManagementService = userManagementService;
    }

    [HttpPost("UserRegisteration")]
    public async Task<IActionResult> Register(RegisterUser registerUser, string role)
    {
        return await _userManagementService.RegisterUser(registerUser, role);
    }
    [HttpDelete("api/DeleteUser")]
    public async Task<IActionResult> Delete(string id)
    {
        return await _userManagementService.Delete(id);
    }

    [HttpGet("ConfirmEmail")]
    public Task<IActionResult> ConfirmEmail(string token, string email)
    {
        return _userManagementService.ConfirmEmail(token, email);
    }
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(Login login)
    {
        return await _userManagementService.LoginUser(login);
    }
    [HttpPatch("updateUserRecord")]
    public Task<IActionResult> UpdateUser(RegisterUser registerUser, string id)
    {
        return _userManagementService.UpdateUserData(registerUser,id);

    }

}
