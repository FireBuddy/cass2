namespace Sona.OtherUtils
{
    using EloBuddy;
    using EloBuddy.SDK.Events;

    using Sona.Extensions;

    internal static class Gapclose
    {
        #region Methods

        internal static void OnGapclose(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Config.IsChecked(Config.GapcloseMenu, "bE") && Spells.E.CanCast())
            {
                Spells.E.Cast();
            }
        }

        #endregion
    }
}