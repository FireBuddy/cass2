namespace CassOp
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    internal class OtherUtils
    {
        #region Static Fields

        private static DangerLevel wanteDangerLevel;

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
                    var delay = Mainframe.RDelay.Next(100, 120);
                    Core.DelayAction(() => Spells.Q.Cast(args.End), delay);
                }
            }

            /*if (Config.IsChecked(Config.Gapclose, "wGapclose"))
            {
                if (Spells.W.IsInRange(sender) && Spells.W.IsReady())
                {
                    var ePred = Spells.W.GetPrediction(sender);
                    if (ePred.HitChancePercent >= 90)
                    {
                        var delay = Mainframe.RDelay.Next(100, 120);
                        Core.DelayAction(() => Spells.W.Cast(ePred.CastPosition), delay);
                    }
                }
            }*/
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
                    wanteDangerLevel = DangerLevel.Low;
                    break;
                case 1:
                    wanteDangerLevel = DangerLevel.Medium;
                    break;
                case 2:
                    wanteDangerLevel = DangerLevel.High;
                    break;
                default:
                    wanteDangerLevel = DangerLevel.High;
                    break;
            }

            if (Spells.R.IsReady() && sender.IsValidTarget(Spells.R.Range) && args.DangerLevel == wanteDangerLevel)
            {
                var rPred = Spells.R.GetPrediction(sender);
                if (rPred.HitChancePercent >= 90)
                {
                    var delay = Mainframe.RDelay.Next(100, 120);
                    Core.DelayAction(() => Spells.R.Cast(rPred.CastPosition), delay);
                }
            }
        }

        #endregion
    }
}