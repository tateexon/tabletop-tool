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
            this.ResetMagicColor();
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
            this.MagicHealth[mode] = new Dictionary<Section, int>();
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

        private void ResetMagicColor()
        {
            if (this.colorSet)
            {
                return;
            }

            this.colorSet = true;
            this.MagicColor = new Dictionary<Section, Color>();
            this.MagicColor[Section.Top] = Colors.Red;
            this.MagicColor[Section.Bot] = Colors.Blue;
            this.MagicColor[Section.TopLeft] = Colors.Red;
            this.MagicColor[Section.TopRight] = Colors.Blue;
            this.MagicColor[Section.BotLeft] = Colors.Purple;
            this.MagicColor[Section.BotRight] = Colors.Orange;
            this.MagicColor[Section.MidLeft] = Colors.Green;
            this.MagicColor[Section.MidRight] = Colors.Pink;
            this.MagicColor[Section.None] = Colors.Black;
        }

        public Color GetMagicColor(Section s)
        {
            ResetMagicColor();
            return this.MagicColor[s];
        }

        public void SetMagicColor(Section s, Color c)
        {
            ResetMagicColor();
            this.MagicColor[s] = c;
        }
    }
}