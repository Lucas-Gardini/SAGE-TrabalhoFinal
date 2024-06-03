using SAGE.Modules.Generic;

namespace SAGE.Modules.Usuarios
{
    /// <summary>
    /// Serviço de usuários que herda de <see cref="GenericService{Usuario}"/>.
    /// </summary>
    class UsuariosService : GenericService<Usuario>
    {
        // Usuário atualmente logado
        private static Usuario usuarioLogado = new();

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="UsuariosService"/>.
        /// </summary>
        public UsuariosService(): base()
        {
            // Hash da senha antes de inserir no banco de dados
            base.BeforeInsert = (entity) => {
                entity.Identificador = GerarNovoIdentificador();
                entity.Senha = BCrypt.Net.BCrypt.HashPassword(entity.Senha);
            };
        }

        /// <summary>
        /// Obtém o usuário atualmente logado.
        /// </summary>
        /// <returns>O usuário logado.</returns>
        public static Usuario GetUsuarioLogado()
        {
            return usuarioLogado;
        }

        /// <summary>
        /// Cria recursos padrão para a tabela de usuários.
        /// </summary>
        public override void CreateDefaultResources()
        {
            this.InsertIfNotExists(p => p.Identificador == "000000", new Usuario()
            {
                Identificador = "000000",
                Nome = "Master",
                Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
            });
        }

        /// <summary>
        /// Valida a senha do usuário.
        /// </summary>
        /// <param name="identificador">Identificador do usuário.</param>
        /// <param name="senha">Senha do usuário.</param>
        /// <returns>O usuário autenticado se a validação for bem-sucedida; caso contrário, um novo usuário.</returns>
        public Usuario ValidarSenha(string identificador, string senha)
        {
            var usuarioAutenticado = this.GetOne(a => a.Identificador == identificador);

            // Verifica se o usuário foi encontrado
            if (usuarioAutenticado == null)
                return new Usuario();

            // Verifica a senha
            var success = BCrypt.Net.BCrypt.Verify(senha, usuarioAutenticado.Senha);

            // Se a senha estiver correta, define o usuário logado
            if (success)
            {
                usuarioLogado = usuarioAutenticado;
                return usuarioAutenticado;
            }
            return new Usuario();
        }

        /// <summary>
        /// Gera um novo identificador incrementando o último identificador existente.
        /// </summary>
        /// <returns>O novo identificador gerado.</returns>
        public string GerarNovoIdentificador()
        {
            var ultimoUsuario = this.GetMany().OrderByDescending(u => u.Identificador).FirstOrDefault();

            if (ultimoUsuario == null || string.IsNullOrEmpty(ultimoUsuario.Identificador))
            {
                return "000001";
            }

            // Incrementa o identificador
            int novoIdNumerico = int.Parse(ultimoUsuario.Identificador) + 1;
            return novoIdNumerico.ToString("D6"); // Garante que o identificador tenha 6 dígitos
        }
    }
}
