using System;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace XinZhao
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != Champion.XinZhao.ToString())
            {
                return;
            }
            Spells.LoadSpells();
            Config.CallMenu();
            Mainframe.Init();
        }
    }
}