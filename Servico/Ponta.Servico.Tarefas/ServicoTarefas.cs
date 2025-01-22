using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;

using System.Net;
using System.Text.Json;

namespace Ponta.Servico.Tarefas;

public class ServicoTarefas(IRepositorioTarefa repositorio)
{
    public async Task<HttpResponseMessage> BuscarTarefas()
    {
        try
        {
            var tarefas = await repositorio.ObterTarefasAsync();
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(tarefas))
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

    public async Task<HttpResponseMessage> BuscarTarefas(StatusTarefa status)
    {
        try
        {
            var tarefas = await repositorio.ObterTarefasPorStatusAsync(status);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(tarefas))
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

}
