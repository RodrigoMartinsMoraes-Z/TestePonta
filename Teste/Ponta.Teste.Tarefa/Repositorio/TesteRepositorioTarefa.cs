using MockQueryable.Moq;

using Moq;

using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Contexto.Tarefa.Repositorio;

namespace Ponta.Teste.Tarefa.Repositorio;

public class TesteRepositorioTarefa
{
    private readonly Mock<IContextoTarefa> _mockContexto;
    private readonly RepositorioTarefa _repositorio;

    public TesteRepositorioTarefa()
    {
        _mockContexto = new Mock<IContextoTarefa>();
        _repositorio = new RepositorioTarefa(_mockContexto.Object);
    }

    [Fact]
    public async Task SalvarTarefaAsync_DeveAdicionarTarefa()
    {
        // Arrange
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            DataCriacao = DateTime.Today,
            DataInicio = DateTime.Today,
            Titulo = "titulo de teste",
            Descricao = "descricao de teste",
            Status = StatusTarefa.Pendente,
            Prioridade = Prioridade.Baixa,
        };
        _mockContexto.Setup(c => c.Tarefas.Add(tarefa));
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repositorio.SalvarTarefaAsync(tarefa);

        // Assert
        _mockContexto.Verify(c => c.Tarefas.Add(tarefa), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task AtualizarTarefaAsync_DeveAtualizarTarefa()
    {
        // Arrange
        var tarefaOriginal = new Contexto.Tarefa.Entidades.Tarefa
        {
            DataCriacao = DateTime.Today.AddDays(-1),
            DataInicio = DateTime.Today.AddDays(-1),
            Titulo = "titulo original",
            Descricao = "descricao original",
            Status = StatusTarefa.Pendente,
            Prioridade = Prioridade.Baixa,
        };

        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa> { tarefaOriginal }.AsQueryable().BuildMockDbSet();
        _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

        // Modifica a tarefa
        tarefaOriginal.Titulo = "titulo atualizado";
        tarefaOriginal.Descricao = "descricao atualizada";
        tarefaOriginal.Status = StatusTarefa.Concluida;
        tarefaOriginal.Prioridade = Prioridade.Alta;

        _mockContexto.Setup(c => c.Tarefas.Update(tarefaOriginal));
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repositorio.AtualizarTarefaAsync(tarefaOriginal);

        // Assert
        _mockContexto.Verify(c => c.Tarefas.Update(tarefaOriginal), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
        Assert.Equal("titulo atualizado", tarefaOriginal.Titulo);
        Assert.Equal("descricao atualizada", tarefaOriginal.Descricao);
        Assert.Equal(StatusTarefa.Concluida, tarefaOriginal.Status);
        Assert.Equal(Prioridade.Alta, tarefaOriginal.Prioridade);
    }


    [Fact]
    public async Task ExcluirTarefaAsync_DeveExcluirTarefa()
    {
        // Arrange
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            DataCriacao = DateTime.Today,
            DataInicio = DateTime.Today,
            Titulo = "titulo de teste",
            Descricao = "descricao de teste",
            Status = StatusTarefa.Pendente,
            Prioridade = Prioridade.Baixa,
        };
        _mockContexto.Setup(c => c.Tarefas.Remove(tarefa));
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repositorio.ExcluirTarefaAsync(tarefa);

        // Assert
        _mockContexto.Verify(c => c.Tarefas.Remove(tarefa), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task ObterTarefaPorGuidAsync_DeveRetornarTarefa()
    {
        var guidTeste = Guid.NewGuid();

        // Arrange
        var tarefa = new Contexto.Tarefa.Entidades.Tarefa
        {
            DataCriacao = DateTime.Today,
            DataInicio = DateTime.Today,
            Titulo = "titulo de teste",
            Descricao = "descricao de teste",
            Status = StatusTarefa.Pendente,
            Prioridade = Prioridade.Baixa,
            Guid = guidTeste
        };
        var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa> { tarefa }.AsQueryable().BuildMockDbSet();

        _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

        // Act
        var result = await _repositorio.ObterTarefaPorGuidAsync(guidTeste);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guidTeste, result.Guid);
    }
}
