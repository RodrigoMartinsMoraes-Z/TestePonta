using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Contexto.Tarefa.Repositorio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace Ponta.Teste.Tarefa.Repositorio
{
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
            var tarefa = new Contexto.Tarefa.Entidades.Tarefa
            {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Baixa,
            };
            _mockContexto.Setup(c => c.Tarefas.Update(tarefa));
            _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            // Act
            var result = await _repositorio.AtualizarTarefaAsync(tarefa);
            // Assert
            _mockContexto.Verify(c => c.Tarefas.Update(tarefa), Times.Once);
            _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(1, result);
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
}
