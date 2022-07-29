using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace MicronTMS.DLL.Implementation
{
    public class Repository<T>
    {
        public Repository()
        {
        }

        public IQueryable<T> GetAll(ISession session)
        {
            return session.Query<T>();
        }

        public virtual IQueryable<T> GetManyQueryable(Func<T, bool> where, ISession session)
        {
            return session.Query<T>().Where(where).AsQueryable();
        }

        public T GetById(string id, ISession session)
        {
            return session.Get<T>(id);
        }

        public void Create(T entity, ISession session)
        {
            session.Save(entity);
        }

        public void Update(T entity, ISession session)
        {
            session.Update(entity);
        }

        public void Delete(string id, ISession session)
        {
            session.Delete(session.Load<T>(id));
        }

        public void DeleteBySql(string sql, ISession session)
        {
            var query = session.CreateSQLQuery(sql);
            int res = query.ExecuteUpdate();
        }
    }
}