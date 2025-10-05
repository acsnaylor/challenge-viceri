using ChallengeViceri.Application.Interfaces;
using ChallengeViceri.Application.Services;

namespace ChallengeViceri.Api.Extensions
{
    public static class DependencyInjectionApplication
    {
        public static IServiceCollection AddDIApplication(this IServiceCollection services)
        {
            services.AddTransient<IHeroService, HeroService>();
            services.AddTransient<ISuperpowerService, SuperpowerService>();
            return services;
        }
    }
}


