namespace Sona.OtherUtils
{
    using System;
    using System.Linq;

    using EloBuddy;

    internal static class Computed
    {
        #region Static Fields

        internal static readonly Random RDelay = new Random();

        #endregion

        #region Methods

        internal static int PassiveCount()
        {
            return Player.Instance.Buffs.First(buff => buff.Name == "sonapassive").Count;
        }

        #endregion
    }
}