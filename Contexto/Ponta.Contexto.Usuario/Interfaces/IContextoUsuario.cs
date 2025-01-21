using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponta.Contexto.Usuario.Interfaces;
public interface IContextoUsuario
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Usuario.Entidades.Usuario> Usuarios { get; set; }
}
