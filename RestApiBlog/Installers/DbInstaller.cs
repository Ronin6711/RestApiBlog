using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestApiBlog.Data;
using RestApiBlog.Services;

namespace RestApiBlog.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DataContext>();
            services.AddControllersWithViews();

            services.AddScoped<IPostService, PostService>();
        }
    }
}
