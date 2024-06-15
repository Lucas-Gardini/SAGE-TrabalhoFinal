using SQLite;
using System.ComponentModel;

namespace SAGE.Modules.Disciplinas
{
    [Table("notas")]
    public class Notas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int DisciplinaId { get; set; }

        [NotNull]
        public int AlunoId { get; set; }

        [NotNull]
        public double Nota { get; set; }

        [NotNull, DefaultValue(true)]
        public bool Prova { get; set; } = true;
    }
}