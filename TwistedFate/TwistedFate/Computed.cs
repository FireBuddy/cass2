﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace TwistedFate
{
    internal class Computed
    {
        public static int RandomDelay(int x)
        {
            var y = x;
            var i = Math.Abs(x);
            while (i >= 10)
            {
                i /= 10;
            }
            i = y / i;
            return Tf.RDelay.Next(y - i, y + i);
        }

        public static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var enemyTarget = EntityManager.Heroes.Enemies.FirstOrDefault(e => e.NetworkId == target.NetworkId);
            if (enemyTarget != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                Config.IsChecked(Config.Combo, "qAAReset"))
            {
                Spells.Q.Cast(enemyTarget);
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "Gate" && Config.IsChecked(Config.Misc, "AutoYAG"))
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }
            if (!sender.IsMe) {}
        }

        public static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (args.Target is AIHeroClient)
            {
                args.Process = CardSelector.Status != SelectStatus.Selecting &&
                               Environment.TickCount - CardSelector.LastWSent > 300;
            }
            if (CardSelector.Status == SelectStatus.Selecting &&
                ((Config.IsChecked(Config.Combo, "disableAAselectingC") &&
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) ||
                 (Config.IsChecked(Config.Harass, "disableAAselectingH") &&
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) ||
                 (Config.IsChecked(Config.LaneClear, "disableAAselectingLC") &&
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) ||
                 (Config.IsChecked(Config.JungleClear, "disableAAselectingJC") &&
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))))
            {
                args.Process = false;
            }
        }
    }
}