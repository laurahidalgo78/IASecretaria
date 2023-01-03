using System;
using System.Collections.Generic;

namespace IASecretaria.Models;

public partial class Intencione
{
    public int IntencionesId { get; set; }

    public string? Descripcion { get; set; }

    public virtual Video? Video { get; set; }
}
