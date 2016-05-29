namespace Sona
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    using Sona.Extensions;
    using Sona.Modes;
    using Sona.OtherUtils;

    internal class Mainframe
    {
        #region Methods

        internal static void Init()
        {
            Game.OnTick += OnTick;
            Orbwalker.OnPreAttack += Combo.OnPreAttack;
            Orbwalker.OnPreAttack += Harass.OnPreAttack;
            Drawing.OnDraw += Drawings.OnDraw;
            Interrupter.OnInterruptableSpell += Interrupt.OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapclose.OnGapclose;
            Obj_AI_Base.OnProcessSpellCast += Computed.OnProcessSpellCast;
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo.Execute();
            }

            if (Config.IsChecked(Config.AutoWMenu, "bW") && Spells.W.CanCast()
                && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.AutoWMenu, "minMana"))
            {
                AutoW.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)
                && Player.Instance.ManaPercent >= Config.GetSliderValue(Config.HarassMenu, "minMana"))
            {
                Harass.Execute();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee.Execute();
            }
        }

        #endregion
    }
}