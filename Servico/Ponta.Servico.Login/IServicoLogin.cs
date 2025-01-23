using Microsoft.AspNetCore.Mvc;

namespace Ponta.Servico.Login;
public interface IServicoLogin
{
    Task<IActionResult> LoginAsync(string login, string senha);
}
