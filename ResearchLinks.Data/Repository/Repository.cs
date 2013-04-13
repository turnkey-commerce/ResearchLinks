using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Entity;

namespace ResearchLinks.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly ResearchLinksContext dataContext;

        public Repository()
        {
            this.dataContext = new ResearchLinksContext();
        }

        public virtual IQueryable<T> Find()
        {
            return dataContext.Set<T>();
        }

        public void Insert(T item)
        {
            dataContext.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            dataContext.Set<T>().Remove(item);
        }

        public void Commit()
        {
            dataContext.SaveChanges();
        }

        public void Dispose()
        {
            if (dataContext != null) {
                dataContext.Dispose();
            }
        }

    }

}
