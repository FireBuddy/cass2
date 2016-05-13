using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                Player.Instance.ManaPercent >= Config.GetSliderValue(Config.JungleClear, "jcMana"))
            {
                Modes.JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) &&
                Player.Instance.ManaPercent >= Config.GetSliderValue(Config.Harass, "harassMana"))
            {
                Modes.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                Player.Instance.ManaPercent >= Config.GetSliderValue(Config.LaneClear, "lcMana"))
            {
                Modes.LaneClear();
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (!Config.IsChecked(Config.Draw, "drawXinsec") || Player.Instance.IsDead)
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

            if (xinsecTarget != null && Spells.E.CanCast() && Spells.R.CanCast() &&
                !xinsecTarget.HasBuff("XinZhaoIntimidate") && !xinsecTarget.IsInvulnerable)
            {
                Drawing.DrawText(
                    Drawing.WorldToScreen(xinsecTarget.Position), Color.AntiqueWhite,
                    "Xinsec:" + Environment.NewLine + xinsecTarget.ChampionName, 10);
                if (Config.IsChecked(Config.Draw, "drawXinsecpred"))
                {
                    var extendToPos = Vector3.Zero;
                    Obj_AI_Base bestAlly =
                        EntityManager.Heroes.Allies.Where(
                            x => x.Distance(Player.Instance.Position) <= 1500 && !x.IsMe && !x.IsDead && x.IsValid)
                            .OrderByDescending(x => x.CountAlliesInRange(750))
                            .FirstOrDefault();
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
                            EntityManager.Turrets.Allies.Where(t => t.Distance(Player.Instance.Position) <= 1500)
                                .OrderBy(t => t.Distance(Player.Instance.Position))
                                .FirstOrDefault();
                        if (closeTurret != null)
                        {
                            extendToPos = closeTurret.Position;
                        }
                        else
                        {
                            var nex =
                                ObjectManager.Get<Obj_Building>()
                                    .FirstOrDefault(x => x.Name.StartsWith("HQ") && x.IsAlly);
                            if (nex != null)
                            {
                                extendToPos = nex.Position;
                            }
                        }
                    }
                    var xinsecTargetExtend = xinsecTarget.Position.Extend(extendToPos, -200).To3D();
                    Drawing.DrawCircle(xinsecTargetExtend, 100, Color.AliceBlue);
                }
            }
        }
    }
}