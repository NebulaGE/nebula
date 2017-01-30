
using System.Collections.Generic;
using System.Data.Entity; 

namespace Nebula.Domain.Abstract
{
    public abstract class BaseRepository<T> where T : class, new()
    {
        public IDbContextNebula _context;

        internal BaseRepository(IDbContextNebula context)
        {
            if (_context == null) _context = context;
        }

        public virtual T Fetch(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual T Fetch(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IDbSet<T> Set()
        {
            return _context.Set<T>();
        }

        public virtual void Save(T entity)
        {
            Save(_context.Set<T>(), entity);
        }

        public virtual void Delete(int id)
        {
            Delete(Fetch(id));
        }

        public virtual void Delete(T entity)
        {
            Delete(_context.Set<T>(), entity);
        }

        protected virtual void Save(IDbSet<T> set, T entity)
        {
            var entry = _context.Entry(entity);
            if (entry == null || entry.State == EntityState.Detached) set.Add(entity);
            _context.SaveChanges();
        }

        protected virtual void Delete(IDbSet<T> set, T entity)
        {
            set.Remove(entity);
            _context.SaveChanges();
        }
    }
}
