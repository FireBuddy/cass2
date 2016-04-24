using System;
using EloBuddy;
using EloBuddy.SDK;

namespace TwistedFate
{
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
        public static Cards Select;
        public static int LastWSent;
        public static int LastSendWSent = 0;

        static CardSelector()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Game.OnUpdate += OnGameUpdate;
        }

        public static SelectStatus Status { get; set; }

        private static void SendWPacket()
        {
            if (Config.IsChecked(Config.Misc, "humanizePicks"))
            {
                Core.DelayAction(
                    () => Player.Instance.Spellbook.CastSpell(SpellSlot.W, false),
                    Computed.RandomDelay(Config.GetSliderValue(Config.Misc, "humanizeInt")));
            }
            else
            {
                Player.Instance.Spellbook.CastSpell(SpellSlot.W, false);
            }
        }

        public static void StartSelecting(Cards card)
        {
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Environment.TickCount - LastWSent > 170 + Game.Ping / 2)
                {
                    Player.Instance.Spellbook.CastSpell(SpellSlot.W, false);
                    LastWSent = Environment.TickCount;
                }
            }
        }

        private static void OnGameUpdate(EventArgs args)
        {
            var wName = Player.Instance.Spellbook.GetSpell(SpellSlot.W).Name;
            var wState = Player.Instance.Spellbook.CanUseSpell(SpellSlot.W);

            if ((wState == SpellState.Ready && wName == "PickACard" &&
                 (Status != SelectStatus.Selecting || Environment.TickCount - LastWSent > 500)) ||
                Player.Instance.IsDead)
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

            if (Select == Cards.Blue && wName.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase))
            {
                SendWPacket();
            }
            else if (Select == Cards.Yellow && wName.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase))
            {
                SendWPacket();
            }
            else if (Select == Cards.Red && wName.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase))
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

            if (args.SData.Name.Equals("GoldCardLock", StringComparison.InvariantCultureIgnoreCase) ||
                args.SData.Name.Equals("BlueCardLock", StringComparison.InvariantCultureIgnoreCase) ||
                args.SData.Name.Equals("RedCardLock", StringComparison.InvariantCultureIgnoreCase))
            {
                Status = SelectStatus.Selected;
            }
        }
    }
}