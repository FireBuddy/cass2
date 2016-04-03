using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Soraka_HealBot
{
    public static class Modes
    {
        private static AIHeroClient _Player => ObjectManager.Player;

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            var qPred = Spells.Q.GetPrediction(target);
            if (Config.IsChecked(Config.Combo, "useQInCombo") && Spells.Q.IsReady() && _Player.Mana >= 40 && Spells.Q.IsInRange(qPred.CastPosition) && qPred.HitChancePercent >= 70)
            {
                Spells.Q.Cast(qPred.CastPosition);
            }
            var ePred = Spells.E.GetPrediction(target);
            if (Spells.E.IsReady() && _Player.Mana >= 70 &&
                Config.IsChecked(Config.Combo, "useEInCombo") && Spells.E.IsInRange(ePred.CastPosition) &&
                ePred.HitChancePercent >= 70)
            {
                if (Config.IsChecked(Config.Combo, "eOnlyCC"))
                {
                    if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Fear) ||
                        target.HasBuffOfType(BuffType.Flee) || target.HasBuffOfType(BuffType.Knockup) ||
                        target.HasBuffOfType(BuffType.Polymorph) || target.HasBuffOfType(BuffType.Snare) ||
                        target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Taunt) ||
                        target.HasBuffOfType(BuffType.Slow) || ePred.HitChancePercent >= 100)
                    {
                        Spells.E.Cast(ePred.CastPosition);
                    }
                }
                else
                {
                    Spells.E.Cast(ePred.CastPosition);
                }
            }
        }

        public static void LaneClear()
        {
            if (Spells.Q.IsReady() && Config.IsChecked(Config.LaneClear, "useQInLC") && Spells.Q.IsReady() &&
                _Player.Mana >= 40 && _Player.CanCast &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.LaneClear, "manaLaneClear"))
            {
                /*
                var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(
                    EntityManager.UnitTeam.Enemy, _Player.ServerPosition, Spells.Q.Range);
                if (minions != null)
                {
                    var minionsToQ = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(
                        minions, Spells.Q.Width, (int)Spells.Q.Range);
                    if (minionsToQ.HitNumber >= Config.GetSliderValue(Config.LaneClear, "qTargets"))
                    {
                        Spells.Q.Cast(minionsToQ.CastPosition);
                        Chat.Print("minions");
                    }
                }*/
                var test =
                    EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                        x => x.Distance(_Player.ServerPosition) <= Spells.Q.Range + Spells.Q.Width).AsEnumerable();
                if (test != null)
                {
                    var minionsToQ = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(
                        test, Spells.Q.Width * 1.125f, (int) Spells.Q.Range);
                    if (minionsToQ.HitNumber >= Config.GetSliderValue(Config.LaneClear, "qTargets"))
                    {
                        Spells.Q.Cast(minionsToQ.CastPosition);
                        //Chat.Print("test");
                    }
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            var qPred = Spells.Q.GetPrediction(target);
            var ePred = Spells.E.GetPrediction(target);
            if (Config.IsChecked(Config.Harass, "useQInHarass") && Spells.Q.IsReady() && _Player.Mana >= 40 &&
                _Player.CanCast && Spells.Q.IsInRange(qPred.CastPosition) && qPred.HitChancePercent >= 90 &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaHarass"))
            {
                Spells.Q.Cast(qPred.CastPosition);
            }
            if (Spells.E.IsReady() && _Player.Mana >= 70 && _Player.CanCast &&
                Config.IsChecked(Config.Harass, "useEInHarass") && Spells.E.IsInRange(ePred.CastPosition) &&
                ePred.HitChancePercent >= 90 &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaHarass"))
            {
                if (Config.IsChecked(Config.Combo, "eOnlyCC"))
                {
                    if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Fear) ||
                        target.HasBuffOfType(BuffType.Flee) || target.HasBuffOfType(BuffType.Knockup) ||
                        target.HasBuffOfType(BuffType.Polymorph) || target.HasBuffOfType(BuffType.Snare) ||
                        target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Taunt) ||
                        target.HasBuffOfType(BuffType.Slow) || ePred.HitChancePercent >= 100)
                    {
                        Spells.E.Cast(ePred.CastPosition);
                    }
                }
                else
                {
                    Spells.E.Cast(ePred.CastPosition);
                }
            }
        }

        public static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (args.Target.Type == GameObjectType.obj_AI_Minion &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                Config.IsChecked(Config.Harass, "disableAAH"))
            {
                var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.Harass, "allyRangeH"));
                if (air > 1)
                {
                    args.Process = false;
                }
            }
            if (Config.IsChecked(Config.Combo, "comboDisableAA") &&
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                args.Target.Type == GameObjectType.AIHeroClient)
            {
                args.Process = false;
            }
        }
    }
}