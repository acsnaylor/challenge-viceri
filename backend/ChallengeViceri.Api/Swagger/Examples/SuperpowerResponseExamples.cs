using System.Net;
using ChallengeViceri.Domain.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace ChallengeViceri.Api.Swagger.Examples
{
    public class ApiSuperpowersListResponseExample : IExamplesProvider<ApiResponse<IEnumerable<SuperpowerResponse>>>
    {
        public ApiResponse<IEnumerable<SuperpowerResponse>> GetExamples()
        {
            var list = new List<SuperpowerResponse>
            {
                new SuperpowerResponse { Id = 1, Name = "Força", Description = "Força sobre-humana" },
                new SuperpowerResponse { Id = 2, Name = "Velocidade", Description = "Movimento ultra rápido" },
                new SuperpowerResponse { Id = 3, Name = "Voo", Description = "Capacidade de voar" }
            };
            return new ApiResponse<IEnumerable<SuperpowerResponse>>(HttpStatusCode.OK, list);
        }
    }

    public class ApiSuperpowersNotFoundResponseExample : IExamplesProvider<ApiResponse<IEnumerable<SuperpowerResponse>>>
    {
        public ApiResponse<IEnumerable<SuperpowerResponse>> GetExamples()
        {
            return new ApiResponse<IEnumerable<SuperpowerResponse>>(HttpStatusCode.NotFound, null, new Dictionary<string, string>
            {
                {"Superpowers", "Nenhum superpoder cadastrado."}
            });
        }
    }
}


