using Microsoft.EntityFrameworkCore;
using RestApiBlog.Data;
using RestApiBlog.Domain;

namespace RestApiBlog.Services
{
    public class GameService : IGameService
    {
        private readonly DataContext _dataContext;

        public GameService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateGameAsync(Game game)
        {
            await _dataContext.Games.AddAsync(game);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteGameAsync(Guid gameId)
        {
            var game = await GetGameByIdAsync(gameId);
            _dataContext.Games.Remove(game);

            if (game == null)
                return false;

            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> GameNameAlreadyExists(string gameName)
        {
            var game = await _dataContext.Games.AsNoTracking().SingleOrDefaultAsync(x => x.Name == gameName);

            if (game != null)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _dataContext.Games.ToListAsync();
        }

        public async Task<Game> GetGameByIdAsync(Guid gameId)
        {
            return await _dataContext.Games.SingleOrDefaultAsync(x => x.Id == gameId);
        }

        public async Task<Game> GetGameByNameAsync(string gameName)
        {
            return await _dataContext.Games.SingleOrDefaultAsync(x => x.Name == gameName);
        }

        public async Task<bool> UpdateGameAsync(Game gameToUpdate)
        {
            _dataContext.Games.Update(gameToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }
    }
}
