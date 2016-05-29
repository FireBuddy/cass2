namespace XinZhao
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class Modes
    {
        #region Public Methods and Operators

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            var wantedTarget = TargetSelector.GetTarget(Spells.E.Range + 150, DamageType.Physical);
            if (target == null || target.IsInvulnerable)
            {
                return;
            }

            var enemiesAroundPlayer =
                EntityManager.Heroes.Enemies.Where(en => en.Distance(Player.Instance.Position) <= Spells.R.Range);
            if (enemiesAroundPlayer.Count() >= Config.GetSliderValue(Config.Combo, "comboMinR") && Spells.R.CanCast())
            {
                Spells.R.Cast();
            }

            if (!Orbwalker.IsAutoAttacking && Spells.W.CanCast() && target.IsValidTarget(175))
            {
                Spells.W.Cast();
            }

            if (!Config.IsChecked(Config.Combo, "useEcombo") || !Spells.E.CanCast())
            {
                return;
            }

            if (Config.IsChecked(Config.Combo, "comboETower") && target.Position.UnderEnemyTurret())
            {
                return;
            }

            if (Config.GetComboBoxValue(Config.Combo, "comboEmode") == 0)
            {
                if (target != wantedTarget)
                {
                    return;
                }

                var pathEndLoc = target.Path.OrderByDescending(x => x.Distance(target)).FirstOrDefault();
                var dist = Player.Instance.Position.Distance(target.Position);
                var distToPath = pathEndLoc.Distance(Player.Instance.Position);
                if (distToPath <= dist)
                {
                    return;
                }

                if (target.Distance(Player.Instance.Position) >= 300
                    && (Player.Instance.HasBuff("XenZhaoComboTarget") || Spells.Q.CanCast()))
                {
                    Spells.E.Cast(target);
                }

                var movspeedDif = Player.Instance.MoveSpeed - target.MoveSpeed;
                if (movspeedDif <= 0 && !Player.Instance.IsInAutoAttackRange(target))
                {
                    Spells.E.Cast(target);
                }

                var timeToReach = dist / movspeedDif;
                if (timeToReach > 4)
                {
                    Spells.E.Cast(target);
                }
            }

            if (Config.GetComboBoxValue(Config.Combo, "comboEmode") == 1)
            {
                if (target.Distance(Player.Instance.Position) >= 225)
                {
                    Spells.E.Cast(target);
                }
            }
        }

        public static void Flee()
        {
            throw new NotImplementedException();
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            if (target == null || target.IsInvulnerable)
            {
                return;
            }

            if (Config.IsChecked(Config.Harass, "harassETower") && target.Position.UnderEnemyTurret())
            {
                return;
            }

            if (target.Distance(Player.Instance.Position) >= 255 && Player.Instance.HasBuff("XenZhaoComboTarget")
                && Spells.E.CanCast() && Config.IsChecked(Config.Harass, "useEharass"))
            {
                Spells.E.Cast(target);
            }

            if ((target.Distance(Player.Instance.Position) > Spells.E.Range * 0.55f
                 && target.MoveSpeed + 5 >= Player.Instance.MoveSpeed)
                || (target.Distance(Player.Instance.Position) > Spells.E.Range * 0.70f))
            {
                if (!Spells.E.CanCast())
                {
                    /*
                    target = TargetSelector.GetTarget(175, DamageType.Physical);
*/
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

            if (Config.IsChecked(Config.JungleClear, "useEJC") && Spells.E.CanCast()
                && jngTarget.IsValidTarget(Spells.E.Range))
            {
                Spells.E.Cast(jngTarget);
            }
        }

        public static void LaneClear()
        {
            var minz =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.E.Range).OrderByDescending(m => m.MaxHealth);
            var minion = minz.FirstOrDefault();
            if (minion == null || Orbwalker.IsAutoAttacking)
            {
                return;
            }

            foreach (var mina in minz)
            {
                var minAoE = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => mina.Distance(m) <= 100);
                if (minAoE.Count() >= Config.GetSliderValue(Config.LaneClear, "lcEtargets")
                    && Config.IsChecked(Config.LaneClear, "useELC") && Spells.E.CanCast())
                {
                    Spells.E.Cast(mina);
                }
            }
        }

        public static void LastHit()
        {
            throw new NotImplementedException();
        }

        public static void PermActive()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}