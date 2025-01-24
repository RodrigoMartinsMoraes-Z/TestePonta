using Microsoft.EntityFrameworkCore;

using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;

namespace Ponta.Contexto.Tarefa.Repositorio;

public class RepositorioTarefa(IContextoTarefa contexto)
: IRepositorioTarefa
{
    public async Task<int> SalvarTarefaAsync(Tarefa.Entidades.Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        _ = contexto.Tarefas.Add(tarefa);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AtualizarTarefaAsync(Tarefa.Entidades.Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        _ = contexto.Tarefas.Update(tarefa);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> ExcluirTarefaAsync(Tarefa.Entidades.Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        _ = contexto.Tarefas.Remove(tarefa);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<Tarefa.Entidades.Tarefa?> ObterTarefaPorGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await contexto.Tarefas.FirstOrDefaultAsync(t => t.Guid == guid, cancellationToken);
    }

    public async Task<IEnumerable<Tarefa.Entidades.Tarefa>> ObterTarefasAsync(CancellationToken cancellationToken = default)
    {
        return await contexto.Tarefas.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tarefa.Entidades.Tarefa>> ObterTarefasPorUsuarioAsync(Guid guidUsuario, CancellationToken cancellationToken = default)
    {
        return await contexto.Tarefas.Where(t => t.GuidUsuario == guidUsuario).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tarefa.Entidades.Tarefa>> ObterTarefasPorStatusAsync(StatusTarefa status, CancellationToken cancellationToken = default)
    {
        return await contexto.Tarefas.Where(t => t.Status == status).ToListAsync(cancellationToken);
    }
}
