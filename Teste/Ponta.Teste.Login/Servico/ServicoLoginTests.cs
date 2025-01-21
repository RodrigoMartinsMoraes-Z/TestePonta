using Moq;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Servico.Login;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Teste.Login.Servico;
public class ServicoLoginTests
{
    private readonly Mock<IRepositorioUsuario> _mockRepositorio;
    private readonly ServicoLogin _servicoLogin;

    public ServicoLoginTests()
    {
        _mockRepositorio = new Mock<IRepositorioUsuario>();
        _servicoLogin = new ServicoLogin(_mockRepositorio.Object);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarOk_QuandoLoginBemSucedido()
    { // Arrange
        var login = "login_teste";
        var senha = "senha_teste";
        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Login = login,
            Senha = senha,
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorLoginAsync(login, CancellationToken.None)).ReturnsAsync(usuario);

        // Act
        var response = await _servicoLogin.LoginAsync(login, senha);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarNotFound_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        var login = "login_inexistente";
        var senha = "senha_teste";

        _mockRepositorio.Setup(r => r.ObterUsuarioPorLoginAsync(login, CancellationToken.None)).ReturnsAsync((Contexto.Usuario.Entidades.Usuario)null);

        // Act
        var response = await _servicoLogin.LoginAsync(login, senha);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarUnauthorized_QuandoSenhaIncorreta()
    {
        // Arrange
        var login = "login_teste";
        var senhaCorreta = "senha_correta";
        var senhaIncorreta = "senha_incorreta";

        var usuario = new Contexto.Usuario.Entidades.Usuario
        {
            Login = login,
            Senha = senhaCorreta,
            Nome = "nome_teste"
        };

        _mockRepositorio.Setup(r => r.ObterUsuarioPorLoginAsync(login, CancellationToken.None)).ReturnsAsync(usuario);

        // Act
        var response = await _servicoLogin.LoginAsync(login, senhaIncorreta);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


}
