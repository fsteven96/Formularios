using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DynamicFormsAPI.Repositories;
using DynamicFormsAPI.Models;
using Microsoft.Extensions.Logging;

namespace DynamicFormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly FormularioRepository _repo;
        private readonly ILogger<RespuestaController> _logger;

        public RespuestaController(FormularioRepository repo, ILogger<RespuestaController> logger)
        {
            _repo = repo;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostResultados(List<Respuesta> respuestas)
        {
            Console.WriteLine(respuestas);
            if (respuestas == null)
            {
                return BadRequest(false);
            }

            try
            {
                bool isSaved = await _repo.SaveResultadosAsync(respuestas);

                if (!isSaved)
                {
                    _logger.LogError("No se pudo guardar la respuesta.");
                    return BadRequest("Hubo un problema al guardar la respuesta.");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al guardar la respuesta.");
                return StatusCode(500, false);
            }
        }

        [HttpGet("{formularioId}")]
        public async Task<IActionResult> GetRespuestas(int formularioId)
        {
            try
            {
                var respuestas = await _repo.ObtenerRespuestasAsync(formularioId);

                var resultado = respuestas.Select(r => new
                {
                    r.Id,
                    FormularioNombre = r.Formulario != null ? r.Formulario.Nombre : "Formulario no disponible",
                    CampoNombre = r.Campo != null ? r.Campo.Nombre : "Campo no disponible",
                    r.Valor
                }).ToList();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las respuestas: {ex.Message}");

                return StatusCode(500, "Hubo un error al procesar la solicitud.");
            }
        }



    }
}
