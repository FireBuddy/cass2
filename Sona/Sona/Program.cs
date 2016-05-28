namespace Sona
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != Champion.Sona.ToString())
            {
                return;
            }

            Bootstrap.Init(null);
            Spells.InitSpells();
            Config.InitMenu();
            Mainframe.Init();
        }

        #endregion
    }
}