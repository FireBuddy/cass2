namespace TwistedFate
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK;

    public enum Cards
    {
        Red, 

        Yellow, 

        Blue, 

        None
    }

    public enum SelectStatus
    {
        Ready, 

        Selecting, 

        Selected, 

        Cooldown
    }

    internal class CardSelector
    {
        #region Constructors and Destructors

        static CardSelector()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Game.OnUpdate += OnGameUpdate;
        }

        #endregion

        #region Public Properties

        public static Cards Select { get; set; }

        public static SelectStatus Status { get; set; }

        #endregion

        #region Properties

        internal static int LastSendWSent { get; } = 0;

        internal static int LastWSent { get; private set; }

        private static int Delay { get; set; }

        #endregion

        #region Public Methods and Operators

        public static void StartSelecting(Cards card)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Environment.TickCount - LastWSent > 170 + (Game.Ping / 2))
                {
                    Player.Instance.Spellbook.CastSpell(SpellSlot.W, false);
                    LastWSent = Environment.TickCount;
                    Delay = Config.IsChecked(Config.Misc, "humanizePicks")
                                ? Computed.RandomDelay(Config.GetSliderValue(Config.Misc, "humanizeInt"))
                                : 0;
                }
            }
        }

        #endregion

        #region Methods

        private static void OnGameUpdate(EventArgs args)
        {
            var wName = Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name;
            var wState = Player.Instance.Spellbook.CanUseSpell(SpellSlot.W);

            if ((wState == SpellState.Ready && wName == "PickACard"
                 && (Status != SelectStatus.Selecting || Environment.TickCount - LastWSent > 500))
                || Player.Instance.IsDead)
            {
                Status = SelectStatus.Ready;
            }
            else if (wState == SpellState.Cooldown && wName == "PickACard")
            {
                Select = Cards.None;
                Status = SelectStatus.Cooldown;
            }
            else if (wState == SpellState.Surpressed && !Player.Instance.IsDead)
            {
                Status = SelectStatus.Selected;
            }

            if (Select == Cards.Blue && wName.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase)
                && Environment.TickCount - Delay > LastWSent)
            {
                SendWPacket();
            }
            else if (Select == Cards.Yellow && wName.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase)
                     && Environment.TickCount - Delay > LastWSent)
            {
                SendWPacket();
            }
            else if (Select == Cards.Red && wName.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase)
                     && Environment.TickCount - Delay > LastWSent)
            {
                SendWPacket();
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name.Equals("PickACard", StringComparison.InvariantCultureIgnoreCase))
            {
                Status = SelectStatus.Selecting;
            }

            if (args.SData.Name.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase)
                || args.SData.Name.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase)
                || args.SData.Name.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase))
            {
                Status = SelectStatus.Selected;
            }
        }

        private static void SendWPacket()
        {
            if (Config.IsChecked(Config.Misc, "cancelAApicking") && Orbwalker.IsAutoAttacking)
            {
                Orbwalker.ResetAutoAttack();
            }

            Player.Instance.Spellbook.CastSpell(SpellSlot.W, false);
        }

        #endregion
    }
}