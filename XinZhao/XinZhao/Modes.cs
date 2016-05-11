using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace XinZhao
{
    internal class Modes
    {
        public static void PermActive()
        {
            throw new NotImplementedException();
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            var enemiesAroundPlayer =
                EntityManager.Heroes.Enemies.Where(en => en.Distance(Player.Instance.Position) <= Spells.R.Range);
            if (enemiesAroundPlayer.Count() >= Config.GetSliderValue(Config.Combo, "comboMinR") && Spells.R.CanCast())
            {
                Spells.R.Cast();
            }
            if (Config.IsChecked(Config.Combo, "comboETower") && target.Position.UnderEnemyTurret())
            {
                return;
            }
            if (target.Distance(Player.Instance.Position) >= 300 && Player.Instance.HasBuff("XenZhaoComboTarget") &&
                Spells.E.CanCast() && Config.IsChecked(Config.Combo, "useEcombo"))
            {
                Spells.E.Cast(target);
            }
            if ((target.Distance(Player.Instance.Position) > Spells.E.Range * 0.65f &&
                 target.MoveSpeed + 15 >= Player.Instance.MoveSpeed) ||
                (target.Distance(Player.Instance.Position) > Spells.E.Range * 0.80f))
            {
                if (!Spells.E.CanCast())
                {
                    target = TargetSelector.GetTarget(175, DamageType.Physical);
                }
                else
                {
                    if (Config.IsChecked(Config.Combo, "useEcombo"))
                    {
                        Spells.E.Cast(target);
                    }
                }
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "harassETower") && target.Position.UnderEnemyTurret())
            {
                return;
            }
            if (target.Distance(Player.Instance.Position) >= 255 && Player.Instance.HasBuff("XenZhaoComboTarget") &&
                Spells.E.CanCast() && Config.IsChecked(Config.Harass, "useEharass"))
            {
                Spells.E.Cast(target);
            }
            if ((target.Distance(Player.Instance.Position) > Spells.E.Range * 0.55f &&
                 target.MoveSpeed + 5 >= Player.Instance.MoveSpeed) ||
                (target.Distance(Player.Instance.Position) > Spells.E.Range * 0.70f))
            {
                if (!Spells.E.CanCast())
                {
                    target = TargetSelector.GetTarget(175, DamageType.Physical);
                }
                else
                {
                    if (Config.IsChecked(Config.Harass, "useEharass"))
                    {
                        Spells.E.Cast(target);
                    }
                }
            }
        }

        public static void LaneClear()
        {
            var minz =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.E.Range + 200)
                    .OrderByDescending(m => m.MaxHealth);
            var minion = minz.FirstOrDefault();
            if (minion == null || Orbwalker.IsAutoAttacking)
            {
                return;
            }
            foreach (var mina in minz)
            {
                var minAoE = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => mina.Distance(m) <= 100);
                if (minAoE.Count() >= Config.GetSliderValue(Config.LaneClear, "lcEtargets") &&
                    Config.IsChecked(Config.LaneClear, "useELC") && mina.IsValidTarget(Spells.E.Range) &&
                    Spells.E.CanCast())
                {
                    Spells.E.Cast(mina);
                }
            }
        }

        public static void JungleClear()
        {
            var jngTargets =
                EntityManager.MinionsAndMonsters.Monsters.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.E.Range).OrderByDescending(m => m.MaxHealth);
            var jngTarget = jngTargets.FirstOrDefault();
            if (jngTarget == null || Orbwalker.IsAutoAttacking)
            {
                return;
            }
            if (Config.IsChecked(Config.JungleClear, "useEJC") && Spells.E.CanCast() &&
                jngTarget.IsValidTarget(Spells.E.Range))
            {
                Spells.E.Cast(jngTarget);
            }
        }

        public static void LastHit()
        {
            throw new NotImplementedException();
        }

        public static void Flee()
        {
            throw new NotImplementedException();
        }
    }
}