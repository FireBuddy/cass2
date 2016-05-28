namespace Sona
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    internal class Spells
    {
        #region Public Properties

        public static Spell.Active E { get; private set; }

        public static SpellDataInst Flash { get; private set; }

        public static Spell.Active Q { get; private set; }

        public static Spell.Skillshot R { get; private set; }

        public static Spell.Active W { get; private set; }

        #endregion

        #region Properties

        private static Spellbook LocalBook => Player.Instance.Spellbook;

        #endregion

        #region Methods

        internal static void InitSpells()
        {
            Q = new Spell.Active(SpellSlot.Q, 830);
            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Active(SpellSlot.E, 250);
            R = new Spell.Skillshot(SpellSlot.R, 875, SkillShotType.Linear, 500, 3000, 125)
                    {
                       AllowedCollisionCount = int.MaxValue 
                    };
            Flash = Player.Spells.FirstOrDefault(spell => spell.SData.Name == "SummonerFlash");
        }

        #endregion
    }
}