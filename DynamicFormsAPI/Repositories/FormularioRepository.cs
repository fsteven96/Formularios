using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DynamicFormsAPI.Data;
using DynamicFormsAPI.Models;
using Microsoft.Extensions.Logging;

namespace DynamicFormsAPI.Repositories
{
    public class FormularioRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FormularioRepository> _logger;
        public FormularioRepository(AppDbContext context, ILogger<FormularioRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Formulario>> ObtenerFormulariosAsync()
        {
             try
            {
                // Intentamos obtener los formularios desde la base de datos
                return await _context.Formularios.ToListAsync();
            }
            catch (Exception ex)
            {
                // Si hay un error, lo registramos en el repositorio
                _logger.LogError(ex, "Error al obtener los formularios desde la base de datos.");
                // Re-throw la excepción para que sea manejada en el controlador
                throw;
            }
        }


        
        public async Task<Campo> ObtenerCamposPorIdAsync(int id)
        {
             try
            {
                return await _context.Campos.FirstOrDefaultAsync(f => f.FormularioId == id); 
            }
            catch (Exception ex)
            {
                // Si hay un error, lo registramos en el repositorio
                _logger.LogError(ex, "Error al obtener los formularios desde la base de datos.");
                // Re-throw la excepción para que sea manejada en el controlador
                throw;
            }
        }



    }
}
