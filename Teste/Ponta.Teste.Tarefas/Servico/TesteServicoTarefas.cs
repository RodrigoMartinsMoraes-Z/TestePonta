using Microsoft.AspNetCore.Mvc;

using Moq;

using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Servico.Tarefas;

namespace Ponta.Teste.Tarefas.Servico;
public class TesteServicoTarefas
{
    private readonly Mock<IRepositorioTarefa> repositorio;
    private readonly ServicoTarefas servicoTarefas;

    public TesteServicoTarefas()
    {
        repositorio = new Mock<IRepositorioTarefa>();
        servicoTarefas = new ServicoTarefas(repositorio.Object);
    }

    [Fact]
    public async Task BuscarTarefas_DeveRetornarTodasTarefas()
    {
        // Arrange
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
        {
            new()
            {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste",
                Descricao = "descricao_teste",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            },
            new()
            {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 2",
                Descricao = "descricao_teste 2",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            }
        };

        repositorio.Setup(r => r.ObterTarefasAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tarefas);

        // Act
        var result = await servicoTarefas.BuscarTarefas() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(tarefas, result.Value);
    }


    [Fact]
    public async Task BuscarTarefasPendentes_DeveRetornarTodasTarefasPendentes()
    {
        // Arrange
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
        {
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste",
                Descricao = "descricao_teste",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 2",
                Descricao = "descricao_teste 2",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 3",
                Descricao = "descricao_teste 3",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Concluida
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 4",
                Descricao = "descricao_teste 4",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Cancelada
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 5",
                Descricao = "descricao_teste 5",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            }
        };

        repositorio
            .Setup(r => r.ObterTarefasPorStatusAsync(It.IsAny<Contexto.Tarefa.Enums.StatusTarefa>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Pendente));

        // Act
        var result = await servicoTarefas.BuscarTarefas(Contexto.Tarefa.Enums.StatusTarefa.Pendente) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(
            tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Pendente),
            result.Value
            );
    }
    [Fact]
    public async Task BuscarTarefasEmAndamento_DeveRetornarTodasTarefasEmAndamento()
    {
        // Arrange
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
        {
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste",
                Descricao = "descricao_teste",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 2",
                Descricao = "descricao_teste 2",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 3",
                Descricao = "descricao_teste 3",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Concluida
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 4",
                Descricao = "descricao_teste 4",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Cancelada
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 5",
                Descricao = "descricao_teste 5",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            }
        };

        repositorio
            .Setup(r => r.ObterTarefasPorStatusAsync(It.IsAny<Contexto.Tarefa.Enums.StatusTarefa>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.EmAndamento));

        // Act
        var result = await servicoTarefas.BuscarTarefas(Contexto.Tarefa.Enums.StatusTarefa.EmAndamento) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(
            tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.EmAndamento),
            result.Value
            );
    }
    [Fact]
    public async Task BuscarTarefasConcluidas_DeveRetornarTodasTarefasConcluidas()
    {
        // Arrange
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
        {
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste",
                Descricao = "descricao_teste",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 2",
                Descricao = "descricao_teste 2",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 3",
                Descricao = "descricao_teste 3",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Concluida
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 4",
                Descricao = "descricao_teste 4",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Cancelada
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 5",
                Descricao = "descricao_teste 5",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            }
        };

        repositorio
            .Setup(r => r.ObterTarefasPorStatusAsync(It.IsAny<Contexto.Tarefa.Enums.StatusTarefa>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Concluida));

        // Act
        var result = await servicoTarefas.BuscarTarefas(Contexto.Tarefa.Enums.StatusTarefa.Concluida) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(
            tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Concluida),
            result.Value
            );
    }

    [Fact]
    public async Task BuscarTarefasCanceladas_DeveRetornarTodasTarefasCanceladas()
    {
        // Arrange
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
        {
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste",
                Descricao = "descricao_teste",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Pendente
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 2",
                Descricao = "descricao_teste 2",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 3",
                Descricao = "descricao_teste 3",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Concluida
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 4",
                Descricao = "descricao_teste 4",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.Cancelada
            },
            new() {
                Guid = Guid.NewGuid(),
                Titulo = "titulo_teste 5",
                Descricao = "descricao_teste 5",
                DataInicio = DateTime.Now,
                DataCriacao = DateTime.Today,
                GuidUsuario = Guid.NewGuid(),
                Status = Contexto.Tarefa.Enums.StatusTarefa.EmAndamento
            }
        };

        repositorio
            .Setup(r => r.ObterTarefasPorStatusAsync(It.IsAny<Contexto.Tarefa.Enums.StatusTarefa>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Cancelada));

        // Act
        var result = await servicoTarefas.BuscarTarefas(Contexto.Tarefa.Enums.StatusTarefa.Cancelada) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equivalent(
            tarefas.Where(t => t.Status == Contexto.Tarefa.Enums.StatusTarefa.Cancelada),
            result.Value
            );
    }
}
