namespace ElectronicJournal.Core.Entities
{
    
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }
    }
    public abstract class Entity : Entity<int>, IEntity<int>, IEntity
    {
        
    }
}
