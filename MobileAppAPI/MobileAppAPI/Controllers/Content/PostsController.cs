using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using MobileAppAPI.DBModels;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection;
using System;
using MobileAppAPI.ControllerModels;
using Microsoft.AspNetCore.Authorization;
using MobileAppAPI.DBModels.Content;
using MobileAppAPI.ControllerModels.Content.Input;
using MobileAppAPI.ControllerModels.Content.Response;

namespace MobileAppAPI.Controllers.Content
{

    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddPost([FromBody] ControllerModels.Content.Input.PostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid post data.");
            }

            // Map model data to Post entity
            var post = new DBModels.Content.PostModel
            {
                UserId = model.UserId,
                PostLocation = model.PostLocation,
                PostDateTime = DateTime.UtcNow,
                PostText = model.PostText,
                PostImage = model.PostImage
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return Ok(new { Message = "Post added successfully!" });
        }

        [HttpDelete("{postid}")]
        [Authorize]
        public IActionResult DeletePost(int postid)
        {
            var post = _context.Posts.Find(postid);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return Ok(new { Message = "Post deleted successfully!" });
        }

        [HttpGet("{postid}")]
        [Authorize]
        public IActionResult GetPost(int postid)
        {
            var post = _context.Posts.Find(postid);
            if (post == null)
            {
                return NotFound();
            }

            // Map Post entity to PostResponseModel
            var postResponse = new PostResponseModel
            {
                PostDateTime = post.PostDateTime,
                PostText = post.PostText,
                PostImage = post.PostImage
            };

            return Ok(postResponse);
        }
    }






}
