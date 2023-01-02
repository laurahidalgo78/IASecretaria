using IASecretaria.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IASecretaria.Datos
{
    public class BaseDatosQaA : IdentityDbContext
    {
        public BaseDatosQaA(DbContextOptions options) : base(options)
        {

        }

        // Modelos a conectar
        public DbSet<Prediction> prediction { get; set; }
    }


}
