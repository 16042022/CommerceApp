using Microsoft.AspNetCore.Antiforgery;

namespace CommerceApp.ConfigMiddleware
{
    public class SendFortegyTokenToClient
    {
        private readonly RequestDelegate next;
        public SendFortegyTokenToClient (RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke (HttpContext context,
            IAntiforgery antiforgery)
        {
           var requestPath = context.Request.Path.Value;
           if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
                || string.Equals(requestPath, "/index.html",
             StringComparison.OrdinalIgnoreCase))
            {
              var tokenSet = antiforgery.GetAndStoreTokens(context);
              context.Response.Cookies.Append("XSRF-TOKEN",
              tokenSet.RequestToken!,
              new CookieOptions { HttpOnly = false });
           }
           await next(context);
        }
    }

    public static class GenerateFortegyToken
    {
        public static IApplicationBuilder GetFortegyToken(this IApplicationBuilder app)
        {
            app.UseMiddleware<SendFortegyTokenToClient>();
            return app;
        }
    }
}
