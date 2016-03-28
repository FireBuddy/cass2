using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Soraka_HealBot
{
    public static class Mainframe
    {
        private const bool Devmode = false;
        private static AIHeroClient _Player => ObjectManager.Player;

        public static void Init()
        {
            Game.OnTick += Game_OnTick;
            //Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Modes.OnBeforeAttack;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Gapcloser.OnGapcloser += OtherUtils.OnGapCloser;
            //Dash.OnDash += OnDash;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }
            if (Config.IsChecked(Config.HealBot, "autoR") && Spells.R.IsReady() && _Player.Mana >= 100)
            {
                AutoR();
            }
            if (Spells.W.IsReady())
            {
                HealBotW();
            }
            if (Config.IsChecked(Config.Harass, "autoQHarass"))
            {
                AutoQ();
            }
            if (Config.IsChecked(Config.Harass, "autoEHarass"))
            {
                AutoE();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Modes.Harass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Modes.LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                //Modes.JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                //Modes.Flee();
                //_fleeActivated = true;
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) {}

        private static void Drawing_OnDraw(EventArgs args)
        {
            //Circle.Draw(Color.Red, Spells.Q.Range, Player.Instance.Position);
        }

        private static void HealBotW()
        {
            var ent =
                EntityManager.Heroes.Allies.Where(
                    allies =>
                        !allies.IsMe && !allies.IsDead && !allies.IsInShopRange() && !allies.IsZombie &&
                        allies.Distance(_Player) <= Spells.W.Range).ToList();
            var allyInNeed =
                ent.OrderBy(x => x.Health)
                    .FirstOrDefault(
                        x => !x.IsInShopRange() && Config.IsChecked(Config.HealBotTeam, "autoW_" + x.BaseSkinName));
            if (allyInNeed == null || _Player.HasBuff("Recall"))
            {
                return;
            }
            if (allyInNeed.HealthPercent <= Config.GetSliderValue(Config.HealBot, "allyHPToW") &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.HealBot, "manaToW") &&
                _Player.HealthPercent >= Config.GetSliderValue(Config.HealBot, "playerHpToW"))
            {
                Spells.W.Cast(allyInNeed);
            }
            if (allyInNeed.HealthPercent <= Config.GetSliderValue(Config.HealBot, "allyHPToWBuff") &&
                _Player.HasBuff("SorakaQRegen") &&
                _Player.HealthPercent >= Config.GetSliderValue(Config.HealBot, "playerHpToW") &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.HealBot, "manaToW"))
            {
                Spells.W.Cast(allyInNeed);
            }
        }

        private static void AutoQ()
        {
            if (!Spells.Q.IsReady() || !(_Player.Mana >= 40) || !_Player.CanCast ||
                !(_Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaAutoHarass")) ||
                _Player.HasBuff("Recall"))
            {
                return;
            }
            var bush =
                ObjectManager.Get<GrassObject>().OrderBy(br => br.Distance(_Player.ServerPosition)).FirstOrDefault();
            if (bush != null &&
                (Config.IsChecked(Config.Harass, "dontHarassInBush") &&
                 _Player.ServerPosition.IsInRange(bush, bush.BoundingRadius)))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.IsUnderEnemyturret() &&
                _Player.IsUnderEnemyturret())
            {
                return;
            }
            var qPred = Spells.Q.GetPrediction(target);
            if (Spells.Q.IsInRange(qPred.CastPosition) && qPred.HitChancePercent >= 90)
            {
                Spells.Q.Cast(qPred.CastPosition);
            }
        }

        private static void AutoE()
        {
            if (!Spells.E.IsReady() || !(_Player.Mana >= 70) || !_Player.CanCast ||
                !(_Player.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaAutoHarass")) ||
                _Player.HasBuff("Recall"))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            var ePred = Spells.E.GetPrediction(target);
            if (!Spells.E.IsInRange(ePred.CastPosition) && ePred.HitChancePercent < 70)
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.IsUnderEnemyturret() &&
                _Player.IsUnderEnemyturret())
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "eOnlyCCHarass"))
            {
                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Fear) ||
                    target.HasBuffOfType(BuffType.Flee) || target.HasBuffOfType(BuffType.Knockup) ||
                    target.HasBuffOfType(BuffType.Polymorph) || target.HasBuffOfType(BuffType.Snare) ||
                    target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Taunt) ||
                    target.HasBuffOfType(BuffType.Slow))
                {
                    Spells.E.Cast(ePred.CastPosition);
                }
            }
            else
            {
                Spells.E.Cast(ePred.CastPosition);
            }
        }

        private static void AutoR()
        {
            if (!Config.IsChecked(Config.HealBot, "cancelBase") && _Player.HasBuff("Recall"))
            {
                return;
            }
            var alliesToR =
                EntityManager.Heroes.Allies.Where(
                    h => !h.IsMe && !h.IsDead && !h.IsInShopRange() && !h.HasBuff("Recall")).AsEnumerable();
            foreach (var ally in alliesToR)
            {
                if (!(ally.HealthPercent <= Config.GetSliderValue(Config.HealBot, "autoRHP")) ||
                    !Config.IsChecked(Config.HealBotTeam, "autoR_" + ally.BaseSkinName))
                {
                    continue;
                }
                var enemiesAroundAlly =
                    EntityManager.Heroes.Enemies.Where(
                        en => en.Distance(ally.ServerPosition) <= Config.GetSliderValue(Config.HealBot, "enemyRange"))
                        .ToArray();
                var enemiesAroundAllyCount = enemiesAroundAlly.Count();
                /*foreach (var enAl in enemiesAroundAlly)
                    {
                    }*/
                if (enemiesAroundAllyCount < Config.GetSliderValue(Config.HealBot, "autoREnemies") ||
                    !Spells.R.IsReady())
                {
                    continue;
                }
                var delay = OtherUtils.RDelay.Next(100, 120);
                Core.DelayAction(() => _Player.Spellbook.CastSpell(SpellSlot.R), delay);
            }
        }
    }
}