using System;
using System.Linq;

namespace Ponta.Servico.Login;
public interface IServicoLogin
{
    Task<HttpResponseMessage> LoginAsync(string login, string senha);
}
