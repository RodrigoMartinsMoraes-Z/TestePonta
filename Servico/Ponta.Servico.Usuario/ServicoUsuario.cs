using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Servico.Usuario
{
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

                if (usuarioExistente is null)
                    return new NotFoundResult();

                return new OkObjectResult(usuarioExistente);
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
                if (await CamposInvalidos(usuario))
                    return new BadRequestResult();

                await repositorio.SalvarUsuarioAsync(usuario);

                return new OkObjectResult(usuario);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
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
}
