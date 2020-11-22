using Abp.Domain.Entities;

namespace RedLife.Core.LastId
{
    public class LastUserId : Entity<int>
    {
        public long Counter { get; set; }
    }
}
