using Newtonsoft.Json;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Servico.Usuario;
public class ServicoUsuario(IRepositorioUsuario repositorio) : IServicoUsuario
{
    public async Task<HttpResponseMessage> AtualizarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(usuario.Guid);

            if (usuarioExistente is null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            await repositorio.AtualizarUsuarioAsync(usuario);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

    public async Task<HttpResponseMessage> ExcluirUsuarioAsync(Guid guid)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(guid);

            if (usuarioExistente is null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            await repositorio.ExcluirUsuarioAsync(usuarioExistente);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

    public async Task<HttpResponseMessage> ObterUsuarioPorGuidAsync(Guid guid)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(guid);

            if (usuarioExistente is null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                    new StringContent(JsonConvert.SerializeObject(usuarioExistente), Encoding.UTF8, "application/json")
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

    public async Task<HttpResponseMessage> SalvarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        try
        {
            if (await CamposInvalidos(usuario))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            await repositorio.SalvarUsuarioAsync(usuario);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(ex.Message)
            };
        }
    }

    private async Task<bool> CamposInvalidos(Contexto.Usuario.Entidades.Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.Senha) ||
            string.IsNullOrWhiteSpace(usuario.Nome) ||
            string.IsNullOrWhiteSpace(usuario.Login))
            return true;

        return false;
    }
}
