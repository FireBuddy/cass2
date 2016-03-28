using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace CassOp
{
    /*
    TODO:
        Calc combo dmg, use R on less than Config Min to kill if not possible otherwise
    */

    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != Champion.Cassiopeia.ToString())
            {
                return;
            }
            Config.CallMenu();
            Spells.LoadSpells();
            Mainframe.Init();
        }
    }
}