namespace Olaf
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class Mainframe
    {
        #region Static Fields

        internal static Vector3 PlayerIssuePos;

        internal static Vector3 PickUpPosition;

        #endregion

        #region Public Properties

        public static Random RDelay { get; } = new Random();

        #endregion

        #region Properties

        private static AIHeroClient Player => EloBuddy.Player.Instance;

        #endregion

        #region Public Methods and Operators

        public static void Init()
        {
            Game.OnTick += OnGameUpdate;
            GameObject.OnCreate += Computed.OnObjectCreate;
            GameObject.OnDelete += Computed.OnObjectDelete;
            EloBuddy.Player.OnIssueOrder += Computed.OnIssueOrder;
            Orbwalker.OnUnkillableMinion += Computed.OnUnkillableMinion;
            Drawing.OnDraw += OnDraw;

            Modes.FcTimer1.Elapsed += Computed.FcTimer1Elapsed;
            Modes.FcTimer2.Elapsed += Computed.FcTimer2Elapsed;
            Modes.FjTimer1.Elapsed += Computed.FjTimer1Elapsed;
            Modes.FjTimer2.Elapsed += Computed.FjTimer2Elapsed;
        }

        public static void OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Config.IsChecked(Config.Misc, "autoPick") && Spells.AxeObject != null
                && Spells.AxeObject.Position.Distance(Player.Position)
                <= Config.GetSliderValue(Config.Misc, "axePickRange") && !Computed.IsPickingUp && !Modes.Bursting
                && Modes.SafeToPickup && Orbwalker.LastHitMinion == null)
            {
                PickUpPosition = Spells.AxeObject.Position.Shorten(Player.Position, RDelay.Next(-70, 10));
                if (!PickUpPosition.UnderEnemyTurret())
                {
                    Computed.IsPickingUp = true;
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    Game.OnTick += PickUpAxe;
                    Game.OnTick -= OnGameUpdate;
                }
            }

            if (Config.IsChecked(Config.AutoR, "useAutoR")
                && Player.HealthPercent <= Config.GetSliderValue(Config.AutoR, "autoRHP"))
            {
                Modes.AutoR();
            }

            if (Config.IsChecked(Config.Misc, "autoEKS"))
            {
                Modes.AutoEks();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                && Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "harassMana"))
            {
                Modes.Harass();
            }

            if (Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "autoHarassMana"))
            {
                Modes.AutoHarass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)
                && Player.ManaPercent >= Config.GetSliderValue(Config.JungleClear, "jungleClearMana"))
            {
                Modes.JungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)
                && Player.ManaPercent >= Config.GetSliderValue(Config.LaneClear, "laneClearMana"))
            {
                Modes.LaneClear();
            }
        }

        public static void PickUpAxe(EventArgs args)
        {
            if (Spells.AxeObject.BBox.Contains(Player.Position) != ContainmentType.Contains
                || Spells.AxeObject.BBox.Contains(Player.Position) != ContainmentType.Intersects)
            {
                PickUpPosition.X += RDelay.Next(-3, 3);
                PickUpPosition.Y += RDelay.Next(-3, 3);
                Orbwalker.MoveTo(PickUpPosition);
            }
        }

        #endregion

        #region Methods

        private static void OnDraw(EventArgs args)
        {
            if (Spells.AxeObject != null && Config.IsChecked(Config.Draw, "drawAxe"))
            {
                Circle.Draw(Color.DarkRed, Spells.AxeObject.BoundingRadius, 5f, Spells.AxeObject);
                if (PickUpPosition != Vector3.Zero && Config.IsChecked(Config.Draw, "drawPickup"))
                {
                    Circle.Draw(Color.DarkRed, 50, PickUpPosition);
                }
            }

            if (Config.IsChecked(Config.Draw, "drawStates"))
            {
                Drawing.DrawText(0, 0, System.Drawing.Color.FloralWhite, "Auto R: ");
                if (Config.IsChecked(Config.AutoR, "useAutoR") && !Config.IsChecked(Config.AutoR, "autoRonlyCombo"))
                {
                    Drawing.DrawText(
                        55, 
                        0, 
                        System.Drawing.Color.Green, 
                        Config.IsChecked(Config.AutoR, "useAutoR").ToString());
                }

                if (Config.IsChecked(Config.AutoR, "useAutoR") && Config.IsChecked(Config.AutoR, "autoRonlyCombo"))
                {
                    Drawing.DrawText(55, 0, System.Drawing.Color.YellowGreen, "Only Combo");
                }

                if (!Config.IsChecked(Config.AutoR, "useAutoR"))
                {
                    Drawing.DrawText(
                        55, 
                        0, 
                        System.Drawing.Color.DarkRed, 
                        Config.IsChecked(Config.AutoR, "useAutoR").ToString());
                }

                if (Config.IsChecked(Config.AutoR, "useAutoR") && Config.IsChecked(Config.AutoR, "humanAutoR"))
                {
                    Drawing.DrawText(140, 0, System.Drawing.Color.YellowGreen, "Humanized");
                }

                Drawing.DrawText(0, 25, System.Drawing.Color.FloralWhite, "Auto Q: ");
                Drawing.DrawText(
                    55, 
                    25, 
                    Config.IsChecked(Config.Harass, "useQAutoHarass")
                        ? System.Drawing.Color.Green
                        : System.Drawing.Color.DarkRed, 
                    Config.IsChecked(Config.Harass, "useQAutoHarass").ToString());

                Drawing.DrawText(0, 50, System.Drawing.Color.FloralWhite, "Auto E: ");
                Drawing.DrawText(
                    55, 
                    50, 
                    Config.IsChecked(Config.Harass, "useEAutoHarass")
                        ? System.Drawing.Color.Green
                        : System.Drawing.Color.DarkRed, 
                    Config.IsChecked(Config.Harass, "useEAutoHarass").ToString());
                Drawing.DrawText(0, 75, System.Drawing.Color.FloralWhite, "PickUp: ");
                Drawing.DrawText(
                    55, 
                    75, 
                    Config.IsChecked(Config.Misc, "autoPick")
                        ? System.Drawing.Color.Green
                        : System.Drawing.Color.DarkRed, 
                    Config.IsChecked(Config.Misc, "autoPick").ToString());
            }
        }

        #endregion
    }
}