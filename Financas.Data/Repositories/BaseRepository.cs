using Financas.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Financas.Data.Repositories
{
    public abstract class BaseRepository<E> : IRepository<E>
            where E : BaseEntity
    {
        private readonly FinancasDbContext _context;

        protected FinancasDbContext Context => _context;

        protected BaseRepository(FinancasDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> InsertAndGetId(E entity)
        {
            await _context.Set<E>().AddAsync(entity);
            await SaveChanges();
            return entity.Id;
        }

        public async Task<int> SaveChanges()
        {
            VerificarEntidades();
            return await _context.SaveChangesAsync();
        }


        private void VerificarEntidades()
        {
            //var entidades = _context.ChangeTracker.Entries();
            //foreach (var entidade in entidades)
            //{
            //    if (entidade.State == EntityState.Added)
            //    {
            //        if (entidade.Entity.GetType().IsAssignableTo(typeof(IDateTimeTrackedEntity)))
            //        {
            //            ((IDateTimeTrackedEntity)entidade.Entity).AtualizarDataCriacao();
            //        }
            //    }
            //    else if (entidade.State == EntityState.Modified)
            //    {
            //        if (entidade.Entity.GetType().IsAssignableTo(typeof(IDateTimeTrackedEntity)))
            //        {
            //            ((IDateTimeTrackedEntity)entidade.Entity).AtualizarDataAtualizacao();
            //        }
            //    }
            //    else if (entidade.State == EntityState.Deleted)
            //    {
            //        if (entidade.Entity.GetType().IsAssignableTo(typeof(ISoftDelete)))
            //        {
            //            ((ISoftDelete)entidade.Entity).Deletar();
            //            entidade.State = EntityState.Modified;
            //        }
            //    }
            //}
        }

        public IQueryable<E> GetAll()
        {
            return _context.Set<E>().AsQueryable();
        }


        public void Update(E entity)
        {
            _context.Set<E>()
                .Update(entity);
        }

        public IQueryable<E> GetById(Guid id)
        {
            return _context.Set<E>()
                .Where(c => c.Id == id);
        }

        public void Insert(E entity)
        {
            _context.Set<E>()
                .Add(entity);
        }
    }
}
