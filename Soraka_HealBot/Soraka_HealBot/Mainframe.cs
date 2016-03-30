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
        private static Array AlliesToHeal;
        private static AIHeroClient _Player => Player.Instance;

        public static void Init()
        {
            Game.OnTick += Game_OnTick;
            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Modes.OnBeforeAttack;
            Interrupter.OnInterruptableSpell += OtherUtils.OnInterruptableSpell;
            Gapcloser.OnGapcloser += OtherUtils.OnGapCloser;
            //Dash.OnDash += OnDash;
        }

        private static void OnGameUpdate(EventArgs args) {}

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }
            if (Config.IsChecked(Config.AutoRMenu, "autoR") && Spells.R.IsReady() && _Player.Mana >= 100)
            {
                AutoR();
            }
            if (Config.IsChecked(Config.AssistKS, "autoAssistKS") && Spells.R.IsReady() &&
                Config.GetComboBoxValue(Config.AssistKS, "assMode") == 1)
            {
                AssistKS();
            }
            if (Spells.W.IsReady() && Config.IsChecked(Config.AutoWMenu, "autoW"))
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

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && args.Target.IsAlly &&
                (Config.IsChecked(Config.AutoWMenu, "wOnKill") || Config.IsChecked(Config.AutoRMenu, "rOnKill")))
            {
                var enemy = EntityManager.Heroes.Enemies.FirstOrDefault(en => en.Name == sender.Name);
                var usedSpell = args.Slot;
                var targetedAlly =
                    EntityManager.Heroes.Allies.FirstOrDefault(
                        ally => ally.Name == args.Target.Name && ally.Health <= enemy.GetSpellDamage(ally, usedSpell));
                if (targetedAlly != null &&
                    (Config.IsChecked(Config.AutoWMenu, "wOnKill") && Spells.W.IsReady() &&
                     targetedAlly.Distance(_Player) <= Spells.W.Range))
                {
                    Spells.W.Cast(targetedAlly);
                }
                if (targetedAlly != null && (Config.IsChecked(Config.AutoRMenu, "rOnKill") && Spells.R.IsReady()))
                {
                    Spells.R.Cast();
                }
            }
            if (sender.IsAlly && Config.IsChecked(Config.AssistKS, "autoAssistKS") &&
                Config.GetComboBoxValue(Config.AssistKS, "assMode") == 0 && Spells.R.IsReady())
            {
                var allySpellSlot = args.Slot;
                var ally = EntityManager.Heroes.Allies.FirstOrDefault(a => a.Name == sender.Name);
                var enemies =
                    EntityManager.Heroes.Enemies.Where(
                        e =>
                            ally.Distance(e) <= 1000 && e.HealthPercent < 20 && e.IsHPBarRendered && !e.IsDead &&
                            e.Distance(_Player) > 2000);
                foreach (var enemy in enemies)
                {
                    if (ally.GetSpellDamage(enemy, allySpellSlot) > enemy.Health)
                    {
                        _Player.Spellbook.CastSpell(SpellSlot.R);
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.Draw, "wRangeDraw"))
            {
                Circle.Draw(Color.White, Spells.W.Range, Player.Instance.Position);
            }
        }

        private static void HealBotW()
        {
            var ent =
                EntityManager.Heroes.Allies.Where(
                    allies =>
                        !allies.IsMe && !allies.IsDead && !allies.IsInShopRange() && !allies.IsZombie &&
                        allies.Distance(_Player) <= Spells.W.Range && !allies.HasBuff("Recall") &&
                        Config.IsChecked(Config.AutoWMenu, "autoW_" + allies.BaseSkinName)).ToList();
            if (!ent.Any())
            {
                return;
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

            if (allyInNeed == null || _Player.HasBuff("Recall"))
            {
                return;
            }
            if (allyInNeed.HealthPercent <=
                Config.GetSliderValue(Config.AutoWMenu, "autoW_HP_" + allyInNeed.BaseSkinName) &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.AutoWMenu, "manaToW") &&
                _Player.HealthPercent >= Config.GetSliderValue(Config.AutoWMenu, "playerHpToW"))
            {
                Spells.W.Cast(allyInNeed);
            }
            if (allyInNeed.HealthPercent <=
                Config.GetSliderValue(Config.AutoWMenu, "autoWBuff_HP_" + allyInNeed.BaseSkinName) &&
                _Player.HasBuff("SorakaQRegen") &&
                _Player.HealthPercent >= Config.GetSliderValue(Config.AutoWMenu, "playerHpToW") &&
                _Player.ManaPercent >= Config.GetSliderValue(Config.AutoWMenu, "manaToW"))
            {
                Spells.W.Cast(allyInNeed);
            }
        }

        private static void AssistKS()
        {
            if ((!Config.IsChecked(Config.AssistKS, "assCancelBase") && _Player.HasBuff("Recall")))
            {
                return;
            }
            var allies =
                EntityManager.Heroes.Allies.Where(
                    a =>
                        !a.IsMe && !a.IsDead && !a.IsInShopRange() && !a.HasBuff("Recall") &&
                        a.Distance(_Player) >= 2000).AsEnumerable();
            var enemies =
                EntityManager.Heroes.Enemies.Where(
                    e => !e.IsDead && !e.HasBuff("Recall") && e.HealthPercent <= 20 && e.Distance(_Player) > 2000)
                    .AsEnumerable();
            foreach (var ally in allies)
            {
                foreach (var enemy in enemies)
                {
                    var dmg = 0f;
                    dmg += ally.GetSpellDamage(enemy, SpellSlot.Q);
                    dmg += ally.GetSpellDamage(enemy, SpellSlot.W);
                    dmg += ally.GetSpellDamage(enemy, SpellSlot.E);
                    dmg += ally.GetSpellDamage(enemy, SpellSlot.R);
                    if (ally.Distance(enemy) <= 1000 && !enemy.HasBuff("Recall") && !enemy.IsInvulnerable &&
                        enemy.IsHPBarRendered &&
                        (dmg / 3 > enemy.Health || ally.GetAutoAttackDamage(enemy) * 2 > enemy.Health))
                    {
                        //Chat.Print(enemy.Name + " is about to get killed from: " + ally.Name);
                        _Player.Spellbook.CastSpell(SpellSlot.R);
                    }
                }
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
            /*
            TODO:
                add max enemies around ally, so ult not gets wasted when its like 1v5 and he couldnt escape anyway
            */
            if (!Config.IsChecked(Config.AutoRMenu, "cancelBase") && _Player.HasBuff("Recall"))
            {
                return;
            }
            var alliesToR =
                EntityManager.Heroes.Allies.Where(
                    h => !h.IsMe && !h.IsDead && !h.IsInShopRange() && !h.HasBuff("Recall")).ToList();
            foreach (var ally in alliesToR)
            {
                if (!(ally.HealthPercent <= Config.GetSliderValue(Config.AutoRMenu, "autoRHP")) ||
                    !Config.IsChecked(Config.AutoRMenu, "autoR_" + ally.BaseSkinName))
                {
                    continue;
                }
                var enemiesAroundAlly =
                    EntityManager.Heroes.Enemies.Where(
                        en => en.Distance(ally.ServerPosition) <= Config.GetSliderValue(Config.AutoRMenu, "enemyRange"))
                        .ToArray();
                var enemiesAroundAllyCount = enemiesAroundAlly.Count();
                /*foreach (var enAl in enemiesAroundAlly)
                    {
                    }*/
                if (enemiesAroundAllyCount < Config.GetSliderValue(Config.AutoRMenu, "autoREnemies") ||
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