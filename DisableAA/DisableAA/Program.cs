namespace DisableAA
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Program
    {
        #region Static Fields

        internal static Menu DisableMenu;

        #endregion

        #region Methods

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadingComplete;
        }

        private static void OnLoadingComplete(EventArgs args)
        {
            DisableMenu = MainMenu.AddMenu("Disable AA", "disableaa");
            DisableMenu.Add("key", new KeyBind("Key to disable AA", false, KeyBind.BindTypes.HoldActive));
            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (DisableMenu["key"].Cast<KeyBind>().CurrentValue && !Orbwalker.DisableAttacking)
            {
                Orbwalker.DisableAttacking = true;
            }

            if (!DisableMenu["key"].Cast<KeyBind>().CurrentValue && Orbwalker.DisableAttacking)
            {
                Orbwalker.DisableAttacking = false;
            }
        }

        #endregion
    }
}