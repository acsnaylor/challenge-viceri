using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ChallengeViceri.Application.Services;
using ChallengeViceri.Domain.Entities;
using ChallengeViceri.Domain.Requests;
using ChallengeViceri.Infrastructure.Data.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChallengeViceri.Tests;

public class HeroServiceTests
{
    private readonly Mock<IRepositoryAsync<Hero>> _heroRepo = new();
    private readonly Mock<IRepositoryAsync<Superpower>> _spRepo = new();
    private readonly Mock<IRepositoryAsync<HeroSuperpower>> _hsRepo = new();

    private HeroService CreateService() => new HeroService(_heroRepo.Object, _spRepo.Object, _hsRepo.Object);

    [Fact]
    public async Task AddHeroAsync_ReturnsBadRequest_WhenNameEmpty()
    {
        var svc = CreateService();
        var req = new HeroRequest { Name = "", HeroName = "Batman", BirthDate = DateTime.UtcNow, Height = 1.8, Weight = 80, SuperpowerIds = new List<int> { 1 } };
        var res = await svc.AddHeroAsync(req, default);
        res.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddHeroAsync_ReturnsBadRequest_WhenDuplicateHeroName()
    {
        _heroRepo.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hero, bool>>>(), default))
                 .ReturnsAsync(true);
        var svc = CreateService();
        var req = new HeroRequest { Name = "Bruce", HeroName = "Batman", BirthDate = DateTime.UtcNow, Height = 1.8, Weight = 80, SuperpowerIds = new List<int> { 1 } };
        var res = await svc.AddHeroAsync(req, default);
        res.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddHeroAsync_ReturnsBadRequest_WhenSuperpowerNotFound()
    {
        _heroRepo.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hero, bool>>>(), default))
                 .ReturnsAsync(false);
        _spRepo.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Superpower, bool>>>(), default))
               .ReturnsAsync(false);
        var svc = CreateService();
        var req = new HeroRequest { Name = "Bruce", HeroName = "Batman", BirthDate = DateTime.UtcNow, Height = 1.8, Weight = 80, SuperpowerIds = new List<int> { 99 } };
        var res = await svc.AddHeroAsync(req, default);
        res.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddHeroAsync_CreatesHero_WithSuperpowers()
    {
        _heroRepo.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hero, bool>>>(), default))
                 .ReturnsAsync(false);
        _spRepo.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Superpower, bool>>>(), default))
               .ReturnsAsync(true);
        _heroRepo.Setup(r => r.InsertAsync(It.IsAny<Hero>(), default))
                 .Callback((Hero h, CancellationToken _) => h.Id = 1)
                 .ReturnsAsync((Hero h, CancellationToken _) => h);
        _hsRepo.Setup(r => r.InsertAsync(It.IsAny<HeroSuperpower>(), default))
               .ReturnsAsync((HeroSuperpower hs, CancellationToken _) => hs);
        _heroRepo.Setup(r => r.GetByIdAsync(It.IsAny<object>(), It.IsAny<string?>(), default))
                 .ReturnsAsync(new Hero
                 {
                     Id = 1,
                     Name = "Bruce",
                     HeroName = "Batman",
                     BirthDate = DateTime.UtcNow,
                     Height = 1.8,
                     Weight = 80,
                     HeroSuperpowers = new List<HeroSuperpower>
                     {
                         new HeroSuperpower{ HeroId = 1, SuperpowerId = 1, Superpower = new Superpower{ Id = 1, Name = "Força" } }
                     }
                 });

        var svc = CreateService();
        var req = new HeroRequest { Name = "Bruce", HeroName = "Batman", BirthDate = DateTime.UtcNow, Height = 1.8, Weight = 80, SuperpowerIds = new List<int> { 1 } };
        var res = await svc.AddHeroAsync(req, default);
        res.Code.Should().Be(HttpStatusCode.Created);
        res.Result.Should().NotBeNull();
        res.Result!.Superpowers.Should().HaveCount(1);
    }

    [Fact]
    public async Task ListHeroesAsync_ReturnsNotFound_WhenEmpty()
    {
        _heroRepo.Setup(r => r.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Hero, bool>>>(), It.IsAny<string?>(), true, default))
                 .ReturnsAsync(Array.Empty<Hero>());
        var svc = CreateService();
        var res = await svc.ListHeroesAsync(default);
        res.Code.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetHeroById_BadRequest_WhenInvalidId()
    {
        var svc = CreateService();
        var res = await svc.GetHeroByIdAsync(0, default);
        res.Code.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteHero_NotFound_WhenMissing()
    {
        _heroRepo.Setup(r => r.GetByIdAsync(It.IsAny<object>(), default)).ReturnsAsync((Hero?)null);
        var svc = CreateService();
        var res = await svc.DeleteHeroAsync(123, default);
        res.Code.Should().Be(HttpStatusCode.NotFound);
    }
}
