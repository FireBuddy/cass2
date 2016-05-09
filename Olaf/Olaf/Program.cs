using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Olaf
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != Champion.Olaf.ToString())
            {
                return;
            }
            Spells.LoadSpells();
            Config.CallMenu();
            Mainframe.Init();
        }
    }
}