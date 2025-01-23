using Microsoft.AspNetCore.Mvc;

using Ponta.Contexto.Tarefa.Interfaces;

namespace Ponta.Servico.Tarefa
{
    public class ServicoTarefa : IServicoTarefa
    {
        private readonly IRepositorioTarefa repositorio;

        public ServicoTarefa(IRepositorioTarefa repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<IActionResult> NovaTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado)
        {
            try
            {
                if (await CamposInvalidos(tarefa))
                {
                    return new BadRequestObjectResult("Campos inválidos");
                }

                tarefa.GuidUsuario = guidUsuarioLogado;

                await repositorio.SalvarTarefaAsync(tarefa);
                return new OkObjectResult(tarefa);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<IActionResult> AtualizarTarefa(Contexto.Tarefa.Entidades.Tarefa tarefa, Guid guidUsuarioLogado)
        {
            var tarefaExistente = await repositorio.ObterTarefaPorGuidAsync(tarefa.Guid);

            if (tarefaExistente is null)
            {
                return new NotFoundResult();
            }

            if (tarefaExistente.GuidUsuario != guidUsuarioLogado)
            {
                return new UnauthorizedResult();
            }

            if (await CamposInvalidos(tarefa))
            {
                return new BadRequestObjectResult("Campos inválidos");
            }

            tarefaExistente.Titulo = tarefa.Titulo;
            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.DataFim = tarefa.DataFim;
            tarefaExistente.DataInicio = tarefa.DataInicio;
            tarefaExistente.Prioridade = tarefa.Prioridade;
            tarefaExistente.Status = tarefa.Status;

            await repositorio.AtualizarTarefaAsync(tarefaExistente);

            return new OkObjectResult(tarefaExistente);
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
}
