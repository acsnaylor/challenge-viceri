using System.Net;
using ChallengeViceri.Domain.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace ChallengeViceri.Api.Swagger.Examples
{
    public class ApiHeroCreatedResponseExample : IExamplesProvider<ApiResponse<HeroResponse>>
    {
        public ApiResponse<HeroResponse> GetExamples()
        {
            var hero = new HeroResponse
            {
                Id = 1,
                Name = "Bruce Wayne",
                HeroName = "Batman",
                BirthDate = new DateTime(1972, 2, 19),
                Height = 1.88,
                Weight = 95.0,
                Superpowers = new List<SuperpowerResponse>
                {
                    new SuperpowerResponse { Id = 1, Name = "Força", Description = "Força sobre-humana" },
                    new SuperpowerResponse { Id = 3, Name = "Voo", Description = "Capacidade de voar" }
                }
            };
            return new ApiResponse<HeroResponse>(HttpStatusCode.Created, hero);
        }
    }

    public class ApiHeroResponseExample : IExamplesProvider<ApiResponse<HeroResponse>>
    {
        public ApiResponse<HeroResponse> GetExamples()
        {
            var hero = new HeroResponse
            {
                Id = 2,
                Name = "Clark Kent",
                HeroName = "Superman",
                BirthDate = new DateTime(1975, 6, 18),
                Height = 1.91,
                Weight = 107.0,
                Superpowers = new List<SuperpowerResponse>
                {
                    new SuperpowerResponse { Id = 1, Name = "Força", Description = "Força sobre-humana" },
                    new SuperpowerResponse { Id = 2, Name = "Velocidade", Description = "Movimento ultra rápido" }
                }
            };
            return new ApiResponse<HeroResponse>(HttpStatusCode.OK, hero);
        }
    }

    public class ApiHeroesListResponseExample : IExamplesProvider<ApiResponse<IEnumerable<HeroResponse>>>
    {
        public ApiResponse<IEnumerable<HeroResponse>> GetExamples()
        {
            var list = new List<HeroResponse>
            {
                new HeroResponse
                {
                    Id = 1,
                    Name = "Bruce Wayne",
                    HeroName = "Batman",
                    BirthDate = new DateTime(1972, 2, 19),
                    Height = 1.88,
                    Weight = 95.0,
                    Superpowers = new List<SuperpowerResponse>
                    {
                        new SuperpowerResponse { Id = 1, Name = "Força" },
                        new SuperpowerResponse { Id = 3, Name = "Voo" }
                    }
                },
                new HeroResponse
                {
                    Id = 2,
                    Name = "Clark Kent",
                    HeroName = "Superman",
                    BirthDate = new DateTime(1975, 6, 18),
                    Height = 1.91,
                    Weight = 107.0,
                    Superpowers = new List<SuperpowerResponse>
                    {
                        new SuperpowerResponse { Id = 1, Name = "Força" },
                        new SuperpowerResponse { Id = 2, Name = "Velocidade" }
                    }
                }
            };
            return new ApiResponse<IEnumerable<HeroResponse>>(HttpStatusCode.OK, list);
        }
    }

    public class ApiHeroesNotFoundResponseExample : IExamplesProvider<ApiResponse<IEnumerable<HeroResponse>>>
    {
        public ApiResponse<IEnumerable<HeroResponse>> GetExamples()
        {
            return new ApiResponse<IEnumerable<HeroResponse>>(HttpStatusCode.NotFound, null, new Dictionary<string, string>
            {
                {"Heroes", "Nenhum super-herói cadastrado."}
            });
        }
    }

    public class ApiHeroValidationErrorExample : IExamplesProvider<ApiResponse<HeroResponse>>
    {
        public ApiResponse<HeroResponse> GetExamples()
        {
            return new ApiResponse<HeroResponse>(HttpStatusCode.BadRequest, null, new Dictionary<string, string>
            {
                { "Validation", "Nome de herói é obrigatório." }
            });
        }
    }

    public class ApiDeleteHeroResponseExample : IExamplesProvider<ApiResponse<string>>
    {
        public ApiResponse<string> GetExamples()
        {
            return new ApiResponse<string>(HttpStatusCode.OK, "Super-herói 'Batman' excluí­do com sucesso.");
        }
    }

    public class ApiDeleteHeroNotFoundResponseExample : IExamplesProvider<ApiResponse<string>>
    {
        public ApiResponse<string> GetExamples()
        {
            return new ApiResponse<string>(HttpStatusCode.NotFound, null, new Dictionary<string, string>
            {
                {"Hero", "Super-herói não encontrado."}
            });
        }
    }
}

