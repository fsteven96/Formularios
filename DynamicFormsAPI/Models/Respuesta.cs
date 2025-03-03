
using System.Text.Json.Serialization;
namespace DynamicFormsAPI.Models
{
    public class Respuesta
    {
        public int Iclsd { get; set; }

        public int FormularioId { get; set; }

        public int CampoId { get; set; }
        public string Valor { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [JsonIgnore] 
        public Formulario Formulario { get; set; }

        [JsonIgnore] 
        public Campo Campo { get; set; }
    }
}
