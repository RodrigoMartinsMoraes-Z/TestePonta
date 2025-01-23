using MockQueryable.Moq;

using Moq;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;

using System.Security.Cryptography;
using System.Text;

namespace Ponta.Teste.Usuario.Repositorio;
public class TesteRepositorioUsuario
{
    private readonly Mock<IContextoUsuario> _mockContexto;
    private readonly RepositorioUsuario _repositorio;

    public TesteRepositorioUsuario()
    {
        _mockContexto = new Mock<IContextoUsuario>();
        _repositorio = new RepositorioUsuario(_mockContexto.Object);
    }

    [Fact]
    public async Task ObterUsuarioPorGuidAsync_DeveRetornarUsuario()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Login = "login de teste",
            Senha = "senha de teste",
            Nome = "nome de teste"
        };

        var usuarios = new List<Contexto.Usuario.Entidades.Usuario> { usuario }.AsQueryable().BuildMockDbSet();

        _mockContexto.Setup(c => c.Usuarios).Returns(usuarios.Object);

        // Act
        var result = await _repositorio.ObterUsuarioPorGuidAsync(usuario.Guid);

        // Assert
        Assert.Equal(usuario, result);
    }

    [Fact]
    public async Task ExcluirUsuarioAsync_DeveExcluirUsuario()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Login = "login de teste",
            Senha = "senha de teste",
            Nome = "nome de teste"
        };

        var usuarios = new List<Contexto.Usuario.Entidades.Usuario> { usuario }.AsQueryable().BuildMockDbSet();

        _mockContexto.Setup(c => c.Usuarios).Returns(usuarios.Object);
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repositorio.ExcluirUsuarioAsync(usuario);

        // Assert
        _mockContexto.Verify(c => c.Usuarios.Remove(usuario), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task AtualizarUsuarioAsync_DeveAtualizarUsuario()
    {
        var guidTeste = Guid.NewGuid();

        // Arrange
        var usuarioOriginal = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = guidTeste,
            Login = "login de teste",
            Senha = "senha de teste",
            Nome = "nome de teste"
        };

        var usuarios = new List<Contexto.Usuario.Entidades.Usuario> { usuarioOriginal }.AsQueryable().BuildMockDbSet();
        _mockContexto.Setup(c => c.Usuarios).Returns(usuarios.Object);

        // Modifica o usuário
        usuarioOriginal.Login = "login atualizado";
        usuarioOriginal.Senha = "senha atualizada";
        usuarioOriginal.Nome = "nome atualizado";

        _mockContexto.Setup(c => c.Usuarios.Update(usuarioOriginal));
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        string senhaCriptografadaEsperada = GerarSenhaCriptografada("senha atualizada");

        // Act
        var result = await _repositorio.AtualizarUsuarioAsync(usuarioOriginal);

        // Assert
        _mockContexto.Verify(c => c.Usuarios.Update(usuarioOriginal), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
        Assert.Equal("login atualizado", usuarioOriginal.Login);
        Assert.Equal(senhaCriptografadaEsperada, usuarioOriginal.Senha);
        Assert.Equal("nome atualizado", usuarioOriginal.Nome);
    }

    [Fact]
    public async Task SalvarUsuarioAsync_DeveSalvarUsuario()
    {
        // Arrange
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Guid = Guid.NewGuid(),
            Login = "login de teste",
            Senha = "senha de teste",
            Nome = "nome de teste"
        };

        _mockContexto.Setup(c => c.Usuarios.Add(usuario));
        _mockContexto.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _repositorio.SalvarUsuarioAsync(usuario);

        // Assert
        _mockContexto.Verify(c => c.Usuarios.Add(usuario), Times.Once);
        _mockContexto.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(1, result);
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
