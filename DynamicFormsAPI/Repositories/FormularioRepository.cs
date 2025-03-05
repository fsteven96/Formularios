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
                return await _context.Formularios
                     .Include(f => f.Campos)  // Incluimos los campos asociados al formulario
                     .ToListAsync();

            }
            catch (Exception ex)
            {
                // Si hay un error, lo registramos en el repositorio
                _logger.LogError(ex, "Error al obtener los formularios desde la base de datos.");
                // Re-throw la excepción para que sea manejada en el controlador
                throw;
            }
        }


        
        public async Task<Formulario> ObtenerCamposAsync(int id)
        {
             try
            {
                if (id == 0) {
                    //Es para craer un nuevo elemento
                     return new Formulario
                    {
                        Id = 0, // Indicamos que es un nuevo formulario
                        Nombre = "", // Nombre vacío, ya que es un formulario nuevo
                        Campos = new List<Campo> 
                        {
                            new Campo { Id = 0, FormularioId = 0, Nombre = "", Tipo = "" }
                        } 
                    };
                } else {
                    //Es para editar un nuevo elemento
                    var formulario = await _context.Formularios
                                           .Include(f => f.Campos) // Incluimos los campos asociados al formulario
                                           .FirstOrDefaultAsync(f => f.Id == id); // Obtenemos el formulario por su Id

                    return formulario;  
                }
                
            }
            catch (Exception ex)
            {
                // Si hay un error, lo registramos en el repositorio
                _logger.LogError(ex, "Error al obtener los formularios desde la base de datos.");
                // Re-throw la excepción para que sea manejada en el controlador
                throw;
            }
        }




        public async Task<bool> SaveFormularioAsync(Formulario formulario)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Guardamos el formulario
                
                _context.Formularios.Add(formulario);
                await _context.SaveChangesAsync(); // Aquí se asignará el formulario.Id
                
                // Guardamos los campos relacionados
                foreach (var campo in formulario.Campos)
                {
                    
                    if (_context.Entry(campo).State == EntityState.Detached)
                    {
                        campo.Id = 0;
                        campo.FormularioId = formulario.Id; // Establecemos la relación con el formulario
                        _context.Campos.Add(campo);
                    }
                    
                    // campo.FormularioId = formulario.Id; // Establecemos la relación con el formulario
                    // _context.Campos.Add(campo);
                }
              
                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); // Confirmamos la transacción

                return true; // Si todo fue exitoso, retornamos true
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Revertimos los cambios en caso de error
                _logger.LogError(ex, "Hubo un error al guardar el formulario y los campos.");
                return false; // Si ocurrió un error, retornamos false
            }
        }



        public async Task<bool> UpdateFormularioAsync(Formulario formulario)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var formularioExistente = await _context.Formularios
                    .Include(f => f.Campos) 
                    .FirstOrDefaultAsync(f => f.Id == formulario.Id);

                if (formularioExistente == null)
                {
                    return false; 
                }

               
                formularioExistente.Nombre = formulario.Nombre;

                
                var camposIdsRecibidos = formulario.Campos.Select(c => c.Id).ToList();

                
                var camposAEliminar = formularioExistente.Campos
                    .Where(c => !camposIdsRecibidos.Contains(c.Id))
                    .ToList();

                foreach (var campo in camposAEliminar)
                {
                    _context.Campos.Remove(campo); 
                }

                // Actualizamos los campos o agregamos nuevos
                foreach (var campo in formulario.Campos)
                {
                    if (campo.Id == 0)
                    {
                        // Si el campo tiene Id = 0, lo agregamos como nuevo
                        campo.FormularioId = formulario.Id; // Relacionamos el campo con el formulario
                        _context.Campos.Add(campo);
                    }
                    else
                    {
                        // Si el campo ya tiene un Id, lo actualizamos
                        var campoExistente = formularioExistente.Campos.FirstOrDefault(c => c.Id == campo.Id);
                        if (campoExistente != null)
                        {
                            campoExistente.Nombre = campo.Nombre;
                            campoExistente.Tipo = campo.Tipo;
                        }
                    }
                }

                // Guardamos los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Confirmamos la transacción
                await transaction.CommitAsync();

                return true; 
            }
            catch (Exception ex)
            {
                // En caso de error, revertimos la transacción
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Hubo un error al actualizar el formulario y sus campos.");
                return false; // Si hubo error, retornamos false
            }
        }


        public async Task<bool> EliminarFormulario(int id){
            try
            {
                var formulario = await _context.Formularios
                    .Include(f => f.Campos)
                    .FirstOrDefaultAsync(f => f.Id == id);
                
                if (formulario == null)
                {
                    _logger.LogWarning($"No se encontró el formulario con ID {id}");
                    return false;
                }
                _context.Formularios.Remove(formulario);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al guardar el formulario y los campos.");
                return false;
            }
            
        }


        public async Task<bool> SaveResultadosAsync(List<Respuesta> respuestas)
        {
            try
            {
                await _context.Respuestas.AddRangeAsync(respuestas); 
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubo un error al guardar la respuesta.");
                return false;
            }
        }

       public async Task<List<Respuesta>> ObtenerRespuestasAsync(int formularioId)
        {
            try
            {
                var respuestas = await _context.Respuestas
                    .Include(r => r.Formulario) 
                    .Include(r => r.Campo)       
                    .Where(r => r.FormularioId == formularioId)  
                    .ToListAsync();  

                return respuestas;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener respuestas: {ex.Message}");

                return new List<Respuesta>();
            }
        }




    }



}
