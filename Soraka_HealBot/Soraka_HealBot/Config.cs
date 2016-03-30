using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Soraka_HealBot
{
    public static class Config
    {
        private static Menu Soraka;
        public static Menu Combo, Harass, LaneClear, HealBot, HealBotTeam, AssistKS, Interrupter, Gapclose, Draw, Dev;

        public static void CallMenu()
        {
            Soraka = MainMenu.AddMenu("Soraka", "Soraka");
            Soraka.AddGroupLabel("HealBot");
            Soraka.AddLabel("by mztikk");

            Combo = Soraka.AddSubMenu("Combo", "Combo");
            Combo.AddGroupLabel("Options for Combo");
            Combo.Add("useQInCombo", new CheckBox("Use Q"));
            Combo.Add("useEInCombo", new CheckBox("Use E"));
            Combo.Add("eOnlyCC", new CheckBox("Use E only on CC'd or 100% Hit", false));
            Combo.Add("comboDisableAA", new CheckBox("Disable AA on heroes in combo mode", false));

            Harass = Soraka.AddSubMenu("Harass", "Harass");
            Harass.AddGroupLabel("Options for Harass");
            Harass.Add("useQInHarass", new CheckBox("Use Q"));
            Harass.Add("useEInHarass", new CheckBox("Use E", false));
            Harass.Add("disableAAH", new CheckBox("Disable AA on minions while Harass"));
            Harass.Add("eOnlyCCHarass", new CheckBox("Use E only on CC'd or 100% Hit", true));
            Harass.Add("manaHarass", new Slider("Min Mana % to Harass", 40, 0, 100));
            Harass.Add(
                "allyRangeH", new Slider("Allies in range x to disable AA on Minions in Harass Mode", 1400, 0, 5000));
            Harass.AddSeparator();
            Harass.AddGroupLabel("Auto Harass");
            Harass.Add("autoQHarass", new CheckBox("Auto Q", false));
            Harass.Add("autoEHarass", new CheckBox("Auto E", false));
            Harass.Add("dontAutoHarassTower", new CheckBox("Dont Auto Harass under Tower", true));
            Harass.Add("dontHarassInBush", new CheckBox("Dont Auto Harass when in Bush", true));
            Harass.Add("manaAutoHarass", new Slider("Min Mana % to Auto Harass", 60, 0, 100));

            LaneClear = Soraka.AddSubMenu("LaneClear", "LaneClear");
            LaneClear.AddGroupLabel("Options for LaneClear");
            LaneClear.Add("useQInLC", new CheckBox("Use Q", true));
            LaneClear.Add("qTargets", new Slider("Min Targets to hit for Q", 6, 1, 20));
            LaneClear.Add("manaLaneClear", new Slider("Min Mana % to LaneClear", 60, 0, 100));

            HealBot = Soraka.AddSubMenu("HealBot", "HealBot");
            HealBot.AddLabel("Default Settings work pretty good");
            HealBot.AddLabel("Shouldn't change much here");
            HealBot.AddGroupLabel("Auto R");
            HealBot.Add("autoR", new CheckBox("Auto use R", true));
            HealBot.Add("cancelBase", new CheckBox("Cancel Recall to Auto R", true));
            HealBot.Add("rOnKill", new CheckBox("Try to R Ally who'd die on targeted ability, ignores all other settings", true));
            //HealBot.Add("lowAlliesForR", new Slider("Number of Allies to be low for Auto R", 1, 1, 5));
            HealBot.Add("autoRHP", new Slider("HP % to trigger R Logic", 15, 1, 100));
            HealBot.Add("autoREnemies", new Slider("Number of Enemies around Ally to R", 1, 1, 5));
            HealBot.Add("enemyRange", new Slider("Enemies in x Distance of Ally to R", 900, 1, 5000));
            HealBot.AddSeparator();
            HealBot.AddGroupLabel("Auto W");
            HealBot.Add("autoW", new CheckBox("Auto use W", true));
            HealBot.Add("wOnKill", new CheckBox("Try to W Ally who'd die on targeted ability, ignores all other settings", true));
            //HealBot.Add("allyHPToW", new Slider("HP % to Auto W", 50, 1, 100));
            //HealBot.Add("allyHPToWBuff", new Slider("HP % to Auto W with Q Buff", 75, 1, 100));
            HealBot.Add("manaToW", new Slider("Min Mana % to Auto W", 10, 0, 100));
            HealBot.Add("playerHpToW", new Slider("Min Player HP % to Auto W", 25, 6, 100));

            var allAllies = EntityManager.Heroes.Allies.Where(ally => !ally.IsMe).ToArray();
            HealBotTeam = Soraka.AddSubMenu("HealBot Team", "HealBotTeam");
            HealBotTeam.AddGroupLabel("Auto R Teammate Settings");
            foreach (var ally in allAllies)
            {
                HealBotTeam.Add(
                    "autoR_" + ally.BaseSkinName, new CheckBox("Auto Heal " + ally.BaseSkinName + " with R", true));
            }
            HealBotTeam.AddGroupLabel("Auto W Teammate Settings");
            foreach (var ally in allAllies)
            {
                HealBotTeam.Add(
                    "autoW_" + ally.BaseSkinName, new CheckBox("Auto Heal " + ally.BaseSkinName + " with W", true));
                HealBotTeam.Add(
                    "autoW_HP_" + ally.BaseSkinName, new Slider("HP % to heal " + ally.BaseSkinName + " with W", 50, 1, 100));
                HealBotTeam.Add(
                    "autoWBuff_HP_" + ally.BaseSkinName, new Slider("HP % to heal " + ally.BaseSkinName + " with W + Q Buff", 75, 1, 100));
            }
            /*foreach (var ally in allAllies)
            {
                HealBotTeam.Add(
                    "autoW_HP_" + ally.BaseSkinName, new Slider("HP % to heal " + ally.BaseSkinName + " with W", 50, 1, 100));
                HealBotTeam.Add(
                    "autoWBuff_HP_" + ally.BaseSkinName, new Slider("HP % to heal " + ally.BaseSkinName + " with W + Q Buff", 75, 1, 100));
            }*/

            AssistKS = Soraka.AddSubMenu("AssistKS", "assistks");
            AssistKS.AddGroupLabel("Options for AssistKS");
            AssistKS.AddLabel("This tries to ult when an ally is about to get a kill, so you can get an assist");
            AssistKS.AddLabel("Consider this a beta thingy");
            AssistKS.Add("autoAssistKS", new CheckBox("Use R to Auto AssistKS", false));
            AssistKS.Add("assCancelBase", new CheckBox("Cancel Recall to AssistKS", false));
            AssistKS.Add("assMode", new ComboBox("AssistKS Mode", 0, "Safe", "Wild"));
            AssistKS.AddLabel("Safe Mode will only trigger on ally spellcasts able to kill the target");
            AssistKS.AddLabel(" calculated by dmg lib might miss but shouldnt very often, doesnt register for AA's");
            AssistKS.AddSeparator();
            AssistKS.AddLabel("Wild mode tries to calculate combo dmg and predict a kill");
            AssistKS.AddLabel(" will cast more often but will also fail more often");

            Interrupter = Soraka.AddSubMenu("Interrupter", "Interrupter");
            Interrupter.AddGroupLabel("Options for Interrupter");
            Interrupter.Add("bInterrupt", new CheckBox("Interrupt spells with E", true));
            Interrupter.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 0, "Low", "Medium", "High"));

            Gapclose = Soraka.AddSubMenu("Anti GapCloser", "AntiGapCloser");
            Gapclose.AddGroupLabel("Options for Anti GapClose");
            Gapclose.Add("qGapclose", new CheckBox("Anti GapClose with Q", false));
            Gapclose.Add("eGapclose", new CheckBox("Anti GapClose with E", false));

            Draw = Soraka.AddSubMenu("Drawings", "drawings");
            Draw.AddGroupLabel("Options for Drawings");
            Draw.Add("wRangeDraw", new CheckBox("Draw W Range", false));
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