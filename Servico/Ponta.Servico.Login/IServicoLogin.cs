using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;

namespace Ponta.Servico.Login;
public interface IServicoLogin
{
    Task<IActionResult> LoginAsync(string login, string senha);
}
