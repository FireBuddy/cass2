using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace CassOp
{
    internal class Spells
    {
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Skillshot R;
        public static SpellDataInst Flash = Player.Spells.FirstOrDefault(args => args.SData.Name == "SummonerFlash");
        public static float QCasted = 0f;
        public static float WCasted = 0f;
        public static float ECasted = 0f;
        public static float WMaxRange = 800;
        public static float WMinRange = 550;
        public static Vector3 LastQPos = new Vector3();
        public static Vector3 LastWPos = new Vector3();
        public static bool FlashR;

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Circular, 400, null, 130);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Cone, spellWidth: 160);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, 500, spellWidth: 80);
        }
    }
}