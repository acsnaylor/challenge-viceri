using ChallengeViceri.Application.Interfaces;
using ChallengeViceri.Domain.Entities;
using ChallengeViceri.Domain.Requests;
using ChallengeViceri.Domain.Responses;
using ChallengeViceri.Infrastructure.Data.Repositories;
using System.Net;

namespace ChallengeViceri.Application.Services
{
    public class HeroService : IHeroService
    {
        private readonly IRepositoryAsync<Hero> _heroRepository;
        private readonly IRepositoryAsync<Superpower> _superpowerRepository;
        private readonly IRepositoryAsync<HeroSuperpower> _heroSuperpowerRepository;

        public HeroService(
            IRepositoryAsync<Hero> heroRepository,
            IRepositoryAsync<Superpower> superpowerRepository,
            IRepositoryAsync<HeroSuperpower> heroSuperpowerRepository)
        {
            _heroRepository = heroRepository;
            _superpowerRepository = superpowerRepository;
            _heroSuperpowerRepository = heroSuperpowerRepository;
        }

        public async Task<ApiResponse<HeroResponse>> AddHeroAsync(HeroRequest request, CancellationToken cancellationToken)
        {
            var validationError = await ValidateHeroRequestAsync(request, null, cancellationToken);
            if (validationError != null)
                return new ApiResponse<HeroResponse>(HttpStatusCode.BadRequest, null, new Dictionary<string, string> { { "Validation", validationError } });

            var hero = new Hero
            {
                Name = request.Name,
                HeroName = request.HeroName,
                BirthDate = request.BirthDate.HasValue ? DateTime.SpecifyKind(request.BirthDate.Value, DateTimeKind.Unspecified) : null,
                Height = request.Height,
                Weight = request.Weight
            };

            await _heroRepository.InsertAsync(hero, cancellationToken);
            _heroRepository.SaveChanges();

            if (request.SuperpowerIds?.Any() == true)
            {
                foreach (var spId in request.SuperpowerIds.Distinct())
                {
                    await _heroSuperpowerRepository.InsertAsync(new HeroSuperpower
                    {
                        HeroId = hero.Id,
                        SuperpowerId = spId
                    }, cancellationToken);
                }
                _heroRepository.SaveChanges();
            }

            var response = await MapToHeroResponseAsync(hero.Id, cancellationToken);
            return new ApiResponse<HeroResponse>(HttpStatusCode.Created, response);
        }

        public async Task<ApiResponse<IEnumerable<HeroResponse>>> ListHeroesAsync(CancellationToken cancellationToken)
        {
            var heroes = await _heroRepository.GetAllAsync(includeProperties: "HeroSuperpowers,HeroSuperpowers.Superpower", cancellationToken: cancellationToken);
            if (heroes == null || !heroes.Any())
            {
                return new ApiResponse<IEnumerable<HeroResponse>>(HttpStatusCode.NotFound, null, new Dictionary<string, string> { { "Heroes", "Nenhum super-herói cadastrado." } });
            }

            var list = heroes.Select(h => new HeroResponse
            {
                Id = h.Id,
                Name = h.Name,
                HeroName = h.HeroName,
                BirthDate = h.BirthDate,
                Height = h.Height,
                Weight = h.Weight,
                Superpowers = h.HeroSuperpowers.Select(hs => new SuperpowerResponse
                {
                    Id = hs.SuperpowerId,
                    Name = hs.Superpower?.Name ?? string.Empty,
                    Description = hs.Superpower?.Description
                }).ToList()
            }).ToList();

            return new ApiResponse<IEnumerable<HeroResponse>>(HttpStatusCode.OK, list);
        }

        public async Task<ApiResponse<HeroResponse>> GetHeroByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0) return new ApiResponse<HeroResponse>(HttpStatusCode.BadRequest, null, new Dictionary<string, string> { { "Id", "Id inválido." } });

            var hero = await _heroRepository.GetByIdAsync(id, includeProperties: "HeroSuperpowers,HeroSuperpowers.Superpower", cancellationToken: cancellationToken);
            if (hero == null)
            {
                return new ApiResponse<HeroResponse>(HttpStatusCode.NotFound, null, new Dictionary<string, string> { { "Hero", "Super-herói não encontrado." } });
            }

