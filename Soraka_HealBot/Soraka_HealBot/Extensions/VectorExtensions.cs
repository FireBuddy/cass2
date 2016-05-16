namespace Soraka_HealBot.Extensions
{
    using System.Linq;

    using EloBuddy.SDK;

    using SharpDX;

    public static class VectorExtensions
    {
        #region Public Methods and Operators

        public static bool UnderAllyTurret(this Vector3 position)
            => EntityManager.Turrets.Allies.Any(turret => turret.Distance(position) < 775);

        public static bool UnderEnemyTurret(this Vector3 position)
            => EntityManager.Turrets.Enemies.Any(turret => turret.Distance(position) < 775);

        #endregion
    }
}