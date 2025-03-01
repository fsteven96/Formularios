using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicFormsAPI.Repositories;
using DynamicFormsAPI.Models;
// using Microsoft.Extensions.Logging;
namespace DynamicFormsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormularioController : ControllerBase
    {
        private readonly FormularioRepository _repo;
        private readonly ILogger<FormularioController> _logger;

        public FormularioController(FormularioRepository repo, ILogger<FormularioController> logger)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormulario()
        {
                      try
            {
                // Intentamos obtener los formularios
                var formularios = await _repo.ObtenerFormulariosAsync();
                return Ok(formularios); // Si no hay error, devolvemos los formularios
            }
            catch (Exception ex)
            {
                // Aquí registramos cualquier error adicional si no fue registrado en el repositorio
                _logger.LogError(ex, "Ocurrió un error al obtener los formularios en el controlador.");
                
                // Devolvemos un error genérico al cliente
                return StatusCode(500, "Ocurrió un error inesperado al obtener los formularios.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Campo>> GetCamposPorID(int id)
        {
                      try
            {
                // Intentamos obtener los formularios
                var campo = await _repo.ObtenerCamposPorIdAsync(id);
                return Ok(campo); // Si no hay error, devolvemos los formularios
            }
            catch (Exception ex)
            {
                // Aquí registramos cualquier error adicional si no fue registrado en el repositorio
                _logger.LogError(ex, "Ocurrió un error al obtener los formularios en el controlador.");
                
                // Devolvemos un error genérico al cliente
                return StatusCode(500, "Ocurrió un error inesperado al obtener los formularios.");
            }
        }


        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Formulario>>> GetRespuestaPorID()
        // {
        //               try
        //     {
        //         // Intentamos obtener los formularios
        //         var formularios = await _repo.ObtenerFormulariosAsync();
        //         return Ok(formularios); // Si no hay error, devolvemos los formularios
        //     }
        //     catch (Exception ex)
        //     {
        //         // Aquí registramos cualquier error adicional si no fue registrado en el repositorio
        //         _logger.LogError(ex, "Ocurrió un error al obtener los formularios en el controlador.");
                
        //         // Devolvemos un error genérico al cliente
        //         return StatusCode(500, "Ocurrió un error inesperado al obtener los formularios.");
        //     }
        // }


    }
}
