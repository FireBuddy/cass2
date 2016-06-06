namespace CassOp
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Mainframe
    {
        #region Static Fields

        public static readonly Random RDelay = new Random();

        #endregion

        #region Properties

        internal static bool DebugDrawings { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void Init()
        {
            Game.OnTick += OnGameUpdate;
            Drawing.OnDraw += OnDraw;

            // Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Computed.OnSpellCast;
            Spellbook.OnCastSpell += Computed.OnSpellbookCastSpell;
            Orbwalker.OnUnkillableMinion += Computed.OnUnkillableMinion;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Gapcloser.OnGapcloser += OtherUtils.OnGapCloser;
            Chat.OnInput += OnChatInput;
            Modes.Obj_AI_Base.OnBasicAttack += Obj_AI_Base_OnBasicAttack;
        }

        #endregion

        #region Methods

        private static void OnChatInput(ChatInputEventArgs args)
        {
            if (args.Input == "//debugdraw")
            {
                DebugDrawings = !DebugDrawings;
                args.Process = false;
            }

            if (args.Input == "//mrdmg")
            {
                args.Process = false;
                Chat.Print("PlayerMR: " + Player.Instance.FlatMagicReduction);
                foreach (var enemy in EntityManager.Heroes.Enemies)
                {
                    Chat.Print(enemy.ChampionName + ": " + Spells.GetEDamage(enemy));
                }
            }
        }

        private static void OnDraw(EventArgs args)
        {
            if (DebugDrawings)
            {
                var front =
                    EntityManager.Heroes.Enemies.Where(e => !e.IsDead && e.IsVisible && e.IsValid && e.IsHPBarRendered);
                foreach (var f in front)
                {
                    var relPos = f.Position.Shorten(Player.Instance.Position, -300);
                    ColorBGRA drawColor = f.Health <= Computed.ComboDmg(f) * Spells.ComboDmgMod
                                              ? Color.OrangeRed
                                              : Color.Yellow;
                    Circle.Draw(drawColor, 100, relPos);
                    Drawing.DrawText(Drawing.WorldToScreen(relPos), System.Drawing.Color.AliceBlue, f.Name, 2);
                    if (f.IsFacing(Player.Instance))
                    {
                        Drawing.DrawText(Drawing.WorldToScreen(f.Position), System.Drawing.Color.OrangeRed, "Facing", 3);
                    }
                }
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }

            Modes.PermActive();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Modes.Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Modes.LaneClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Modes.JungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                Modes.LastHit();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                // Modes.Flee();
                // _fleeActivated = true;
            }

            if (Config.IsChecked(Config.Misc, "clearE")
                && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.Misc, "manaClearE")
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Computed.AutoClearE();
            }

            if (Config.IsChecked(Config.Misc, "tearStackQ")
                && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.Misc, "manaTearStack")
                && Computed.HasTear() && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                Computed.TearStack();
            }
        }

        #endregion
    }
}
