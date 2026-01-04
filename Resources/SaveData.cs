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
        private Dictionary<GameMode, Dictionary<Section, MagicPlayerData>> MagicHealth { get; set; }

        [Export]
        private Dictionary<Section, Color> MagicColor { get; set; }
        public string SentFromScene;
        public Section ActiveSection;
        private bool colorSet;

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
            this.ResetMagicHealth();
            this.ResetDice();
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
                this.ResetMagicPlayerHealth(mode);
            }
        }

        private void ResetMagicPlayerHealth(GameMode mode)
        {
            this.MagicHealth[mode] = new Dictionary<Section, MagicPlayerData>();
            System.Array values = Enum.GetValues(typeof(Section));

            // setup the new player data objects
            foreach (Section n in values)
            {
                switch (mode)
                {
                    case GameMode.Magic60Card:
                        this.MagicHealth[mode][n] = new MagicPlayerData(n, 20);
                        break;
                    case GameMode.MagicCommander:
                        this.MagicHealth[mode][n] = new MagicPlayerData(n, 40);
                        break;
                    default:
                        GD.PrintErr($"WTF Happened resetting magic health values? {mode}");
                        break;
                }
            }

            // load all players into other commanders for commander damage calculations
            foreach (Section n in values)
            {
                foreach (Section nn in values)
                {
                    this.MagicHealth[mode][n].AddCommander(this.MagicHealth[mode][nn]);
                }
            }
        }

        public MagicPlayerData GetMagicPlayer(Section s)
        {
            if (this.MagicHealth == null)
            {
                this.MagicHealth = new Dictionary<GameMode, Dictionary<Section, MagicPlayerData>>();
                this.ResetMagicHealth();
            }
            if (this.GameMode.Equals(GameMode.None))
            {
                return null;
            }

            if (!this.MagicHealth.ContainsKey(this.GameMode))
            {
                this.ResetMagicPlayerHealth(this.GameMode);
            }
            return this.MagicHealth[this.GameMode][s];
        }

        public void ResetDice()
        {
            this.NumberOfDice = 2;
            this.DiceSize = 6;
        }
    }
}