using SQLite;

namespace SAGE.Modules.Generic
{
    /// <summary>
    /// Serviço genérico para operações CRUD usando SQLite.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public class GenericService<T> where T : class, new()
    {
        // Caminho para o banco de dados
        public readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "sage.db3");

        /// <summary>
        /// Ação a ser executada antes de inserir uma entidade.
        /// </summary>
        public Action<T> BeforeInsert { get; set; } = (entity) => { };

        /// <summary>
        /// Ação a ser executada depois de inserir uma entidade.
        /// </summary>
        public Action<T> AfterInsert { get; set; } = (entity) => { };

        /// <summary>
        /// Ação a ser executada antes de atualizar uma entidade.
        /// </summary>
        public Action<T> BeforeUpdate { get; set; } = (entity) => { };

        /// <summary>
        /// Ação a ser executada depois de atualizar uma entidade.
        /// </summary>
        public Action<T> AfterUpdate { get; set; } = (entity) => { };

        /// <summary>
        /// Ação a ser executada antes de deletar uma entidade.
        /// </summary>
        public Action<T> BeforeDelete { get; set; } = (entity) => { };

        /// <summary>
        /// Ação a ser executada depois de deletar uma entidade.
        /// </summary>
        public Action<T> AfterDelete { get; set; } = (entity) => { };

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GenericService{T}"/>.
        /// </summary>
        public GenericService()
        {
            InitializeService();
            CreateDefaultResources();
        }

        /// <summary>
        /// Método virtual para criar recursos padrão. Pode ser sobrescrito em subclasses.
        /// </summary>
        public virtual void CreateDefaultResources() { }

        /// <summary>
        /// Inicializa o serviço criando a tabela no banco de dados.
        /// </summary>
        private void InitializeService()
        {
            using var connection = new SQLiteConnection(this.DbPath);
            connection.CreateTable<T>();
        }

        /// <summary>
        /// Insere uma entidade no banco de dados.
        /// </summary>
        /// <param name="entity">Entidade a ser inserida.</param>
        public void InsertOne(T entity)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            BeforeInsert(entity);
            connection.Insert(entity);
            AfterInsert(entity);
        }

        /// <summary>
        /// Insere uma entidade no banco de dados se não existir.
        /// </summary>
        /// <param name="predicate">Predicado para verificar se a entidade existe.</param>
        /// <param name="entity">Entidade a ser inserida.</param>
        public void InsertIfNotExists(Func<T, bool> predicate, T entity)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            if (!connection.Table<T>().Any(predicate))
            {
                BeforeInsert(entity);
                connection.Insert(entity);
                AfterInsert(entity);
            }
        }

        /// <summary>
        /// Insere várias entidades no banco de dados.
        /// </summary>
        /// <param name="entities">Coleção de entidades a serem inseridas.</param>
        public void InsertMany(IEnumerable<T> entities)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            foreach (var entity in entities)
            {
                BeforeInsert(entity);
                connection.Insert(entity);
                AfterInsert(entity);
            }
        }

        /// <summary>
        /// Atualiza uma entidade no banco de dados.
        /// </summary>
        /// <param name="entity">Entidade a ser atualizada.</param>
        public void UpdateOne(T entity)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            BeforeUpdate(entity);
            connection.Update(entity);
            AfterUpdate(entity);
        }

        /// <summary>
        /// Atualiza várias entidades no banco de dados.
        /// </summary>
        /// <param name="predicate">Predicado para selecionar entidades.</param>
        /// <param name="action">Ação a ser executada em cada entidade antes de atualizar.</param>
        public void UpdateMany(Func<T, bool> predicate, Action<T> action)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            var entities = connection.Table<T>().Where(predicate);
            foreach (var entity in entities)
            {
                action(entity);
                BeforeUpdate(entity);
                connection.Update(entity);
                AfterUpdate(entity);
            }
        }

        /// <summary>
        /// Deleta uma entidade do banco de dados.
        /// </summary>
        /// <param name="entity">Entidade a ser deletada.</param>
        public void DeleteOne(T entity)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            BeforeDelete(entity);
            connection.Delete(entity);
            AfterDelete(entity);
        }

        /// <summary>
        /// Deleta várias entidades do banco de dados.
        /// </summary>
        /// <param name="predicate">Predicado para selecionar entidades a serem deletadas.</param>
        public void DeleteMany(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            var entities = connection.Table<T>().Where(predicate);
            foreach (var entity in entities)
            {
                BeforeDelete(entity);
                connection.Delete(entity);
                AfterDelete(entity);
            }
        }

        /// <summary>
        /// Obtém uma entidade do banco de dados.
        /// </summary>
        /// <param name="predicate">Predicado para selecionar a entidade.</param>
        /// <returns>A entidade que satisfaz o predicado.</returns>
        public T? GetOne(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            return connection.Table<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Obtém todas as entidades do banco de dados.
        /// </summary>
        /// <returns>Lista de todas as entidades.</returns>
        public List<T> GetMany()
        {
            using var connection = new SQLiteConnection(this.DbPath);
            return connection.Table<T>().ToList();
        }

        /// <summary>
        /// Obtém várias entidades do banco de dados com base em um predicado.
        /// </summary>
        /// <param name="predicate">Predicado para selecionar as entidades.</param>
        /// <returns>Lista de entidades que satisfazem o predicado.</returns>
        public List<T> GetMany(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            return connection.Table<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Obtém uma entidade do banco de dados pelo ID.
        /// </summary>
        /// <param name="id">ID da entidade.</param>
        /// <returns>A entidade com o ID especificado.</returns>
        public T GetById(int id)
        {
            using var connection = new SQLiteConnection(this.DbPath);
            return connection.Get<T>(id);
        }
    }
}
