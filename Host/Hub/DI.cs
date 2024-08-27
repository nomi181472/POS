namespace ConfigResource{
    public static class ConfigDI{
        public static IServiceCollection AddConfigDI(this IServiceCollection services,IConfiguration configuration){
            services.AddAuthDI(configuration);
            return services;
        }
        public static IServiceCollection AddAuthDI(this IServiceCollection services,IConfiguration configuration){
            services.AddCors();
            services.AddAuthentication().AddBearerToken();
            services.AddAuthorization();
            return services;
        }
    }
}
