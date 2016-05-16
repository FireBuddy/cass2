namespace Olaf
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    internal class Config
    {
        #region Properties

        internal static Menu AutoR { get; private set; }

        internal static Menu Combo { get; private set; }

        internal static Menu Draw { get; private set; }

        internal static Menu Harass { get; private set; }

        internal static Menu JungleClear { get; private set; }

        internal static Menu LaneClear { get; private set; }

        internal static Menu Misc { get; private set; }

        internal static Menu Olaf1 { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void CallMenu()
        {
            Olaf1 = MainMenu.AddMenu("Olaf", "Olaf");

            Combo = Olaf1.AddSubMenu("Combo", "combo");
            Combo.Add("useQCombo", new CheckBox("Use Q"));
            Combo.Add("useWCombo", new CheckBox("Use W"));
            Combo.Add("useECombo", new CheckBox("Use E"));
            Combo.Add("useTiamatCombo", new CheckBox("Use Tiamat/Hydra"));
            Combo.Add("potionOnBurst", new CheckBox("Use Corrupting Potion on Burst"));
            Combo.Add("potionOnLv1", new CheckBox("Use Corrupting Potion on Level 1 All in"));

            Harass = Olaf1.AddSubMenu("Harass", "Harass");
            Harass.Add("useQHarass", new CheckBox("Use Q"));
            Harass.Add("useEHarass", new CheckBox("Use E"));
            Harass.Add("harassHP", new Slider("Min Health % to use E", 50));
            Harass.Add("harassMana", new Slider("Min Mana % to Harass", 50));
            Harass.AddSeparator(5);
            Harass.AddGroupLabel("Auto Harass");
            Harass.Add("useQAutoHarass", new CheckBox("Use Q", false));
            Harass.Add("useEAutoHarass", new CheckBox("Use E", false));
            Harass.Add("autoHarassHP", new Slider("Min Health % to Auto E", 70));
            Harass.Add("autoHarassMana", new Slider("Min Mana % to Auto Harass", 70));

            LaneClear = Olaf1.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.Add("useQLaneClear", new CheckBox("Use Q"));
            LaneClear.Add("minQTargets", new Slider("Min Targets to hit for Q", 6, 1, 10));
            LaneClear.Add("useELaneClear", new CheckBox("Use E"));
            LaneClear.Add("laneClearEonlyKill", new CheckBox("E only on killable"));
            LaneClear.Add("laneClearHP", new Slider("Min Health % to use E", 20));
            LaneClear.Add("laneClearMana", new Slider("Min Mana % to LaneClear", 20));

            JungleClear = Olaf1.AddSubMenu("JungleClear", "JungleClear");
            JungleClear.Add("useQJungleClear", new CheckBox("Use Q"));
            JungleClear.Add("useWJungleClear", new CheckBox("Use W"));
            JungleClear.Add("useEJungleClear", new CheckBox("Use E"));
            JungleClear.Add("useTiamatJungleClear", new CheckBox("Use Tiamat/Hydra"));
            JungleClear.Add("jungleClearMana", new Slider("Min Mana % to JungleClear", 10));

            Draw = Olaf1.AddSubMenu("Draw", "Draw");
            Draw.Add("drawAxe", new CheckBox("Draw Circle around Axe"));
            Draw.Add("drawPickup", new CheckBox("Draw PickUp Position"));
            Draw.Add("drawStates", new CheckBox("Draw States"));

            Misc = Olaf1.AddSubMenu("Misc", "Misc");
            Misc.Add("autoPick", new CheckBox("Use Auto Axe PickUp"));
            Misc.Add("axePickRange", new Slider("Range to pick up Axe", 450, 10, 1500));
            Misc.Add("autoEKS", new CheckBox("Auto E KS"));
            Misc.Add("eOnMinz", new CheckBox("Auto E on unkillable(AA) minion"));

            AutoR = Olaf1.AddSubMenu("Auto R", "AutoR");
            AutoR.Add("useAutoR", new CheckBox("Use Auto R"));
            AutoR.Add("autoRHP", new Slider("HP % to trigger Auto R", 50));
            AutoR.Add("humanAutoR", new CheckBox("Humanize Auto R"));
            AutoR.Add("autoRonlyCombo", new CheckBox("Only in Combo Mode"));
            AutoR.AddSeparator(10);
            AutoR.Add("autoRStun", new CheckBox("On Stun?"));
            AutoR.Add("autoRSnare", new CheckBox("On Snare?"));
            AutoR.Add("autoRCharm", new CheckBox("On Charm?"));
            AutoR.Add("autoRFear", new CheckBox("On Fear?"));
            AutoR.Add("autoRBlind", new CheckBox("On Blind?"));
            AutoR.Add("autoRFlee", new CheckBox("On Flee?"));
            AutoR.Add("autoRPolymorph", new CheckBox("On Polymorph?"));
            AutoR.Add("autoRTaunt", new CheckBox("On Taunt?"));
            AutoR.Add("autoRSilence", new CheckBox("On Silence?"));
            AutoR.Add("autoRSlow", new CheckBox("On Slow?"));
            AutoR.Add("autoRSuppression", new CheckBox("On Suppression?"));
            AutoR.Add("autoRKnockup", new CheckBox("On Knockup?"));
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