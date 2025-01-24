using Microsoft.EntityFrameworkCore;

using Ponta.Contexto.Usuario.Interfaces;

namespace Ponta.Contexto.Usuario.Contexto;

public class ContextoUsuario : DbContext, IContextoUsuario
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = "Host=localhost;Port=5432;Database=PONTA.USUARIO;Username=postgres;Password=123;";
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario.Entidades.Usuario>(usuario =>
            {
                usuario.HasKey(t => t.Id);
                usuario.Property(t => t.Guid).IsRequired().HasDefaultValueSql("gen_random_uuid()");
                usuario.Property(t => t.Nome).IsRequired();
                usuario.Property(t => t.Login).IsRequired();
                usuario.Property(t => t.Senha).IsRequired();
            });

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);

    public DbSet<Entidades.Usuario> Usuarios { get; set; }
}
