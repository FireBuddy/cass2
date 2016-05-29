namespace Sona.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;

    using Sona.Extensions;

    internal static class Flee
    {
        #region Methods

        internal static void Execute()
        {
            if (Player.Instance.HasBuff("sonapassiveattack") && Spells.LastSpellSlot == SpellSlot.E)
            {
                var target = TargetSelector.GetTarget(Player.Instance.AttackRange, DamageType.Magical);
                if (target != null)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                }
            }

            if (Spells.E.CanCast())
            {
                Spells.E.Cast();
            }
        }

        #endregion
    }
}