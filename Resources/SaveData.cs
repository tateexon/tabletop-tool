namespace Save
{
    using Godot;
    using Godot.Collections;
    using System;

    [Tool]
    [GlobalClass]
    public partial class SaveData : Resource
    {
        [Export]
        public bool AudioEnabled = true;

        [Export]
        private Dictionary<GameMode, Dictionary<Section, int>> MagicHealth { get; set; }

        // roll dice
        [Export]
        public int NumberOfDice { get; set; }

        [Export]
        public int DiceSize { get; set; }

        [Export]
        public GameMode GameMode { get; set; }

        [Export]
        public int TimerSeconds { get; set; }

        public void Reset()
        {
            ResetMagicHealth();
            ResetDice();
            if (this.TimerSeconds <= 0)
            {
                this.TimerSeconds = 600;
            }
        }

        public void ResetMagicHealth()
        {
            Array<GameMode> modes = [GameMode.Magic60Card, GameMode.MagicCommander];

            foreach (var mode in modes)
            {
                this.ResetMagicHealth(mode);
            }
        }

        private void ResetMagicHealth(GameMode mode)
        {
            MagicHealth[mode] = new Dictionary<Section, int>();
            System.Array values = Enum.GetValues(typeof(Section));
            foreach (Section n in values)
            {
                switch (mode)
                {
                    case GameMode.Magic60Card:
                        MagicHealth[mode][n] = 20;
                        break;
                    case GameMode.MagicCommander:
                        MagicHealth[mode][n] = 40;
                        break;
                    default:
                        GD.PrintErr($"WTF Happened resetting magic health values? {mode}");
                        break;
                }
            }
        }

        public int GetMagicHealth(Section s)
        {
            if (this.MagicHealth == null)
            {
                this.MagicHealth = new Dictionary<GameMode, Dictionary<Section, int>>();
                this.ResetMagicHealth();
            }

            if (this.GameMode.Equals(GameMode.None))
            {
                return 0;
            }

            if (!this.MagicHealth.ContainsKey(this.GameMode))
            {
                this.ResetMagicHealth(this.GameMode);
            }

            return this.MagicHealth[this.GameMode][s];
        }

        public void SetMagicHealth(Section s, int value)
        {
            if (this.MagicHealth == null)
            {
                this.MagicHealth = new Dictionary<GameMode, Dictionary<Section, int>>();
                this.ResetMagicHealth();
            }

            if (this.GameMode.Equals(GameMode.None))
            {
                return;
            }
            this.MagicHealth[this.GameMode][s] = value;
        }

        public void ResetDice()
        {
            NumberOfDice = 2;
            DiceSize = 6;
        }
    }
}