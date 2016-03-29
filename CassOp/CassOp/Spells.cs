﻿using EloBuddy;
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
        public static float QCasted = 0f;
        public static float WCasted = 0f;
        public static float ECasted = 0f;
        public static Vector3 LastQPos = new Vector3();
        public static Vector3 LastWPos = new Vector3();
        public static bool flashR;

        public static void LoadSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, 400, null, 75);
            W = new Spell.Skillshot(SpellSlot.W, 850, SkillShotType.Circular, spellWidth: 125);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, spellWidth: 80);
        }
    }
}