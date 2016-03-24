using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace DisableAA
{
    public static class Config
    {
        private static Menu _disableAa;
        public static Menu Harass, LaneClear, LastHit;

        public static void CallMenu()
        {
            _disableAa = MainMenu.AddMenu("DisableAA", "DisableAA");

            Harass = _disableAa.AddSubMenu("Harass", "Harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("disableAAIH", new CheckBox("Disable AA on minions in Harass Mode", true));
            Harass.Add("allyRangeH", new Slider("Allies in range x to disable AA in Harass Mode", 1400, 0, 5000));

            LaneClear = _disableAa.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("disableAAILC", new CheckBox("Disable AA on minions in LaneClear Mode", true));
            LaneClear.Add("allyRangeLC", new Slider("Allies in range x to disable AA in LaneClear Mode", 1400, 0, 5000));

            LastHit = _disableAa.AddSubMenu("LastHit", "LastHit");
            LastHit.AddGroupLabel("Options for LastHit");
            LastHit.Add("disableAAILH", new CheckBox("Disable AA on minions in LastHit Mode", true));
            LastHit.Add("allyRangeLH", new Slider("Allies in range x to disable AA in LastHit Mode", 1400, 0, 5000));
        }

        public static bool IsChecked(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int GetSliderValue(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }
    }
}
