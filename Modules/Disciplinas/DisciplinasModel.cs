using SQLite;

namespace SAGE.Modules.Disciplinas
{
    [Table("disciplinas")]
    public class Disciplina
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Sigla { get; set; } = "";

        [MaxLength(100), NotNull]
        public string Nome { get; set; } = "";

        [MaxLength(100), NotNull]
        public string Professor { get; set; } = "";

        [NotNull]
        public bool Share { get; set; }

        [NotNull]
        public int AlunoId { get; set; }
    }
}
