using Ponta.Contexto.Usuario.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Servico.Login;
public class ServicoLogin(IRepositorioUsuario repositorio)
{
    public async Task<HttpResponseMessage> LoginAsync(string login, string senha)
    {
        var usuario = await repositorio.ObterUsuarioPorLoginAsync(login);

        if (usuario == null)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        if (usuario.Senha != GerarSenhaCriptografada(senha))
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        return new HttpResponseMessage(HttpStatusCode.OK);
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
