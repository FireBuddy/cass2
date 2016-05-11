using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace XinZhao
{
    class Spells
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;

        public static SpellDataInst Flash;
        public static float FlashRange = 425;

        public static Item Tiamat;
        public static Item RavHydra;
        public static Item TitHydra;
        public static Item CorruptingPotion;


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
    }
}
