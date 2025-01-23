using Microsoft.AspNetCore.Mvc;

using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;

using System.Text.Json;

namespace Ponta.Servico.Tarefas
{
    public class ServicoTarefas : IServicoTarefas
    {
        private readonly IRepositorioTarefa repositorio;

        public ServicoTarefas(IRepositorioTarefa repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IActionResult> BuscarTarefas()
        {
            try
            {
                var tarefas = await repositorio.ObterTarefasAsync();
                return new OkObjectResult(tarefas);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> BuscarTarefas(StatusTarefa status)
        {
            try
            {
                var tarefas = await repositorio.ObterTarefasPorStatusAsync(status);
                return new OkObjectResult(tarefas);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
