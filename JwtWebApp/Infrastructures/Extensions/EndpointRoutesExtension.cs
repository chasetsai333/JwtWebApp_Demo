using JwtWebApp.Infrastructure.Helpers;
using System.Security.Claims;

namespace JwtWebApp.Infrastructures.Extensions
{
    public static class EndpointRoutesExtension
    {
        record LoginViewModel(string Username, string Password);

        public static void MapJwtRouter(this IEndpointRouteBuilder endpoint)
        {
            // 登入並取得 JWT Token
            endpoint.MapPost("/jwt/signin", (LoginViewModel login, JwtHelper jwt) =>
            {
                if (ValidateUser(login))
                {
                    var token = jwt.GenerateToken(login.Username);
                    return Results.Ok(new { token });
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("SignIn")
                .WithTags("Jwt")
                .AllowAnonymous();

            // 取得 JWT Token 中的所有 Claims
            endpoint.MapGet("/jwt/claims", (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Claims.Select(p => new { p.Type, p.Value }));
            })
                .WithName("Claims")
                .WithTags("Jwt")
                .RequireAuthorization();

            // 取得 JWT Token 中的使用者名稱
            endpoint.MapGet("/jwt/username", (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Identity?.Name);
            })
                .WithName("Username")
                .WithTags("Jwt")
                .RequireAuthorization();

            // 取得使用者是否擁有特定角色
            endpoint.MapGet("/jwt/isInRole", (ClaimsPrincipal user, string name) =>
            {
                return Results.Ok(user.IsInRole(name));
            })
                .WithName("IsInRole")
                .WithTags("Jwt")
                .RequireAuthorization();

            // 取得 JWT Token 中的 JWT ID
            endpoint.MapGet("/jwt/id", (ClaimsPrincipal user) =>
            {
                return Results.Ok(user.Claims.FirstOrDefault(p => p.Type == "jti")?.Value);
            })
                .WithName("JwtId")
                .WithTags("Jwt")
                .RequireAuthorization();

            static bool ValidateUser(LoginViewModel login)
            {
                return true;
            }

        }
    }
}
