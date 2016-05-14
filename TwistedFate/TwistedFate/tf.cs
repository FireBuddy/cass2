using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace TwistedFate
{
    public static class Tf
    {
        public static readonly Random RDelay = new Random();
        private static AIHeroClient Player => EloBuddy.Player.Instance;

        internal static int originalSkinId;


        public static void Init()
        {
            Game.OnTick += OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Computed.YellowIntoQ;
            Orbwalker.OnPreAttack += Computed.OnBeforeAttack;
            Orbwalker.OnPostAttack += Computed.OnPostAttack;
            Drawing.OnDraw += Drawing_OnDraw;

            originalSkinId = Player.SkinId;
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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                //Modes.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                //Modes.Flee();
            }
        }

        //private static void Game_OnTick(EventArgs args) {}
        //private static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs args) {}
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.Misc, "drawRrange"))
            {
                Circle.Draw(Color.AliceBlue, Spells.R.Range, Player);
            }
        }

        public static void OnUseSkinChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (!args.NewValue)
            {
                Player.SetSkinId(originalSkinId);
            }
            if (args.NewValue)
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
    }
}