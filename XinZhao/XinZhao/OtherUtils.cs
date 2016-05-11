using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;

namespace XinZhao
{
    internal static class OtherUtils
    {
        private static DangerLevel _wanteDangerLevel;
        public static void OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (!sender.IsEnemy || Player.Instance.IsRecalling() || !Config.IsChecked(Config.Misc, "useInterrupt"))
            {
                return;
            }
            switch (Config.GetComboBoxValue(Config.Misc, "dangerL"))
            {
                case 0:
                    _wanteDangerLevel = DangerLevel.Low;
                    break;
                case 1:
                    _wanteDangerLevel = DangerLevel.Medium;
                    break;
                case 2:
                    _wanteDangerLevel = DangerLevel.High;
                    break;
                default:
                    _wanteDangerLevel = DangerLevel.High;
                    break;
            }
            if (Spells.R.CanCast() && sender.IsValidTarget(Spells.R.Range) && e.DangerLevel == _wanteDangerLevel)
            {
                Spells.R.Cast();
            }
        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static bool CanCast(this Spell.Active spellActive)
        {
            return spellActive.IsLearned && spellActive.IsReady() &&
                   Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellActive.Slot).SData.Mana;
        }

        public static bool CanCast(this Spell.Targeted spellTargeted)
        {
            return spellTargeted.IsLearned && spellTargeted.IsReady() &&
                   Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellTargeted.Slot).SData.Mana;
        }

        public static bool CanCast(this Spell.Skillshot spellSkillshot)
        {
            return spellSkillshot.IsLearned && spellSkillshot.IsReady() &&
                   Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellSkillshot.Slot).SData.Mana;
        }
    }
}