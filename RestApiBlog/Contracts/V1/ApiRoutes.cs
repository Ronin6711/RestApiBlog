namespace RestApiBlog.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAllPosts = Base + "/posts";

            public const string CreatePost = Base + "/posts";

            public const string GetPost = Base + "/posts/{postId}";

            public const string UpdatePost = Base + "/posts/{postId}";

            public const string DeletePost = Base + "/posts/{postId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";

        }

        public static class Games
        {
            public const string GetAll = Base + "/games";

            public const string CreateGame = Base + "/games";

            public const string GetGame = Base + "/games/{gameId}";

            public const string UpdateGame = Base + "/games/{gameId}";

            public const string DeleteGame = Base + "/games/{gameId}";
        }

        public static class PublicProfiles
        {
            public const string GetAll = Base + "/publicProfiles";

            public const string CreatePublicProfile = Base + "/publicProfiles";

            public const string GetPublicProfile = Base + "/publicProfiles/{publicProfileId}";

            public const string UpdatePublicProfile = Base + "/publicProfiles/{publicProfileId}";

            public const string DeletePublicProfile = Base + "/publicProfiles/{publicProfileId}";
        }
    }
}