            var response = new HeroResponse
            {
                Id = hero.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => new SuperpowerResponse
                {
                    Id = hs.SuperpowerId,
                    Name = hs.Superpower?.Name ?? string.Empty,
                    Description = hs.Superpower?.Description
                }).ToList()
            };

            return new ApiResponse<HeroResponse>(HttpStatusCode.OK, response);
        }

        public async Task<ApiResponse<HeroResponse>> UpdateHeroAsync(int id, HeroRequest request, CancellationToken cancellationToken)
        {
            if (id <= 0) return new ApiResponse<HeroResponse>(HttpStatusCode.BadRequest, null, new Dictionary<string, string> { { "Id", "Id inválido." } });

            var existing = await _heroRepository.GetByIdAsync(id, includeProperties: "HeroSuperpowers", cancellationToken: cancellationToken);
            if (existing == null)
                return new ApiResponse<HeroResponse>(HttpStatusCode.NotFound, null, new Dictionary<string, string> { { "Hero", "Super-herói não encontrado." } });

            var validationError = await ValidateHeroRequestAsync(request, id, cancellationToken);
            if (validationError != null)
                return new ApiResponse<HeroResponse>(HttpStatusCode.BadRequest, null, new Dictionary<string, string> { { "Validation", validationError } });

            existing.Name = request.Name;
            existing.HeroName = request.HeroName;
            existing.BirthDate = request.BirthDate.HasValue ? DateTime.SpecifyKind(request.BirthDate.Value, DateTimeKind.Unspecified) : null;
            existing.Height = request.Height;
            existing.Weight = request.Weight;

            var current = existing.HeroSuperpowers.Select(hs => hs.SuperpowerId).ToHashSet();
            var desired = (request.SuperpowerIds ?? new List<int>()).Distinct().ToHashSet();

            var toRemove = current.Except(desired).ToList();
            var toAdd = desired.Except(current).ToList();

            if (toRemove.Count > 0)
            {
                var trackedToRemove = existing.HeroSuperpowers.Where(hs => toRemove.Contains(hs.SuperpowerId)).ToList();
                foreach (var hs in trackedToRemove)
                {
                    _heroSuperpowerRepository.Delete(hs);
                }
            }

            foreach (var spId in toAdd)
            {
                await _heroSuperpowerRepository.InsertAsync(new HeroSuperpower { HeroId = existing.Id, SuperpowerId = spId }, cancellationToken);
            }

            _heroRepository.SaveChanges();

            var response = await MapToHeroResponseAsync(existing.Id, cancellationToken);
            return new ApiResponse<HeroResponse>(HttpStatusCode.OK, response);
        }

        public async Task<ApiResponse<string>> DeleteHeroAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0) return new ApiResponse<string>(HttpStatusCode.BadRequest, null, new Dictionary<string, string> { { "Id", "Id inválido." } });

            var hero = await _heroRepository.GetByIdAsync(id, cancellationToken);
            if (hero == null)
                return new ApiResponse<string>(HttpStatusCode.NotFound, null, new Dictionary<string, string> { { "Hero", "Super-herói não encontrado." } });

            _heroRepository.Delete(hero);
            _heroRepository.SaveChanges();

            return new ApiResponse<string>(HttpStatusCode.OK, $"Super-herói '{hero.HeroName}' excluÃ­do com sucesso.");
        }

        private async Task<string?> ValidateHeroRequestAsync(HeroRequest request, int? updatingId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return "Nome Ã© obrigatório.";
            if (string.IsNullOrWhiteSpace(request.HeroName)) return "Nome de herói Ã© obrigatório.";
            if (request.BirthDate == null) return "Data de nascimento Ã© obrigatória.";
            if (request.Height <= 0) return "Altura deve ser maior que zero.";
            if (request.Weight <= 0) return "Peso deve ser maior que zero.";
            if (request.SuperpowerIds == null || !request.SuperpowerIds.Any()) return "Selecione ao menos um superpoder.";

            var exists = await _heroRepository.ExistsAsync(h => h.HeroName == request.HeroName && (!updatingId.HasValue || h.Id != updatingId.Value), cancellationToken);
            if (exists) return "Já existe um super-herói com esse nome de herói.";

            var spIds = request.SuperpowerIds.Distinct().ToList();
            foreach (var spId in spIds)
            {
                var spExists = await _superpowerRepository.ExistsAsync(s => s.Id == spId, cancellationToken);
                if (!spExists) return $"Superpoder Id {spId} não encontrado.";
            }

            return null;
        }

        private async Task<HeroResponse> MapToHeroResponseAsync(int id, CancellationToken cancellationToken)
        {
            var hero = await _heroRepository.GetByIdAsync(id, includeProperties: "HeroSuperpowers,HeroSuperpowers.Superpower", cancellationToken: cancellationToken);
            return new HeroResponse
            {
                Id = hero!.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => new SuperpowerResponse
                {
                    Id = hs.SuperpowerId,
                    Name = hs.Superpower?.Name ?? string.Empty,
                    Description = hs.Superpower?.Description
                }).ToList()
            };
        }
    }
}


