using Microsoft.EntityFrameworkCore;
using RestApiBlog.Data;
using RestApiBlog.Domain;

namespace RestApiBlog.Services
{
    public class PublicProfileService : IPublicProfileService
    {
        private readonly DataContext _dataContext;

        public PublicProfileService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreatePublicProfileAsync(PublicProfile publicProfile)
        {
            await _dataContext.PublicProfiles.AddAsync(publicProfile);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeletePublicProfileAsync(Guid publicProfileId)
        {
            var publicProfile = await GetPublicProfileByIdAsync(publicProfileId);
            _dataContext.PublicProfiles.Remove(publicProfile);

            if (publicProfile == null)
                return false;

            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<List<PublicProfile>> GetAllPublicProfileAsync()
        {
            return await _dataContext.PublicProfiles.ToListAsync();
        }

        public async Task<PublicProfile> GetPublicProfileByIdAsync(Guid publicProfileId)
        {
            return await _dataContext.PublicProfiles.SingleOrDefaultAsync(x => x.Id == publicProfileId);
        }

        public async Task<bool> UpdatePublicProfileAsync(PublicProfile publicProfile)
        {
            _dataContext.PublicProfiles.Update(publicProfile);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UserOwnsPublicProfileAsync(Guid publicProfileId, string userId)
        {
            var publicProfile = await _dataContext.PublicProfiles.AsNoTracking().SingleOrDefaultAsync(x => x.Id == publicProfileId);

            if (publicProfile == null)
            {
                return false;
            }

            if (publicProfile.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> PublicProfileAlready(string userId)
        {
            var publicProfile = await _dataContext.PublicProfiles.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);

            if (publicProfile != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> NickNameAlreadyExists(string nickName)
        {
            var name = await _dataContext.PublicProfiles.AsNoTracking().SingleOrDefaultAsync(x => x.NickName == nickName);

            if (name != null)
            {
                return false;
            }

            return true;
        }
    }
}
