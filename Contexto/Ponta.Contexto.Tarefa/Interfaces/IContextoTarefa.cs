using Microsoft.EntityFrameworkCore;

namespace Ponta.Contexto.Tarefa.Interfaces;
public interface IContextoTarefa
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Tarefa.Entidades.Tarefa> Tarefas { get; set; }
}
