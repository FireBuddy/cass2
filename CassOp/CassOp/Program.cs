using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace CassOp
{
    /*
    TODO:
        
    */

    internal class Program
    {
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
    }
}