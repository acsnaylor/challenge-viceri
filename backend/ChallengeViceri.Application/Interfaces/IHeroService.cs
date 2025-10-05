using ChallengeViceri.Domain.Requests;
using ChallengeViceri.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeViceri.Application.Interfaces
{
    public interface IHeroService
    {
        Task<ApiResponse<HeroResponse>> AddHeroAsync(HeroRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<HeroResponse>>> ListHeroesAsync(CancellationToken cancellationToken);
        Task<ApiResponse<HeroResponse>> GetHeroByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<HeroResponse>> UpdateHeroAsync(int id, HeroRequest request, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteHeroAsync(int id, CancellationToken cancellationToken);
    }
}

