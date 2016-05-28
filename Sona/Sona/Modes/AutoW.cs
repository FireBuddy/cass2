namespace Sona.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal static class AutoW
    {
        #region Methods

        internal static void Execute()
        {
            if (Player.Instance.IsRecalling() || Player.Instance.IsInShopRange())
            {
                return;
            }

            var woundedAlly =
                EntityManager.Heroes.Allies.Where(
                    ally =>
                    !ally.IsMe && !ally.IsDead && !ally.IsZombie && ally.Distance(Player.Instance) <= Spells.W.Range)
                    .OrderBy(ally => ally.Health)
                    .FirstOrDefault();
            if (woundedAlly != null && woundedAlly.HealthPercent <= Config.GetSliderValue(Config.AutoWMenu, "allyWhp"))
            {
                Spells.W.Cast();
            }

            if (Player.Instance.HealthPercent <= Config.GetSliderValue(Config.AutoWMenu, "playerWhp"))
            {
                Spells.W.Cast();
            }
        }

        #endregion
    }
}