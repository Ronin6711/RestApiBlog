using RestApiBlog.Domain;

namespace RestApiBlog.Services
{
    public interface IGameService
    {
        Task<List<Game>> GetAllGamesAsync();

        Task<Game> GetGameByIdAsync(Guid gameId);

        Task<Game> GetGameByNameAsync(string gameName);

        Task<bool> CreateGameAsync(Game game);

        Task<bool> UpdateGameAsync(Game gameToUpdate);

        Task<bool> DeleteGameAsync(Guid gameId);

        Task<bool> GameNameAlreadyExists(string gameName);

    }
}
