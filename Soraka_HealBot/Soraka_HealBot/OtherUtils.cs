using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace Soraka_HealBot
{
    internal class OtherUtils
    {
        private static DangerLevel _wantedLevel;
        public static readonly Random RDelay = new Random();

        public static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly || !Config.IsChecked(Config.Interrupter, "bInterrupt"))
            {
                return;
            }
            switch (Config.GetComboBoxValue(Config.Interrupter, "dangerL"))
            {
                case 0:
                    _wantedLevel = DangerLevel.Low;
                    break;
                case 1:
                    _wantedLevel = DangerLevel.Medium;
                    break;
                case 2:
                    _wantedLevel = DangerLevel.High;
                    break;
            }
            if (Spells.E.IsReady() && Spells.E.IsInRange(sender) && args.DangerLevel == _wantedLevel)
            {
                var ePred = Spells.E.GetPrediction(sender);
                if (ePred.HitChancePercent >= 90)
                {
                    var delay = RDelay.Next(100, 120);
                    Core.DelayAction(() => Spells.E.Cast(ePred.CastPosition), delay);
                }
            }
        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly)
            {
                return;
            }
            if (Config.IsChecked(Config.Gapclose, "qGapclose"))
            {
                if (Spells.Q.IsInRange(args.End) && Spells.Q.IsReady())
                {
                        var delay = RDelay.Next(100, 120);
                        Core.DelayAction(() => Spells.Q.Cast(args.End), delay);
                }
            }
            if (Config.IsChecked(Config.Gapclose, "eGapclose"))
            {
                if (Spells.E.IsInRange(args.End) && Spells.E.IsReady())
                {
                        var delay = RDelay.Next(100, 120);
                        Core.DelayAction(() => Spells.E.Cast(args.End), delay);
                }
            }
        }
    }
}