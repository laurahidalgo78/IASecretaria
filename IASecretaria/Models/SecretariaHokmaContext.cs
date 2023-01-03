using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IASecretaria.Models;

public partial class SecretariaHokmaContext : DbContext
{
    public SecretariaHokmaContext()
    {
    }

    public SecretariaHokmaContext(DbContextOptions<SecretariaHokmaContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HOKMA_009\\SQLEXPRESS01;Database=SecretariaHokma;User ID=HOKMA_009\\julian.gonzalez;TrustServerCertificate=true;Encrypt=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
