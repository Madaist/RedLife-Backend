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
            lastUserId.LastId++;
            _lastUserIdRepository.Update(lastUserId);
            return lastUserId.LastId;
        }
    }
}
