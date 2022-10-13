using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RestApiBlog.Contracts.V1;
using RestApiBlog.Contracts.V1.Requests;
using RestApiBlog.Contracts.V1.Responses;
using RestApiBlog.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestApiBlog.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient _testClient;
        private readonly IServiceProvider _serviceProvider;
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Program>()
                 .WithWebHostBuilder(builder =>
                 {
                     builder.ConfigureServices(services =>
                     {
                         services.RemoveAll(typeof(DbContextOptions<DataContext>));
                         services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                     });
                 });
            _serviceProvider = appFactory.Services;
            _testClient = appFactory.CreateClient();
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }

        protected async Task AuthenticateAsync()
        {
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Posts.CreatePost, request);
            return await response.Content.ReadAsAsync<PostResponse>();
        }

        private async Task<string> GetJwtAsync()
        {

            var response = await _testClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test@gmail.com",
                Password = "Pass123!"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
