using Abp.Application.Services;
using RedLife.Application.Achievements.Dto;

namespace RedLife.Application.Achievements
{
    public interface IAchievementsAppService : IApplicationService
    {
        public AchievementsDto GetAchievements();
    }
}
