using RestApiBlog.Domain;

namespace RestApiBlog.Contracts.V1.Responses
{
    public class PublicProfileResponse
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string NickName { get; set; }

        public string? Discord { get; set; }

        public string? Avatar { get; set; }

        public string Status { get; set; }

        public IEnumerable<GameUsersResponse> Games { get; set; }
    }
}
