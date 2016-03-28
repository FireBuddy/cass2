using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace CassOp
{
    internal class Modes
    {
        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget(Spells.Q.Range))
            {
                return;
            }

            if (Config.IsChecked(Config.Combo, "useQInCombo") && Spells.Q.IsReady() &&
                !target.HasBuffOfType(BuffType.Poison))
            {
                var qPred = Spells.Q.GetPrediction(target);
                if (qPred.HitChancePercent >= 85)
                {
                    Spells.Q.Cast(qPred.CastPosition);
                }
            }

            if (Config.IsChecked(Config.Combo, "useWInCombo") && Spells.W.IsReady() &&
                target.IsValidTarget(Spells.W.Range))
            {
                if (Config.IsChecked(Config.Combo, "comboWonlyCD"))
                {
                    if (!Spells.Q.IsReady() && (Spells.QCasted - Game.Time) < -0.45f &&
                        !target.HasBuffOfType(BuffType.Poison))
                    {
                        var wPred = Spells.W.GetPrediction(target);
                        if (wPred.HitChancePercent >= 85)
                        {
                            Spells.W.Cast(wPred.CastPosition);
                        }
                    }
                }
                else
                {
                    var wPred = Spells.W.GetPrediction(target);
                    if (wPred.HitChancePercent >= 85)
                    {
                        Spells.W.Cast(wPred.CastPosition);
                    }
                }
            }

            if (Config.IsChecked(Config.Combo, "useEInCombo") && Spells.E.IsReady() &&
                target.IsValidTarget(Spells.E.Range) &&
                (!Config.IsChecked(Config.Combo, "comboEonP") || target.HasBuffOfType(BuffType.Poison)))
            {
                if (Config.IsChecked(Config.Combo, "humanEInCombo"))
                {
                    var delay = Computed.RandomDelay(Config.GetSliderValue(Config.Misc, "humanDelay"));
                    Core.DelayAction(() => Spells.E.Cast(target), delay);
                }
                else
                {
                    Spells.E.Cast(target);
                }
            }

            if (Config.IsChecked(Config.Combo, "useRInCombo") && Spells.R.IsReady())
            {
                var countFace =
                    EntityManager.Heroes.Enemies.Count(
                        h => h.IsValidTarget(Spells.R.Range) && h.IsFacing(ObjectManager.Player));
                if (countFace >= Config.GetSliderValue(Config.Combo, "comboMinR") &&
                    target.IsFacing(ObjectManager.Player) && target.IsValidTarget(Spells.R.Range))
                {
                    Spells.R.Cast(target);
                }
            }
        }

        internal static void JungleClear()
        {
            var minions = EntityManager.MinionsAndMonsters.Monsters;
            if (!minions.Any() || minions == null ||
                ObjectManager.Player.ManaPercent < Config.GetSliderValue(Config.JungleClear, "manaToJC"))
            {
                return;
            }
            if (Config.IsChecked(Config.JungleClear, "useQInJC") && Spells.Q.IsReady())
            {
                var qFarmLoc =
                    Computed.GetBestCircularFarmLocation(
                        minions.Where(m => m.Distance(ObjectManager.Player) <= Spells.Q.Range)
                            .Select(mx => mx.ServerPosition.To2D())
                            .ToList(), Spells.Q.Width * 1.5f, Spells.Q.Range);
                if (qFarmLoc.MinionsHit > 0)
                {
                    Spells.Q.Cast(qFarmLoc.Position.To3D());
                }
            }

            if (Config.IsChecked(Config.JungleClear, "useWInJC") && Spells.W.IsReady())
            {
                var wFarmLoc =
                    Computed.GetBestCircularFarmLocation(
                        minions.Where(m => m.Distance(ObjectManager.Player) <= Spells.W.Range)
                            .Select(mx => mx.ServerPosition.To2D())
                            .ToList(), Spells.W.Width, Spells.W.Range);
                if (wFarmLoc.MinionsHit >= Config.GetSliderValue(Config.LaneClear, "minWInLC"))
                {
                    Spells.W.Cast(wFarmLoc.Position.To3D());
                }
            }

            if (Config.IsChecked(Config.JungleClear, "useEInJC") && Spells.E.IsReady())
            {
                var jngToE =
                    EntityManager.MinionsAndMonsters.Monsters.FirstOrDefault(
                        m =>
                            m.IsValidTarget(Spells.E.Range) &&
                            (Config.IsChecked(Config.JungleClear, "jungEonP") && m.HasBuffOfType(BuffType.Poison)));
                if (jngToE != null)
                {
                    Spells.E.Cast(jngToE);
                }
            }
        }

        internal static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget(Spells.Q.Range) ||
                ObjectManager.Player.ManaPercent < Config.GetSliderValue(Config.Harass, "manaToHarass"))
            {
                return;
            }

            if (Config.IsChecked(Config.Harass, "useQInHarass") && Spells.Q.IsReady() &&
                !target.HasBuffOfType(BuffType.Poison) &&
                (Orbwalker.LastHitMinion == null && !Orbwalker.IsAutoAttacking))
            {
                var qPred = Spells.Q.GetPrediction(target);
                if (qPred.HitChancePercent >= 90)
                {
                    Spells.Q.Cast(qPred.CastPosition);
                }
            }

            if (Config.IsChecked(Config.Harass, "useWInHarass") && Spells.W.IsReady() &&
                target.IsValidTarget(Spells.W.Range) && (Orbwalker.LastHitMinion == null && !Orbwalker.IsAutoAttacking))
            {
                if (Config.IsChecked(Config.Harass, "harassWonlyCD"))
                {
                    if (!Spells.Q.IsReady() && (Spells.QCasted - Game.Time) < -0.45f &&
                        !target.HasBuffOfType(BuffType.Poison))
                    {
                        var wPred = Spells.W.GetPrediction(target);
                        if (wPred.HitChancePercent >= 85)
                        {
                            Spells.W.Cast(wPred.CastPosition);
                        }
                    }
                }
                else
                {
                    var wPred = Spells.W.GetPrediction(target);
                    if (wPred.HitChancePercent >= 85)
                    {
                        Spells.W.Cast(wPred.CastPosition);
                    }
                }
            }

            if (Config.IsChecked(Config.Harass, "useEInHarass") && Spells.E.IsReady() &&
                target.IsValidTarget(Spells.E.Range) &&
                (!Config.IsChecked(Config.Harass, "harassEonP") || target.HasBuffOfType(BuffType.Poison)))
            {
                if (Config.IsChecked(Config.Harass, "humanEInHarass"))
                {
                    var delay = Computed.RandomDelay(Config.GetSliderValue(Config.Misc, "humanDelay"));
                    Core.DelayAction(() => Spells.E.Cast(target), delay);
                }
                else
                {
                    Spells.E.Cast(target);
                }
            }
        }

        internal static void LaneClear()
        {
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions;
            if (!minions.Any() || minions == null ||
                ObjectManager.Player.ManaPercent < Config.GetSliderValue(Config.LaneClear, "manaToLC"))
            {
                return;
            }

            if (Config.IsChecked(Config.LaneClear, "useQInLC") && Spells.Q.IsReady() &&
                (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                var qFarmLoc =
                    Computed.GetBestCircularFarmLocation(
                        minions.Where(m => m.Distance(ObjectManager.Player) <= Spells.Q.Range)
                            .Select(mx => mx.ServerPosition.To2D())
                            .ToList(), Spells.Q.Width * 1.5f, Spells.Q.Range);
                if (qFarmLoc.MinionsHit >= Config.GetSliderValue(Config.LaneClear, "minQInLC"))
                {
                    Spells.Q.Cast(qFarmLoc.Position.To3D());
                }
            }

            if (Config.IsChecked(Config.LaneClear, "useWInLC") && Spells.W.IsReady() &&
                (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                var wFarmLoc =
                    Computed.GetBestCircularFarmLocation(
                        minions.Where(m => m.Distance(ObjectManager.Player) <= Spells.W.Range)
                            .Select(mx => mx.ServerPosition.To2D())
                            .ToList(), Spells.W.Width, Spells.W.Range);
                if (wFarmLoc.MinionsHit >= Config.GetSliderValue(Config.LaneClear, "minWInLC"))
                {
                    Spells.W.Cast(wFarmLoc.Position.To3D());
                }
            }

            if (Config.IsChecked(Config.LaneClear, "useEInLC") && Spells.E.IsReady())
            {
                var minToE =
                    EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                        m =>
                            m.IsValidTarget(Spells.E.Range) &&
                            ObjectManager.Player.GetSpellDamage(m, SpellSlot.E) > m.Health &&
                            ((Config.IsChecked(Config.LaneClear, "laneEonP") && m.HasBuffOfType(BuffType.Poison)) ||
                             (Config.IsChecked(Config.LaneClear, "useManaEInLC") &&
                              ObjectManager.Player.ManaPercent <= Config.GetSliderValue(Config.LaneClear, "manaEInLC"))));
                if (minToE != null)
                {
                    Spells.E.Cast(minToE);
                }
            }
        }

        internal static void PermActive()
        {
            if (Config.IsKeyPressed(Config.Misc, "assistedR"))
            {
                Computed.AssistedR();
            }
            if (Config.IsChecked(Config.Misc, "eKillSteal"))
            {
                Computed.KillSteal("E");
            }
            if (Config.IsChecked(Config.Harass, "autoEHarass") &&
                ObjectManager.Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaToAutoHarass"))
            {
                Computed.AutoE();
            }
            if (Config.IsChecked(Config.Harass, "autoQHarass") &&
                ObjectManager.Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaToAutoHarass"))
            {
                Computed.AutoQ();
            }
            if (Config.IsChecked(Config.Harass, "autoWHarass") &&
                ObjectManager.Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaToAutoHarass"))
            {
                Computed.AutoW();
            }
        }

        public static void LastHit() {}
    }
}