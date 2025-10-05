using ChallengeViceri.Domain.Responses;

namespace ChallengeViceri.Application.Interfaces
{
    public interface ISuperpowerService
    {
        Task<ApiResponse<IEnumerable<SuperpowerResponse>>> ListAsync(CancellationToken cancellationToken);
    }
}


