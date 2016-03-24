using System;
using EloBuddy;
using EloBuddy.SDK;

namespace DisableAA
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
            if (args.Target.Type == GameObjectType.obj_AI_Minion)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Config.IsChecked(Config.Harass, "disableAAIH"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.Harass, "allyRangeH"));
                    if (air > 1)
                    {
                        args.Process = false;
                    }
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Config.IsChecked(Config.LaneClear, "disableAAILC"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.LaneClear, "allyRangeLC"));
                    if (air > 1)
                    {
                        args.Process = false;
                    }
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) && Config.IsChecked(Config.LastHit, "disableAAILH"))
                {
                    var air = _Player.CountAlliesInRange(Config.GetSliderValue(Config.LastHit, "allyRangeLH"));
                    if (air > 1)
                    {
                        args.Process = false;
                    }
                }
            }
        }
    }
}

