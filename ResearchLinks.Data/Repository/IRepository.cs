using System;
using System.Linq;

namespace ResearchLinks.Data.Repository
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> Find();
        void Insert(T item);
        void Delete(T item);
        void Commit();
    }
}
