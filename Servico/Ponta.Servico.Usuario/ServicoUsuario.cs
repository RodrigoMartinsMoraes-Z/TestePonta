using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Servico.Usuario;
public class ServicoUsuario(IRepositorioUsuario repositorio) : IServicoUsuario
{
    public Task<HttpResponseMessage> AtualizarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> ExcluirUsuarioAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> ObterUsuarioPorGuidAsync(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> SalvarUsuarioAsync(Contexto.Usuario.Entidades.Usuario usuario)
    {
        throw new NotImplementedException();
    }
}
