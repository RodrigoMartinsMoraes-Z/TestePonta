using Microsoft.EntityFrameworkCore;

namespace Ponta.Contexto.Usuario.Interfaces;
public interface IContextoUsuario
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Usuario.Entidades.Usuario> Usuarios { get; set; }
}
