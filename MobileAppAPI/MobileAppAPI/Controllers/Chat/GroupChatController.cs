using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.Services.Accounts;
using System.Security.Claims;

namespace MobileAppAPI.Controllers.Chat
{
    //[Route("groupchat")]
    //public class GroupChatController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    private static IConfiguration _configuration;
    //    public GroupChatController(ApplicationDbContext context, IConfiguration configuration)
    //    {
    //        _context = context;
    //        _configuration = configuration;

    //    }
    //    [Authorize]
    //    [HttpPost("new")]
    //    public async Task<IActionResult> NewChat()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //    [Authorize]
    //    [HttpDelete("delete")]
    //    public async Task<IActionResult> DeleteChat()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //    [Authorize]
    //    [HttpDelete("leave")]
    //    public async Task<IActionResult> LeaveChat()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //    [Authorize]
    //    [HttpDelete("adduser")]
    //    public async Task<IActionResult> AddUserToChat()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //    [Authorize]
    //    [HttpDelete("archive")]
    //    public async Task<IActionResult> ArchiveChat()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }

    //    [Authorize]
    //    [HttpDelete("loadchats")]
    //    public async Task<IActionResult> LoadChats()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //    [Authorize]
    //    [HttpDelete("loadmeta")]
    //    public async Task<IActionResult> LoadChatMetadata()
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
    //        }
    //        var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
    //        var claimsIdentity = User.Identity as ClaimsIdentity;
    //        if (claimsIdentity != null)
    //        {
    //            AccountService actsrv = new AccountService(_context, _configuration);
    //            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
    //            response = await actsrv.DeleteUser(userId);
    //        }
    //        if (!(response.Code == ResponseCode.Failure))
    //        {
    //            return Ok(response);
    //        }
    //        return BadRequest(response);
    //    }
    //}
}
