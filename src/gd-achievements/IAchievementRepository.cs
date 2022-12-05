using System.Collections.Generic;

namespace GodotRts.Achievements
{
    public interface IAchievementRepository
    {
        bool ProgressAchievement(string key, int progress);
        bool UnlockAchievement(string key);
        Achievement GetAchievement(string key);
        IEnumerable<Achievement> GetForList();
    }
}