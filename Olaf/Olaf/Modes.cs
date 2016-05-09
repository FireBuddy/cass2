using System;
using System.Linq;
using System.Timers;
using EloBuddy;
using EloBuddy.SDK;

namespace Olaf
{
    internal class Modes
    {
        public static bool Bursting;
        public static bool SafeToPickup = true;
        public static Timer FcTimer1 = new Timer(2000) { AutoReset = false };
        public static Timer FcTimer2 = new Timer(2000) { AutoReset = false };

        public static Timer FjTimer1 = new Timer(2000) { AutoReset = false };
        public static Timer FjTimer2 = new Timer(2000) { AutoReset = false };

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range - 100, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            if (Spells.AxeObject != null &&
                (Player.Instance.Position.Distance(Spells.AxeObject.Position) <
                 target.Distance(Spells.AxeObject.Position) ||
                 target.Distance(Spells.AxeObject.Position) >= Config.GetSliderValue(Config.Misc, "axePickRange")))
            {
                SafeToPickup = false;
            }
            else
            {
                SafeToPickup = true;
            }
            if (target.IsValidTarget(Spells.Q.Range) && Spells.Q.IsReady() &&
                Config.IsChecked(Config.Combo, "useQCombo"))
            {
                if (Player.Instance.HasItem(Spells.CorruptingPotion.Id) && Spells.CorruptingPotion.IsReady() &&
                    !Player.HasBuff("ItemDarkCrystalFlask") && Config.IsChecked(Config.Combo, "potionOnLv1") &&
                    Player.Instance.Level == 1)
                {
                    Spells.CorruptingPotion.Cast();
                }
                Spells.Q.Cast(target);
            }
            if (Player.Instance.HealthPercent <= 50 && target.IsValidTarget(Player.Instance.GetAutoAttackRange()) &&
                Spells.W.IsReady())
            {
                Spells.W.Cast();
            }
            if (target.IsValidTarget(Spells.E.Range) && Spells.E.IsReady() &&
                Config.IsChecked(Config.Combo, "useECombo"))
            {
                if (Player.Instance.HasItem(Spells.CorruptingPotion.Id) && Spells.CorruptingPotion.IsReady() &&
                    !Player.HasBuff("ItemDarkCrystalFlask") && Config.IsChecked(Config.Combo, "potionOnBurst"))
                {
                    Spells.CorruptingPotion.Cast();
                }
                Bursting = true;
                SafeToPickup = false;
                FcTimer1.Start();
                Spells.E.Cast(target);
                if (target.IsValidTarget(Player.Instance.GetAutoAttackRange()) && Spells.W.IsReady())
                {
                    Spells.W.Cast();
                }
                Orbwalker.ResetAutoAttack();
                Orbwalker.OnPostAttack += Computed.FastCombo1;
            }
        }

