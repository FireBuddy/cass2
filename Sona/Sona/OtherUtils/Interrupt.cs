namespace Sona.OtherUtils
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;

    using Sona.Extensions;

    internal static class Interrupt
    {
        #region Static Fields

        private static DangerLevel wanteDangerLevel;

        #endregion

        #region Methods

        internal static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsMe || sender.IsAlly || !Config.IsChecked(Config.InterrupterMenu, "br"))
            {
                return;
            }

            switch (Config.GetComboBoxValue(Config.InterrupterMenu, "dangerL"))
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

            if (Spells.R.CanCast() && sender.IsValidTarget(Spells.R.Range) && e.DangerLevel == wanteDangerLevel)
            {
                var delay = Computed.RDelay.Next(100, 120);
                Core.DelayAction(() => Spells.R.Cast(sender), delay);
            }
        }

        #endregion
    }
}