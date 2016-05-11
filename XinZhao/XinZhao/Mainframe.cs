using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace XinZhao
{
    internal class Mainframe
    {
        public static readonly Random RDelay = new Random();

        public static void Init()
        {
            Game.OnTick += OnGameUpdate;
            Orbwalker.OnPostAttack += Computed.JungleOnPostAttack;
            Orbwalker.OnPostAttack += Computed.ComboOnPostAttack;
            Orbwalker.OnPostAttack += Computed.LaneOnPostAttack;
            Orbwalker.OnPostAttack += Computed.HarassOnPostAttack;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Computed.OnSpellCast;
            Drawing.OnDraw += OnDraw;
            //Orbwalker.OnUnkillableMinion += Computed.OnUnkillableMinion;
            //Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
        }


        private static void OnGameUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }
            if (Config.IsKeyPressed(Config.Misc, "xinsecKey"))
            {
                Orbwalker.MoveTo(Game.CursorPos);
                Computed.Xinsec();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.JungleClear, "jcMana"))
            {
                Modes.JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.Harass, "harassMana"))
            {
                Modes.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.LaneClear, "lcMana"))
            {
                Modes.LaneClear();
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Config.IsChecked(Config.Draw, "drawXinsec"))
            {
                return;
            }
            AIHeroClient xinsecTarget = null;
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
            
            if (xinsecTarget != null && Spells.E.CanCast() && Spells.R.CanCast() && !xinsecTarget.HasBuff("XinZhaoIntimidate")) 
            {
                Drawing.DrawText(
                        Drawing.WorldToScreen(xinsecTarget.Position), Color.AntiqueWhite, "Xinsec", 10);
                if (Config.IsChecked(Config.Draw, "drawXinsecpred"))
                {
                    var xinsecTargetExtend = Vector3.Zero;
                    var closeAllies =
                        EntityManager.Heroes.Allies.Where(a => !a.IsMe && a.Distance(Player.Instance.Position) <= 750 && a.Distance(xinsecTarget) > 150)
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
                    Drawing.DrawCircle(xinsecTargetExtend, 100, Color.AliceBlue);
                }
                
            }
        }
    }
}