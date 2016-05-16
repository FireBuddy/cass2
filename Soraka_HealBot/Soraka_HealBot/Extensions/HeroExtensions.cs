using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Soraka_HealBot.Extensions
{
    public static class HeroExtensions
    {
        public static float GetTotalDamageFromLib(this AIHeroClient attacker, Obj_AI_Base defender)
        {
            var slots = new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
            var spellDmg =
                attacker.Spellbook.Spells.Where(spell => spell.IsReady && slots.Contains(spell.Slot))
                    .Sum(spell => attacker.GetSpellDamage(defender, spell.Slot));
            var aaDmg = attacker.CanAttack ? attacker.GetAutoAttackDamage(defender) : 0f;
            return spellDmg + aaDmg;
        }

        public static float GetEnemiesDamageNearAlly(this AIHeroClient ally, float percent = 0.8f, float range = 750)
        {
            var fullDmg = 0f;
            var slots = new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(en => en.Distance(ally) <= range))
            {
                fullDmg +=
                    enemy.Spellbook.Spells.Where(spell => spell.IsReady && slots.Contains(spell.Slot))
                        .Sum(spell => enemy.GetSpellDamage(ally, spell.Slot));
                fullDmg += enemy.GetAutoAttackDamage(ally);
            }
            return fullDmg * percent;
        }

        public static float GetAlliesDamageNearEnemy(this AIHeroClient enemy, float percent = 0.8f, float range = 750)
        {
            var fullDmg = 0f;
            var slots = new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
            foreach (var ally in EntityManager.Heroes.Allies.Where(al => al.Distance(enemy) <= range))
            {
                fullDmg +=
                    ally.Spellbook.Spells.Where(spell => spell.IsReady && slots.Contains(spell.Slot))
                        .Sum(spell => ally.GetSpellDamage(enemy, spell.Slot));
                fullDmg += ally.GetAutoAttackDamage(enemy);
            }
            return fullDmg * percent;
        }
    }
}