using System;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace Support_Mode
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            Bootstrap.Init(null);
            Config.CallMenu();
            Mainstuff.Init();
        }
    }
}
