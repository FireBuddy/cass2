using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Soraka_HealBot
{
    public static class Config
    {
        internal static bool UseCustomPrio;

        private static Menu _soraka;

        public static Menu Combo,
            Harass,
            LaneClear,
            HealBot,
            HealBotTeam,
            AssistKs,
            Interrupter,
            Gapclose,
            Draw,
            Dev,
            AutoWMenu,
            AutoRMenu;

        public static void CallMenu()
        {
            _soraka = MainMenu.AddMenu("Soraka", "Soraka");
            _soraka.AddGroupLabel("HealBot");
            _soraka.AddLabel("by mztikk");

            Combo = _soraka.AddSubMenu("Combo", "Combo");
            Combo.AddGroupLabel("Options for Combo");
            Combo.Add("useQInCombo", new CheckBox("Use Q"));
            Combo.Add("useEInCombo", new CheckBox("Use E"));
            Combo.Add("eOnlyCC", new CheckBox("Use E only on immobile", false));
            Combo.Add("comboDisableAA", new CheckBox("Disable AA on heroes in combo mode", false));
            Combo.Add("bLvlDisableAA", new CheckBox("Disable AA after Level x", false));
            Combo.Add("lvlDisableAA", new Slider("Min Level to disable AA", 8, 1, 18));

            Harass = _soraka.AddSubMenu("Harass", "Harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("useQInHarass", new CheckBox("Use Q"));
            Harass.Add("useEInHarass", new CheckBox("Use E", false));
            Harass.Add("disableAAH", new CheckBox("Disable AA on minions while Harass"));
            Harass.Add("eOnlyCCHarass", new CheckBox("Use E only on immobile"));
            Harass.Add("manaHarass", new Slider("Min Mana % to Harass", 40));
            Harass.Add(
                "allyRangeH", new Slider("Allies in range x to disable AA on Minions in Harass Mode", 1400, 0, 5000));
            Harass.AddSeparator();
            Harass.AddGroupLabel("Auto Harass");
            Harass.Add("autoQHarass", new CheckBox("Auto Q", false));
            Harass.Add("autoEHarass", new CheckBox("Auto E", false));
            Harass.Add("dontAutoHarassTower", new CheckBox("Dont Auto Harass under Tower"));
            Harass.Add("dontHarassInBush", new CheckBox("Dont Auto Harass when in Bush"));
            Harass.Add("manaAutoHarass", new Slider("Min Mana % to Auto Harass", 60));

            LaneClear = _soraka.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("useQInLC", new CheckBox("Use Q"));
            LaneClear.Add("qTargets", new Slider("Min Targets to hit for Q", 6, 1, 20));
            LaneClear.Add("manaLaneClear", new Slider("Min Mana % to LaneClear", 60));

            var allAllies = EntityManager.Heroes.Allies.Where(ally => !ally.IsMe).ToArray();
            AutoWMenu = _soraka.AddSubMenu("Auto W", "autow");
            AutoWMenu.AddGroupLabel("Auto W");
            AutoWMenu.Add("autoW", new CheckBox("Auto use W"));
            AutoWMenu.AddSeparator(5);
            AutoWMenu.Add(
                "wHealMode",
                new ComboBox("Priority Mode", 0, "Lowest Health", "Total AD", "Total AP", "AD+AP", "Closest", "Custom Priority"));
            AutoWMenu.Add("manaToW", new Slider("Min Mana % to Auto W", 10));
            AutoWMenu.Add("playerHpToW", new Slider("Min Player HP % to Auto W", 25, 6));
            AutoWMenu.AddGroupLabel("Auto W Teammate Settings");
            foreach (var ally in allAllies)
            {
                AutoWMenu.AddLabel(ally.BaseSkinName);
                AutoWMenu.Add("autoW_" + ally.BaseSkinName, new CheckBox("Auto Heal " + ally.BaseSkinName + " with W"));
                AutoWMenu.Add(
                    "autoW_HP_" + ally.BaseSkinName, new Slider("HP % to heal " + ally.BaseSkinName + " with W", 50, 1));
                AutoWMenu.Add(
                    "autoWBuff_HP_" + ally.BaseSkinName,
                    new Slider("HP % to heal " + ally.BaseSkinName + " with W + Q Buff", 75, 1));
                    AutoWMenu.Add("autoWPrio" + ally.BaseSkinName, new Slider("Custom Priority", 1, 1, 5));
                AutoWMenu.AddSeparator(6);
            }

            AutoRMenu = _soraka.AddSubMenu("Auto R", "autor");
            AutoRMenu.AddGroupLabel("Auto R");
            AutoRMenu.Add("autoR", new CheckBox("Auto use R"));
            AutoRMenu.Add("cancelBase", new CheckBox("Cancel Recall to Auto R"));
            AutoRMenu.AddSeparator(5);
            AutoRMenu.Add("autoRHP", new Slider("HP % to trigger R Logic", 15, 1));
            AutoRMenu.AddGroupLabel("Auto R Teammate Settings");
            foreach (var ally in allAllies)
            {
                AutoRMenu.Add("autoR_" + ally.BaseSkinName, new CheckBox("Auto Heal " + ally.BaseSkinName + " with R"));
                AutoRMenu.AddSeparator(2);
            }

            AssistKs = _soraka.AddSubMenu("AssistKS", "assistks");
            AssistKs.AddGroupLabel("Options for AssistKS");
            AssistKs.AddLabel("This tries to ult when an ally is about to get a kill, so you can get an assist");
            AssistKs.Add("autoAssistKS", new CheckBox("Use R to Auto AssistKS", false));
            AssistKs.Add("assCancelBase", new CheckBox("Cancel Recall to AssistKS", false));

            Interrupter = _soraka.AddSubMenu("Interrupter", "Interrupter");
            Interrupter.AddGroupLabel("Options for Interrupter");
            Interrupter.Add("bInterrupt", new CheckBox("Interrupt spells with E"));
            Interrupter.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 0, "Low", "Medium", "High"));

            Gapclose = _soraka.AddSubMenu("Anti Gapcloser", "AntiGapcloser");
            Gapclose.AddGroupLabel("Options for Anti Gapclose");
            Gapclose.Add("qGapclose", new CheckBox("Anti Gapclose with Q", false));
            Gapclose.Add("eGapclose", new CheckBox("Anti Gapclose with E", false));

            Draw = _soraka.AddSubMenu("Drawings", "drawings");
            Draw.AddGroupLabel("Options for Drawings");
            Draw.Add("wRangeDraw", new CheckBox("Draw W Range", false));
            Draw.Add("qRange", new CheckBox("Draw Q Range", false));
            Draw.Add("onlyReady", new CheckBox("Only when Spells are ready"));

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
    }
}