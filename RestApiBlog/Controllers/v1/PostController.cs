using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiBlog.Cache;
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
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;

        public PostController(IPostService postService, IMapper mapper, IGameService gameService)
        {
            _postService = postService;
            _mapper = mapper;
            _gameService = gameService;
        }

        [HttpGet(ApiRoutes.Posts.GetAllPosts)]
        [Cached(300)]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postService.GetPostsAsync();

            return Ok(_mapper.Map<List<PostResponse>>(posts));
        }

        [HttpGet(ApiRoutes.Posts.GetPost)]
        [Cached(300)]
        public async Task<IActionResult> GetPost([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPut(ApiRoutes.Posts.UpdatePost)]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post"});
            }

            var listGames = new List<PostGame>();
            foreach (var game in request.Games)
            {
                var gameInDb = await _gameService.GetGameByNameAsync(game.Name);
                if (gameInDb == null)
                {
                    return BadRequest(new { Error = $"Game '{game.Name}' does not exist in the database" });
                }
                listGames.Add(new PostGame { Game = gameInDb });
            }

            var post = await _postService.GetPostByIdAsync(postId);
            post.DateCreated = DateTime.Now;
            post.Header = request.Header;
            post.Description = request.Description;
            post.Image = request.Image;
            post.Games = listGames;


            var updated = await _postService.UpdatePostAsync(post);

            if(updated)
                return Ok(_mapper.Map<PostResponse>(post));

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
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var newPostId = Guid.NewGuid();

            var listGames = new List<PostGame>();
            foreach (var game in request.Games)
            {
                var gameInDb = await _gameService.GetGameByNameAsync(game.Name);
                if (gameInDb == null)
                {
                    return BadRequest(new { Error = $"Game '{game.Name}' does not exist in the database" });
                }
                listGames.Add(new PostGame { Game = gameInDb, PostId = newPostId });
            }

            var post = new Post
            {
                Id = newPostId,
                DateCreated = DateTime.Now,
                Header = request.Header,
                Description = request.Description,
                Image = request.Image,
                Games = listGames,
                UserId = HttpContext.GetUserId(),
            };

            await _postService.CreatePostAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.GetPost.Replace("{postId}", post.Id.ToString());

            return Created(locationUri, _mapper.Map<PostResponse>(post));
        }
    }
}
