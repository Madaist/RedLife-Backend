using Abp.Domain.Entities;

namespace RedLife.Core.LastId
{
    public class LastUserId : Entity<int>
    {
        public long LastId { get; set; }
    }
}
