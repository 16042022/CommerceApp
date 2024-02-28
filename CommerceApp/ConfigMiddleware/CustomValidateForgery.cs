using Microsoft.AspNetCore.Antiforgery;

namespace CommerceApp.ConfigMiddleware
{
    public class CustomValidateForgery
    {
        private readonly RequestDelegate next;
        public CustomValidateForgery (RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke (HttpContext context, IAntiforgery antiforgery)
        {
            bool isGetRequest = context.Request.Method == "GET";
            if (!isGetRequest)
            {
                await antiforgery.ValidateRequestAsync (context);
            }
            await next(context);
        }
    }

    public static class ValidatingForgeryToken
    {
        public static IApplicationBuilder CustomForgeryValidate (this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomValidateForgery>();
            return app;
        }
    } 
}
