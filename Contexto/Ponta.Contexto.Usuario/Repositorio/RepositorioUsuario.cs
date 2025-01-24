using Microsoft.EntityFrameworkCore;

using Ponta.Contexto.Usuario.Interfaces;

namespace Ponta.Contexto.Usuario.Repositorio;

public class RepositorioUsuario(IContextoUsuario contexto)
: IRepositorioUsuario
{
    public async Task<int> SalvarUsuarioAsync(Usuario.Entidades.Usuario usuario, CancellationToken cancellationToken = default)
    {
        _ = contexto.Usuarios.Add(usuario);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AtualizarUsuarioAsync(Usuario.Entidades.Usuario usuario, CancellationToken cancellationToken = default)
    {
        _ = contexto.Usuarios.Update(usuario);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> ExcluirUsuarioAsync(Usuario.Entidades.Usuario usuario, CancellationToken cancellationToken = default)
    {
        _ = contexto.Usuarios.Remove(usuario);
        return await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<Usuario.Entidades.Usuario?> ObterUsuarioPorGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios.FirstOrDefaultAsync(u => u.Guid == guid, cancellationToken);
    }

    public async Task<Usuario.Entidades.Usuario?> ObterUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios.FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }
}
