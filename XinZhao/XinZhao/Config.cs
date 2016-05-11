using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace XinZhao
{
    class Config
    {
        private static Menu _xinZhao;
        public static Menu Combo, Harass, LaneClear, JungleClear, Draw, Misc;
        public static void CallMenu()
        {
            _xinZhao =
                MainMenu.AddMenu("XinZhao", "XinZhao");

            Combo = _xinZhao.
            AddSubMenu("Combo", "combo");
            Combo.Add("useQcombo", new CheckBox("Use Q"));
            Combo.Add("useWcombo", new CheckBox("Use W"));
            Combo.Add("useEcombo", new CheckBox("Use E"));
            Combo.Add("comboETower", new CheckBox("Don't E under tower", false));
            Combo.Add("useRcombo", new CheckBox("Use R"));
            Combo.Add("comboMinR", new Slider("Min Targets to hit for R", 5, 1, 5));

            Harass = _xinZhao.
                AddSubMenu("Harass", "Harass");
            Harass.Add("useQharass", new CheckBox("Use Q"));
            Harass.Add("useWharass", new CheckBox("Use W"));
            Harass.Add("useEharass", new CheckBox("Use E"));
            Harass.Add("harassETower", new CheckBox("Don't E under tower"));
            Harass.Add("harassMana", new Slider("Min Mana % to Harass", 80));

            LaneClear = _xinZhao.
                AddSubMenu("LaneClear", "LaneClear");
            LaneClear.Add("useQLC", new CheckBox("Use Q", false));
            LaneClear.Add("useWLC", new CheckBox("Use W", false));
            LaneClear.Add("useELC", new CheckBox("Use E"));
            LaneClear.Add("lcEtargets", new Slider("Min Targets to hit for E", 3, 0, 10));
            LaneClear.Add("lcMana", new Slider("Min Mana % to LaneClear", 80));

            JungleClear = _xinZhao.
                AddSubMenu("JungleClear", "JungleClear");
            JungleClear.Add("useQJC", new CheckBox("Use Q"));
            JungleClear.Add("useWJC", new CheckBox("Use W"));
            JungleClear.Add("useEJC", new CheckBox("Use E", false));
            JungleClear.Add("jcMana", new Slider("Min Mana % to JungleClear"));

            Draw = _xinZhao.
                AddSubMenu("Draw", "Draw");
            Draw.Add("drawXinsec", new CheckBox("Draw Xinsec Target"));
            Draw.Add("drawXinsecpred", new CheckBox("Draw Xinsec move pos"));

            Misc = _xinZhao.
                AddSubMenu("Misc", "Misc");
            Misc.Add("xinsecKey", new KeyBind("Xinsec", false, KeyBind.BindTypes.HoldActive, 'T'));
            Misc.Add("xinsecFlash", new CheckBox("Use Flash with Xinsec"));
            Misc.Add(
                "xinsecTargetting",
                new ComboBox("Xinsec Targetting", 0, "Selected Target", "Target Selector", "Lowest MaxHealth"));
            Misc.AddSeparator(5);
            Misc.Add("useInterrupt", new CheckBox("Interrupt Spells with R", false));
            Misc.Add("dangerL", new ComboBox("Min DangerLevel to interrupt", 2, "Low", "Medium", "High"));
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
