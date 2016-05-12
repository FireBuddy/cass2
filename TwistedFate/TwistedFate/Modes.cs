using System;
using System.Linq;
using System.Windows.Input;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

// ReSharper disable PossibleMultipleEnumeration

namespace TwistedFate
{
    internal class Modes
    {
        public static void PermActive()
        {
            if (Config.IsChecked(Config.Misc, "autoQonCC"))
            {
                AutoCCQ();
            }
            var wMana = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana;
            if (Player.Instance.Mana >= wMana)
            {
                if (Config.IsKeyPressed(Config.CardSelectorMenu, "csYellow") &&
                    (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.LeftCtrl) &&
                     !Keyboard.IsKeyDown(Key.LeftAlt)))
                {
                    CardSelector.StartSelecting(Cards.Yellow);
                }
                if (Config.IsKeyPressed(Config.CardSelectorMenu, "csBlue") &&
                    (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.LeftCtrl) &&
                     !Keyboard.IsKeyDown(Key.LeftAlt)))
                {
                    CardSelector.StartSelecting(Cards.Blue);
                }
                if (Config.IsKeyPressed(Config.CardSelectorMenu, "csRed") &&
                    (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.LeftCtrl) &&
                     !Keyboard.IsKeyDown(Key.LeftAlt)))
                {
                    CardSelector.StartSelecting(Cards.Red);
                }
            }
            var qMana = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana;
            if (Config.IsChecked(Config.Misc, "qKillsteal") && Player.Instance.Mana >= qMana && Spells.Q.IsReady())
            {
                var entKs =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        h =>
                            h.IsValidTarget(Spells.Q.Range) && h.Health < Player.Instance.GetSpellDamage(h, SpellSlot.Q));
                if (entKs != null)
                {
                    Spells.Q.Cast(entKs);
                }
            }
            if (Config.IsChecked(Config.Harass, "autoQ") && Player.Instance.Mana >= qMana &&
                Player.Instance.ManaPercent >= Config.GetSliderValue(Config.Harass, "manaToAHarass") &&
                Spells.Q.IsReady() && (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
                if (target.IsValidTarget(Spells.Q.Range))
                {
                    var qPred = Spells.Q.GetPrediction(target);
                    if (qPred.HitChancePercent >= 90)
                    {
                        Spells.Q.Cast(qPred.CastPosition);
                    }
                }
            }
        }

        public static void Combo()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (!target.IsValidTarget(Spells.Q.Range))
            {
                return;
            }
            var qMana = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana;
            var wMana = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana;
            if (target.Distance(Player.Instance.Position) <= Player.Instance.AttackRange + 100 &&
                Player.Instance.Mana >= wMana && Player.Instance.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready &&
                Config.IsChecked(Config.Combo, "useWinCombo"))
            {
                switch (Config.GetComboBoxValue(Config.Combo, "wModeC"))
                {
                    case 0:
                        CardSelector.StartSelecting(Player.Instance.Mana < qMana + wMana ? Cards.Blue : Cards.Yellow);
                        break;
                    case 1:
                        CardSelector.StartSelecting(Cards.Yellow);
                        break;
                    case 2:
                        CardSelector.StartSelecting(Cards.Blue);
                        break;
                    case 3:
                        CardSelector.StartSelecting(Cards.Red);
                        break;
                }
            }
            var qPred = Spells.Q.GetPrediction(target);
            if (target != null && Player.Instance.Mana >= qMana && qPred.HitChancePercent >= 90 &&
                !Config.IsChecked(Config.Combo, "qAAReset") && Config.IsChecked(Config.Combo, "useQinCombo"))
            {
                Spells.Q.Cast(qPred.CastPosition);
            }
        }

        public static void Harass()
        {
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (!target.IsValidTarget(Spells.Q.Range) ||
                Player.Instance.ManaPercent < Config.GetSliderValue(Config.Harass, "manaToHarass"))
            {
                return;
            }
            var qMana = Player.Instance.Spellbook.GetSpell(SpellSlot.Q).SData.Mana;
            var wMana = Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.Mana;
            if (target.Distance(Player.Instance.Position) <= Player.Instance.AttackRange + 100 &&
                Player.Instance.Mana >= wMana && Player.Instance.Spellbook.CanUseSpell(SpellSlot.W) == SpellState.Ready &&
                Config.IsChecked(Config.Harass, "useWinHarass"))
            {
                switch (Config.GetComboBoxValue(Config.Harass, "wModeH"))
                {
                    case 0:
                        CardSelector.StartSelecting(
                            Player.Instance.ManaPercent < Config.GetSliderValue(Config.Harass, "manaToHarass") + 10
                                ? Cards.Blue
                                : Cards.Yellow);
                        break;
                    case 1:
                        CardSelector.StartSelecting(Cards.Yellow);
                        break;
                    case 2:
                        CardSelector.StartSelecting(Cards.Blue);
                        break;
                    case 3:
                        CardSelector.StartSelecting(Cards.Red);
                        break;
                }
            }
            var qPred = Spells.Q.GetPrediction(target);
            if (target != null && Player.Instance.Mana >= qMana && qPred.HitChancePercent >= 90 &&
                Config.IsChecked(Config.Harass, "useQinHarass"))
            {
                Spells.Q.Cast(qPred.CastPosition);
            }
        }

        public static void LaneClear()
        {
            if (Orbwalker.LaneClearMinion != null)
            {
                var target =
               EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(
                   x => x.NetworkId == Orbwalker.LaneClearMinion.NetworkId);
                if (target == null || Player.Instance.ManaPercent < Config.GetSliderValue(Config.LaneClear, "manaToLC"))
                {
                    return;
                }
                switch (Config.GetComboBoxValue(Config.LaneClear, "wModeLC"))
                {
                    case 0:
                        if (Player.Instance.ManaPercent >=
                            Math.Max(30f, Config.GetSliderValue(Config.LaneClear, "manaToLC") + 10))
                        {
                            var targetAoE =
                                EntityManager.MinionsAndMonsters.EnemyMinions.Count(a => a.Distance(target) <= 250);
                            if (targetAoE > 2)
                            {
                                CardSelector.StartSelecting(Cards.Red);
                            }
                        }
                        else
                        {
                            CardSelector.StartSelecting(Cards.Blue);
                        }
                        break;
                    case 1:
                        CardSelector.StartSelecting(Cards.Yellow);
                        break;
                    case 2:
                        CardSelector.StartSelecting(Cards.Blue);
                        break;
                    case 3:
                        CardSelector.StartSelecting(Cards.Red);
                        break;
                }
            }
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget(Spells.Q.Range));
            if (minions.Any() && Config.IsChecked(Config.LaneClear, "useQinLC") && Spells.Q.IsReady())
            {
                var farm = EntityManager.MinionsAndMonsters.GetLineFarmLocation(
                    minions, Spells.Q.Width, (int) Spells.Q.Range);
                if (farm.HitNumber >= Config.GetSliderValue(Config.LaneClear, "qTargetsLC"))
                {
                    Spells.Q.Cast(farm.CastPosition);
                }
            }
        }

        public static void JungleClear()
        {
            var jungle =
                EntityManager.MinionsAndMonsters.Monsters.Where(
                    x => x.Distance(Player.Instance.Position) <= Player.Instance.AttackRange + 200)
                    .OrderByDescending(x => x.MaxHealth);
            if (!jungle.Any() || Player.Instance.ManaPercent < Config.GetSliderValue(Config.JungleClear, "manaToJC"))
            {
                return;
            }
            switch (Config.GetComboBoxValue(Config.JungleClear, "wModeJC"))
            {
                case 0:
                    if (jungle.Any(x => x.Name.StartsWith("SRU_Baron") || x.Name.StartsWith("SRU_Dragon")))
                    {
                        CardSelector.StartSelecting(Cards.Blue);
                    }
                    else
                    {
                        if (Player.Instance.ManaPercent >=
                            Math.Max(30f, Config.GetSliderValue(Config.JungleClear, "manaToJC") + 10))
                        {
                            var targetAoE = jungle.Count(x => x.Distance(jungle.FirstOrDefault()) <= 250);
                            CardSelector.StartSelecting(targetAoE > 2 ? Cards.Red : Cards.Yellow);
                        }
                        else
                        {
                            CardSelector.StartSelecting(Cards.Blue);
                        }
                    }
                    break;
                case 1:
                    CardSelector.StartSelecting(Cards.Yellow);
                    break;
                case 2:
                    CardSelector.StartSelecting(Cards.Blue);
                    break;
                case 3:
                    CardSelector.StartSelecting(Cards.Red);
                    break;
            }
            if (Config.IsChecked(Config.JungleClear, "useQinJC") && Spells.Q.IsReady())
            {
                var target = jungle.FirstOrDefault(x => x.IsValidTarget(Spells.Q.Range));
                if (target != null)
                {
                    Spells.Q.Cast(target);
                }
            }
        }

        public static void LastHit()
        {
            throw new NotImplementedException();
        }

        public static void Flee()
        {
            throw new NotImplementedException();
        }

        public static void AutoCCQ()
        {
            if (!Spells.Q.IsReady())
            {
                return;
            }
            foreach (var qPred in EntityManager.Heroes.Enemies.Where(m => m.IsValidTarget(Spells.Q.Range)).Select(enemy => Spells.Q.GetPrediction(enemy)).Where(qPred => qPred.HitChance == HitChance.Immobile)) {
                Spells.Q.Cast(qPred.CastPosition);
            }
        }
    }
}