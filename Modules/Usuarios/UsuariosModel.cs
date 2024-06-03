using SQLite;

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
}
