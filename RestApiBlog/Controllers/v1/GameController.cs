using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiBlog.Cache;
using RestApiBlog.Contracts.V1;
using RestApiBlog.Contracts.V1.Requests;
using RestApiBlog.Contracts.V1.Responses;
using RestApiBlog.Domain;
using RestApiBlog.Services;

namespace RestApiBlog.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, DefaultUser")]
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Games.GetAll)]
        [Cached(300)]
        public async Task<IActionResult> GetAll()
        {
            var games = await _gameService.GetAllGamesAsync();

            return Ok(_mapper.Map<List<GameResponse>>(games));
        }

        [HttpGet(ApiRoutes.Games.GetGame)]
        [Cached(300)]
        public async Task<IActionResult> GetGame([FromRoute] Guid gameId)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);

            if (game == null)
                return NotFound();

            return Ok(_mapper.Map<GameResponse>(game));
        }

        [HttpPut(ApiRoutes.Games.UpdateGame)]
        public async Task<IActionResult> UpdateGame([FromRoute] Guid gameId, [FromBody] UpdateGameRequest gameRequest)
        {
            var game = await _gameService.GetGameByIdAsync(gameId);
            game.Name = gameRequest.Name;
            game.Image = gameRequest.Image;

            var gameNameAlreadyExists = await _gameService.GameNameAlreadyExists(gameRequest.Name);

            if (!gameNameAlreadyExists)
            {
                return BadRequest(new { error = $"{gameRequest.Name} name already exists" });
            }

            var updated = await _gameService.UpdateGameAsync(game);

            if (updated)
                return Ok(_mapper.Map<GameResponse>(game));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Games.DeleteGame)]
        [Authorize(Policy = "Developer")]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid gameId)
        {
            var deleted = await _gameService.DeleteGameAsync(gameId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.Games.CreateGame)]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameRequest gameRequest)
        {
            var gameNameAlreadyExists = await _gameService.GameNameAlreadyExists(gameRequest.Name);

            if (!gameNameAlreadyExists)
            {
                return BadRequest(new { error = $"{gameRequest.Name} name already exists" });
            }

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = gameRequest.Name,
                Image = gameRequest.Image
            };

            await _gameService.CreateGameAsync(game);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Games.GetGame.Replace("{gameId}", game.Id.ToString());

            return Created(locationUri, _mapper.Map<GameResponse>(game));
        }

    }
}
