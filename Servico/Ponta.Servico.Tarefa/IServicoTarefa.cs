namespace Ponta.Servico.Tarefa;

public interface IServicoTarefa
{
    Task<HttpResponseMessage> AtualizarTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado);
    Task<HttpResponseMessage> NovaTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado);
}
