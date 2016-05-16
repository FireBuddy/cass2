namespace Olaf
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    internal class Spells
    {
        #region Public Properties

        public static GameObject AxeObject { get; set; }

        public static Item CorruptingPotion { get; set; }

        public static Spell.Targeted E { get; set; }

        public static Spell.Skillshot Q { get; set; }

        public static Spell.Active R { get; set; }

        public static Item RavHydra { get; set; }

        public static Item Tiamat { get; set; }

        public static Item TitHydra { get; set; }

        public static Spell.Active W { get; set; }

        #endregion

        #region Public Methods and Operators

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

        #endregion
    }
}