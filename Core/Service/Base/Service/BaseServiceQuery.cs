using Core.Service.Base.DataSets;
using System.Linq.Expressions;

namespace Core.Service.Base.Service {
    //Dependencies
    public static class BaseServiceConstants {

        public const int PageDataSizeDefault = 20;

    }

    //Properties
    public abstract partial class BaseServiceQuery<T, TR, TEnum> : IBaseServiceQuery<T>
                                     where T : BaseModel<TEnum>
                                     where TR : IBaseRepository<T>, new()
                                     where TEnum : struct, IComparable, IFormattable, IConvertible
    {

        private TR _Repository;
        protected TR Repository
        {
            get
            {
                if (_Repository == null)
                    _Repository = new TR();

                return _Repository;
            }
        }
    }

    //Main
    public abstract partial class BaseServiceQuery<T, TR, TEnum>
    {

        private int GetPageSize(int passedPageSize)
        {
            return passedPageSize == 0 ? BaseServiceConstants.PageDataSizeDefault : passedPageSize;
        }

        #region Save
        public virtual void Save()
        {
            Repository.Save();
        }

        public virtual void Save(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Repository.Add(entity);
            Repository.Save();
        }

        public virtual void Save(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            Repository.Add(entities);
            Repository.Save();
        }

        public virtual void Save(T entity, bool saveIfNotExist)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var oldEntity = Repository.Find(entity.Id);

            if (oldEntity == null && saveIfNotExist)
                Repository.Add(entity);

            Repository.Save();
        }

