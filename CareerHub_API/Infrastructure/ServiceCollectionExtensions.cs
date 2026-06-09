using CareerHub_API.Services;
using CareerHub_API.Repositories; // if you have repository interfaces/implementations
using Microsoft.Extensions.DependencyInjection;

namespace CareerHub_API.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        // ------------------------------
        // Auth feature DI
        // ------------------------------
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAuthService, AuthService>();

            // Repositories (if any for Auth, e.g., user repo)
            // services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        // ------------------------------
        // Job feature DI
        // ------------------------------
        public static IServiceCollection AddJobServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IJobListingService, JobListingService>();

            // Repositories
            services.AddScoped<IJobListingRepository, JobListingRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            return services;
        }

        // ------------------------------
        // Applications feature DI
        // ------------------------------
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IApplicationService, ApplicationService>();

            // Repositories
            services.AddScoped<IApplicationRepository, ApplicationRepository>();

            return services;
        }

        
    }
}