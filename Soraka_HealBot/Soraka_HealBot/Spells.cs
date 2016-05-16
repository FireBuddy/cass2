using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Soraka_HealBot
{
    public static class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Skillshot E;
        public static Spell.Active R;

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, (int) 0.283f, 1100, 235);
            W = new Spell.Targeted(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, (int) 0.5f, 1750, 235);
            R = new Spell.Active(SpellSlot.R);
        }

        public static float GetUltHeal(AIHeroClient target)
        {
            var baseHeal = new[] { 0, 150, 250, 350 }[R.Level] + (Player.Instance.TotalMagicalDamage * 0.55f);
            if (target.HealthPercent < 40)
            {
                return baseHeal * 1.5f;
            }
            return baseHeal;
        }

        public static float GetUltHeal()
        {
            var baseHeal = new[] { 0, 150, 250, 350 }[R.Level] + (Player.Instance.TotalMagicalDamage * 0.55f);
            return baseHeal;
        }

        public static float GetWHeal()
        {
            var amount = new[] { 0, 110, 140, 170, 200 }[W.Level] + (Player.Instance.TotalMagicalDamage * 0.6f);
            return amount;
        }
    }
}