using SQLite;
using System.ComponentModel;

namespace SAGE.Modules.Usuarios
{
    [Table("usuarios")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string? Identificador { get; set; } = null;

        [MaxLength(100), NotNull]
        public string Nome { get; set; } = "";

        [MaxLength(100), NotNull]
        public string Senha { get; set; } = "";
    }

    [Table("sessoes")]
    public class Sessao
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string? Token { get; set; } = null;

        [NotNull]
        public int UsuarioId { get; set; }

        [NotNull]
        public string DataCriacao { get; set; } = "";

        [NotNull]
        public string DataExpiracao { get; set; } = "";
    }
}
