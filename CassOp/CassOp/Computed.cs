using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace CassOp
{
    internal class Computed
    {
        public static AIHeroClient _Player => ObjectManager.Player;

        public static void AutoQ()
        {
            if (!Spells.Q.IsReady() && (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                return;
            }
            var bush =
                ObjectManager.Get<GrassObject>().OrderBy(br => br.Distance(_Player.ServerPosition)).FirstOrDefault();
            if (bush != null &&
                (Config.IsChecked(Config.Harass, "dontAutoHarassInBush") &&
                 _Player.ServerPosition.IsInRange(bush, bush.BoundingRadius)))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget(Spells.Q.Range))
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.IsUnderEnemyturret() &&
                _Player.IsUnderEnemyturret())
            {
                return;
            }
            if (Spells.Q.IsReady() && !target.HasBuffOfType(BuffType.Poison))
            {
                var qPred = Spells.Q.GetPrediction(target);
                if (qPred.HitChancePercent >= 90)
                {
                    Spells.Q.Cast(qPred.CastPosition);
                    //Spells.QCasted = Game.Time;
                }
            }
        }

        public static void AutoE()
        {
            if (!Spells.E.IsReady() && (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                return;
            }
            var bush =
                ObjectManager.Get<GrassObject>().OrderBy(br => br.Distance(_Player.ServerPosition)).FirstOrDefault();
            if (bush != null &&
                (Config.IsChecked(Config.Harass, "dontAutoHarassInBush") &&
                 _Player.ServerPosition.IsInRange(bush, bush.BoundingRadius)))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget(Spells.Q.Range))
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.IsUnderEnemyturret() &&
                _Player.IsUnderEnemyturret())
            {
                return;
            }
            if (Spells.E.IsReady() && target.IsValidTarget(Spells.E.Range) &&
                (!Config.IsChecked(Config.Harass, "autoHarassEonP") || target.HasBuffOfType(BuffType.Poison)))
            {
                if (Config.IsChecked(Config.Harass, "humanEInAutoHarass"))
                {
                    var delay = RandomDelay(Config.GetSliderValue(Config.Misc, "humanDelay"));
                    Core.DelayAction(() => Spells.E.Cast(target), delay);
                }
                else
                {
                    Spells.E.Cast(target);
                }
            }
        }

        public static void AutoW()
        {
            if (!Spells.W.IsReady() && (Orbwalker.LastHitMinion != null && Orbwalker.IsAutoAttacking))
            {
                return;
            }
            var bush =
                ObjectManager.Get<GrassObject>().OrderBy(br => br.Distance(_Player.ServerPosition)).FirstOrDefault();
            if (bush != null &&
                (Config.IsChecked(Config.Harass, "dontAutoHarassInBush") &&
                 _Player.ServerPosition.IsInRange(bush, bush.BoundingRadius)))
            {
                return;
            }
            var target = TargetSelector.GetTarget(Spells.W.Range, DamageType.Magical);
            if (target == null || !target.IsValidTarget(Spells.Q.Range))
            {
                return;
            }
            if (Config.IsChecked(Config.Harass, "dontAutoHarassTower") && target.IsUnderEnemyturret() &&
                _Player.IsUnderEnemyturret())
            {
                return;
            }
            if (Spells.W.IsReady() && target.IsValidTarget(Spells.W.Range))
            {
                var wPred = Spells.W.GetPrediction(target);
                if (wPred.HitChancePercent >= 85)
                {
                    Spells.W.Cast(wPred.CastPosition);
                }
            }
        }

        public static void AssistedR()
        {
            if (Config.IsKeyPressed(Config.Misc, "assistedR") && Spells.R.IsReady())
            {
                var enemyNearMouse =
                    EntityManager.Heroes.Enemies.OrderBy(entity => entity.Distance(Game.CursorPos, true))
                        .FirstOrDefault(entity => entity.IsValidTarget(Spells.R.Range));
                if (enemyNearMouse != null)
                {
                    Spells.R.Cast(enemyNearMouse);
                }
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /* -> Didnt work - use Spellbook.OnCastSpell
            if (sender.IsMe && args.SData.Name == "CassiopeiaPetrifyingGaze" &&
                Config.IsChecked(Config.Misc, "antiMissR"))
            {
                if (EntityManager.Heroes.Enemies.Count(h => h.IsValidTarget(Spells.R.Range) && h.IsFacing(_Player)) < 1)
                {
                    args.Process = false;
                }
            }*/
        }

        public static void OnSpellbookCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == SpellSlot.R && Config.IsChecked(Config.Misc, "antiMissR"))
            {
                if (EntityManager.Heroes.Enemies.Count(h => h.IsValidTarget(Spells.R.Range) && h.IsFacing(_Player)) < 1)
                {
                    args.Process = false;
                }
            }
        }

        public static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "CassiopeiaNoxiousBlast")
                {
                    Spells.QCasted = Game.Time;
                }
            }
        }

        public static void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (Config.IsChecked(Config.Misc, "eLastHit") && _Player.GetSpellDamage(target, SpellSlot.E) > target.Health &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Spells.E.Cast(target);
            }
        }

        public static void KillSteal(string mode)
        {
            if (mode == "E" && Spells.E.IsReady())
            {
                var entKs =
                    EntityManager.Heroes.Enemies.FirstOrDefault(
                        h => h.IsValidTarget(Spells.E.Range) && h.Health < _Player.GetSpellDamage(h, SpellSlot.E));
                if (entKs != null)
                {
                    Spells.E.Cast(entKs);
                }
            }
        }

        public static int RandomDelay(int x)
        {
            var y = x;
            if (x >= 100000000)
            {
                x /= 100000000;
            }
            if (x >= 10000)
            {
                x /= 10000;
            }
            if (x >= 100)
            {
                x /= 100;
            }
            if (x >= 10)
            {
                x /= 10;
            }
            x = y / x;
            return Mainframe.RDelay.Next(y - x, y + x);
        }

        /**/

        public static FarmLocation GetBestCircularFarmLocation(List<Vector2> minionPositions,
            float width,
            float range,
            int useMECMax = 9)
        {
            var result = new Vector2();
            var minionCount = 0;
            var startPos = ObjectManager.Player.ServerPosition.To2D();

            range = range * range;

            if (minionPositions.Count == 0)
            {
                return new FarmLocation(result, minionCount);
            }

            /* Use MEC to get the best positions only when there are less than 9 positions because it causes lag with more. */
            if (minionPositions.Count <= useMECMax)
            {
                var subGroups = GetCombinations(minionPositions);
                foreach (var subGroup in subGroups)
                {
                    if (subGroup.Count > 0)
                    {
                        var circle = MEC.GetMec(subGroup);

                        if (circle.Radius <= width && circle.Center.Distance(startPos, true) <= range)
                        {
                            minionCount = subGroup.Count;
                            return new FarmLocation(circle.Center, minionCount);
                        }
                    }
                }
            }
            else
            {
                foreach (var pos in minionPositions)
                {
                    if (pos.Distance(startPos, true) <= range)
                    {
                        var count = minionPositions.Count(pos2 => pos.Distance(pos2, true) <= width * width);

                        if (count >= minionCount)
                        {
                            result = pos;
                            minionCount = count;
                        }
                    }
                }
            }

            return new FarmLocation(result, minionCount);
        }

        private static List<List<Vector2>> GetCombinations(List<Vector2> allValues)
        {
            var collection = new List<List<Vector2>>();
            for (var counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                var combination = allValues.Where((t, i) => (counter & (1 << i)) == 0).ToList();

                collection.Add(combination);
            }
            return collection;
        }

        public struct FarmLocation
        {
            public int MinionsHit;
            public Vector2 Position;

            public FarmLocation(Vector2 position, int minionsHit)
            {
                Position = position;
                MinionsHit = minionsHit;
            }
        }

        /**/
    }
}