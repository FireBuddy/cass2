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

        internal static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            Spells.LastSpellSlot = args.Slot;
        }

        internal static int PassiveCount()
        {
            return Player.Instance.Buffs.First(buff => buff.Name == "sonapassive").Count;
        }

        #endregion
    }
}