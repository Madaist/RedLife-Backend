using Abp.Domain.Repositories;

namespace RedLife.Core.LastId
{
    public class LastUserIdManager
    {
        private readonly IRepository<LastUserId> _lastUserIdRepository;

        public LastUserIdManager(IRepository<LastUserId> lastUserIdRepository)
        {
            _lastUserIdRepository = lastUserIdRepository;
        }

        public long GetAndUpdateLastUserId()
        {
            var lastUserId =  _lastUserIdRepository.GetAllList()[0];
            lastUserId.Counter++;
            _lastUserIdRepository.Update(lastUserId);
            return lastUserId.Counter;
        }
    }
}
