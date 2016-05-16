using EloBuddy;
using EloBuddy.SDK;

namespace Soraka_HealBot.Extensions
{
    public static class SpellExtensions
    {
        public static bool CanCast(this Spell.Active spellActive) => spellActive.IsLearned && spellActive.IsReady() &&
                                                                     Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellActive.Slot).SData.Mana;

        public static bool CanCast(this Spell.Targeted spellTargeted) => spellTargeted.IsLearned && spellTargeted.IsReady() &&
                                                                         Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellTargeted.Slot).SData.Mana;

        public static bool CanCast(this Spell.Skillshot spellSkillshot) => spellSkillshot.IsLearned && spellSkillshot.IsReady() &&
                                                                           Player.Instance.Mana >= Player.Instance.Spellbook.GetSpell(spellSkillshot.Slot).SData.Mana;
    }
}