using MockQueryable.Moq;

using Moq;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;

namespace Ponta.Teste.Login.Repositorio;
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
    public async Task ObterUsuarioPorLoginAsync_DeveRetornarUsuario()
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
        var result = await _repositorio.ObterUsuarioPorLoginAsync(usuario.Login);

        // Assert
        Assert.Equal(usuario, result);
    }
}
