namespace TwistedFate
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    public static class Tf
    {
        #region Static Fields

        public static readonly Random RDelay = new Random();

        #endregion

        #region Properties

        internal static int OriginalSkinId { get; set; }

        private static AIHeroClient Player => EloBuddy.Player.Instance;

        #endregion

        #region Public Methods and Operators

        public static void Init()
        {
            Spellbook.OnCastSpell += Computed.SafeCast;
            Game.OnTick += OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Computed.YellowIntoQ;
            Orbwalker.OnPreAttack += Computed.OnBeforeAttack;
            Orbwalker.OnPostAttack += Computed.OnPostAttack;
            Drawing.OnDraw += Drawing_OnDraw;

            OriginalSkinId = Player.SkinId;
            if (Config.IsChecked(Config.Misc, "useSkin"))
            {
                Player.SetSkinId(Config.GetSliderValue(Config.Misc, "skinId"));
            }
        }

        public static void OnSkinSliderChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            if (Config.IsChecked(Config.Misc, "useSkin"))
            {
                Player.SetSkinId(args.NewValue);
            }
        }

        public static void OnUseSkinChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (!args.NewValue)
            {
                Player.SetSkinId(OriginalSkinId);
            }

            if (args.NewValue)
            {
                Player.SetSkinId(Config.GetSliderValue(Config.Misc, "skinId"));
            }
        }

        #endregion

        #region Methods

        // private static void Game_OnTick(EventArgs args) {}
        // private static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs args) {}
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.Misc, "drawRrange"))
            {
                Circle.Draw(Color.AliceBlue, Spells.R.Range, Player);
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
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

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Modes.JungleClear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Modes.LaneClear();
            }
        }

        #endregion
    }
}