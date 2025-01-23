using Microsoft.AspNetCore.Mvc; // Import para IActionResult

using Moq;

using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Servico.Tarefa;

namespace Ponta.Teste.Tarefa.Servico;

public class TesteServicoTarefa
{
    private readonly Mock<IRepositorioTarefa> _mockRepositorio;
    private readonly ServicoTarefa _servicoTarefa;

    public TesteServicoTarefa()
    {
        _mockRepositorio = new Mock<IRepositorioTarefa>();
        _servicoTarefa = new ServicoTarefa(_mockRepositorio.Object);
    }

    [Fact]
    public async Task NovaTarefa_DeveCriarNovaTarefa_QuandoCamposValidos()
    {
        // Arrange
        Guid guidUsuarioLogado = Guid.NewGuid();
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            Guid = Guid.NewGuid(),
            Titulo = "titulo_teste",
            Descricao = "descricao_teste",
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(1),
            GuidUsuario = guidUsuarioLogado
        };
        _mockRepositorio.Setup(r => r.SalvarTarefaAsync(tarefa, CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        var result = await _servicoTarefa.NovaTarefa(tarefa, guidUsuarioLogado) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task NovaTarefa_DeveRetornarErro_QuandoCamposInvalidos()
    {
        // Arrange
        Guid guidUsuarioLogado = Guid.NewGuid();
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            Titulo = string.Empty,
            Guid = Guid.NewGuid(),
            Descricao = "descricao_teste",
            DataInicio = DateTime.Today.AddDays(-1),
            GuidUsuario = guidUsuarioLogado
        };

        // Act
        var result = await _servicoTarefa.NovaTarefa(tarefa, guidUsuarioLogado) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task AtualizarTarefa_DeveAtualizarTarefa_QuandoCamposValidos()
    {
        // Arrange
        Guid guidUsuarioLogado = Guid.NewGuid();
        var tarefaOriginal = new Contexto.Tarefa.Entidades.Tarefa
        {
            Guid = Guid.NewGuid(),
            Titulo = "titulo_teste",
            Descricao = "descricao_teste",
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(1),
            GuidUsuario = guidUsuarioLogado,
            Status = StatusTarefa.Pendente
        };

        // Configura o mock para retornar a tarefa original quando consultada pelo GUID
        _mockRepositorio.Setup(r => r.ObterTarefaPorGuidAsync(tarefaOriginal.Guid, It.IsAny<CancellationToken>())).ReturnsAsync(tarefaOriginal);

        // Modifica a tarefa
        var tarefaAtualizada = new Contexto.Tarefa.Entidades.Tarefa
        {
            Guid = tarefaOriginal.Guid,
            Titulo = "titulo_atualizado",
            Descricao = "descricao_atualizada",
            DataInicio = tarefaOriginal.DataInicio.AddDays(1),
            DataFim = tarefaOriginal.DataFim.AddDays(1),
            GuidUsuario = guidUsuarioLogado,
            Status = StatusTarefa.Concluida
        };

        // Configura o mock para atualizar a tarefa e aplicar as mudanças ao objeto original
        _mockRepositorio.Setup(r => r.AtualizarTarefaAsync(It.IsAny<Contexto.Tarefa.Entidades.Tarefa>(), It.IsAny<CancellationToken>()))
            .Callback<Contexto.Tarefa.Entidades.Tarefa, CancellationToken>((t, token) =>
            {
                tarefaOriginal.Titulo = t.Titulo;
                tarefaOriginal.Descricao = t.Descricao;
                tarefaOriginal.DataInicio = t.DataInicio;
                tarefaOriginal.DataFim = t.DataFim;
                tarefaOriginal.Status = t.Status;
            })
            .ReturnsAsync(1);

        // Act
        var result = await _servicoTarefa.AtualizarTarefa(tarefaAtualizada, guidUsuarioLogado) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("titulo_atualizado", tarefaOriginal.Titulo);
        Assert.Equal("descricao_atualizada", tarefaOriginal.Descricao);
        Assert.Equal(tarefaAtualizada.DataInicio, tarefaOriginal.DataInicio);
        Assert.Equal(tarefaAtualizada.DataFim, tarefaOriginal.DataFim);
        Assert.Equal(StatusTarefa.Concluida, tarefaOriginal.Status);
    }

    [Fact]
    public async Task AtualizarTarefa_DeveRetornarNaoAutorizado_QuandoUsuarioDiferente()
    {
        // Arrange
        var guidUsuario = Guid.NewGuid();
        var guidUsuarioLogado = Guid.NewGuid();
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            Guid = Guid.NewGuid(),
            Titulo = "titulo_teste",
            Descricao = "descricao_teste",
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(1),
            GuidUsuario = guidUsuario
        };

        _mockRepositorio.Setup(r => r.ObterTarefaPorGuidAsync(tarefa.Guid, It.IsAny<CancellationToken>())).ReturnsAsync(tarefa);

        // Act
        var result = await _servicoTarefa.AtualizarTarefa(tarefa, guidUsuarioLogado) as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public async Task AtualizarTarefa_DeveRetornarNaoEncontrado_QuandoTarefaNaoExiste()
    {
        // Arrange
        var guidUsuario = Guid.NewGuid();
        var guidUsuarioLogado = Guid.NewGuid();
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            Guid = Guid.NewGuid(),
            Titulo = "titulo_teste",
            Descricao = "descricao_teste",
            DataInicio = DateTime.Now,
            DataFim = DateTime.Now.AddDays(1),
            GuidUsuario = guidUsuario
        };

        _mockRepositorio.Setup(r => r.ObterTarefaPorGuidAsync(tarefa.Guid, It.IsAny<CancellationToken>())).ReturnsAsync((Contexto.Tarefa.Entidades.Tarefa)null);

        // Act
        var result = await _servicoTarefa.AtualizarTarefa(tarefa, guidUsuarioLogado) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }
}
