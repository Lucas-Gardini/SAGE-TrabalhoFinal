using SAGE.Modules.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAGE.Modules.Usuarios
{
    internal class UsuariosService : GenericService<Usuario>
    {
        public UsuariosService()
        {
            BeforeInsert = (entity) => entity.Senha = BCrypt.Net.BCrypt.HashPassword(entity.Senha);
        }

        public override void CreateDefaultResources()
        {
            this.InsertIfNotExists(p => p.Nome == "Master", new Usuario()
            {
                Nome = "Master",
                Senha = "master"
            });
        }
    }
}
