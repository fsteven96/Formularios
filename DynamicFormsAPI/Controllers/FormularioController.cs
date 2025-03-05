using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicFormsAPI.Repositories;
using DynamicFormsAPI.Models;
using Microsoft.Extensions.Logging;
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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));  // Asegúrate de que logger no sea null

            _repo = repo;
        }

        [HttpGet("formularios")]
        public async Task<ActionResult<IEnumerable<Formulario>>> GetFormulario()
        {
            try
            {
                var formularios = await _repo.ObtenerFormulariosAsync();
                
                return Ok(formularios); 
            }
            catch (Exception ex)
            {
                // Aquí registramos cualquier error adicional si no fue registrado en el repositorio
                _logger.LogError(ex, "Ocurrió un error al obtener los formularios en el controlador."); _logger.LogError("No se pudo guardar el formulario.");
                                return BadRequest("Hubo un problema al guardar el formulario.");
                
                // Devolvemos un error genérico al cliente
                return StatusCode(500, "Ocurrió un error inesperado al obtener los formularios.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Formulario>> GetCamposPorID(int id)
        {
                      try
            {
                
                var formulario = await _repo.ObtenerCamposAsync(id);
                // Console.WriteLine(formulario);
                return Ok(formulario); // Si no hay error, devolvemos los formularios
            }
            catch (Exception ex)
            {
                // Aquí registramos cualquier error adicional si no fue registrado en el repositorio
                _logger.LogError(ex, "Ocurrió un error al obtener los formularios en el controlador.");
                
                // Devolvemos un error genérico al cliente
                return StatusCode(500, "Ocurrió un error inesperado al obtener los formularios.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<bool>> PostFormulario(Formulario formulario)
        {
            // Verificamos si el formulario es válido
            Console.WriteLine("here controller");
            if (formulario == null)
            {
                return BadRequest(false); // Si no es válido, retornamos false
            }

            try
            {
                
                // Llamamos al repositorio para guardar el formulario con sus campos
                bool isSaved = await _repo.SaveFormularioAsync(formulario);

                // Retornamos true si el formulario se guardó correctamente, de lo contrario false
                if (isSaved == false)
                {
                    _logger.LogError("No se pudo guardar el formulario.");
                    return BadRequest("Hubo un problema al guardar el formulario.");
                };
                return isSaved ? Ok(true) : BadRequest(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al guardar el formulario.");
                return StatusCode(500, false); 
            }
        }

        
        [HttpPut]
        public async Task<ActionResult<bool>> PutFormulario(Formulario formulario)
        {
            if (formulario == null || formulario.Id == 0)
            {
                return BadRequest("Formulario inválido o no proporcionado.");
            }

            try
            {
                // Llamamos al repositorio para actualizar el formulario y sus campos
                bool isUpdated = await _repo.UpdateFormularioAsync(formulario);

                // Verificamos si la actualización fue exitosa
                if (!isUpdated)
                {
                    return NotFound("Formulario no encontrado o no pudo ser actualizado.");
                }

                return Ok(true); // Si todo fue exitoso, retornamos Ok
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al actualizar el formulario.");
                return StatusCode(500, "Hubo un error al procesar la solicitud.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFormulario(int id){
            try
            {
                bool isDelete = await _repo.EliminarFormulario(id);
                 if (!isDelete)
                {
                    return NotFound($"No se encontró el formulario con ID {id}");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al guardar el formulario.");
                return StatusCode(500, false);
            }
        }


      



    }
}
