using Ponta.Contexto.Tarefa.Enums;

namespace Ponta.Contexto.Tarefa.Interfaces;

public interface IRepositorioTarefa
{
    Task<int> AtualizarTarefaAsync(Entidades.Tarefa tarefa, CancellationToken cancellationToken = default);

    Task<int> ExcluirTarefaAsync(Entidades.Tarefa tarefa, CancellationToken cancellationToken = default);

    Task<Entidades.Tarefa?> ObterTarefaPorGuidAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<IEnumerable<Entidades.Tarefa>> ObterTarefasAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Entidades.Tarefa>> ObterTarefasPorStatusAsync(StatusTarefa status, CancellationToken cancellationToken = default);

    Task<IEnumerable<Entidades.Tarefa>> ObterTarefasPorUsuarioAsync(Guid guidUsuario, CancellationToken cancellationToken = default);

    Task<int> SalvarTarefaAsync(Entidades.Tarefa tarefa, CancellationToken cancellationToken = default);
}
