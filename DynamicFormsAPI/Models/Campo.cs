using System.Text.Json.Serialization;
namespace DynamicFormsAPI.Models
{
    public class Campo
    {
        public int Id { get; set; }
        public int FormularioId { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } // Ejemplo: "text", "number", "date"
        [JsonIgnore] 
        public Formulario? Formulario { get; set; }
    }
}
