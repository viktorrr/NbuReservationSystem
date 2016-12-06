namespace NbuReservationSystem.Data.Common
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Models;

    public interface IRepository<T> : IRepository<T, int>
        where T : BaseModel<int>
    {
    }

    public interface IRepository<T, in TKey>
        where T : BaseModel<TKey>
    {
        IQueryable<T> All();

        IQueryable<T> AllBy(Expression<Func<T, bool>> func);

        IQueryable<T> AllWithDeleted();

        T GetById(TKey id);

        T GetBy(Expression<Func<T, bool>> func);

        void Add(T entity);

        void Delete(T entity);

        void HardDelete(T entity);

        void Save();
    }
}
