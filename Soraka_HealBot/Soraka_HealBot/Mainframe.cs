namespace Soraka_HealBot
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    using Soraka_HealBot.Extensions;
    using Soraka_HealBot.Modes;

    public static class Mainframe
    {
        #region Public Methods and Operators

        public static void Init()
        {
            Game.OnTick += Game_OnTick;
            Obj_AI_Base.OnProcessSpellCast += Events.SavingGrace;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Events.OnBeforeAttack;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Gapcloser.OnGapcloser += OtherUtils.OnGapCloser;
        }

        #endregion

        #region Methods

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.Draw, "wRangeDraw")
                && (!Config.IsChecked(Config.Draw, "onlyReady") || Spells.W.CanCast()))
            {
                Circle.Draw(Color.White, Spells.W.Range, Player.Instance.Position);
            }

            if (Config.IsChecked(Config.Draw, "qRange")
                && (!Config.IsChecked(Config.Draw, "onlyReady") || Spells.Q.CanCast()))
            {
                Circle.Draw(Color.White, Spells.Q.Range, Player.Instance.Position);
            }

            /* Debug Drawings
            var validAllies = EntityManager.Heroes.Allies.Where(ally => !ally.IsMe && !ally.IsDead && !ally.IsZombie);
            var drawTip = 10;
            var sortedAllies =
                validAllies.OrderByDescending(
                    ally => Config.GetSliderValue(Config.AutoWMenu, "autoWPrio" + ally.BaseSkinName))
                    .ThenBy(ally => ally.Health);
            foreach (var ally in sortedAllies)
            {
                Drawing.DrawText(0, drawTip, System.Drawing.Color.White, ally.BaseSkinName, 2);
                drawTip += 20;
            }
            
            var ent = EntityManager.Heroes.Allies.Where(ally => !ally.IsDead && !ally.IsMe && !ally.IsZombie).ToList();
            var drawTip = 10;
            foreach (var stuff in ent)
            {
                Drawing.DrawText(10 * drawTip, 0, System.Drawing.Color.White, stuff.Name, 2);
                Drawing.DrawText(10 * drawTip, 20, System.Drawing.Color.White, "Health: " + stuff.Health, 2);
                Drawing.DrawText(10 * drawTip, 40, System.Drawing.Color.White, "TAD: " + stuff.TotalAttackDamage, 2);
                Drawing.DrawText(10 * drawTip, 60, System.Drawing.Color.White, "TAP: " + stuff.TotalMagicalDamage, 2);
                drawTip += 20;
            }

            var allyInNeed = new AIHeroClient();
            switch (Config.GetComboBoxValue(Config.AutoWMenu, "wHealMode"))
            {
                case 0:
                    allyInNeed = ent.OrderBy(x => x.Health).FirstOrDefault();
                    break;
                case 1:
                    allyInNeed = ent.OrderByDescending(x => x.TotalAttackDamage).ThenBy(u => u.Health).FirstOrDefault();
                    break;
                case 2:
                    allyInNeed = ent.OrderByDescending(x => x.TotalMagicalDamage).ThenBy(u => u.Health).FirstOrDefault();
                    break;
                case 3:
                    allyInNeed =
                        ent.OrderByDescending(x => x.TotalAttackDamage + x.TotalMagicalDamage)
                            .ThenBy(u => u.Health)
                            .FirstOrDefault();
                    break;
                case 4:
                    allyInNeed = ent.OrderBy(x => x.Distance(_Player)).ThenBy(u => u.Health).FirstOrDefault();
                    break;
            }
            Drawing.DrawText(0, 0, System.Drawing.Color.Red, allyInNeed.Name, 2);
            */
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }

            if (Config.IsChecked(Config.AutoRMenu, "autoR") && Spells.R.CanCast())
            {
                Computed.AutoR();
            }

            if (Config.IsChecked(Config.AssistKs, "autoAssistKS") && Spells.R.CanCast())
            {
                Computed.AssistKs();
            }

            if (Spells.W.CanCast() && Config.IsChecked(Config.AutoWMenu, "autoW"))
            {
                Computed.HealBotW();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass.Execute();
            }

            if (Config.IsChecked(Config.Harass, "autoQHarass"))
            {
                AutoHarass.AutoQ();
            }

            if (Config.IsChecked(Config.Harass, "autoEHarass"))
            {
                AutoHarass.AutoE();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear.Execute();
            }
        }

        #endregion
    }
}