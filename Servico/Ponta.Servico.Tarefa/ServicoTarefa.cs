using Ponta.Contexto.Tarefa.Interfaces;

using System.Net;

namespace Ponta.Servico.Tarefa;

public class ServicoTarefa(IRepositorioTarefa repositorio)
: IServicoTarefa
{
    public async Task<HttpResponseMessage> NovaTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado)
    {
        try
        {
            if (await CamposInvalidos(tarefa))
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Campos inválidos")
                };

            tarefa.GuidUsuario = guidUsuarioLogado;

            await repositorio.SalvarTarefaAsync(tarefa);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }


    public async Task<HttpResponseMessage> AtualizarTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado)
    {
        var tarefaExistente = await repositorio.ObterTarefaPorGuidAsync(tarefa.Guid);

        if (tarefaExistente is null)
            return new HttpResponseMessage(HttpStatusCode.NotFound);

        if (tarefaExistente.GuidUsuario != guidUsuarioLogado)
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);

        if (await CamposInvalidos(tarefa))
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Campos inválidos")
            };

        await repositorio.AtualizarTarefaAsync(tarefa);

        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    private async Task<bool> CamposInvalidos(Contexto.Tarefa.Entidades.Tarefa tarefa)
    {
        if (string.IsNullOrWhiteSpace(tarefa.Descricao) ||
            string.IsNullOrWhiteSpace(tarefa.Titulo))
        {
            return true;
        }

        return false;
    }
}
