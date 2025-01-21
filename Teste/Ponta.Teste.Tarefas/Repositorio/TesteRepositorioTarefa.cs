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
using System.Timers;

using Xunit;

namespace Ponta.Teste.Tarefas.Repositorio
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
        public async Task ObterTarefasAsync_DeveRetornarTarefas()
        {
            // Arrange
            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Baixa,
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasAsync();

            // Assert
            Assert.Equal(await tarefas.Object.CountAsync(), result.Count());
            Assert.Equal(await tarefas.Object.FirstOrDefaultAsync(), result.First());
        }

        [Fact]
        public async Task ObterTarefasPorStatusAsync_DeveRetornarTarefasComOStatusPendente()
        {
            // Arrange
            const int QUANTIDADE_PENDENTE = 2;

            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Baixa,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 2 ",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Cancelada,
                Prioridade = Prioridade.Alta,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 3",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Urgente,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 4",
                Descricao = "descricao de teste 4",
                Status = StatusTarefa.Concluida,
                Prioridade = Prioridade.Normal,
                }

            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasPorStatusAsync(StatusTarefa.Pendente);

            // Assert
            Assert.Equal(QUANTIDADE_PENDENTE, result.Count());
            Assert.Equivalent(tarefas.Object.Where(t => t.Status == StatusTarefa.Pendente), result);
        }

        [Fact]
        public async Task ObterTarefasPorStatusAsync_DeveRetornarTarefasComOStatusCancelada()
        {
            // Arrange
            const int QUANTIDADE_CANCELADA = 2;

            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.Cancelada,
                Prioridade = Prioridade.Baixa,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 2 ",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Cancelada,
                Prioridade = Prioridade.Alta,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 3",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Urgente,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 4",
                Descricao = "descricao de teste 4",
                Status = StatusTarefa.Concluida,
                Prioridade = Prioridade.Normal,
                }

            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasPorStatusAsync(StatusTarefa.Cancelada);

            // Assert
            Assert.Equal(QUANTIDADE_CANCELADA, result.Count());
            Assert.Equivalent(tarefas.Object.Where(t => t.Status == StatusTarefa.Cancelada), result);
        }

        [Fact]
        public async Task ObterTarefasPorStatusAsync_DeveRetornarTarefasComOStatusConcluida()
        {
            // Arrange

            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Baixa,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 2 ",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Cancelada,
                Prioridade = Prioridade.Alta,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 3",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Urgente,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 4",
                Descricao = "descricao de teste 4",
                Status = StatusTarefa.Concluida,
                Prioridade = Prioridade.Normal,
                }

            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasPorStatusAsync(StatusTarefa.Concluida);

            // Assert
            Assert.Single(result);
            Assert.Equivalent(tarefas.Object.Where(t => t.Status == StatusTarefa.Concluida), result);
        }

        [Fact]
        public async Task ObterTarefasPorStatusAsync_DeveRetornarTarefasComOStatusEmAndamento()
        {
            // Arrange

            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste",
                Descricao = "descricao de teste",
                Status = StatusTarefa.EmAndamento,
                Prioridade = Prioridade.Baixa,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 2 ",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Cancelada,
                Prioridade = Prioridade.Alta,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 3",
                Descricao = "descricao de teste 3",
                Status = StatusTarefa.Pendente,
                Prioridade = Prioridade.Urgente,
                },
                new ()
                {
                DataCriacao = DateTime.Today,
                DataInicio = DateTime.Today,
                Titulo = "titulo de teste 4",
                Descricao = "descricao de teste 4",
                Status = StatusTarefa.Concluida,
                Prioridade = Prioridade.Normal,
                }

            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasPorStatusAsync(StatusTarefa.EmAndamento);

            // Assert
            Assert.Single(result);
            Assert.Equivalent(tarefas.Object.Where(t => t.Status == StatusTarefa.EmAndamento), result);
        }

        [Fact]
        public async Task ObterTarefasPorUsuarioAsync_DeveRetornarTarefasDoUsuario()
        {
            // Arrange
            const int QUATIDADE_TAREFAS_USUARIO1 = 3;

            var guidUsuario1 = Guid.NewGuid();
            var guidUsuario2 = Guid.NewGuid();
            var tarefas = new List<Contexto.Tarefa.Entidades.Tarefa>
            {
                new()
                {
                    DataCriacao = DateTime.Today,
                    DataInicio = DateTime.Today,
                    Titulo = "titulo de teste",
                    Descricao = "descricao de teste",
                    Status = StatusTarefa.Cancelada,
                    Prioridade = Prioridade.Baixa,
                    GuidUsuario = guidUsuario1
                },
                new()
                {
                    DataCriacao = DateTime.Today,
                    DataInicio = DateTime.Today,
                    Titulo = "titulo de teste 2 ",
                    Descricao = "descricao de teste 3",
                    Status = StatusTarefa.Cancelada,
                    Prioridade = Prioridade.Alta,
                    GuidUsuario = guidUsuario2
                },
                new()
                {
                    DataCriacao = DateTime.Today,
                    DataInicio = DateTime.Today,
                    Titulo = "titulo de teste 3",
                    Descricao = "descricao de teste 3",
                    Status = StatusTarefa.Pendente,
                    Prioridade = Prioridade.Urgente,
                    GuidUsuario = guidUsuario1
                },
                new()
                {
                    DataCriacao = DateTime.Today,
                    DataInicio = DateTime.Today,
                    Titulo = "titulo de teste 4",
                    Descricao = "descricao de teste 4",
                    Status = StatusTarefa.Concluida,
                    Prioridade = Prioridade.Normal,
                    GuidUsuario = guidUsuario1
                }
            }.AsQueryable().BuildMockDbSet();

            _mockContexto.Setup(c => c.Tarefas).Returns(tarefas.Object);

            // Act
            var result = await _repositorio.ObterTarefasPorUsuarioAsync(guidUsuario1);

            // Assert
            Assert.Equal(QUATIDADE_TAREFAS_USUARIO1, result.Count());
            Assert.Equivalent(tarefas.Object.Where(t => t.GuidUsuario == guidUsuario1), result);
        }
    }
}
