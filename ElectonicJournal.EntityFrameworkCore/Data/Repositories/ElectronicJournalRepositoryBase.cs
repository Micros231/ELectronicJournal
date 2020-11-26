using ElectronicJournal.Core.Entities;

namespace ElectronicJournal.EntityFrameworkCore.Data.Repositories
{
    public class ElectronicJournalRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<ElectronicJournalDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public ElectronicJournalRepositoryBase(ElectronicJournalDbContext dbContext) : base(dbContext)
        {
            
        }
    }

    public class ElectronicJournalRepositoryBase<TEntity> : ElectronicJournalRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        public ElectronicJournalRepositoryBase(ElectronicJournalDbContext dbContext) : base(dbContext)
        {

        }
    }
}
