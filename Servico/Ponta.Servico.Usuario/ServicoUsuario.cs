using Microsoft.AspNetCore.Mvc;

using Ponta.Contexto.Usuario.Interfaces;

namespace Ponta.Servico.Usuario;

public class ServicoUsuario : IServicoUsuario
{
    private readonly IRepositorioUsuario repositorio;

    public ServicoUsuario(IRepositorioUsuario repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<IActionResult> AtualizarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(usuario.Guid);

            if (usuarioExistente is null)
                return new NotFoundResult();

            await repositorio.AtualizarUsuarioAsync(usuario);

            return new OkObjectResult(usuario);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> ExcluirUsuarioAsync(Guid guid)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(guid);

            if (usuarioExistente is null)
                return new NotFoundResult();

            await repositorio.ExcluirUsuarioAsync(usuarioExistente);

            return new OkResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> ObterUsuarioPorGuidAsync(Guid guid)
    {
        try
        {
            var usuarioExistente = await repositorio.ObterUsuarioPorGuidAsync(guid);

            return usuarioExistente is null ? new NotFoundResult() : new OkObjectResult(usuarioExistente);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> SalvarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        try
        {
            if (CamposInvalidos(usuario))
                return new BadRequestResult();

            await repositorio.SalvarUsuarioAsync(usuario);

            return new OkObjectResult(usuario);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    private bool CamposInvalidos(Contexto.Usuario.Entidades.Usuario usuario)
    {
        return string.IsNullOrWhiteSpace(usuario.Senha) ||
            string.IsNullOrWhiteSpace(usuario.Nome) ||
            string.IsNullOrWhiteSpace(usuario.Login);
    }
}
