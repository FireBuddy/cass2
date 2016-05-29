namespace TwistedFate
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    public static class Config
    {
        #region Public Properties

        public static Menu CardSelectorMenu { get; set; }

        public static Menu Combo { get; set; }

        public static Menu Fate { get; private set; }

        public static Menu Harass { get; set; }

        public static Menu JungleClear { get; set; }

        public static Menu LaneClear { get; set; }

        public static Menu Misc { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void CallMenu()
        {
            Fate = MainMenu.AddMenu("Twisted Fate", "twistedfate");
            Fate.AddGroupLabel("Twisted Fate by mztikk");

            Combo = Fate.AddSubMenu("Combo", "combo");
            Combo.Add("useQinCombo", new CheckBox("Use Q in combo"));
            Combo.Add("qAAReset", new CheckBox("Only use Q after AA to reset", false));
            Combo.Add("useWinCombo", new CheckBox("Use W in Combo"));
            Combo.Add("yellowIntoQ", new CheckBox("Q after Yellow Card", false));
            Combo.Add("wModeC", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            Combo.Add("disableAAselectingC", new CheckBox("Disable AA while selecting a card", false));

            Harass = Fate.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Harass");
            Harass.Add("useQinHarass", new CheckBox("Use Q in Harass"));
            Harass.Add("useWinHarass", new CheckBox("Use W in Harass"));
            Harass.Add("wModeH", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            Harass.Add("disableAAselectingH", new CheckBox("Disable AA while selecting a card", false));
            Harass.Add("manaToHarass", new Slider("Min Mana % to Harass", 50));
            Harass.AddGroupLabel("Auto Harass");
            Harass.Add("autoQ", new CheckBox("Auto Q Harass"));
            Harass.Add("manaToAHarass", new Slider("Min Mana % to Auto Harass", 50));

            LaneClear = Fate.AddSubMenu("LaneClear", "laneclear");
            LaneClear.Add("useQinLC", new CheckBox("Use Q in LaneClear"));
            LaneClear.Add("qTargetsLC", new Slider("Min Targets to hit for Q", 3, 1, 10));
            LaneClear.Add("useWinLC", new CheckBox("Use W in LaneClear"));
            LaneClear.Add(
                "wModeLC", 
                new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            LaneClear.Add("disableAAselectingLC", new CheckBox("Disable AA while selecting a card", false));
            LaneClear.Add("manaToLC", new Slider("Min Mana % to LaneClear", 30));

            JungleClear = Fate.AddSubMenu("JungleClear", "jungleclear");
            JungleClear.Add("useQinJC", new CheckBox("Use Q in JungleClear"));
            JungleClear.Add("useWinJC", new CheckBox("Use W in JungleClear"));
            JungleClear.Add(
                "wModeJC", 
                new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            JungleClear.Add("disableAAselectingJC", new CheckBox("Disable AA while selecting a card", false));
            JungleClear.Add("manaToJC", new Slider("Min Mana % to JungleClear", 10));

            CardSelectorMenu = Fate.AddSubMenu("CardSelector", "cardselector");
            CardSelectorMenu.Add(
                "csYellow", 
                new KeyBind("Select Yellow Card", false, KeyBind.BindTypes.HoldActive, 'W'));
            CardSelectorMenu.Add("csBlue", new KeyBind("Select Blue Card", false, KeyBind.BindTypes.HoldActive, 'E'));
            CardSelectorMenu.Add("csRed", new KeyBind("Select Red Card", false, KeyBind.BindTypes.HoldActive, 'T'));

            Misc = Fate.AddSubMenu("Misc", "misc");
            Misc.Add("AutoYAG", new CheckBox("Auto Yellow on R teleport"));
            Misc.Add("qKillsteal", new CheckBox("Killsteal with Q"));
            Misc.Add("autoYellowIntoQ", new CheckBox("Auto Q after Yellow Card", false));
            Misc.Add("autoQonCC", new CheckBox("Auto Q on immobile targets", false));
            Misc.Add("cancelAApicking", new CheckBox("Cancel AA right before card pick", false));
            Misc.Add("drawRrange", new CheckBox("Draw R range", false));
            Misc.Add("humanizePicks", new CheckBox("Humanize Card Picks"));
            Misc.Add("humanizeInt", new Slider("Humanize", 50, 10, 250));
            Misc.AddSeparator(10);
            Misc.Add("useSkin", new CheckBox("Use Skinchanger", false)).OnValueChange += Tf.OnUseSkinChange;
            Misc.Add("skinId", new Slider("Skin ID", 0, 0, 9)).OnValueChange += Tf.OnSkinSliderChange;
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