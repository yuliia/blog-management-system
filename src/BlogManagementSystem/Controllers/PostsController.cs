using System;
using System.Net;
using System.Threading.Tasks;
using BlogManagementSystem.Exceptions;
using BlogManagementSystem.Mappings;
using BlogManagementSystem.Models;
using BlogManagementSystem.Repositories;
using BlogManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogManagementSystem.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public PostsController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PostResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _blogRepository.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }

            var response = post.ToResponse();

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PostResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddPost([FromBody] CreatePostRequest request)
        {
            var post = new Post(request.Title, request.Text, request.Date);

            post = await _blogRepository.AddPost(post);

            var response = post.ToResponse();

            return Ok(response);
        }

        [HttpPost]
        [Route("{id}/comment")]
        [ProducesResponseType(typeof(CommentResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] CreateCommentRequest request)
        {
            var comment = new Comment(id, request.Text, request.Date);

            comment = await _blogRepository.AddComment(comment);

            var response = comment.ToResponse();

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PostResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPosts([FromQuery]int limit = 10, [FromQuery]int offset = 0)
        {
            var posts = await _blogRepository.GetPosts(limit, offset);

            var response = posts.ToResponseList();

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                await _blogRepository.DeletePost(id);
            }
            catch (DeletingPostWithCommentsException)
            {
                return BadRequest("Can't delete post with comments");
            }

            return Ok();
        }
    }
}