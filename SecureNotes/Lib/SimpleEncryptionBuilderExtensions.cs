using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Simple.Encryption
{
    public static class SimpleEncryptionBuilderExtensions
    {
        public static IServiceCollection AddHielkoEncryption(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<EncryptionOptions>().Bind(config.GetSection("Simple:Encryption"));
            services.AddSingleton<IEncryption, Encryption>();

            return services;
        }
    }
}
