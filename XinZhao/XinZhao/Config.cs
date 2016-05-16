namespace XinZhao
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Config
    {
        #region Properties

        internal static Menu Combo { get; private set; }

        internal static Menu Draw { get; private set; }

        internal static Menu Harass { get; private set; }

        internal static Menu JungleClear { get; private set; }

        internal static Menu LaneClear { get; private set; }

        internal static Menu Misc { get; private set; }

        internal static Menu Zhao { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void CallMenu()
        {
            Zhao = MainMenu.AddMenu("XinZhao", "XinZhao");

            Combo = Zhao.AddSubMenu("Combo", "combo");
            Combo.Add("useQcombo", new CheckBox("Use Q"));
            Combo.Add("useWcombo", new CheckBox("Use W"));
            Combo.Add("useEcombo", new CheckBox("Use E"));
            Combo.Add("comboETower", new CheckBox("Don't E under tower", false));
            Combo.Add("comboEmode", new ComboBox("E Usage Mode", 0, "Try to not waste", "Use when out of melee range"));
            Combo.Add("useRcombo", new CheckBox("Use R"));
            Combo.Add("comboMinR", new Slider("Min Targets to hit for R", 5, 1, 5));

            Harass = Zhao.AddSubMenu("Harass", "Harass");
            Harass.Add("useQharass", new CheckBox("Use Q"));
            Harass.Add("useWharass", new CheckBox("Use W"));
            Harass.Add("useEharass", new CheckBox("Use E"));
            Harass.Add("harassETower", new CheckBox("Don't E under tower"));
            Harass.Add("harassMana", new Slider("Min Mana % to Harass", 80));

            LaneClear = Zhao.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.Add("useQLC", new CheckBox("Use Q", false));
            LaneClear.Add("useWLC", new CheckBox("Use W", false));
            LaneClear.Add("useELC", new CheckBox("Use E"));
            LaneClear.Add("lcEtargets", new Slider("Min Targets to hit for E", 3, 0, 10));
            LaneClear.Add("lcMana", new Slider("Min Mana % to LaneClear", 80));

            JungleClear = Zhao.AddSubMenu("JungleClear", "JungleClear");
            JungleClear.Add("useQJC", new CheckBox("Use Q"));
            JungleClear.Add("useWJC", new CheckBox("Use W"));
            JungleClear.Add("useEJC", new CheckBox("Use E", false));
            JungleClear.Add("jcMana", new Slider("Min Mana % to JungleClear"));

            Draw = Zhao.AddSubMenu("Draw", "Draw");
            Draw.Add("drawXinsec", new CheckBox("Draw Xinsec Target"));
            Draw.Add("drawXinsecpred", new CheckBox("Draw Xinsec move pos"));

            Misc = Zhao.AddSubMenu("Misc", "Misc");
            Misc.AddLabel("Xinsec");
            Misc.Add("xinsecKey", new KeyBind("Xinsec", false, KeyBind.BindTypes.HoldActive, 'T'));
            Misc.Add("xinsecFlash", new CheckBox("Use Flash with Xinsec"));
            Misc.Add(
                "xinsecTargetting", 
                new ComboBox("Xinsec Targetting", 0, "Selected Target", "Target Selector", "Lowest MaxHealth"));

            Misc.AddSeparator(5);
            Misc.AddLabel("Misc");
            Misc.Add("useInterrupt", new CheckBox("Interrupt Spells with R", false));
            Misc.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 2, "Low", "Medium", "High"));
        }

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

        public static bool IsKeyPressed(Menu obj, string value)
        {
            return obj[value].Cast<KeyBind>().CurrentValue;
        }

        #endregion
    }
}