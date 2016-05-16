namespace Olaf
{
    using System;
    using System.Linq;
    using System.Timers;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX;

    internal static class Computed
    {
        #region Properties

        internal static bool IsPickingUp { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void FastCombo1(AttackableUnit target, EventArgs args)
        {
            if (Spells.W.IsReady() && Config.IsChecked(Config.Combo, "useWCombo"))
            {
                Spells.W.Cast();
            }

            if (Player.Instance.HasItem(Spells.Tiamat.Id) && Spells.Tiamat.IsReady()
                && Config.IsChecked(Config.Combo, "useTiamatCombo"))
            {
                Spells.Tiamat.Cast();
                Orbwalker.ResetAutoAttack();
            }

            if (Player.Instance.HasItem(Spells.RavHydra.Id) && Spells.RavHydra.IsReady()
                && Config.IsChecked(Config.Combo, "useTiamatCombo"))
            {
                Spells.RavHydra.Cast();
                Orbwalker.ResetAutoAttack();
            }

            if (Player.Instance.HasItem(Spells.TitHydra.Id) && Spells.TitHydra.IsReady()
                && Config.IsChecked(Config.Combo, "useTiamatCombo"))
            {
                Spells.TitHydra.Cast();
                Orbwalker.ResetAutoAttack();
            }

            Orbwalker.ResetAutoAttack();

            Orbwalker.OnPostAttack -= FastCombo1;
            Orbwalker.OnPostAttack += FastCombo2;
        }

        public static void FastJungle1(AttackableUnit target, EventArgs args)
        {
            var objTarget = target as Obj_AI_Base;
            if (objTarget != null
                && (Spells.W.IsReady() && Config.IsChecked(Config.JungleClear, "useWJungleClear")
                    && objTarget.Health >= Player.Instance.GetAutoAttackDamage(objTarget) * 4))
            {
                Spells.W.Cast();
            }

            if (Player.Instance.HasItem(Spells.Tiamat.Id) && Spells.Tiamat.IsReady()
                && Config.IsChecked(Config.JungleClear, "useTiamatJungleClear"))
            {
                Spells.Tiamat.Cast();
                Orbwalker.ResetAutoAttack();
            }

            if (Player.Instance.HasItem(Spells.RavHydra.Id) && Spells.RavHydra.IsReady()
                && Config.IsChecked(Config.JungleClear, "useTiamatJungleClear"))
            {
                Spells.RavHydra.Cast();
                Orbwalker.ResetAutoAttack();
            }

            if (Player.Instance.HasItem(Spells.TitHydra.Id) && Spells.TitHydra.IsReady()
                && Config.IsChecked(Config.JungleClear, "useTiamatJungleClear"))
            {
                Spells.TitHydra.Cast();
                Orbwalker.ResetAutoAttack();
            }

            Orbwalker.ResetAutoAttack();

            Orbwalker.OnPostAttack -= FastJungle1;
            Orbwalker.OnPostAttack += FastJungle2;
        }

        public static void FcTimer1Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Modes.Bursting)
            {
                Orbwalker.OnPostAttack -= FastCombo1;
            }

            Modes.Bursting = false;
            Modes.SafeToPickup = true;
        }

        public static void FcTimer2Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Modes.Bursting)
            {
                Orbwalker.OnPostAttack -= FastCombo2;
            }

            Modes.Bursting = false;
            Modes.SafeToPickup = true;
        }

        public static void FjTimer1Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Modes.Bursting)
            {
                Orbwalker.OnPostAttack -= FastJungle1;
            }

            Modes.Bursting = false;
            Modes.SafeToPickup = true;
        }

        public static void FjTimer2Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Modes.Bursting)
            {
                Orbwalker.OnPostAttack -= FastCombo2;
            }

            Modes.Bursting = false;
            Modes.SafeToPickup = true;
        }

        public static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe)
            {
                Mainframe.PlayerIssuePos = args.TargetPosition;
            }
        }

        public static void OnObjectCreate(GameObject sender, EventArgs eventArgs)
        {
            /*if (sender.Name.Contains("olaf"))
            {
                Chat.Print(sender.Name);
            }*/
            if (sender.Name == "olaf_axe_totem_team_id_green.troy")
            {
                Spells.AxeObject = sender;
            }
        }

        public static void OnObjectDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name == "olaf_axe_totem_team_id_green.troy")
            {
                Spells.AxeObject = null;
                Mainframe.PickUpPosition = Vector3.Zero;
                IsPickingUp = false;
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
                Game.OnTick -= Mainframe.PickUpAxe;
                Game.OnTick += Mainframe.OnGameUpdate;
            }
        }

        public static void OnSpellbookCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            throw new NotImplementedException();
        }

        public static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            throw new NotImplementedException();
        }

        public static void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            if (target.Health <= Player.Instance.GetSpellDamage(target, SpellSlot.E) && Spells.E.IsReady()
                && target.IsValidTarget(Spells.E.Range) && Config.IsChecked(Config.Misc, "eOnMinz"))
            {
                Spells.E.Cast(target);
            }
        }

        public static bool UnderEnemyTurret(this Vector3 position)
        {
            return EntityManager.Turrets.Enemies.Any(turret => turret.Distance(position) < 775);
        }

        #endregion

        #region Methods

        private static void FastCombo2(AttackableUnit target, EventArgs args)
        {
            Modes.Bursting = false;
            Modes.SafeToPickup = true;
            Orbwalker.OnPostAttack -= FastCombo2;
            Modes.FcTimer1.Stop();
        }

        private static void FastJungle2(AttackableUnit target, EventArgs args)
        {
            Modes.Bursting = false;
            Modes.SafeToPickup = true;
            Orbwalker.OnPostAttack -= FastJungle2;
            Modes.FjTimer1.Stop();
        }

        #endregion
    }
}