using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace TwistedFate
{
    internal class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Active R;

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);
        }
    }
}