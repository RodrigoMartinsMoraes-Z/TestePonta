using Microsoft.AspNetCore.Mvc;

namespace Ponta.Servico.Usuario;
public interface IServicoUsuario
{
    Task<IActionResult> SalvarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario);
    Task<IActionResult> AtualizarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario);
    Task<IActionResult> ExcluirUsuarioAsync(Guid guid);
    Task<IActionResult> ObterUsuarioPorGuidAsync(Guid guid);
}
