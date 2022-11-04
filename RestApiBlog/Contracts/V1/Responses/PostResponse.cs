namespace RestApiBlog.Contracts.V1.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public string UserId { get; set; }

        public IEnumerable<GameUsersResponse>? Games { get; set; }

    }
}
