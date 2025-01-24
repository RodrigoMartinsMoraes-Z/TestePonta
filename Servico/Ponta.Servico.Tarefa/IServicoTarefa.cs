using Microsoft.AspNetCore.Mvc;

namespace Ponta.Servico.Tarefa;

public interface IServicoTarefa
{
    Task<IActionResult> AtualizarTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado);

    Task<IActionResult> NovaTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado);
}
