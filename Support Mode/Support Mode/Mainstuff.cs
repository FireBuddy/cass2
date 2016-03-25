using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Support_Mode
{
    public static class Mainstuff
    {
        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            Orbwalker.OnPreAttack += OnBeforeAttack;
        }

        private static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (args.Target.Type == GameObjectType.obj_AI_Minion && Config.GlobalToggler)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Config.IsChecked(Config.Harass, "disableAAIH"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.Harass, "allyRangeH"));
                    if (air > 1)
                    {
                        var shieldStacks = _Player.GetBuffCount("TalentReaper");
                        if (shieldStacks > 0 && Config.IsChecked(Config.Harass, "stacksIH"))
                        {

                        }
                        else
                        {
                            args.Process = false;
                        }
                    }
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Config.IsChecked(Config.LaneClear, "disableAAILC"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.LaneClear, "allyRangeLC"));
                    if (air > 1)
                    {
                        var shieldStacks = _Player.GetBuffCount("TalentReaper");
                        if (shieldStacks > 0 && Config.IsChecked(Config.LaneClear, "stacksILC"))
                        {

                        }
                        else
                        {
                            args.Process = false;
                        }
                    }
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && Config.IsChecked(Config.LastHit, "disableAAILH"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.LastHit, "allyRangeLH"));
                    if (air > 1)
                    {
                        var shieldStacks = _Player.GetBuffCount("TalentReaper");
                        if (shieldStacks > 0 && Config.IsChecked(Config.LastHit, "stacksILH"))
                        {

                        }
                        else
                        {
                            args.Process = false;
                        }
                    }
                }
            }
        }
    }
}

