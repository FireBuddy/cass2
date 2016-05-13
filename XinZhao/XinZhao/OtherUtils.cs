using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

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

        public static bool UnderEnemyTurret(this Vector3 position)
        {
            return (EntityManager.Turrets.Enemies.Any(turret => turret.Distance(position) < 775));
        }

        public static Vector3 GetBestAllyPlace(this Vector3 position, float range, float inRange = 750)
        {
            Obj_AI_Base bestAlly =
                EntityManager.Heroes.Allies.Where(
                    x => x.Distance(Player.Instance.Position) <= range && !x.IsMe && !x.IsDead && x.IsValid)
                    .OrderByDescending(x => x.CountAlliesInRange(inRange))
                    .FirstOrDefault();
            if (bestAlly != null)
            {
                var bestAllyMasz =
                    EntityManager.Heroes.Allies.Where(
                        a => a.Distance(bestAlly.Position) <= inRange && !a.IsMe && !a.IsDead && a.IsValid).ToArray();
                var bestallv2 = new Vector2[bestAllyMasz.Count()];
                for (var i = 0; i < bestAllyMasz.Count(); i++)
                {
                    bestallv2[i] = bestAllyMasz[i].Position.To2D();
                }
                return bestallv2.CenterPoint().To3D();
            }
            var closeTurret =
                EntityManager.Turrets.Allies.Where(t => t.Distance(Player.Instance.Position) <= range)
                    .OrderBy(t => t.Distance(Player.Instance.Position))
                    .FirstOrDefault();
            if (closeTurret != null)
            {
                return closeTurret.Position;
            }
            var nex = ObjectManager.Get<Obj_Building>().FirstOrDefault(x => x.Name.StartsWith("HQ") && x.IsAlly);
            return nex?.Position ?? Vector3.Zero;
        }
    }
}