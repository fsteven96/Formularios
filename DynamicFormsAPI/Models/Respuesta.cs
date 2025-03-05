
using System.Text.Json.Serialization;
namespace DynamicFormsAPI.Models
{
    public class Respuesta
    {
        public int Id { get; set; }

        public int FormularioId { get; set; }

        public int CampoId { get; set; }
        public string Valor { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public Formulario? Formulario { get; set; }  
        public Campo? Campo { get; set; }  
    }
}
