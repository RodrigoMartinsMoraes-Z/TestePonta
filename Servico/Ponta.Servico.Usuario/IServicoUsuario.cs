using System;
using System.Linq;

namespace Ponta.Servico.Usuario;
public interface IServicoUsuario
{
    Task<HttpResponseMessage> SalvarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario);
    Task<HttpResponseMessage> AtualizarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario);
    Task<HttpResponseMessage> ExcluirUsuarioAsync(Guid guid);
    Task<HttpResponseMessage> ObterUsuarioPorGuidAsync(Guid guid);
}
