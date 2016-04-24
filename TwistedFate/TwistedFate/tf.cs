using System;
using EloBuddy;
using EloBuddy.SDK;

namespace TwistedFate
{
    public static class Tf
    {
        public static readonly Random RDelay = new Random();
        private static AIHeroClient Player => EloBuddy.Player.Instance;


        public static void Init()
        {
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Orbwalker.OnPreAttack += OnBeforeAttack;
            Orbwalker.OnPostAttack += Computed.OnPostAttack;
            //Drawing.OnDraw += Drawing_OnDraw;
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

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "Gate" && Config.IsChecked(Config.Misc, "AutoYAG"))
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
            if (!sender.IsMe) {}
        }

        //private static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs args) {}

        private static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (args.Target is AIHeroClient)
            {
                args.Process = CardSelector.Status != SelectStatus.Selecting &&
                               Environment.TickCount - CardSelector.LastWSent > 300;
            }
        }

        //private static void Drawing_OnDraw(EventArgs args) {}
    }
}