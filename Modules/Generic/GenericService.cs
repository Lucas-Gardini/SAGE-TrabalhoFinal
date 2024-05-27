using SQLite;

namespace SAGE.Modules.Generic
{
    public class GenericService<T> where T : class, new()
    {
        private readonly string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "sage.db3");

        public Action<T> BeforeInsert { get; set; } = (entity) => { };
        public Action<T> AfterInsert { get; set; } = (entity) => { };

        public Action<T> BeforeUpdate { get; set; } = (entity) => { };
        public Action<T> AfterUpdate { get; set; } = (entity) => { };

        public Action<T> BeforeDelete { get; set; } = (entity) => { };
        public Action<T> AfterDelete { get; set; } = (entity) => { };

        public GenericService()
        {
            InitializeService();
            CreateDefaultResources();
        }

        public virtual void CreateDefaultResources() { }

        private void InitializeService()
        {
            using var connection = new SQLiteConnection(this._dbPath);
            connection.CreateTable<T>();
        }

        public void InsertOne(T entity)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            BeforeInsert(entity);
            connection.Insert(entity);
            AfterInsert(entity);
        }

        public void InsertIfNotExists(Func<T, bool> predicate, T entity)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            if (!connection.Table<T>().Any(predicate))
            {
                BeforeInsert(entity);
                connection.Insert(entity);
                AfterInsert(entity);
            }
        }

        public void InsertMany(IEnumerable<T> entities)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            foreach (var entity in entities)
            {
                BeforeInsert(entity);
                connection.Insert(entity);
                AfterInsert(entity);
            }
        }

        public void UpdateOne(T entity)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            BeforeUpdate(entity);
            connection.Update(entity);
            AfterUpdate(entity);
        }

        public void UpdateMany(Func<T, bool> predicate, Action<T> action)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            var entities = connection.Table<T>().Where(predicate);
            foreach (var entity in entities)
            {
                action(entity);
                BeforeUpdate(entity);
                connection.Update(entity);
                AfterUpdate(entity);
            }
        }

        public void DeleteOne(T entity)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            BeforeDelete(entity);
            connection.Delete(entity);
            AfterDelete(entity);
        }

        public void DeleteMany(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            var entities = connection.Table<T>().Where(predicate);
            foreach (var entity in entities)
            {
                BeforeDelete(entity);
                connection.Delete(entity);
                AfterDelete(entity);
            }
        }

        public T GetOne(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            return connection.Table<T>().Where(predicate).First();
        }

        public List<T> GetMany()
        {
            using var connection = new SQLiteConnection(this._dbPath);
            return connection.Table<T>().ToList();
        }

        public List<T> GetMany(Func<T, bool> predicate)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            return connection.Table<T>().Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            using var connection = new SQLiteConnection(this._dbPath);
            return connection.Get<T>(id);
        }
    }
}
