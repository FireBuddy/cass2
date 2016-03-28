using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace CassOp
{
    internal class Config
    {
        private static Menu Cassop;
        public static Menu Combo, Harass, LaneClear, JungleClear, Interrupter, Gapclose, Misc;

        public static void CallMenu()
        {
            Cassop = MainMenu.AddMenu("Cassiopeia", "cass");
            Cassop.AddGroupLabel("Cassiopeia by mztikk");

            Combo = Cassop.AddSubMenu("Combo", "combo");
            Combo.AddGroupLabel("Options for Combo");
            Combo.Add("useQInCombo", new CheckBox("Use Q", true));
            Combo.Add("useWInCombo", new CheckBox("Use W", true));
            Combo.Add("useEInCombo", new CheckBox("Use E", true));
            Combo.Add("useRInCombo", new CheckBox("Use R", true));
            Combo.Add("comboEonP", new CheckBox("E only on poisoned", true));
            Combo.Add("humanEInCombo", new CheckBox("Humanize E casts", true));
            Combo.Add("comboWonlyCD", new CheckBox("W only on Q CD and no Poison", true));
            Combo.Add("comboMinR", new Slider("Min enemis to hit for R", 3, 1, 5));
            //Combo.Add("ignoreRonKill", new CheckBox("Ignore Min enemis when R on kill", true));

            Harass = Cassop.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("useQInHarass", new CheckBox("Use Q", true));
            Harass.Add("useWInHarass", new CheckBox("Use W", false));
            Harass.Add("useEInHarass", new CheckBox("Use E", true));
            Harass.Add("harassEonP", new CheckBox("E only on poisoned", true));
            Harass.Add("humanEInHarass", new CheckBox("Humanize E casts", true));
            Harass.Add("harassWonlyCD", new CheckBox("W only on Q CD and no Poison", true));
            Harass.Add("manaToHarass", new Slider("Min Mana % to Harass", 40, 0, 100));
            Harass.AddSeparator();
            Harass.AddGroupLabel("Options for Auto Harass");
            Harass.Add("autoQHarass", new CheckBox("Auto Q", true));
            Harass.Add("autoWHarass", new CheckBox("Auto W", false));
            Harass.Add("autoEHarass", new CheckBox("Auto E", false));
            Harass.Add("autoRHarass", new CheckBox("Auto R", false));
            Harass.Add("autoHarassEonP", new CheckBox("E only on poisoned", true));
            Harass.Add("humanEInAutoHarass", new CheckBox("Humanize E casts", true));
            Harass.Add("dontAutoHarassInBush", new CheckBox("Dont Auto Harass in Bush", true));
            Harass.Add("dontAutoHarassTower", new CheckBox("Dont Auto Harass under Tower", true));
            Harass.Add("manaToAutoHarass", new Slider("Min Mana % to Auto Harass", 60, 0, 100));

            LaneClear = Cassop.AddSubMenu("LaneClear", "laneclear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("useQInLC", new CheckBox("Use Q", false));
            LaneClear.Add("useWInLC", new CheckBox("Use W", true));
            LaneClear.Add("useEInLC", new CheckBox("Use E", true));
            LaneClear.Add("laneEonP", new CheckBox("E only on poisoned", true));
            LaneClear.Add("minQInLC", new Slider("Min Enemies to Hit for Q", 3, 1, 9));
            LaneClear.Add("minWInLC", new Slider("Min Enemies to Hit for W", 3, 1, 9));
            LaneClear.Add("useManaEInLC", new CheckBox("Use Mana Threshhold", false));
            LaneClear.Add("manaEInLC", new Slider("If Mana below this ignore poison for E LastHit", 30, 1, 100));
            LaneClear.Add("manaToLC", new Slider("Min Mana % to LaneClear", 20, 0, 100));

            JungleClear = Cassop.AddSubMenu("JungleClear", "jungleclear");
            JungleClear.AddGroupLabel("Options for LaneClear");
            JungleClear.Add("useQInJC", new CheckBox("Use Q", true));
            JungleClear.Add("useWInJC", new CheckBox("Use W", true));
            JungleClear.Add("useEInJC", new CheckBox("Use E", true));
            JungleClear.Add("jungEonP", new CheckBox("E only on poisoned", true));
            JungleClear.Add("manaToJC", new Slider("Min Mana % to JungleClear", 10, 0, 100));


            Interrupter = Cassop.AddSubMenu("Interrupter", "Interrupter");
            Interrupter.AddGroupLabel("Options for Interrupter");
            Interrupter.Add("bInterrupt", new CheckBox("Interrupt spells with R", true));
            Interrupter.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 2, "Low", "Medium", "High"));

            Gapclose = Cassop.AddSubMenu("Anti GapCloser", "AntiGapCloser");
            Gapclose.AddGroupLabel("Options for Anti GapClose");
            Gapclose.Add("qGapclose", new CheckBox("Anti GapClose with Q", false));
            Gapclose.Add("wGapclose", new CheckBox("Anti GapClose with W", true));

            Misc = Cassop.AddSubMenu("Misc", "misc");
            Misc.AddGroupLabel("Misc Options");
            Misc.Add("antiMissR", new CheckBox("Block R Casts if they miss/don't face", true));
            Misc.Add("assistedR", new KeyBind("Assisted R", false, KeyBind.BindTypes.HoldActive, 'R'));
            Misc.Add("eLastHit", new CheckBox("Use E to kill unkillable (AA) minions", true));
            Misc.Add("eKillSteal", new CheckBox("Use E to Killsteal", true));
            Misc.Add("humanDelay", new Slider("Humanize", 30, 1, 500));
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