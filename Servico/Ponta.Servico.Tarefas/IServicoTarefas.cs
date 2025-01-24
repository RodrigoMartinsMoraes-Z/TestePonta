using Microsoft.AspNetCore.Mvc;

using Ponta.Contexto.Tarefa.Enums;

namespace Ponta.Servico.Tarefas;

public interface IServicoTarefas
{
    Task<IActionResult> BuscarTarefas();

    Task<IActionResult> BuscarTarefas(StatusTarefa status);
}
