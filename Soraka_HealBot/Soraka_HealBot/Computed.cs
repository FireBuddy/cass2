namespace Soraka_HealBot
{
    using System.Collections.Generic;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Soraka_HealBot.Extensions;

    internal static class Computed
    {
        #region Methods

        internal static void AssistKs()
        {
            if (!Config.IsChecked(Config.AssistKs, "assCancelBase") && Player.Instance.HasBuff("Recall"))
            {
                return;
            }

            var alliesToConsider =
                EntityManager.Heroes.Allies.Where(
                    ally =>
                    !ally.IsMe && !ally.IsDead && !ally.IsInShopRange() && !ally.IsZombie && !ally.HasBuff("Recall")
                    && ally.Distance(Player.Instance) > 2000);
            foreach (var ally in alliesToConsider)
            {
                var enemiesAroundAlly =
                    EntityManager.Heroes.Enemies.Where(
                        enemy =>
                        enemy.Distance(ally) <= 1000 && enemy.Distance(Player.Instance) > 2000 && !enemy.IsDead
                        && !enemy.IsZombie && enemy.IsHPBarRendered && enemy.IsValid);
                foreach (var enemy in enemiesAroundAlly)
                {
                    if (!Spells.R.CanCast())
                    {
                        return;
                    }

                    if (enemy.Health <= ally.GetAlliesDamageNearEnemy())
                    {
                        Player.Instance.Spellbook.CastSpell(SpellSlot.R);
                    }
                }
            }
        }

        internal static void AutoR()
        {
            /*
            TODO:
                add max enemies around ally, so ult not gets wasted when its like 1v5 and he couldnt escape anyway
            */
            if (!Config.IsChecked(Config.AutoRMenu, "cancelBase") && Player.Instance.HasBuff("Recall"))
            {
                return;
            }

            var alliesToCheck =
                EntityManager.Heroes.Allies.Where(
                    ally =>
                    !ally.IsMe && !ally.IsDead && !ally.IsZombie && !ally.IsInShopRange() && !ally.HasBuff("Recall")
                    && Config.IsChecked(Config.AutoRMenu, "autoR_" + ally.BaseSkinName)
                    && ally.HealthPercent <= Config.GetSliderValue(Config.AutoRMenu, "autoRHP"));
            foreach (var ally in alliesToCheck)
            {
                if (ally.CountEnemiesInRange(950) < 1)
                {
                    continue;
                }

                if (ally.Health > ally.GetEnemiesDamageNearAlly(1.2f, 950)
                    || ally.Health + Spells.GetUltHeal(ally) < ally.GetEnemiesDamageNearAlly(0.8f, 950))
                {
                    continue;
                }

                if (!Spells.R.CanCast())
                {
                    return;
                }

                var delay = OtherUtils.RDelay.Next(20, 50);
                Core.DelayAction(() => Player.Instance.Spellbook.CastSpell(SpellSlot.R), delay);
            }
        }

        internal static void HealBotW()
        {
            if (Player.Instance.ManaPercent < Config.GetSliderValue(Config.AutoWMenu, "manaToW")
                || Player.Instance.HealthPercent < Config.GetSliderValue(Config.AutoWMenu, "playerHpToW")
                || Player.Instance.IsRecalling())
            {
                return;
            }

            var validAlliesInRange =
                EntityManager.Heroes.Allies.Where(
                    ally =>
                    !ally.IsMe && !ally.IsDead && !ally.IsZombie && ally.IsHPBarRendered && !ally.IsInShopRange()
                    && ally.Distance(Player.Instance) <= Spells.W.Range && !ally.IsRecalling()
                    && Config.IsChecked(Config.AutoWMenu, "autoW_" + ally.BaseSkinName)).ToList();
            if (!validAlliesInRange.Any())
            {
                return;
            }

            IEnumerable<AIHeroClient> alliesToConsider;
            if (Player.Instance.HasBuff("SorakaQRegen"))
            {
                alliesToConsider =
                    validAlliesInRange.Where(
                        ally =>
                        ally.HealthPercent
                        <= Config.GetSliderValue(Config.AutoWMenu, "autoWBuff_HP_" + ally.BaseSkinName)).ToList();
            }
            else
            {
                alliesToConsider =
                    validAlliesInRange.Where(
                        ally =>
                        ally.HealthPercent <= Config.GetSliderValue(Config.AutoWMenu, "autoW_HP_" + ally.BaseSkinName))
                        .ToList();
            }

            if (!alliesToConsider.Any())
            {
                return;
            }

            AIHeroClient allyToHeal = null;
            switch (Config.GetComboBoxValue(Config.AutoWMenu, "wHealMode"))
            {
                case 0:
                    allyToHeal = alliesToConsider.OrderBy(x => x.Health).First();
                    break;
                case 1:
                    allyToHeal =
                        alliesToConsider.OrderByDescending(x => x.TotalAttackDamage).ThenBy(x => x.Health).First();
                    break;
                case 2:
                    allyToHeal =
                        alliesToConsider.OrderByDescending(x => x.TotalMagicalDamage).ThenBy(x => x.Health).First();
                    break;
                case 3:
                    allyToHeal =
                        alliesToConsider.OrderByDescending(x => x.TotalAttackDamage + x.TotalMagicalDamage)
                            .ThenBy(x => x.Health)
                            .First();
                    break;
                case 4:
                    allyToHeal =
                        alliesToConsider.OrderBy(x => x.Distance(Player.Instance)).ThenBy(x => x.Health).First();
                    break;
                case 5:
                    allyToHeal =
                        alliesToConsider.OrderByDescending(
                            x => Config.GetSliderValue(Config.AutoWMenu, "autoWPrio" + x.BaseSkinName))
                            .ThenBy(x => x.Health)
                            .First();
                    break;
            }

            Spells.W.Cast(allyToHeal);
        }

        #endregion
    }
}