namespace Sona
{
    using System;

    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal static class Config
    {
        #region Properties

        internal static Menu AutoRMenu { get; private set; }

        internal static Menu AutoWMenu { get; private set; }

        internal static Menu ComboMenu { get; private set; }

        internal static Menu DrawMenu { get; private set; }

        internal static Menu GapcloseMenu { get; private set; }

        internal static Menu HarassMenu { get; private set; }

        internal static Menu InterrupterMenu { get; private set; }

        internal static Menu LaneClearMenu { get; private set; }

        internal static Menu SonaMenu { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static int GetComboBoxValue(Menu obj, string value)
        {
            return obj[value].Cast<ComboBox>().CurrentValue;
        }

        public static int GetSliderValue(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }

        public static bool IsChecked(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        #endregion

        #region Methods

        internal static void InitMenu()
        {
            SonaMenu = MainMenu.AddMenu("Sona", "sona");

            ComboMenu = SonaMenu.AddSubMenu("Combo", "combo");
            ComboMenu.Add("bQ", new CheckBox("Use Q"));
            ComboMenu.Add("bE", new CheckBox("Use E"));
            ComboMenu.Add("bR", new CheckBox("Use R"));
            ComboMenu.Add("minR", new Slider("Min Targets for R", 3, 1, 5));
            ComboMenu.Add("bSmartAA", new CheckBox("AA only after Q or on 3rd passive"));
            ComboMenu.AddGroupLabel("Flash R Settings");
            ComboMenu.Add("bFlashR", new CheckBox("Use Flash R"));
            ComboMenu.Add("minFlashR", new Slider("Min Targets for Flash R", 5, 1, 5));

            HarassMenu = SonaMenu.AddSubMenu("Harass", "harass");
            HarassMenu.Add("bQ", new CheckBox("Use Q"));
            HarassMenu.Add("minMana", new Slider("Min Mana % to Harass", 50));
            HarassMenu.AddSeparator(10);
            HarassMenu.Add("aaMins", new CheckBox("Disable AA on minions" + Environment.NewLine + "when allies nearby"));

            AutoWMenu = SonaMenu.AddSubMenu("Auto W", "autow");
            AutoWMenu.Add("bW", new CheckBox("Use W"));
            AutoWMenu.Add("allyWhp", new Slider("Ally HP % to W", 50, 1));
            AutoWMenu.Add("playerWhp", new Slider("Player HP % to W", 50, 1));
            AutoWMenu.Add("minMana", new Slider("Min Mana % to W", 20));

            GapcloseMenu = SonaMenu.AddSubMenu("AntiGapclose", "antigapclose");
            GapcloseMenu.Add("bE", new CheckBox("Use E", false));

            InterrupterMenu = SonaMenu.AddSubMenu("Interrupter", "interrupter");
            InterrupterMenu.Add("bR", new CheckBox("Use R", false));
            InterrupterMenu.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 2, "Low", "Medium", "High"));

            DrawMenu = SonaMenu.AddSubMenu("Drawings", "drawings");
            DrawMenu.Add("bQ", new CheckBox("Draw Q Range", false));
            DrawMenu.Add("bW", new CheckBox("Draw W Range", false));
            DrawMenu.Add("bE", new CheckBox("Draw E Range", false));
            DrawMenu.Add("bR", new CheckBox("Draw R Range", false));
            DrawMenu.Add("onlyRdy", new CheckBox("Draw only when spells are ready"));
            DrawMenu.Add("drawFR", new CheckBox("Draw possible FlashUlt Targets", false));
        }

        #endregion
    }
}