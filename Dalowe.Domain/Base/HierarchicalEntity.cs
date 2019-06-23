namespace Dalowe.Domain.Base
{
    public abstract class HierarchicalEntity : Entity
    {
        public long SortOrder { get; set; }
    }
}