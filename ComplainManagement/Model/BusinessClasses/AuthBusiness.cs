using ComplainManagement.Model.Authentication.Login;
using ComplainManagement.Model.Authentication.Signup;
using ComplainMgt.Services.Model;
using ComplainMgt.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComplainManagement.Model.BusinessClasses;
public interface IAuthBusiness
{
    Task<IActionResult> RegisterUser(RegisterUser registerUser, string role);
    Task<IActionResult> Delete(string id);
    Task<IEnumerable<IdentityUser>> GetUsers();
    Task<IActionResult> ConfirmEmail(string token, string email);
    Task<IActionResult> LoginUser(Login login);
    Task<IActionResult> UpdateUserData(RegisterUser registerUser, string id);
}

public class AuthBusiness : IAuthBusiness
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailServices _emailService;
    private readonly IConfiguration _configuration;
    public AuthBusiness(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailServices emailService)
    {
        _userManager = userManager;

        _roleManager = roleManager;

        _emailService = emailService;
        _configuration = configuration;

    }
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var userExist = await _userManager.FindByEmailAsync(email);
        if (userExist != null)
        {
            var confirmation = await _userManager.ConfirmEmailAsync(userExist, token);
            if (confirmation != null)
            {
                return new ObjectResult(new Response
                {
                    Status = "Success",
                    Message = "Email Verified"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
        return new ObjectResult(new Response
        {
            Status = "Fail",
            Message = "User Not found"
        })
        {
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new ObjectResult(new Response
            {
                Status = "Failed",
                Message = "User does not exist"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
        else if (user != null)
        {

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ObjectResult(new Response
                {
                    Status = "Success",
                    Message = "USer deleted"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                return new ObjectResult(new Response
                {
                    Status = "Failed",
                    Message = "User cannot Deleted"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        else
        {
            return new ObjectResult(new Response
            {
                Status = "Failed",
                Message = "Only admin can delete"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }


    public async Task<IEnumerable<IdentityUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }


    public async Task<IActionResult> RegisterUser(RegisterUser registerUser, string role)
    {
        var UserExists = await _userManager.FindByEmailAsync(registerUser.EmailAddress);
        if (UserExists == null)
        {
            IdentityUser newUser = new()
            {
                Email = registerUser.EmailAddress,
                UserName = registerUser.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = registerUser.PhoneNumber
            };
            if (await _roleManager.RoleExistsAsync(role))
            {
                var createUser = await _userManager.CreateAsync(newUser, registerUser.Password);
                if (createUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, role);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = "https://localhost:7163/api/Authentication/ConfirmEmail" +
                               $"?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(newUser.Email!)}";
                    
                    var message = new Message(new string[] { newUser.Email! }, "Email Confirmation", confirmationLink!);
                    _emailService.SendEmail(message);
                    return new ObjectResult(new Response
                    {
                        Status = "Success",
                        Message = "User created, Login Now"
                    })
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    // Log the error details to diagnose the issue
                  /*  foreach (var error in createUser.Errors)
                    {
                        // You can log the error messages here or use a logging library
                        Console.WriteLine($"Error: {error.Code}, Description: {error.Description}");
                    }*/

                    return new ObjectResult(new Response
                    {
                        Status = "Failed",
                        Message = "UserName is not available try other!"
                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
                
            }
            return new ObjectResult(new Response
            {
                Status = "Failed",
                Message = "Role does not exist"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        return new ObjectResult(new Response
        {
            Status = "Fail",
            Message = "Email already exists"
        })
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
    public async Task<IActionResult> LoginUser(Login login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        /*var userID = await _userManager.FindByIdAsync(login.ID);*/

        if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = GetToken(authClaims);
            var userEmail = await _userManager.GetEmailAsync(user);

            return new OkObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo,
                Status = "Success",
                Message = "Login Successfully",
                userName= user.UserName,
                userID = user.Id,
                role = roles,
                userEmail = userEmail,

            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        return new ObjectResult(new Response
        {
            Status = "Fail",
            Message = "Invalid login credentials"
        })
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidAudience"],
            audience: _configuration["JWT:ValidIssuer"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: credentials
            );
        return token;
    }
    public async Task<IActionResult> UpdateUserData(RegisterUser registerUser, string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "Invalid credentials, No user exist"
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        user.UserName = registerUser.UserName;
        user.Email = registerUser.EmailAddress;
        var updateResult = await _userManager.UpdateAsync(user);

        if (updateResult.Succeeded)
        {

            return new ObjectResult(new Response
            {
                Status = "Success",
                Message = "User data updated successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        else
        {
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "Failed to update user data"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

    }

}
