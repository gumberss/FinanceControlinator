using Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Extensions
{
    public static class IdentityExtension
    {
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                        .AddEntityFrameworkStores<IdentityAppDbContext>();

            builder.Services.AddDbContext<IdentityAppDbContext>(
                         options => options
                         .UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection")));
        }
    }
}
