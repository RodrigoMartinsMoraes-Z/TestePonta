using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Ponta.Contexto.Usuario.Entidades;
using Ponta.Contexto.Usuario.Interfaces;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Ponta.Servico.Login;
public class ServicoLogin(IRepositorioUsuario repositorio)
: IServicoLogin
{
    public async Task<IActionResult> LoginAsync(string login, string senha)
    {
        var usuario = await repositorio.ObterUsuarioPorLoginAsync(login);

        if (usuario == null)
        {
            return new NotFoundResult();
        }

        if (usuario.Senha != GerarSenhaCriptografada(senha))
        {
            return new UnauthorizedResult();
        }

        var token = await GerarToken(usuario);

        return new OkObjectResult(new { Token = token });
    }


    private static Task<string> GerarToken(Usuario usuario)
    {
        var key = Encoding.ASCII.GetBytes("chave-super-segura-e-bem-maior-para-teste-1234567890");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new("nameid", usuario.Guid.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Task.FromResult(tokenString);
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
