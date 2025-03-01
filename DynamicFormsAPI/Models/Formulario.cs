using System.Collections.Generic;

namespace DynamicFormsAPI.Models
{
    public class Formulario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Campo> Campos { get; set; } = new List<Campo>();
    }
}
