namespace CommerceApp.ConfigMiddleware
{
    public class SecurityExtension
    {
        private readonly RequestDelegate next;
        public SecurityExtension (RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            // control which resources are allowed to be loaded by the browser
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
            // only communicate with the server over HTTPS
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
            // prevent the browser from MIME-sniffing a response away from the declared content type
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            // prevent clickjacking attacks
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            await next(context);
        }
    }
    
    public static class ExtensionSecurity
    {
        public static IApplicationBuilder SecurityMiddleware (this IApplicationBuilder app)
        {
            app.UseMiddleware<SecurityExtension>();
            return app;
        }
    }
}
