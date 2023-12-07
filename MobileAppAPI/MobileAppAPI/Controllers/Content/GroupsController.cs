using Microsoft.AspNetCore.Mvc;

namespace MobileAppAPI.Controllers.Content
{
    public class GroupsController : Controller
    {
        //    [HttpPost]
        //    [Authorize] 
        //    public IActionResult CreateGroup([FromBody] GroupCreateRequest request)
        //    {
        //        
        //        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        //        if (userIdClaim == null)
        //        {
        //            return Unauthorized();
        //        }
        //        request.FounderUserId = int.Parse(userIdClaim.Value);

        //      
        //        var createdGroup = _groupService.CreateGroup(request);


        //        if (createdGroup != null)
        //        {
        //            return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id }, createdGroup);
        //        }
        //        return BadRequest();
        //    }

    }
}
