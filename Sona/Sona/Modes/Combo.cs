namespace Sona.Modes
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Sona.Extensions;
    using Sona.OtherUtils;

    internal static class Combo
    {
        #region Methods

        internal static void Execute()
        {
            if (Config.IsChecked(Config.ComboMenu, "bR") && Spells.R.CanCast())
            {
                var rTarget = TargetSelector.GetTarget(Spells.R.Range, DamageType.Magical);
                if (rTarget != null)
                {
                    var rRectangle = new Geometry.Polygon.Rectangle(
                        Player.Instance.Position, 
                        Player.Instance.Position.Extend(rTarget, Spells.R.Range).To3D(), 
                        Spells.R.Width);
                    if (
                        EntityManager.Heroes.Enemies.Count(
                            enemy =>
                            rRectangle.IsInside(Prediction.Position.PredictUnitPosition(enemy, 200))
                            && !enemy.HasBuffOfType(BuffType.Invulnerability) && !enemy.IsDead && enemy.IsValid)
                        >= Config.GetSliderValue(Config.ComboMenu, "minR"))
                    {
                        Spells.R.Cast(rTarget);
                    }
                }
            }

            if (Config.IsChecked(Config.ComboMenu, "bFlashR") && Spells.R.CanCast() && Spells.Flash.CanCast())
            {
                var rFlashTarget = TargetSelector.GetTarget(Spells.R.Range + 400, DamageType.Magical);
                if (rFlashTarget != null)
                {
                    var flashPos = Player.Instance.Position.Extend(rFlashTarget, 425).To3D();
                    var flashUltRectangle = new Geometry.Polygon.Rectangle(
                        flashPos, 
                        flashPos.Extend(Player.Instance.Position, -Spells.R.Range).To3D(), 
                        Spells.R.Width);
                    if (
                        EntityManager.Heroes.Enemies.Count(
                            enemy =>
                            flashUltRectangle.IsInside(Prediction.Position.PredictUnitPosition(enemy, 200))
                            && !enemy.HasBuffOfType(BuffType.Invulnerability) && !enemy.IsDead && enemy.IsValid)
                        >= Config.GetSliderValue(Config.ComboMenu, "minFlashR"))
                    {
                        Player.CastSpell(Spells.Flash.Slot, flashPos);
                        Core.DelayAction(() => Spells.R.Cast(rFlashTarget), Computed.RDelay.Next(50, 85));

                        /* For when mid animation flash is possible
                        Spells.R.Cast(flashPos);
                        Core.DelayAction(
                            () => Player.CastSpell(Spells.Flash.Slot, flashPos), 
                            Computed.RDelay.Next(150, 250));
                            */
                    }
                }
            }

            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            if (Config.IsChecked(Config.ComboMenu, "bQ") && Spells.Q.CanCast())
            {
                Spells.Q.Cast();
            }

            if (Config.IsChecked(Config.ComboMenu, "bE") && Spells.E.CanCast())
            {
                if (target.Path.Length == 0 || !target.IsMoving)
                {
                    return;
                }

                var pathEndLoc = target.Path.OrderByDescending(x => x.Distance(target)).FirstOrDefault();
                var dist = Player.Instance.Position.Distance(target.Position);
                var distToPath = pathEndLoc.Distance(Player.Instance.Position);
                if (distToPath <= dist)
                {
                    return;
                }

                var movspeedDif = Player.Instance.MoveSpeed - target.MoveSpeed;
                if (movspeedDif <= 0 && !Player.Instance.IsInAutoAttackRange(target))
                {
                    Spells.E.Cast();
                }

                var timeToReach = dist / movspeedDif;
                if (timeToReach > 2.5f)
                {
                    Spells.E.Cast();
                }
            }
        }

        internal static void OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)
                || !Config.IsChecked(Config.ComboMenu, "bSmartAA"))
            {
                return;
            }

            if (!Player.Instance.HasBuff("sonapassiveattack") && !Player.Instance.HasBuff("sonaqprocattacker"))
            {
                args.Process = false;
            }
        }

        #endregion
    }
}