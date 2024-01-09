using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileAppAPI.ControllerModels.Accounts.Input;
using MobileAppAPI.ControllerModels.Chat.Input;
using MobileAppAPI.ControllerModels.Chat.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Chat;
using MobileAppAPI.Services.Accounts;
using MobileAppAPI.Services.Chat;
using System.Security.Claims;

namespace MobileAppAPI.Controllers.Chat
{
    [Route("directchat")]
    public class DirectChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;
        public DirectChatController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        [Authorize]
        [HttpPost("new")]
        public async Task<IActionResult> NewChat([FromBody] DirectChatModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
            }
            var response = GeneralResponseModel<ChatSessionResponseModel>.ErrorResponse("Failed to create chat session");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await chatsrv.CreateSession(userId, model.RecipientID);
            }
            if (response.Code == ResponseCode.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteChat([FromBody] DirectChatModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
            }
            var response = GeneralResponseModel<ChatSessionResponseModel>.ErrorResponse("Failed to delete chat session");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await chatsrv.DeleteSession(userId, model.SessionID);
            }
            if (response.Code == ResponseCode.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //[Authorize]
        //[HttpDelete("leave")]
        //public async Task<IActionResult> LeaveChat([FromBody] DirectChatModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
        //    }
        //    var response = GeneralResponseModel<ChatSessionResponseModel>.ErrorResponse("Failed to leave chat session");
        //    var claimsIdentity = User.Identity as ClaimsIdentity;
        //    if (claimsIdentity != null)
        //    {
        //        DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
        //        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        //        response = await chatsrv.LeaveSession(userId, model.SessionID);
        //    }
        //    if (response.Code == ResponseCode.Success)
        //    {
        //        return Ok(response);
        //    }
        //    return BadRequest(response);
        //}

        //[Authorize]
        //[HttpPost("adduser")]
        //public async Task<IActionResult> AddUserToChat([FromBody] DirectChatModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
        //    }
        //    var response = GeneralResponseModel<ChatSessionResponseModel>.ErrorResponse("Failed to add user to chat session");
        //    var claimsIdentity = User.Identity as ClaimsIdentity;
        //    if (claimsIdentity != null)
        //    {
        //        DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
        //        var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
        //        response = await chatsrv.AddUserToSession(userId, model.ChatID, model.RecipientID);
        //    }
        //    if (response.Code == ResponseCode.Success)
        //    {
        //        return Ok(response);
        //    }
        //    return BadRequest(response);
        //}

        [Authorize]
        [HttpPost("archive")]
        public async Task<IActionResult> ArchiveChat([FromBody] DirectChatModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
            }
            var response = GeneralResponseModel<ParticipantModel>.ErrorResponse("Failed to archive chat session");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await chatsrv.ArchiveSession(userId, model.SessionID);
            }
            if (response.Code == ResponseCode.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("loadchats")]
        public async Task<IActionResult> LoadChats()
        {
            var response = GeneralResponseModel<List<ChatSessionResponseModel>>.ErrorResponse("Failed to load chat sessions");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await chatsrv.LoadSessions(userId);
            }
            if (response.Code == ResponseCode.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("loadmeta")]
        public async Task<IActionResult> LoadChatMetadata([FromBody] DirectChatModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GeneralResponseModel<object>.ErrorResponse("Invalid request"));
            }
            var response = GeneralResponseModel<ChatSessionResponseModel>.ErrorResponse("Failed to load chat session metadata");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                DirectChatSessionService chatsrv = new DirectChatSessionService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await chatsrv.LoadSessionMetadata(userId, model.SessionID);
            }
            if (response.Code == ResponseCode.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