        public static void Harass()
        {
            if (Orbwalker.IsAutoAttacking)
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "useEHarass") && Spells.E.IsReady() &&
                target.IsValidTarget(Spells.E.Range) &&
                Player.Instance.HealthPercent >= Config.GetSliderValue(Config.Harass, "harassHP"))
            {
                Spells.E.Cast(target);
            }
            if (Config.IsChecked(Config.Harass, "useQHarass") && Spells.Q.IsReady() &&
                target.IsValidTarget(Spells.Q.Range))
            {
                Spells.Q.Cast(target);
            }
        }

        public static void AutoHarass()
        {
            if (Orbwalker.IsAutoAttacking)
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "useEAutoHarass") && Spells.E.IsReady() &&
                target.IsValidTarget(Spells.E.Range) &&
                Player.Instance.HealthPercent >= Config.GetSliderValue(Config.Harass, "autoHarassHP"))
            {
                Spells.E.Cast(target);
            }
            if (Config.IsChecked(Config.Harass, "useQAutoHarass") && Spells.Q.IsReady() &&
                target.IsValidTarget(Spells.Q.Range))
            {
                Spells.Q.Cast(target);
            }
        }

        public static void LaneClear()
        {
            if (Orbwalker.IsAutoAttacking)
            {
                return;
            }
            var targets =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.Q.Range);
            var objAiMinions = targets as Obj_AI_Minion[] ?? targets.ToArray();
            if (!objAiMinions.Any())
            {
                return;
            }
            if (Config.IsChecked(Config.LaneClear, "useELaneClear") && Spells.E.IsReady() &&
                Player.Instance.HealthPercent >= Config.GetSliderValue(Config.LaneClear, "laneClearHP"))
            {
                if (Config.IsChecked(Config.LaneClear, "laneClearEonlyKill"))
                {
                    var eTarget =
                        objAiMinions.FirstOrDefault(
                            m =>
                                m.Health <= Player.Instance.GetSpellDamage(m, SpellSlot.E) &&
                                m.IsValidTarget(Spells.E.Range));
                    if (eTarget != null)
                    {
                        Spells.E.Cast(eTarget);
                    }
                }
                else
                {
                    var eTarget = objAiMinions.FirstOrDefault(m => m.IsValidTarget(Spells.E.Range));
                    if (eTarget != null)
                    {
                        Spells.E.Cast(eTarget);
                    }
                }
            }
            if (Config.IsChecked(Config.LaneClear, "useQLaneClear") && Spells.Q.IsReady())
            {
                var qFarmPos = EntityManager.MinionsAndMonsters.GetLineFarmLocation(
                    objAiMinions, Spells.Q.Width, (int) Spells.Q.Range);
                if (qFarmPos.HitNumber >= Config.GetSliderValue(Config.LaneClear, "minQTargets"))
                {
                    Spells.Q.Cast(qFarmPos.CastPosition);
                }
            }
        }

        public static void JungleClear()
        {
            if (Orbwalker.IsAutoAttacking)
            {
                return;
            }
            var target =
                EntityManager.MinionsAndMonsters.Monsters.Where(m => m.Distance(Player.Instance.Position) < 500)
                    .OrderByDescending(m => m.MaxHealth)
                    .FirstOrDefault();
            if (target == null)
            {
                return;
            }
            if (Spells.Q.IsReady() && Config.IsChecked(Config.JungleClear, "useQJungleClear"))
            {
                Spells.Q.Cast(target);
            }
            if (target.IsValidTarget(Spells.E.Range) && Spells.E.IsReady() &&
                Config.IsChecked(Config.JungleClear, "useEJungleClear"))
            {
                Spells.E.Cast(target);
                if (target.Health <=
                    Player.Instance.GetSpellDamage(target, SpellSlot.E) + Player.Instance.GetAutoAttackDamage(target))
                {
                    return;
                }
                Bursting = true;
                FjTimer1.Start();
                Orbwalker.ResetAutoAttack();
                Orbwalker.OnPostAttack += Computed.FastJungle1;
            }
        }

        public static void AutoEks()
        {
            var target =
                EntityManager.Heroes.Enemies.FirstOrDefault(
                    e => e.Health <= Player.Instance.GetSpellDamage(e, SpellSlot.E));
            if (target == null)
            {
                return;
            }
            Spells.E.Cast(target);
        }

        public static void AutoR()
        {
            if (Config.IsChecked(Config.AutoR, "autoRonlyCombo") &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                return;
            }
            if (!Spells.R.IsReady())
            {
                return;
            }
            if ((Player.Instance.HasBuffOfType(BuffType.Stun) && Config.IsChecked(Config.AutoR, "autoRStun")) ||
                (Player.Instance.HasBuffOfType(BuffType.Suppression) &&
                 Config.IsChecked(Config.AutoR, "autoRSuppression")) ||
                (Player.Instance.HasBuffOfType(BuffType.Snare) && Config.IsChecked(Config.AutoR, "autoRSnare")) ||
                (Player.Instance.HasBuffOfType(BuffType.Charm) && Config.IsChecked(Config.AutoR, "autoRCharm")) ||
                (Player.Instance.HasBuffOfType(BuffType.Fear) && Config.IsChecked(Config.AutoR, "autoRFear")) ||
                (Player.Instance.HasBuffOfType(BuffType.Blind) && Config.IsChecked(Config.AutoR, "autoRBlind")) ||
                (Player.Instance.HasBuffOfType(BuffType.Flee) && Config.IsChecked(Config.AutoR, "autoRFlee")) ||
                (Player.Instance.HasBuffOfType(BuffType.Polymorph) && Config.IsChecked(Config.AutoR, "autoRPolymorph")) ||
                (Player.Instance.HasBuffOfType(BuffType.Taunt) && Config.IsChecked(Config.AutoR, "autoRTaunt")) ||
                (Player.Instance.HasBuffOfType(BuffType.Silence) && Config.IsChecked(Config.AutoR, "autoRSilence")) ||
                (Player.Instance.HasBuffOfType(BuffType.Slow) && Config.IsChecked(Config.AutoR, "autoRSlow")) ||
                Player.Instance.HasBuffOfType(BuffType.Knockup) && Config.IsChecked(Config.AutoR, "autoRKnockup"))
            {
                if (Config.IsChecked(Config.AutoR, "humanAutoR"))
                {
                    Core.DelayAction(() => Spells.R.Cast(), Mainframe.RDelay.Next(80, 150));
                }
                else
                {
                    Spells.R.Cast();
                }
            }
        }

        public static void Flee()
        {
            throw new NotImplementedException();
        }
    }
}