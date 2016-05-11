using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace XinZhao
{
    internal class Computed
    {
        public static void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            throw new NotImplementedException();
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "XenZhaoComboTarget")
            {
                Orbwalker.ResetAutoAttack();
            }
        }

        public static void JungleOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) ||
                Player.Instance.ManaPercent < Config.GetSliderValue(Config.JungleClear, "jcMana"))
            {
                return;
            }
            var jngTargets =
                EntityManager.MinionsAndMonsters.Monsters.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.E.Range).OrderByDescending(m => m.MaxHealth);
            var jngTarget = jngTargets.FirstOrDefault();
            if (jngTarget == null || target.NetworkId != jngTarget.NetworkId)
            {
                return;
            }
            var combinedHealth =
                jngTargets.Where(m => m.Distance(Player.Instance.Position) <= 250).Sum(targut => targut.Health);
            if (Spells.W.CanCast() && combinedHealth >= Player.Instance.GetAutoAttackDamage(jngTarget) * 4 &&
                Config.IsChecked(Config.JungleClear, "useWJC"))
            {
                Spells.W.Cast();
            }
            if (Spells.Q.CanCast() && combinedHealth >= Player.Instance.GetAutoAttackDamage(jngTarget) * 4 &&
                Config.IsChecked(Config.JungleClear, "useQJC"))
            {
                Spells.Q.Cast();
            }
        }

        public static void LaneOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) ||
                Player.Instance.ManaPercent < Config.GetSliderValue(Config.LaneClear, "lcMana"))
            {
                return;
            }
            var minz =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m => m.Distance(Player.Instance.Position) <= Spells.E.Range).OrderByDescending(m => m.MaxHealth);
            var minion = minz.FirstOrDefault();
            if (minion == null)
            {
                return;
            }
            var combinedHealth =
                minz.Where(m => m.Distance(Player.Instance.Position) <= 350).Sum(targut => targut.Health);
            if (Spells.W.CanCast() && combinedHealth >= Player.Instance.GetAutoAttackDamage(minion) * 4 &&
                Config.IsChecked(Config.LaneClear, "useWLC"))
            {
                Spells.W.Cast();
            }
            if (Spells.Q.CanCast() && combinedHealth >= Player.Instance.GetAutoAttackDamage(minion) * 4 &&
                Config.IsChecked(Config.LaneClear, "useQLC"))
            {
                Spells.Q.Cast();
            }
        }

        public static void ComboOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var comboTarget = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                if (comboTarget == null || target.NetworkId != comboTarget.NetworkId)
                {
                    return;
                }
                if (Spells.W.CanCast() && Config.IsChecked(Config.Combo, "useWcombo"))
                {
                    Spells.W.Cast();
                }
                if (Spells.Q.CanCast() && Config.IsChecked(Config.Combo, "useQcombo"))
                {
                    Spells.Q.Cast();
                }
            }
        }

        public static void HarassOnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) ||
                Player.Instance.ManaPercent < Config.GetSliderValue(Config.Harass, "harassMana"))
            {
                return;
            }
            var comboTarget = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
            if (comboTarget == null || target.NetworkId != comboTarget.NetworkId)
            {
                return;
            }
            if (Spells.W.CanCast() && Config.IsChecked(Config.Harass, "useWharass"))
            {
                Spells.W.Cast();
            }
            if (Spells.Q.CanCast() && Config.IsChecked(Config.Harass, "useQharass"))
            {
                Spells.Q.Cast();
            }
        }

        public static void Xinsec()
        {
            AIHeroClient xinsecTarget = null;
            Obj_AI_Base eTargetMin = null;
            switch (Config.GetComboBoxValue(Config.Misc, "xinsecTargetting"))
            {
                case 0:
                    xinsecTarget = TargetSelector.SelectedTarget;
                    break;
                case 1:
                    xinsecTarget = TargetSelector.GetTarget(2000, DamageType.Mixed);
                    break;
                case 2:
                    xinsecTarget =
                        EntityManager.Heroes.Enemies.Where(en => en.Distance(Player.Instance.Position) <= 2000)
                            .OrderBy(en => en.MaxHealth)
                            .FirstOrDefault();
                    break;
            }
            if (xinsecTarget == null || !Spells.E.CanCast() || !Spells.R.CanCast() ||
                xinsecTarget.HasBuff("XinZhaoIntimidate"))
            {
                return;
            }
            var xinsecTargetExtend = Vector3.Zero;
            var closeAllies =
                EntityManager.Heroes.Allies.Where(
                    a => !a.IsMe && a.Distance(Player.Instance.Position) <= 750 && a.Distance(xinsecTarget) > 150)
                    .OrderBy(a => a.Distance(Player.Instance.Position));
            var closeTurrets = EntityManager.Turrets.Allies.OrderBy(t => t.Distance(Player.Instance.Position));
            if (closeTurrets.Any())
            {
                xinsecTargetExtend = xinsecTarget.Position.Extend(closeTurrets.FirstOrDefault().Position, -185).To3D();
            }
            if (closeAllies.Any())
            {
                xinsecTargetExtend = xinsecTarget.Position.Extend(closeAllies.FirstOrDefault().Position, -185).To3D();
            }
            var eTarget =
                EntityManager.Heroes.Enemies.Where(
                    m =>
                        m.Distance(xinsecTarget) <= Spells.R.Range - 20 && m.Distance(xinsecTargetExtend) <= 50 &&
                        Player.Instance.Position.Distance(xinsecTarget) + 50 < Player.Instance.Position.Distance(m) &&
                        m.IsValidTarget(Spells.E.Range)).OrderBy(e => e.Distance(Player.Instance)).FirstOrDefault();
            if (eTarget != null)
            {
                Spells.E.Cast(eTarget);
                Core.DelayAction(() => Spells.R.Cast(), 300);
            }
            else
            {
                if (Spells.Flash.IsReady && Config.IsChecked(Config.Misc, "xinsecFlash"))
                {
                    var eTargetHero =
                        EntityManager.Heroes.Enemies.Where(
                            m =>
                                m.Distance(Player.Instance.Position) <= Spells.E.Range &&
                                m.Distance(xinsecTargetExtend) < Spells.FlashRange - 25)
                            .OrderBy(m => m.Distance(xinsecTargetExtend))
                            .FirstOrDefault();
                    if (eTargetHero != null)
                    {
                        eTargetMin = eTargetHero;
                    }
                    var eTargetMinion =
                        EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                            m =>
                                m.Distance(Player.Instance.Position) <= Spells.E.Range &&
                                m.Distance(xinsecTargetExtend) < Spells.FlashRange - 25)
                            .OrderBy(m => m.Distance(xinsecTargetExtend))
                            .FirstOrDefault();
                    if (eTargetMinion != null)
                    {
                        eTargetMin = eTargetMinion;
                    }
                    if (eTargetMin != null)
                    {
                        var castDelay = Mainframe.RDelay.Next(325, 375);
                        Spells.E.Cast(eTargetMin);
                        if (closeTurrets.Any())
                        {
                            xinsecTargetExtend =
                                xinsecTarget.Position.Extend(closeTurrets.FirstOrDefault().Position, -185).To3D();
                        }
                        if (closeAllies.Any())
                        {
                            xinsecTargetExtend =
                                xinsecTarget.Position.Extend(closeAllies.FirstOrDefault().Position, -185).To3D();
                        }
                        Core.DelayAction(() => Player.CastSpell(Spells.Flash.Slot, xinsecTargetExtend), castDelay);
                        Core.DelayAction(() => Spells.R.Cast(), castDelay + 70);
                    }
                }
            }
        }

        public static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe) {}
        }
    }
}