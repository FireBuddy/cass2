namespace TwistedFate
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    internal class Spells
    {
        #region Properties

        internal static Spell.Active E { get; private set; }

        internal static Spell.Skillshot Q { get; private set; }

        internal static Spell.Active R { get; private set; }

        internal static Spell.Active W { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);
        }

        #endregion
    }
}