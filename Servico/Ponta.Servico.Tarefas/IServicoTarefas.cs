using Ponta.Contexto.Tarefa.Enums;

namespace Ponta.Servico.Tarefas;

public interface IServicoTarefas
{
    Task<HttpResponseMessage> BuscarTarefas();
    Task<HttpResponseMessage> BuscarTarefas(StatusTarefa status);
}
