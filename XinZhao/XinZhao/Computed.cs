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
                var comboTarget = TargetSelector.GetTarget(Spells.E.Range - 100, DamageType.Physical);
                if (!Spells.E.CanCast())
                {
                    comboTarget = TargetSelector.GetTarget(175, DamageType.Physical);
                }
                if (comboTarget == null || target.NetworkId != comboTarget.NetworkId || comboTarget.IsInvulnerable)
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
            if (comboTarget == null || target.NetworkId != comboTarget.NetworkId || comboTarget.IsInvulnerable)
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
            Obj_AI_Base eTarget = null;
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
                xinsecTarget.HasBuff("XinZhaoIntimidate") || xinsecTarget.IsInvulnerable)
            {
                return;
            }
            var extendToPos = Vector3.Zero;
            Obj_AI_Base bestAlly = null;
            foreach (var ally in EntityManager.Heroes.Allies.Where(x => x.Distance(Player.Instance.Position) <= 1500 && !x.IsMe && !x.IsDead && x.IsValid))
            {
                if (bestAlly == null)
                {
                    bestAlly = ally;
                }
                if (ally.CountAlliesInRange(750) > bestAlly.CountAlliesInRange(750))
                {
                    bestAlly = ally;
                }
            }
            if (bestAlly != null)
            {
                var bestAllyMasz =
                       EntityManager.Heroes.Allies.Where(
                           a => a.Distance(bestAlly.Position) <= 750 && !a.IsMe && !a.IsDead && a.IsValid)
                           .ToArray();
                if (bestAllyMasz.Any())
                {
                    var bestallv2 = new Vector2[bestAllyMasz.Count()];
                    for (var i = 0; i < bestAllyMasz.Count(); i++)
                    {
                        bestallv2[i] = bestAllyMasz[i].Position.To2D();
                    }
                    extendToPos = bestallv2.CenterPoint().To3D();
                }
            }
            else
            {
                var closeTurret =
                    EntityManager.Turrets.Allies.Where(t => t.Distance(Player.Instance.Position) <= 1000)
                        .OrderBy(t => t.Distance(Player.Instance.Position))
                        .FirstOrDefault();
                if (closeTurret != null)
                {
                    extendToPos = closeTurret.Position;
                }
                else
                {
                    var nex = ObjectManager.Get<Obj_Building>().FirstOrDefault(x => x.Name.StartsWith("HQ") && x.IsAlly);
                    if (nex != null)
                    {
                        extendToPos = nex.Position;
                    }
                }
            }
            var xinsecTargetExtend = xinsecTarget.Position.Extend(extendToPos, -200).To3D();
            var eTargetMinion =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(
                    m =>
                        m.Distance(Player.Instance.Position) <= Spells.E.Range &&
                        m.Distance(xinsecTargetExtend) < Spells.FlashRange - 25)
                    .OrderBy(m => m.Distance(xinsecTargetExtend))
                    .FirstOrDefault();
            if (eTargetMinion != null)
            {
                eTarget = eTargetMinion;
            }
            else
            {
                var eTargetHero =
                    EntityManager.Heroes.Enemies.Where(
                        m =>
                            m.Distance(Player.Instance.Position) <= Spells.E.Range &&
                            m.Distance(xinsecTargetExtend) < Spells.FlashRange - 25 && m != xinsecTarget &&
                            m.NetworkId != xinsecTarget.NetworkId)
                        .OrderBy(m => m.Distance(xinsecTargetExtend))
                        .FirstOrDefault();
                if (eTargetHero != null)
                {
                    eTarget = eTargetHero;
                }
            }
            if (eTarget != null)
            {
                if (eTarget.Distance(xinsecTargetExtend) <= 40 && eTarget.Distance(xinsecTarget) > 7)
                {
                    Spells.E.Cast(eTarget);
                    Core.DelayAction(() => Spells.R.Cast(), 300);
                }
                if (eTarget.Distance(xinsecTargetExtend) > 40 && Spells.Flash.IsReady &&
                    Config.IsChecked(Config.Misc, "xinsecFlash"))
                {
                    var castDelay = Mainframe.RDelay.Next(325, 375);
                    Spells.E.Cast(eTarget);
                    Core.DelayAction(() => Player.CastSpell(Spells.Flash.Slot, xinsecTargetExtend), castDelay);
                    Core.DelayAction(() => Spells.R.Cast(), castDelay + 40);
                }
            }
        }


        public static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe) {}
        }
    }
}