using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace Olaf
{
    internal class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;

        public static Item Tiamat;
        public static Item RavHydra;
        public static Item TitHydra;
        public static Item CorruptingPotion;

        public static GameObject AxeObject;

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R);

            Tiamat = new Item(ItemId.Tiamat, 325);
            RavHydra = new Item(ItemId.Ravenous_Hydra, 325);
            TitHydra = new Item(ItemId.Titanic_Hydra, 325);
            CorruptingPotion = new Item(ItemId.Corrupting_Potion);
        }
    }
}