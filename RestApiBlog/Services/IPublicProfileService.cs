using RestApiBlog.Domain;

namespace RestApiBlog.Services
{
    public interface IPublicProfileService
    {
        Task<List<PublicProfile>> GetAllPublicProfileAsync();

        Task<PublicProfile> GetPublicProfileByIdAsync(Guid publicProfileId);

        Task<bool> CreatePublicProfileAsync(PublicProfile publicProfile);

        Task<bool> UpdatePublicProfileAsync(PublicProfile publicProfile);

        Task<bool> DeletePublicProfileAsync(Guid publicProfileId);

        Task<bool> UserOwnsPublicProfileAsync(Guid publicProfileId, string userId);

        Task<bool> PublicProfileAlready(string userId);

        Task<bool> NickNameAlreadyExists(string nickName);
    }
}
