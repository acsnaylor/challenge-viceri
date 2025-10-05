using ChallengeViceri.Application.Interfaces;
using ChallengeViceri.Domain.Requests;
using ChallengeViceri.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using ChallengeViceri.Api.Swagger.Examples;

namespace ChallengeViceri.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;
        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }
        [SwaggerOperation(Summary = "Listar heróis", Description = "Retorna a lista de super-heróis cadastrados.")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HeroResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<HeroResponse>>), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiHeroesListResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiHeroesNotFoundResponseExample))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _heroService.ListHeroesAsync(cancellationToken);
            return StatusCode((int)result.Code, result);
        }
        [SwaggerOperation(Summary = "Buscar herói por Id", Description = "Retorna um super-herói pelo seu identificador.")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiHeroResponseExample))]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _heroService.GetHeroByIdAsync(id, cancellationToken);
            return StatusCode((int)result.Code, result);
        }
        [SwaggerOperation(Summary = "Criar herói", Description = "Inclui um novo super-herói com superpoderes associados.")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(HeroRequest), typeof(HeroRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ApiHeroCreatedResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiHeroValidationErrorExample))]
        public async Task<IActionResult> Create([FromBody] HeroRequest request, CancellationToken cancellationToken)
        {
            var result = await _heroService.AddHeroAsync(request, cancellationToken);
            return StatusCode((int)result.Code, result);
        }
        [SwaggerOperation(Summary = "Atualizar herói", Description = "Atualiza os dados de um super-herói existente.")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<HeroResponse>), StatusCodes.Status404NotFound)]
        [SwaggerRequestExample(typeof(HeroRequest), typeof(HeroRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiHeroResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiHeroValidationErrorExample))]
        public async Task<IActionResult> Update(int id, [FromBody] HeroRequest request, CancellationToken cancellationToken)
        {
            var result = await _heroService.UpdateHeroAsync(id, request, cancellationToken);
            return StatusCode((int)result.Code, result);
        }
        [SwaggerOperation(Summary = "Excluir herói", Description = "Remove um super-herói pelo seu identificador.")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiDeleteHeroResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiDeleteHeroNotFoundResponseExample))]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _heroService.DeleteHeroAsync(id, cancellationToken);
            return StatusCode((int)result.Code, result);
        }
    }
}

