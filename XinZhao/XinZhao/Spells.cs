namespace XinZhao
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class Spells
    {
        #region Public Properties

        public static Item CorruptingPotion { get; set; }

        public static Spell.Targeted E { get; set; }

        public static SpellDataInst Flash { get; set; }

        public static float FlashRange { get; } = 425;

        public static Spell.Active Q { get; set; }

        public static Spell.Active R { get; set; }

        public static Item RavHydra { get; set; }

        public static Item Tiamat { get; set; }

        public static Item TitHydra { get; set; }

        public static Spell.Active W { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void LoadSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R, 180);

            Flash = Player.Spells.FirstOrDefault(args => args.SData.Name == "SummonerFlash");

            Tiamat = new Item(ItemId.Tiamat, 325);
            RavHydra = new Item(ItemId.Ravenous_Hydra, 325);
            TitHydra = new Item(ItemId.Titanic_Hydra, 325);
            CorruptingPotion = new Item(ItemId.Corrupting_Potion);
        }

        #endregion
    }
}