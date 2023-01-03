using System;
using System.Collections.Generic;

namespace IASecretaria.Models;

public partial class Video
{
    public int VideoId { get; set; }

    public string? Descripcion { get; set; }

    public int IntencionesId { get; set; }

    public virtual Intencione Intenciones { get; set; } = null!;
}
