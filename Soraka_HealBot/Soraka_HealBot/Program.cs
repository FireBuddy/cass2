namespace Soraka_HealBot
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    public static class Program
    {
        #region Public Methods and Operators

        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        #endregion

        #region Methods

        private static void OnLoadingComplete(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName.ToLower() != "soraka")
            {
                return;
            }

            Bootstrap.Init(null);
            Config.CallMenu();
            Spells.LoadSpells();
            Mainframe.Init();
        }

        #endregion
    }
}