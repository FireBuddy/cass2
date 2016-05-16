namespace Soraka_HealBot
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    public static class Spells
    {
        #region Properties

        internal static Spell.Skillshot E { get; private set; }

        internal static Spell.Skillshot Q { get; private set; }

        internal static Spell.Active R { get; private set; }

        internal static Spell.Targeted W { get; private set; }

        #endregion

        #region Public Methods and Operators

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

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, (int)0.283f, 1100, 235);
            W = new Spell.Targeted(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, (int)0.5f, 1750, 235);
            R = new Spell.Active(SpellSlot.R);
        }

        #endregion
    }
}