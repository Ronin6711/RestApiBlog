using RestApiBlog.Domain;

namespace RestApiBlog.Contracts.V1.Requests
{
    public class UpdatePublicProfileRequest
    {
        public string NickName { get; set; }

        public string? Discord { get; set; }

        public string? Avatar { get; set; }

        public string Status { get; set; }

        public virtual List<AddedGameRequest> Games { get; set; }
    }
}
