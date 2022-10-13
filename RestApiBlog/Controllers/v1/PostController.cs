using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiBlog.Contracts.V1;
using RestApiBlog.Contracts.V1.Requests;
using RestApiBlog.Contracts.V1.Responses;
using RestApiBlog.Domain;
using RestApiBlog.Extensions;
using RestApiBlog.Services;

namespace RestApiBlog.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
           _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAllPosts)]
        //[Authorize(Policy = "ViewerAllPost")]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [HttpGet(ApiRoutes.Posts.GetPost)]
        public async Task<IActionResult> GetPost([FromRoute]Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if(post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPut(ApiRoutes.Posts.UpdatePost)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if(!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post"});
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);

            if(updated)
                return Ok(post);

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.DeletePost)]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.CreatePost)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post
            {
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId()
            };

            if (post.Id != Guid.Empty)
                post.Id = Guid.NewGuid();

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.GetPost.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };

            return Created(locationUri, response);
        }
    }
}
