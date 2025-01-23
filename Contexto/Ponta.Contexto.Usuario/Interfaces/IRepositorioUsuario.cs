namespace Ponta.Contexto.Usuario.Interfaces;
public interface IRepositorioUsuario
{
    Task<int> AtualizarUsuarioAsync(Entidades.Usuario usuario, CancellationToken cancellationToken = default);
    Task<int> ExcluirUsuarioAsync(Entidades.Usuario usuario, CancellationToken cancellationToken = default);
    Task<Entidades.Usuario?> ObterUsuarioPorGuidAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<Entidades.Usuario?> ObterUsuarioPorLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<int> SalvarUsuarioAsync(Entidades.Usuario usuario, CancellationToken cancellationToken = default);
}
