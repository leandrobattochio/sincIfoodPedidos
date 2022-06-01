using System;
using System.Linq;
using System.Threading.Tasks;

namespace Financas.Core
{
    public interface IRepository<E> where E : BaseEntity
    {
        Task<Guid> InsertAndGetId(E entity);
        void Insert(E entity);
        Task<int> SaveChanges();

        void Update(E entity);

        IQueryable<E> GetById(Guid id);
        IQueryable<E> GetAll();
    }
}
