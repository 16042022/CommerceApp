using CommerceCore.Application.Feature.Shop.Command;
using CommerceCore.Application.Interface;
using CommerceInfra.DBProvider.NoSQLProvider;
using CommerceInfra.Service.Authorization;
using CommerceInfra.Service.DBService;
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
            services.AddScoped(typeof(ISQLService<>), typeof(MongoShopService<>));
            services.AddTransient(typeof(INoSQLProvider<,>), typeof(MongoProvider<,>));
            // Add some more service config
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssemblies(typeof(CreateShopCommand).Assembly);
            });
            services.AddTransient(typeof(IAuthService<,>), typeof(JWTAuthService<,>));
            return services;
        }
    }
}
