using System.ComponentModel.DataAnnotations;

namespace ComplainManagement.Model.Authentication.Signup;

public class RegisterUser
{
    [Required(ErrorMessage = "Field is required")]
    public string? UserName { get; set; }
    [EmailAddress]
    [Required(ErrorMessage = "Field is required")]
    public string? EmailAddress { get; set; }
    [Required(ErrorMessage = "Field is required")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid contact number")]
    public string? PhoneNumber { get; set; }
    [Required(ErrorMessage = "Field is required")]
    public string? Password { get; set; }
}
