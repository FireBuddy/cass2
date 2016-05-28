namespace Sona.OtherUtils
{
    using System;
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using Sona.Extensions;

    internal class Drawings
    {
        #region Methods

        internal static void OnDraw(EventArgs args)
        {
            if (Config.IsChecked(Config.DrawMenu, "bQ")
                && (!Config.IsChecked(Config.DrawMenu, "onlyRdy") || Spells.Q.CanCast()))
            {
                Drawing.DrawCircle(Player.Instance.Position, Spells.Q.Range, Color.AliceBlue);
            }

            if (Config.IsChecked(Config.DrawMenu, "bW")
                && (!Config.IsChecked(Config.DrawMenu, "onlyRdy") || Spells.W.CanCast()))
            {
                Drawing.DrawCircle(Player.Instance.Position, Spells.W.Range, Color.AliceBlue);
            }

            if (Config.IsChecked(Config.DrawMenu, "bE")
                && (!Config.IsChecked(Config.DrawMenu, "onlyRdy") || Spells.E.CanCast()))
            {
                Drawing.DrawCircle(Player.Instance.Position, Spells.E.Range, Color.AliceBlue);
            }

            if (Config.IsChecked(Config.DrawMenu, "bR")
                && (!Config.IsChecked(Config.DrawMenu, "onlyRdy") || Spells.R.CanCast()))
            {
                Drawing.DrawCircle(Player.Instance.Position, Spells.R.Range, Color.AliceBlue);
            }

            if (Config.IsChecked(Config.DrawMenu, "drawFR")
                && (!Config.IsChecked(Config.DrawMenu, "onlyRdy") || (Spells.R.CanCast() && Spells.Flash.CanCast())))
            {
                var rFlashTarget = TargetSelector.GetTarget(Spells.R.Range + 425, DamageType.Magical);
                if (rFlashTarget != null)
                {
                    var flashPos = Player.Instance.Position.Extend(rFlashTarget, 425).To3D();

                    // Drawing.DrawCircle(flashPos, 75, Color.AliceBlue);
                    var flashUltRectangle = new Geometry.Polygon.Rectangle(
                        flashPos, 
                        flashPos.Extend(Player.Instance.Position, -Spells.R.Range).To3D(), 
                        Spells.R.Width);
                    foreach (var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            e =>
                            flashUltRectangle.IsInside(Prediction.Position.PredictUnitPosition(e, 200))
                            && !e.HasBuffOfType(BuffType.Invulnerability) && !e.IsDead && e.IsValid))
                    {
                        Drawing.DrawCircle(enemy.Position, 100, Color.Red);
                        Drawing.DrawText(Drawing.WorldToScreen(enemy.Position), Color.Red, "Flash Ult", 5);
                    }
                }
            }
        }

        #endregion
    }
}