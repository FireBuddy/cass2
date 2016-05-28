namespace Sona.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Sona.Extensions;

    internal static class Harass
    {
        #region Methods

        internal static void Execute()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            if (Config.IsChecked(Config.HarassMenu, "bQ") && Spells.Q.CanCast())
            {
                Spells.Q.Cast();
            }
        }

        internal static void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                && target.Type == GameObjectType.obj_AI_Minion && Config.IsChecked(Config.HarassMenu, "aaMins")
                && EntityManager.Heroes.Allies.Count(
                    ally => !ally.IsDead && !ally.IsMe && !ally.IsZombie && ally.Distance(Player.Instance) <= 1500) > 0)
            {
                args.Process = false;
            }
        }

        #endregion
    }
}