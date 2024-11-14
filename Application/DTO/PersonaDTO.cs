using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PersonaDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Cedula { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Celular { get; set; }
    }
}