        public virtual void Save(IEnumerable<T> entities, bool saveIfNotExist)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {

                var oldEntity = Repository.Find(entity.Id);

                if (oldEntity == null && saveIfNotExist)
                    Repository.Add(entity);
            }
            Repository.Save();
        }


        #endregion

        #region Update
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Repository.Edit(entity);
            Repository.Save();
        }

        public virtual void Update(IEnumerable<T> entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Repository.Edit(entity);
            Repository.Save();
        }


        #endregion

        #region Delete
        public virtual void Delete(Guid id)
        {
            Repository.Delete(id);
            Repository.Save();
        }

        public virtual void Delete(int id)
        {
            Repository.Delete(id);
            Repository.Save();
        }

        public virtual void Delete(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
                Repository.Delete(id);
            Repository.Save();
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var entities = Repository.All()
                                     .Where(where);

            foreach (var entity in entities)
                Repository.Delete(entity.Id);

            Repository.Save();
        }


        #endregion

        #region Get
        public virtual T Get(Guid id)
        {
            return Repository.Find(id);
        }

        public virtual T Get(int id)
        {
            return Repository.Find(id);
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return Repository.All()
                            .Where(where)
                            .FirstOrDefault();
        }

        public virtual T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include)
                             .Where(where)
                             .FirstOrDefault();
        }
        #endregion

        #region GetAll
        public virtual IEnumerable<T> GetAll()
        {
            return Repository.All();
        }

        public virtual IEnumerable<T> GetAll(params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include);
        }

        public virtual IEnumerable<T> GetAll(int page, int passedPageSize = 0)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.All()
                             .OrderByDescending(a => a.UpdatedAt)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize);
        }

        public virtual IEnumerable<T> GetAll(int page, int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.AllIncluding(include)
                             .OrderByDescending(a => a.UpdatedAt)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize);
        }
        #endregion

        #region GetAllBy
        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where)
        {
            return Repository.All()
                             .Where(where)
                             .OrderByDescending(a => a.UpdatedAt);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, string sortBy, OrderBySort sort)
        {
            return Repository.All()
                             .Where(where)
                             .OrderBy(sortBy, sort);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, int passedPageSize = 0)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.All()
                             .Where(where)
                             .OrderByDescending(a => a.UpdatedAt)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, string sortBy, OrderBySort sort, int passedPageSize = 0)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.All()
                             .Where(where)
                             .OrderBy(sortBy, sort)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, OrderBySort sort, IComparer<T> comparer, int passedPageSize = 0)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.All()
                             .Where(where)
                             .OrderBy(sort, comparer)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize); ;
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, OrderBySort sort, IComparer<T> comparer, int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.AllIncluding(include)
                             .Where(where)
                             .OrderBy(sort, comparer)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize); ;
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, OrderBySort sort, IComparer<T> comparer, params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include)
                             .Where(where)
                             .OrderBy(sort, comparer);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include)
                             .Where(where)
                             .OrderByDescending(a => a.UpdatedAt);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, string sortBy, OrderBySort sort, params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include)
                             .Where(where)
                             .OrderBy(sortBy, sort);
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            var pageSize = GetPageSize(passedPageSize);
            var data = Repository.AllIncluding(include)
                                     .Where(where)
                                     .OrderByDescending(a => a.UpdatedAt)
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize);
            return data;
        }

        public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, string sortBy, OrderBySort sort, int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            var pageSize = GetPageSize(passedPageSize);
            var data = Repository.AllIncluding(include)
                                 .Where(where)
                                 .OrderBy(sortBy, sort)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize);
            return data;
        }

        #endregion

        #region GetInitial

        public virtual IEnumerable<T> GetInitial(int passedPageSize = 0)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.All()
                             .OrderByDescending(a => a.UpdatedAt)
                             .Take(pageSize);
        }

        public virtual IEnumerable<T> GetInitial(int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            var pageSize = GetPageSize(passedPageSize);
            return Repository.AllIncluding(include)
                             .OrderByDescending(a => a.UpdatedAt)
                             .Take(pageSize);
        }

        public virtual IEnumerable<T> GetInitial(int size, int passedPageSize = 0)
        {
            return Repository.All()
                             .OrderByDescending(a => a.UpdatedAt)
                             .Take(size);
        }

        public virtual IEnumerable<T> GetInitial(int size, int passedPageSize = 0, params Expression<Func<T, object>>[] include)
        {
            return Repository.AllIncluding(include)
                             .OrderByDescending(a => a.UpdatedAt)
                             .Take(size);
        }
        #endregion

        #region GetCountBy
        public virtual int GetCountBy(Expression<Func<T, bool>> where)
        {
            var items = Repository.All().Where(where).ToList();
            if (items != null)
                return items.Count;

            return 0;
        }
        #endregion

        #region Check
        public virtual bool Check(int id)
        {
            var entity = Repository.Find(id);
            if (entity != null)
                return true;
            else
                return false;
        }

        public virtual bool Check(Guid id)
        {
            var entity = Repository.Find(id);
            if (entity != null)
                return true;
            else
                return false;
        }

        public virtual bool Check(Expression<Func<T, bool>> where)
        {
            return Repository.All().Any(where);
        }
        #endregion

    }

    //Interface
    public interface IBaseServiceQuery<T>
    {

        void Save();

        void Save(T entity);

        void Save(IEnumerable<T> entities);

        void Save(T entity, bool saveIfNotExist);

        void Save(IEnumerable<T> entities, bool saveIfNotExist);

        void Update(T entity);

        void Update(IEnumerable<T> entity);

        void Delete(Guid id);

        void Delete(IEnumerable<Guid> ids);

        void Delete(Expression<Func<T, bool>> where);

        T Get(Guid id);

        T Get(Expression<Func<T, bool>> where);

        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(int page, int passedPageSize = 0);

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAll(int page, int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, string sortBy, OrderBySort sort);


        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, OrderBySort sort, IComparer<T> comparer, int passedPageSize = 0);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, OrderBySort sort, IComparer<T> comparer, int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, OrderBySort sort, IComparer<T> comparer, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, int passedPageSize = 0);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, string sortBy, OrderBySort sort, int passedPageSize = 0);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, string sortBy, OrderBySort sort, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where, int page, string sortBy, OrderBySort sort, int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        int GetCountBy(Expression<Func<T, bool>> where);

        IEnumerable<T> GetInitial(int passedPageSize = 0);

        IEnumerable<T> GetInitial(int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        IEnumerable<T> GetInitial(int size, int passedPageSize = 0, params Expression<Func<T, object>>[] include);

        bool Check(Guid id);

        bool Check(Expression<Func<T, bool>> where);


    }


}
