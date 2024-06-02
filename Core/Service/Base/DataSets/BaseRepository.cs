using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace Core.Service.Base.DataSets
{
    public abstract class BaseRepository<C, T> : IBaseRepository<T>, IDisposable
                                        where C : DbContext, new()
                                        where T : class, new()
    {
        private C Context { get; set; } = new C();

        public virtual IQueryable<T> All() {
            return Context.Set<T>();
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = Context.Set<T>();

            foreach (var includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            return queryable;
        }

        public virtual T Find(int id) {
            return Context.Set<T>().Find(new object[] {
                id
            });
        }

        public virtual T Find(Guid id) {
            return Context.Set<T>().Find(new object[] {
                id
            });
        }

        public virtual void Add(T entity) {
            Context.Set<T>().Add(entity);
        }

        public virtual void Add(IEnumerable<T> entities) {
            foreach (var entity in entities)
                Context.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity) {
            if (Context.Set<T>().Local.Any(e => e == entity)) {
                Context.Entry(entity).State = EntityState.Modified;
            }
            else {
                Context.Update(entity);
            }
        }

        public virtual void Edit(IEnumerable<T> entities) {
            foreach (var entity in entities) {
                if (Context.Set<T>().Local.Any(e => e == entity)) {
                    Context.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    Context.Update(entity);
                }
            }
        }

        public virtual void Edit(T entity, params Expression<Func<T, object>>[] propertiesToUpdate) {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached) {
                Context.Attach(entity);
                entry.State = EntityState.Modified;
            }

            foreach (var property in propertiesToUpdate)
                entry.Property(property).IsModified = true;
        }

        public virtual void Edit(IEnumerable<T> entities, params Expression<Func<T, object>>[] propertiesToUpdate) {
            foreach (var entity in entities)
                Edit(entity, propertiesToUpdate);
        }

        public virtual void Delete(int id)
        {
            T t = Context.Set<T>().Find(new object[] {
                id
            });
            Context.Set<T>().Remove(t);
        }

        public virtual void Delete(Guid id) {
            T t = Context.Set<T>().Find(new object[] {
                id
            });
            Context.Set<T>().Remove(t);
        }

        public virtual void Save() {
            Context.SaveChanges();
        }

        public virtual async Task SaveAsync() {
            await Context.SaveChangesAsync();
        }

        public virtual void Dispose() {
            Context.Dispose();
        }
    }

    public interface IBaseRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Find(int id);

        T Find(Guid id);

        void Add(T entity);

        void Add(IEnumerable<T> entity);

        void Edit(T entity);

        void Edit(IEnumerable<T> entity);

        void Delete(int id);

        void Delete(Guid id);

        void Save();
    }
}
