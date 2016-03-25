using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Support_Mode
{
    public static class Config
    {
        private static Menu _disableAa;
        public static Menu Main, Harass, LaneClear, LastHit, Draw;
        public static bool GlobalToggler
        {
            get { return Main["globalToggle"].Cast<KeyBind>().CurrentValue; }
        }

        public static void CallMenu()
        {
            _disableAa = MainMenu.AddMenu("Support Mode", "SupportMode");

            Main = _disableAa.AddSubMenu("Main", "Main");
            Main.AddGroupLabel("Global options");
            Main.Add(
                "globalToggle", new KeyBind("Global enable/disable toggle", true, KeyBind.BindTypes.PressToggle, 'O'));

            Harass = _disableAa.AddSubMenu("Harass", "Harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("disableAAIH", new CheckBox("Disable AA on minions in Harass Mode", true));
            Harass.Add("stacksIH", new CheckBox("Still AA when we have shield stacks", false));
            Harass.Add("allyRangeH", new Slider("Allies in range x to disable AA in Harass Mode", 1400, 0, 5000));

            LaneClear = _disableAa.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("disableAAILC", new CheckBox("Disable AA on minions in LaneClear Mode", true));
            LaneClear.Add("stacksILC", new CheckBox("Still AA when we have shield stacks", false));
            LaneClear.Add("allyRangeLC", new Slider("Allies in range x to disable AA in LaneClear Mode", 1400, 0, 5000));

            LastHit = _disableAa.AddSubMenu("LastHit", "LastHit");
            LastHit.AddGroupLabel("Options for LastHit");
            LastHit.Add("disableAAILH", new CheckBox("Disable AA on minions in LastHit Mode", true));
            LastHit.Add("stacksILH", new CheckBox("Still AA when we have shield stacks", false));
            LastHit.Add("allyRangeLH", new Slider("Allies in range x to disable AA in LastHit Mode", 1400, 0, 5000));

            Draw = _disableAa.AddSubMenu("Draw", "Draw");
            Draw.AddGroupLabel("Options for draw stuff");
            Draw.AddGroupLabel("Status Text");
            Draw.Add("globalDraw", new CheckBox("Draw the Status", true));
            Draw.Add("globaldrawX", new Slider("Relative X Position of the Status Text", 35, -100, 100));
            Draw.Add("globaldrawY", new Slider("Relative Y Position of the Status Text", -30, -100, 100));
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
