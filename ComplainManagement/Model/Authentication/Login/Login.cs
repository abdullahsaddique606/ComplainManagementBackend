using System.ComponentModel.DataAnnotations;

namespace ComplainManagement.Model.Authentication.Login;

public class Login
{
    [Required(ErrorMessage = "Field is required")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Field is required")]
    public string? Password { get; set; }
}
