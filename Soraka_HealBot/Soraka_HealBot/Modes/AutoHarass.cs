using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using Soraka_HealBot.Extensions;

namespace Soraka_HealBot.Modes
{
    internal static class AutoHarass
    {
        internal static void AutoQ()
        {
            if (!Spells.Q.CanCast() || Orbwalker.IsAutoAttacking || !Player.Instance.CanCast ||
                Player.Instance.HasBuff("Recall") ||
                Player.Instance.ManaPercent <= Config.GetSliderValue(Config.Harass, "manaAutoHarass"))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.Position.UnderEnemyTurret() &&
                Player.Instance.Position.UnderEnemyTurret())
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontHarassInBush"))
            {
                var bush =
                    ObjectManager.Get<GrassObject>()
                        .OrderBy(br => br.Distance(Player.Instance.ServerPosition))
                        .FirstOrDefault();
                if (bush != null && Player.Instance.Position.IsInRange(bush, bush.BoundingRadius))
                {
                    return;
                }
            }
            Spells.Q.Cast(target);
        }

        internal static void AutoE()
        {
            if (!Spells.E.CanCast() || Orbwalker.IsAutoAttacking || !Player.Instance.CanCast ||
                Player.Instance.HasBuff("Recall") ||
                Player.Instance.ManaPercent <= Config.GetSliderValue(Config.Harass, "manaAutoHarass"))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.Position.UnderEnemyTurret() &&
                Player.Instance.Position.UnderEnemyTurret())
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontHarassInBush"))
            {
                var bush =
                    ObjectManager.Get<GrassObject>()
                        .OrderBy(br => br.Distance(Player.Instance.ServerPosition))
                        .FirstOrDefault();
                if (bush != null && Player.Instance.Position.IsInRange(bush, bush.BoundingRadius))
                {
                    return;
                }
            }
            if (Config.IsChecked(Config.Harass, "eOnlyCCHarass"))
            {
                var ePred = Spells.E.GetPrediction(target);
                if (ePred.HitChance == HitChance.Immobile)
                {
                    Spells.E.Cast(target);
                    return;
                }
                return;
            }
            Spells.E.Cast(target);
        }
    }
}