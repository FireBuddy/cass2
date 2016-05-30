namespace TwistedFate
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    internal class Computed
    {
        #region Public Methods and Operators

        public static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (args.Target is AIHeroClient)
            {
                args.Process = CardSelector.Status != SelectStatus.Selecting
                               && Environment.TickCount - CardSelector.LastWSent > 300;
            }

            if (CardSelector.Status == SelectStatus.Selecting
                && ((Config.IsChecked(Config.Combo, "disableAAselectingC")
                     && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                    || (Config.IsChecked(Config.Harass, "disableAAselectingH")
                        && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                    || (Config.IsChecked(Config.LaneClear, "disableAAselectingLC")
                        && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    || (Config.IsChecked(Config.JungleClear, "disableAAselectingJC")
                        && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))))
            {
                args.Process = false;
            }
        }

        public static void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            var enemyTarget = EntityManager.Heroes.Enemies.FirstOrDefault(e => e.NetworkId == target.NetworkId);
            if (enemyTarget != null && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                && Config.IsChecked(Config.Combo, "qAAReset"))
            {
                Spells.Q.Cast(enemyTarget);
            }
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "Gate" && Config.IsChecked(Config.Misc, "AutoYAG"))
            {
                CardSelector.StartSelecting(Cards.Yellow);
            }

            if (!sender.IsMe)
            {
            }
        }

        public static int RandomDelay(int x)
        {
            var vary = 0;
            if (x < 100)
            {
                vary = 10;
            }

            if (x > 100)
            {
                vary = x / 10;
            }

            return Tf.RDelay.Next(x - vary, x + vary);
        }

        public static void SafeCast(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot != SpellSlot.W) return;
            if (CardSelector.Starting
                && (Config.IsKeyPressed(Config.CardSelectorMenu, "csYellow")
                    || Config.IsKeyPressed(Config.CardSelectorMenu, "csBlue")
                    || Config.IsKeyPressed(Config.CardSelectorMenu, "csRed")))
            {
                args.Process = false;
            }

            if (CardSelector.Starting)
            {
                CardSelector.Starting = false;
            }
        }

        public static void YellowIntoQ(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || args.SData.Name.ToLower() != "goldcardpreattack" || !Spells.Q.IsReady())
            {
                return;
            }

            var qTarget = args.Target as Obj_AI_Base;
            if (qTarget == null || !qTarget.IsValidTarget(Spells.Q.Range))
            {
                return;
            }

            if (Config.IsChecked(Config.Combo, "yellowIntoQ")
                && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                && Config.IsChecked(Config.Combo, "useQinCombo"))
            {
                var qPred = Spells.Q.GetPrediction(qTarget);
                Spells.Q.Cast(qPred.CastPosition);
            }

            if (Config.IsChecked(Config.Misc, "autoYellowIntoQ"))
            {
                var qPred = Spells.Q.GetPrediction(qTarget);
                Spells.Q.Cast(qPred.CastPosition);
            }
        }

        #endregion
    }
}