using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Soraka_HealBot.Extensions;

namespace Soraka_HealBot.Modes
{
    internal static class LaneClear
    {
        public static void Execute()
        {
            if (!Spells.Q.CanCast() || !Config.IsChecked(Config.LaneClear, "useQInLC") ||
                Player.Instance.ManaPercent < Config.GetSliderValue(Config.LaneClear, "manaLaneClear"))
            {
                return;
            }
            var minions =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    minion => minion.IsValidTarget(Spells.Q.Range + 150));
            var farmLoc = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(
                minions, Spells.Q.Width * 1.5f, (int) Spells.Q.Range);
            if (farmLoc.HitNumber >= Config.GetSliderValue(Config.LaneClear, "qTargets"))
            {
                Spells.Q.Cast(farmLoc.CastPosition);
            }
        }
    }
}