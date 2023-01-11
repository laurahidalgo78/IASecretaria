using System;
using System.Collections.Generic;

namespace IASecretaria.Models;

public partial class Persona
{
    public int Idpersona { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Contacto { get; set; }

    public int IdtipoContacto { get; set; }

    public virtual TipoContacto IdtipoContactoNavigation { get; set; } = null!;
}
