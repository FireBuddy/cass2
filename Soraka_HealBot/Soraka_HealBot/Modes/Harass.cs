namespace Soraka_HealBot.Modes
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    using Soraka_HealBot.Extensions;

    internal static class Harass
    {
        #region Methods

        internal static void Execute()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
            if (target == null || Player.Instance.ManaPercent < Config.GetSliderValue(Config.Harass, "manaHarass"))
            {
                return;
            }

            if (Config.IsChecked(Config.Harass, "useQInHarass") && Spells.Q.CanCast())
            {
                Spells.Q.Cast(target);
            }

            if (Config.IsChecked(Config.Harass, "useEInHarass") && Spells.E.CanCast())
            {
                if (Config.IsChecked(Config.Harass, "eOnlyCCHarass"))
                {
                    var ePred = Spells.E.GetPrediction(target);
                    if (ePred.HitChance == HitChance.Immobile)
                    {
                        Spells.E.Cast(target);
                    }

                    return;
                }

                Spells.E.Cast(target);
            }
        }

        #endregion
    }
}