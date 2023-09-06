using ComplainManagement.Model.BusinessClasses;
using ComplainManagement.Model.ComplainClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplainManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComplainController : ControllerBase
{
    private readonly IComplainBusiness _complainBusiness;
    public ComplainController(IComplainBusiness complainBusiness)
    {
        _complainBusiness = complainBusiness;
    }
    [HttpPost("AddComplains")]
    public async  Task <IActionResult> AddComplains(Complain complain){
        return await _complainBusiness.AddComplain(complain);
    }
    [HttpPut("UpdateUser/{id}")]
    public async Task <IActionResult> UpdateComplain(int id, Complain updatedComplain, string role)
    {
        return await _complainBusiness.UpdateComplain(id,updatedComplain,role);
    }
    [HttpGet("GetComplainForUser/{userId}")]
    public async Task<IActionResult> GetComplainForUser(string userId)
    {
        return await _complainBusiness.GetComplainsForUser(userId);
    }
    [HttpGet("GetAllComplains")]
    public async Task<IActionResult> GetAllComplains()
    {
        return await _complainBusiness.GetAllComplains();
    }
    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteComplains(int id)
    {
        return await _complainBusiness.DeleteComplain(id);

    }
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        return await _complainBusiness.GetAllUsers();
    }
    [HttpGet("SearchComplain")]
    public async Task<IActionResult> SearchComplain(int filter,string searchQuery)
    {
        var result = await _complainBusiness.SearchComplain(filter,searchQuery);
        return result;
    }
}

