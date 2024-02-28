using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

namespace CommerceApp.ConfigService
{
    public static class ConfigServiceExtension
    {
        public static IServiceCollection ConfigService(this IServiceCollection services)
        {
            services.AddHttpLogging(log =>
            {
                log.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
                log.RequestBodyLogLimit = 4096;
            });
            services.AddAntiforgery(opt =>
            {
                opt.SuppressXFrameOptionsHeader = false;
                opt.Cookie = new CookieBuilder()
                {
                    HttpOnly = true, SameSite = SameSiteMode.Lax,
                    Expiration = TimeSpan.FromDays(7)
                };
                opt.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
            });
            // Add compression config
            services.AddResponseCompression(compress =>
            {
                compress.Providers.Add<BrotliCompressionProvider>();
                compress.Providers.Add<GzipCompressionProvider>();
            });
            // Add DB config

            // Add some more service config
            return services;
        }
    }
}
