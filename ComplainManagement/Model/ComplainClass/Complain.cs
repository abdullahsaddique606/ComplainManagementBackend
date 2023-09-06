using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplainManagement.Model.ComplainClass;
public class Complain
{
    [Key]
    public int ComplaintId { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Type { get; set; } = null!;


    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public string Status { get; set; }= null!;
}
