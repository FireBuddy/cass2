using System.Linq;
using EloBuddy.SDK;
using SharpDX;

namespace Soraka_HealBot.Extensions
{
    public static class VectorExtensions
    {
        public static bool UnderEnemyTurret(this Vector3 position) => (EntityManager.Turrets.Enemies.Any(turret => turret.Distance(position) < 775));

        public static bool UnderAllyTurret(this Vector3 position) => (EntityManager.Turrets.Allies.Any(turret => turret.Distance(position) < 775));
    }
}