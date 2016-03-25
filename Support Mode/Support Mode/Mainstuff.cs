using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

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
            Drawing.OnDraw += Drawing_OnDraw;
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
                            if (Config.IsChecked(Config.LaneClear, "pushNoCS"))
                            {
                                var targetMinion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => x.Index == target.Index);
                                //var targetMinion = minions.FirstOrDefault();
                                var aaTravelTime = target.Distance(ObjectManager.Player) / _Player.BasicAttack.MissileSpeed + _Player.AttackDelay + Game.Ping / 2f / 1000;
                                if (Prediction.Health.GetPrediction(targetMinion, (int)(aaTravelTime)*1000) <= _Player.GetAutoAttackDamage(targetMinion) + 5)
                                {
                                    /*
                                    var higherHpMinion =
                                        EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(hp => hp.Health > target.Health);
                                    target = higherHpMinion;
                                    */
                                    args.Process = false;
                                }
                            }
                            else
                            {
                                args.Process = false;
                            }
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

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.Draw, "globalDraw") && Config.GlobalToggler)
            {
                Drawing.DrawText(Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(Config.GetSliderValue(Config.Draw, "globaldrawX"), Config.GetSliderValue(Config.Draw, "globaldrawY")), System.Drawing.Color.White, "Support Mode", 2);
            }
        }
    }
}

