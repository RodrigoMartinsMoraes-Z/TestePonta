using Microsoft.AspNetCore.Mvc;

using Moq;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Servico.Usuario;

using System.Security.Cryptography;
using System.Text;

namespace Ponta.Teste.Usuario.Servico;
public class TesteServicoUsuario
{
    private readonly Mock<IRepositorioUsuario> _mockRepositorio;
    private readonly ServicoUsuario _servicoUsuario;

    public TesteServicoUsuario()
    {
        _mockRepositorio = new Mock<IRepositorioUsuario>();
        _servicoUsuario = new ServicoUsuario(_mockRepositorio.Object);
    }

    [Fact]
    public async Task SalvarUsuarioAsync_DeveSalvarUsuario_QuandoTodosCamposPreenchidos()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.SalvarUsuarioAsync(usuario, CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        var result = await _servicoUsuario.SalvarUsuarioAsync(usuario) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task SalvarUsuarioAsync_DeveRetornarErro_QuandoFaltarCampos()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Login = "login_teste",
            Senha = "senha_teste"
        };

        _mockRepositorio.Setup(r => r.SalvarUsuarioAsync(usuario, CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        var result = await _servicoUsuario.SalvarUsuarioAsync(usuario) as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public async Task ObterUsuarioPorGuidAsync_DeveRetornarUsuario_QuandoUsuarioExistente()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(guid, CancellationToken.None)).ReturnsAsync(usuario);
        // Act
        var result = await _servicoUsuario.ObterUsuarioPorGuidAsync(guid) as OkObjectResult;

        // Assert

        Assert.NotNull(result);
        Assert.Equal(usuario, result.Value);
    }

    [Fact]
    public async Task ObterUsuarioPorGuidAsync_DeveRetornarNotFound_QuandoUsuarioInexistente()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(guid, CancellationToken.None)).ReturnsAsync(usuario);
        // Act
        var result = await _servicoUsuario.ObterUsuarioPorGuidAsync(Guid.NewGuid()) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task ExcluirUsuarioAsync_DeveExcluirUsuario_QuandoExistir()
    {
        var guid = Guid.NewGuid();
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(guid, CancellationToken.None)).ReturnsAsync(usuario);
        _mockRepositorio.Setup(r => r.ExcluirUsuarioAsync(usuario, CancellationToken.None)).Returns(Task.FromResult(1));

        // Act
        var result = await _servicoUsuario.ExcluirUsuarioAsync(guid) as OkResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task ExcluirUsuarioAsync_DeveRetornarErro_QuandoNaoExistir()
    {
        var guid = Guid.NewGuid();
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(guid, CancellationToken.None)).ReturnsAsync((Contexto.Usuario.Entidades.Usuario)null);

        // Act
        var result = await _servicoUsuario.ExcluirUsuarioAsync(guid) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task AtualizarUsuarioAsync_DeveAtualizarUsuario_QuandoExistir()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var usuarioOriginal = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_teste",
            Senha = "senha_teste",
            Nome = "nome_teste",
            Id = 1
        };

        // Configura o mock para retornar o usuário original quando consultado pelo GUID
        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(guid, It.IsAny<CancellationToken>())).ReturnsAsync(usuarioOriginal);

        // Modifica o usuário
        var usuarioAtualizado = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guid,
            Login = "login_atualizado",
            Senha = "senha_atualizada",
            Nome = "nome_atualizado",
            Id = 1
        };

        // Configura o mock para atualizar o usuário e aplicar as mudanças ao objeto original
        _mockRepositorio.Setup(r => r.AtualizarUsuarioAsync(It.IsAny<Contexto.Usuario.Entidades.Usuario>(), It.IsAny<CancellationToken>()))
            .Callback<Contexto.Usuario.Entidades.Usuario, CancellationToken>((u, token) =>
            {
                usuarioOriginal.Login = u.Login;
                usuarioOriginal.Senha = "senha_atualizada";
                usuarioOriginal.Nome = u.Nome;
            })
            .ReturnsAsync(1);

        string senhaCriptografadaEsperada = GerarSenhaCriptografada("senha_atualizada");

        // Act
        var result = await _servicoUsuario.AtualizarUsuarioAsync(usuarioAtualizado) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("login_atualizado", usuarioOriginal.Login);
        Assert.Equal("nome_atualizado", usuarioOriginal.Nome);
        Assert.Equal(senhaCriptografadaEsperada, usuarioOriginal.Senha); // Comparar a senha criptografada
    }

    [Fact]
    public async Task AtualizarUsuarioAsync_DeveRetornarNotFound_QuandoNaoExistir()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Nome = "nome_teste",
            Id = 1,
            Login = "login_teste",
            Senha = "senha_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorGuidAsync(usuario.Guid, CancellationToken.None)).ReturnsAsync((Contexto.Usuario.Entidades.Usuario)null);

        // Act
        var result = await _servicoUsuario.AtualizarUsuarioAsync(usuario) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    private static string GerarSenhaCriptografada(string senha)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(senha);

        byte[] hash = SHA256.HashData(bytes);

        StringBuilder builder = new();
        foreach (byte b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}
