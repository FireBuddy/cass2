namespace CassOp
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Events;

    /*
    TODO:
        
    */

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != Champion.Cassiopeia.ToString())
            {
                return;
            }

            Config.CallMenu();
            Spells.LoadSpells();
            Mainframe.Init();
        }

        #endregion
    }
}