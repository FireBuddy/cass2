using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace TwistedFate
{
    public static class Config
    {
        private static Menu _twistedFate;
        public static Menu Combo, Harass, LaneClear, JungleClear, LastHit, Misc, AdcMenu, CardSelectorMenu;

        public static void CallMenu()
        {
            _twistedFate = MainMenu.AddMenu("Twisted Fate", "twistedfate");
            _twistedFate.AddGroupLabel("Twisted Fate by mztikk");

            Combo = _twistedFate.AddSubMenu("Combo", "combo");
            Combo.Add("useQinCombo", new CheckBox("Use Q in combo"));
            Combo.Add("qAAReset", new CheckBox("Only use Q after AA to reset", false));
            Combo.Add("useWinCombo", new CheckBox("Use W in Combo"));
            Combo.Add("yellowIntoQ", new CheckBox("Q after Yellow Card", false));
            Combo.Add("wModeC", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            Combo.Add("disableAAselectingC", new CheckBox("Disable AA while selecting a card", false));

            Harass = _twistedFate.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Harass");
            Harass.Add("useQinHarass", new CheckBox("Use Q in Harass"));
            Harass.Add("useWinHarass", new CheckBox("Use W in Harass"));
            Harass.Add("wModeH", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            Harass.Add("disableAAselectingH", new CheckBox("Disable AA while selecting a card", false));
            Harass.Add("manaToHarass", new Slider("Min Mana % to Harass", 50));
            Harass.AddGroupLabel("Auto Harass");
            Harass.Add("autoQ", new CheckBox("Auto Q Harass"));
            Harass.Add("manaToAHarass", new Slider("Min Mana % to Auto Harass", 50));

            LaneClear = _twistedFate.AddSubMenu("LaneClear", "laneclear");
            LaneClear.Add("useQinLC", new CheckBox("Use Q in LaneClear"));
            LaneClear.Add("qTargetsLC", new Slider("Min Targets to hit for Q", 3, 1, 10));
            LaneClear.Add("useWinLC", new CheckBox("Use W in LaneClear"));
            LaneClear.Add(
                "wModeLC", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            LaneClear.Add("disableAAselectingLC", new CheckBox("Disable AA while selecting a card", false));
            LaneClear.Add("manaToLC", new Slider("Min Mana % to LaneClear", 30));

            JungleClear = _twistedFate.AddSubMenu("JungleClear", "jungleclear");
            JungleClear.Add("useQinJC", new CheckBox("Use Q in JungleClear"));
            JungleClear.Add("useWinJC", new CheckBox("Use W in JungleClear"));
            JungleClear.Add(
                "wModeJC", new ComboBox("W Mode", 0, "Smart Mode", "Always Yellow", "Always Blue", "Always Red"));
            JungleClear.Add("disableAAselectingJC", new CheckBox("Disable AA while selecting a card", false));
            JungleClear.Add("manaToJC", new Slider("Min Mana % to JungleClear", 10));
            
            CardSelectorMenu = _twistedFate.AddSubMenu("CardSelector", "cardselector");
            CardSelectorMenu.Add(
                "csYellow", new KeyBind("Select Yellow Card", false, KeyBind.BindTypes.HoldActive, 'W'));
            CardSelectorMenu.Add("csBlue", new KeyBind("Select Blue Card", false, KeyBind.BindTypes.HoldActive, 'E'));
            CardSelectorMenu.Add("csRed", new KeyBind("Select Red Card", false, KeyBind.BindTypes.HoldActive, 'T'));

            Misc = _twistedFate.AddSubMenu("Misc", "misc");
            Misc.Add("AutoYAG", new CheckBox("Auto Yellow on R teleport"));
            Misc.Add("qKillsteal", new CheckBox("Killsteal with Q"));
            Misc.Add("autoYellowIntoQ", new CheckBox("Auto Q after Yellow Card", false));
            Misc.Add("autoQonCC", new CheckBox("Auto Q on hard CC'd targets", false));
            Misc.Add("cancelAApicking", new CheckBox("Cancel AA right before card pick", false));
            Misc.Add("humanizePicks", new CheckBox("Humanize Card Picks"));
            Misc.Add("humanizeInt", new Slider("Humanize", 50, 10, 300));
        }

        public static bool IsChecked(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int GetSliderValue(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }

        public static int GetComboBoxValue(Menu obj, string value)
        {
            return obj[value].Cast<ComboBox>().CurrentValue;
        }

        public static bool IsKeyPressed(Menu obj, string value)
        {
            return obj[value].Cast<KeyBind>().CurrentValue;
        }
    }
}