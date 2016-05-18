namespace CassOp
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    using SharpDX;

    internal class Spells
    {
        #region Public Properties

        public static bool FlashR { get; set; }

        public static Vector3 LastQPos { get; set; }

        public static Vector3 LastWPos { get; set; }

        public static float QCasted { get; set; } = 0f;

        public static float WCasted { get; set; } = 0f;

        #endregion

        #region Properties

        internal static float ComboDmgMod { get; } = 0.7f;

        internal static Spell.Targeted E { get; private set; }

        internal static float ECasted { get; set; } = 0f;

        internal static SpellDataInst Flash { get; } =
            Player.Spells.FirstOrDefault(args => args.SData.Name == "SummonerFlash");

        internal static Spell.Skillshot Q { get; private set; }

        internal static Spell.Skillshot R { get; private set; }

        internal static Spell.Skillshot W { get; private set; }

        internal static float WMaxRange { get; } = 900;

        internal static float WMinRange { get; } = 500;

        #endregion

        #region Public Methods and Operators

        public static float GetEDamage(Obj_AI_Base target)
        {
            var basedmg = (48 + (4 * Player.Instance.Level)) + (Player.Instance.TotalMagicalDamage * 0.1f);
            if (!target.HasBuffOfType(BuffType.Poison))
            {
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, basedmg);
            }

            var bonusdmg = new[] { 0, 10, 40, 70, 100, 130 }[E.Level] + (Player.Instance.TotalMagicalDamage * 0.35f);
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, basedmg + bonusdmg);
        }

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 400, null, 130);
            W = new Spell.Skillshot(SpellSlot.W, (uint)WMaxRange, SkillShotType.Cone, spellWidth: 160);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, 500, spellWidth: 80);
        }

        #endregion
    }
}