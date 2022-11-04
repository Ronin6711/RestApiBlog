namespace RestApiBlog.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        public string Header { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public virtual List<AddedGameRequest>? Games { get; set; }
    }
}
