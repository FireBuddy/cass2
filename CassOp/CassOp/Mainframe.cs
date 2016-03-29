﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace CassOp
{
    internal class Mainframe
    {
        public static readonly Random RDelay = new Random();
        private static AIHeroClient _Player => EloBuddy.Player.Instance;

        public static void Init()
        {
            Game.OnUpdate += OnGameUpdate;
            //Drawing.OnDraw += OnDraw;
            //Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
            Obj_AI_Base.OnSpellCast += Computed.OnSpellCast;
            Spellbook.OnCastSpell += Computed.OnSpellbookCastSpell;
            Orbwalker.OnUnkillableMinion += Computed.OnUnkillableMinion;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Gapcloser.OnGapcloser += OtherUtils.OnGapCloser;
        }

        private static void OnGameUpdate(EventArgs args)
        {
            if (_Player.IsDead)
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
                //Modes.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                //Modes.Flee();
                //_fleeActivated = true;
            }
        }
        
        private static void OnDraw(EventArgs args)
        {
            var front = EntityManager.Heroes.Enemies.Where(e => !e.IsDead && e.IsVisible && e.IsValid);
            foreach (var f in front)
            {
                var relPos = f.Position.Shorten(_Player.Position, -300);
                Circle.Draw(Color.White, 100, relPos);
                Drawing.DrawText(Drawing.WorldToScreen(relPos), System.Drawing.Color.AliceBlue, f.Name, 2);
            }
        }

        private static void DrawOnEndScene(EventArgs args)
        {
            /*var enemyHeroes = EntityManager.Heroes.Enemies.Where(h => h.Distance(_Player) <= 1500 && !h.IsDead && !h.IsZombie && h.IsVisible).AsEnumerable();
            foreach (var hero in enemyHeroes)
            {
                if (hero.IsFacing(_Player))
                {
                    Drawing.DrawText(Drawing.WorldToScreen(hero.Position) - new Vector2(35, -30), System.Drawing.Color.White, "Facing", 2);
                }
                else
                {
                    Drawing.DrawText(Drawing.WorldToScreen(hero.Position) - new Vector2(35, -30), System.Drawing.Color.Red, "Not Facing", 2);
                }
            }*/
        }
    }
}