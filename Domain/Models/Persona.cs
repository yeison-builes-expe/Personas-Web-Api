using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Cedula { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Celular { get; set; }
}
