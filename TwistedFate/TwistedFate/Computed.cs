using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace TwistedFate
{
    internal class Computed
    {
        public static int RandomDelay(int x)
        {
            var y = x;
            var i = Math.Abs(x);
            while (i >= 10)
            {
                i /= 10;
            }
            i = y / i;
            return Tf.RDelay.Next(y - i, y + i);
        }

        public static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var enemyTarget = EntityManager.Heroes.Enemies.FirstOrDefault(e => e.NetworkId == target.NetworkId);
            if (enemyTarget != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                Config.IsChecked(Config.Combo, "qAAReset"))
            {
                Spells.Q.Cast(enemyTarget);
            }
        }
    }
}