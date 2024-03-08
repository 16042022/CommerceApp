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
