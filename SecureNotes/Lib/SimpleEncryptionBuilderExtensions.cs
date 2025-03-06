namespace Simple.Encryption
{
    public static class SimpleEncryptionBuilderExtensions
    {
        public static IServiceCollection AddSimpleEncryption(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<EncryptionOptions>().Bind(config.GetSection("Simple:Encryption"));
            services.AddSingleton<IEncryption, Encryption>();

            return services;
        }
    }
}