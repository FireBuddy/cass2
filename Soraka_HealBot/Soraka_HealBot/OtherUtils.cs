namespace Soraka_HealBot
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    using Soraka_HealBot.Extensions;

    internal class OtherUtils
    {
        #region Static Fields

        public static readonly Random RDelay = new Random();

        private static DangerLevel wantedLevel;

        #endregion

        #region Public Methods and Operators

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

        public static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (sender.IsMe || sender.IsAlly || !Config.IsChecked(Config.Interrupter, "bInterrupt"))
            {
                return;
            }

            switch (Config.GetComboBoxValue(Config.Interrupter, "dangerL"))
            {
                case 0:
                    wantedLevel = DangerLevel.Low;
                    break;
                case 1:
                    wantedLevel = DangerLevel.Medium;
                    break;
                case 2:
                    wantedLevel = DangerLevel.High;
                    break;
            }

            if (Spells.E.CanCast() && Spells.E.IsInRange(sender) && args.DangerLevel == wantedLevel)
            {
                var delay = RDelay.Next(100, 120);
                Core.DelayAction(() => Spells.E.Cast(sender), delay);
            }
        }

        #endregion
    }
}