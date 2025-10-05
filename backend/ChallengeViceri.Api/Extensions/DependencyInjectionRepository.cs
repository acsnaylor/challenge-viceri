using ChallengeViceri.Infrastructure.Data.Repositories;

namespace ChallengeViceri.Api.Extensions
{
    public static class DependencyInjectionRepository
    {
        public static IServiceCollection AddDIRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

            return services;
        }
    }
}