using ChallengeViceri.Application.Interfaces;
using ChallengeViceri.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using ChallengeViceri.Api.Swagger.Examples;

namespace ChallengeViceri.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuperpowersController : ControllerBase
    {
        private readonly ISuperpowerService _superpowerService;
        public SuperpowersController(ISuperpowerService superpowerService)
        {
            _superpowerService = superpowerService;
        }
        [SwaggerOperation(Summary = "Listar superpoderes", Description = "Retorna a lista de superpoderes para seleção no cadastro de heróis.")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SuperpowerResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<SuperpowerResponse>>), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiSuperpowersListResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiSuperpowersNotFoundResponseExample))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _superpowerService.ListAsync(cancellationToken);
            return StatusCode((int)result.Code, result);
        }
    }
}

