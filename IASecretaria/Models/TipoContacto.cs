using System;
using System.Collections.Generic;

namespace IASecretaria.Models;

public partial class TipoContacto
{
    public int IdtipoContacto { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Persona> Personas { get; } = new List<Persona>();
}
