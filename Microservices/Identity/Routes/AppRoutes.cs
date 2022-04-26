using Identity.DTOs;
using Identity.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Identity.Routes
{
    public static class AppRoutes
    {
        public static void MapRoutes(WebApplication app)
        {
            app.MapPost("/SignIn", [AllowAnonymous] async (UserLoginDTO userDTO, UserManager<IdentityUser> userManager,
                ITokenService tokenService) =>
            {
                var user = await userManager.FindByNameAsync(userDTO.UserName);

                if (user is null || !await userManager.CheckPasswordAsync(user, userDTO.Password))
                    return Results.Unauthorized();

                string token = GenerateToken(app, tokenService, user);

                return Results.Ok(new
                {
                    token
                });

            }).WithName("SignIn");

            app.MapPost("/SignUp", [AllowAnonymous] async (UserLoginDTO userDTO, UserManager<IdentityUser> userManager) =>
            {
                var identityUser = new IdentityUser
                {
                    Email = userDTO.Email,
                    UserName = userDTO.UserName
                };

                var result = await userManager.CreateAsync(identityUser, userDTO.Password);

                return result.Succeeded
                    ? Results.Ok()
                    : Results.BadRequest(result.Errors);

            }).WithName("SignUp");
        }

        private static string GenerateToken(WebApplication app, ITokenService tokenService, IdentityUser? user)
        {
            return tokenService.BuildToken(app.Configuration["Jwt:Key"],
                                 app.Configuration["Jwt:Issuer"],
                                 new[] { app.Configuration["Jwt:ExpenseAud"] },
                                 user!.UserName,
                                 user.Id,
                                 TimeSpan.FromMinutes(int.Parse(app.Configuration["Jwt:Duration"])));
        }
    }
}
