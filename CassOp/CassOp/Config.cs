namespace CassOp
{
    using System;

    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Config
    {
        #region Properties

        internal static Menu Cassop { get; private set; }

        internal static Menu Combo { get; private set; }

        internal static Menu Gapclose { get; private set; }

        internal static Menu Harass { get; private set; }

        internal static Menu Interrupter { get; private set; }

        internal static Menu JungleClear { get; private set; }

        internal static Menu LaneClear { get; private set; }

        internal static Menu LastHit { get; private set; }

        internal static Menu Misc { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void CallMenu()
        {
            Cassop = MainMenu.AddMenu("Cassiopeia", "cass");
            Cassop.AddGroupLabel("Cassiopeia by mztikk");

            Combo = Cassop.AddSubMenu("Combo", "combo");
            Combo.AddGroupLabel("Options for Combo");
            Combo.Add("useQInCombo", new CheckBox("Use Q"));
            Combo.Add("useWInCombo", new CheckBox("Use W"));
            Combo.Add("useEInCombo", new CheckBox("Use E"));
            Combo.Add("useRInCombo", new CheckBox("Use R"));
            Combo.Add("comboEonP", new CheckBox("E only on poisoned"));
            Combo.Add("humanEInCombo", new CheckBox("Humanize E casts"));
            Combo.Add("comboWonlyCD", new CheckBox("W only on Q CD and no Poison"));
            Combo.Add("comboMinR", new Slider("Min enemis to hit for R", 3, 1, 5));
            Combo.Add("comboNoAA", new CheckBox("Disable AA on Heroes in Combo", false));
            Combo.AddGroupLabel("Options for Flash R Combo");
            Combo.Add("comboFlashR", new CheckBox("Flash R Combo on killable", false));
            Combo.Add("maxEnFlash", new Slider("Max enemies around target to Flash R", 2, 0, 4));

            Harass = Cassop.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("useQInHarass", new CheckBox("Use Q"));
            Harass.Add("useWInHarass", new CheckBox("Use W", false));
            Harass.Add("useEInHarass", new CheckBox("Use E"));
            Harass.Add("harassEonP", new CheckBox("E only on poisoned"));
            Harass.Add("humanEInHarass", new CheckBox("Humanize E casts"));
            Harass.Add("harassWonlyCD", new CheckBox("W only on Q CD and no Poison"));
            Harass.Add("manaToHarass", new Slider("Min Mana % to Harass", 40));
            Harass.AddSeparator();
            Harass.AddGroupLabel("Options for Auto Harass");
            Harass.Add("autoQHarass", new CheckBox("Auto Q"));
            Harass.Add("autoWHarass", new CheckBox("Auto W", false));
            Harass.Add("autoEHarass", new CheckBox("Auto E", false));
            Harass.Add("autoHarassEonP", new CheckBox("E only on poisoned"));
            Harass.Add("humanEInAutoHarass", new CheckBox("Humanize E casts"));
            Harass.Add("dontAutoHarassInBush", new CheckBox("Dont Auto Harass in Bush"));
            Harass.Add("dontAutoHarassTower", new CheckBox("Dont Auto Harass under Tower"));
            Harass.Add("manaToAutoHarass", new Slider("Min Mana % to Auto Harass", 60));

            LaneClear = Cassop.AddSubMenu("LaneClear", "laneclear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("useQInLC", new CheckBox("Use Q", false));
            LaneClear.Add("useWInLC", new CheckBox("Use W"));
            LaneClear.Add("useEInLC", new CheckBox("Use E"));
            LaneClear.Add("laneEonP", new CheckBox("E only on poisoned"));
            LaneClear.Add("minQInLC", new Slider("Min Enemies to Hit for Q", 3, 1, 9));
            LaneClear.Add("minWInLC", new Slider("Min Enemies to Hit for W", 3, 1, 9));
            LaneClear.Add("useManaEInLC", new CheckBox("Use Mana Threshhold", false));
            LaneClear.Add("manaEInLC", new Slider("If Mana below this ignore poison for E LastHit", 30, 1));
            LaneClear.Add("manaToLC", new Slider("Min Mana % to LaneClear", 20));

            JungleClear = Cassop.AddSubMenu("JungleClear", "jungleclear");
            JungleClear.AddGroupLabel("Options for JungleClear");
            JungleClear.Add("useQInJC", new CheckBox("Use Q"));
            JungleClear.Add("useWInJC", new CheckBox("Use W"));
            JungleClear.Add("useEInJC", new CheckBox("Use E"));
            JungleClear.Add("jungEonP", new CheckBox("E only on poisoned"));
            JungleClear.Add("manaToJC", new Slider("Min Mana % to JungleClear", 10));

            LastHit = Cassop.AddSubMenu("LastHit", "lasthit");
            LastHit.AddGroupLabel("Options for LastHit");
            LastHit.Add("useEInLH", new CheckBox("Use E"));
            LastHit.Add("lastEonP", new CheckBox("E only on poisoned", false));

            Interrupter = Cassop.AddSubMenu("Interrupter", "Interrupter");
            Interrupter.AddGroupLabel("Options for Interrupter");
            Interrupter.Add("bInterrupt", new CheckBox("Interrupt spells with R"));
            Interrupter.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 2, "Low", "Medium", "High"));

            Gapclose = Cassop.AddSubMenu("Anti GapCloser", "AntiGapCloser");
            Gapclose.AddGroupLabel("Options for Anti GapClose");
            Gapclose.Add("qGapclose", new CheckBox("Anti GapClose with Q", false));

            // Gapclose.Add("wGapclose", new CheckBox("Anti GapClose with W"));
            Misc = Cassop.AddSubMenu("Misc", "misc");
            Misc.AddGroupLabel("Misc Options");
            Misc.Add("antiMissR", new CheckBox("Block R Casts if they miss/don't face"));
            Misc.Add("assistedR", new KeyBind("Assisted R", false, KeyBind.BindTypes.HoldActive, 'R'));
            Misc.Add(
                "eLastHit", 
                new CheckBox("Use E to kill unkillable (AA)" + Environment.NewLine + "minions while LastHit"));
            Misc.Add("eKillSteal", new CheckBox("Use E to Killsteal"));
            Misc.Add("humanDelay", new Slider("Humanize", 30, 1, 500));
            Misc.AddSeparator(5);
            Misc.Add("clearE", new CheckBox("Automatically kill poisoned minions with E", false));
            Misc.Add("manaClearE", new Slider("Min Mana % to Auto E", 10));
            Misc.Add("tearStackQ", new CheckBox("Use Q to stack Tear passively", false));
            Misc.Add("manaTearStack", new Slider("Min Mana % to stack Tear", 50));
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