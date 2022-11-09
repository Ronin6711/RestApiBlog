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
    public class PublicProfileController : Controller
    {
        private readonly IPublicProfileService _publicProfileService;
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;
        public PublicProfileController(IPublicProfileService publicProfileService, IMapper mapper, IGameService gameService)
        {
            _publicProfileService = publicProfileService;
            _mapper = mapper;
            _gameService = gameService;
        }

        [HttpGet(ApiRoutes.PublicProfiles.GetAll)]
        [Cached(300)]
        public async Task<IActionResult> GetAllPublicProfile()
        {
            var publicProfiles = await _publicProfileService.GetAllPublicProfileAsync();

            return Ok(_mapper.Map<List<PublicProfileResponse>>(publicProfiles));
        }

        [HttpGet(ApiRoutes.PublicProfiles.GetPublicProfile)]
        [Cached(300)]
        public async Task<IActionResult> GetPublicProfile([FromRoute] Guid publicProfileId)
        {
            var publicProfiles = await _publicProfileService.GetPublicProfileByIdAsync(publicProfileId);

            if (publicProfiles == null)
                return NotFound();
            

            return Ok(_mapper.Map<PublicProfileResponse>(publicProfiles));
        }

        [HttpPut(ApiRoutes.PublicProfiles.UpdatePublicProfile)]
        public async Task<IActionResult> UpdatePublicProfile([FromRoute] Guid publicProfileId, [FromBody] UpdatePublicProfileRequest publicProfileRequest)
        {
            var userOwnsProfile = await _publicProfileService.UserOwnsPublicProfileAsync(publicProfileId, HttpContext.GetUserId());

            if (!userOwnsProfile)
            {
                return BadRequest(new { error = "You do not own this public profile" });
            }

            var nickNameAlreadyExists = await _publicProfileService.NickNameAlreadyExists(publicProfileRequest.NickName);

            if (!nickNameAlreadyExists)
            {
                return BadRequest(new { error = "This nickname already exists, come up with another one" });
            }

            var listGames = new List<GameUser>();
            foreach (var game in publicProfileRequest.Games)
            {
                var gameInDb = await _gameService.GetGameByNameAsync(game.Name);
                if (gameInDb == null)
                {
                    return BadRequest(new { Error = $"Game '{game.Name}' does not exist in the database" });
                }
                listGames.Add(new GameUser { Game = gameInDb });
            }

            var publicProfile = await _publicProfileService.GetPublicProfileByIdAsync(publicProfileId);
            publicProfile.DateCreated = DateTime.Now;
            publicProfile.NickName = publicProfileRequest.NickName;
            publicProfile.Discord = publicProfileRequest.Discord;
            publicProfile.Avatar = publicProfileRequest.Avatar;
            publicProfile.Status = publicProfileRequest.Status;
            publicProfile.Games = listGames;


            var updated = await _publicProfileService.UpdatePublicProfileAsync(publicProfile);

            if (updated)
                return Ok(_mapper.Map<PublicProfileResponse>(publicProfile));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.PublicProfiles.DeletePublicProfile)]
        public async Task<IActionResult> DeletePublicProfile([FromRoute] Guid publicProfileId)
        {
            var userOwnsProfile = await _publicProfileService.UserOwnsPublicProfileAsync(publicProfileId, HttpContext.GetUserId());

            if (!userOwnsProfile)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var deleted = await _publicProfileService.DeletePublicProfileAsync(publicProfileId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.PublicProfiles.CreatePublicProfile)]
        public async Task<IActionResult> CreatePublicProfile([FromBody] CreatePublicProfileRequest publicProfileRequest)
        {
            var publicProfileAlreadyExists = await _publicProfileService.PublicProfileAlready(HttpContext.GetUserId());

            if (!publicProfileAlreadyExists)
            {
                return BadRequest(new { error = "You have already created a profile" });
            }

            var nickNameAlreadyExists = await _publicProfileService.NickNameAlreadyExists(publicProfileRequest.NickName);

            if (!nickNameAlreadyExists)
            {
                return BadRequest(new { error = "This nickname already exists, come up with another one" });
            }

            var newPublicProfileId = Guid.NewGuid();

            var listGames = new List<GameUser>();
            foreach (var game in publicProfileRequest.Games)
            {
                var gameInDb = await _gameService.GetGameByNameAsync(game.Name);
                if (gameInDb == null)
                {
                    return BadRequest(new { Error = $"Game '{game.Name}' does not exist in the database" });
                }
                listGames.Add(new GameUser { Game = gameInDb, UserId = newPublicProfileId });
            }

            var publicProfile = new PublicProfile()
            {
                Id = newPublicProfileId,
                DateCreated = DateTime.Now,
                NickName = publicProfileRequest.NickName,
                Discord = publicProfileRequest.Discord,
                Avatar = publicProfileRequest.Avatar,
                Status = publicProfileRequest.Status,
                Games = listGames,
                UserId = HttpContext.GetUserId()
            };

            await _publicProfileService.CreatePublicProfileAsync(publicProfile);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.PublicProfiles.GetPublicProfile.Replace("{publicProfileId}", publicProfile.Id.ToString());

            return Created(locationUri, _mapper.Map<PublicProfileResponse>(publicProfile));
        }
    }
}
