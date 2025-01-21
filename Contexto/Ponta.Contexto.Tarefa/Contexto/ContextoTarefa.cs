using Microsoft.EntityFrameworkCore;

using Ponta.Contexto.Tarefa.Interfaces;

namespace Ponta.Contexto.Tarefa.Contexto;
public class ContextoTarefa : DbContext, IContextoTarefa
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = "Host=localhost;Port=5432;Database=PONTA.TAREFA;Username=postgres;Password=123;";
        _ = optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Tarefa.Entidades.Tarefa>(tarefa =>
        {
            tarefa.HasKey(t => t.Id);
            tarefa.Property(t => t.Guid).IsRequired().HasDefaultValueSql("gen_random_uuid()");
            tarefa.Property(t => t.Titulo).IsRequired();
            tarefa.Property(t => t.Descricao).IsRequired();
            tarefa.Property(t => t.DataCriacao).IsRequired();
            tarefa.Property(t => t.Status).IsRequired();
            tarefa.Property(t => t.Prioridade).IsRequired();
            tarefa.Property(t => t.GuidUsuario).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Entidades.Tarefa> Tarefas { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
}
