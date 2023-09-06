using ComplainManagement.Model.ComplainClass;
using ComplainMgt.Services.Model;
using ComplainMgt.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplainManagement.Model.BusinessClasses;
public interface IComplainBusiness
{
    Task<IActionResult> AddComplain(Complain complain);
    Task<IActionResult> UpdateComplain(int id, Complain updatedComplain, string role);
    Task<IActionResult> GetComplainsForUser(string userId);
    Task<IActionResult> GetAllComplains();
    Task<IActionResult> DeleteComplain(int id);
    Task<IActionResult> GetAllUsers();
    Task<IActionResult> SearchComplain(int filter, string searchQuery);
}

public class ComplainBusiness :IComplainBusiness
{
    private readonly ApplicationDBContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailServices _emailService;
    private readonly IEmailNotificationService _emailNotificationService;

    public ComplainBusiness(ApplicationDBContext context, UserManager<IdentityUser> userManager, IEmailServices emailServices, IEmailNotificationService emailNotificationService)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailServices;
        _emailNotificationService =emailNotificationService;
}
   
    public async Task<IActionResult> AddComplain([FromBody] Complain complain)
    {
        if (complain!=null)

        {
            var user = await _userManager.FindByIdAsync(complain.UserId);
            if (user == null)
            {
                return new ObjectResult(new Response
                {
                    Status = "Fail",
                    Message = "No user Found"
                })
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            // status for the new complaint
            complain.Status = "New"; 

            _context.Complains.Add(complain); 
            _emailNotificationService.SendAdminComplainAddedNotification("abdullahsaddique6061@gmail.com", user.Email, complain.Type, complain.Description, complain.Status);
            await _context.SaveChangesAsync();
            return new ObjectResult(new Response
            {
                Status = "Success",
                Message = "Complain added successfully."
            })
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        return new ObjectResult(new Response
        {
            Status = "Fail",
            Message = "Complain cannot added."
        })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
    public async Task<IActionResult> UpdateComplain(int id, Complain updatedComplain,string role)
    {
        var complain = await _context.Complains.FirstOrDefaultAsync(c => c.ComplaintId == id);

        if (complain == null)
        {
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "No complain Found"
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        var originalStatus = complain.Status; // Store the original status for comparison

        // Update the complain's properties with the new values
        complain.Title = updatedComplain.Title;
        complain.Type = updatedComplain.Type;
        complain.Description = updatedComplain.Description;
        complain.Status = updatedComplain.Status;

        //Context Attaching
        /*_context.Complains.Attach(updatedComplain);
        _context.Entry(updatedComplain).State = EntityState.Modified;*/

        // Save changes to persist the updates
        _context.SaveChanges();

        try
        {
            await _context.SaveChangesAsync();
            // Fetch user's email using the userID associated with the complain
            var user = await _userManager.FindByIdAsync(complain.UserId);
            if (role =="Admin")
            {
                // Send user for admin update notification
                _emailNotificationService.SendUserUpdateNotification(user.Email, "abdullahsaddique6061@gmail.com", updatedComplain.Type, updatedComplain.Description, updatedComplain.Status);
               
            }
            else
            {
                // Send admin for user update notification
                _emailNotificationService.SendAdminUpdateNotification("abdullahsaddique6061@gmail.com", user.Email, updatedComplain.Type, updatedComplain.Description, updatedComplain.Status);
            }

            return new ObjectResult(new Response
            {
                Status = "Success",
                Message = "Complain updated successfully."
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        catch (DbUpdateException)
        {
            // Handle update errors
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "Complain cannot be updated."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }


    public async Task<IActionResult> GetComplainsForUser(string userId)
    {
        var complains = await _context.Complains.Where(c => c.UserId == userId).ToListAsync();

        if (complains.Count == 0)
        {
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "No complaints found for the user."
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        return new ObjectResult(complains)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }
    [HttpGet("GetAllComplains")]
    public async Task<IActionResult> GetAllComplains()
    {
       var complaints = await _context.Complains.ToListAsync();
        return new ObjectResult(complaints)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }
    
    public async Task<IActionResult> DeleteComplain(int id)
    {
        var complainID = await _context.Complains.FindAsync(id);
        if (complainID != null)
        {
            _context.Complains.Remove(complainID);
            _context.SaveChanges();
            return new ObjectResult(new Response
            {
                Status = "Success",
                Message = "User Deleted Successfully"
            })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
        return new ObjectResult(new Response
        {
            Status = "Failed",
            Message = "ID does not exist"
        })
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }
    
    public async Task<IActionResult> SearchComplain(int filter,string searchQuery)
    {
        var query = _context.Complains.AsQueryable();

        if (filter == 1)
        {
            query = query.Where(c => c.Type.Contains(searchQuery));
        }
        else if (filter == 2)
        {
            query = query.Where(c => c.Status.Contains(searchQuery));
        }
        else
        {
            return new ObjectResult(new Response
            {
                Status = "Fail",
                Message = "Complain not found."
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        var result = await query.ToListAsync();

        return new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.Select(user => new { Id = user.Id, Email = user.Email }).ToListAsync();

        return new ObjectResult(users)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

}

