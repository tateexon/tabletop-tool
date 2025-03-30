using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class SaveData : Resource
{
    [Export]
    public bool AudioEnabled { get; set; }

    [Export]
    private Dictionary<GameMode, Dictionary<Section, int>> MagicHealth { get; set; }

    // roll dice
    [Export]
    public int NumberOfDice { get; set; }

    [Export]
    public int DiceSize { get; set; }

    [Export]
    public GameMode GameMode { get; set; }

    public void Reset()
    {
        ResetMagicHealth();
        ResetDice();
    }

    public void ResetMagicHealth()
    {
        foreach (var mode in MagicHealth)
        {
            MagicHealth[mode.Key] = new Dictionary<Section, int>();
            System.Array values = Enum.GetValues(typeof(Section));
            foreach (Section n in values)
            {
                switch (mode.Key)
                {
                    case GameMode.Magic60Card:
                        MagicHealth[mode.Key][n] = 20;
                        break;
                    case GameMode.MagicCommander:
                        MagicHealth[mode.Key][n] = 40;
                        break;
                    default:
                        GD.PrintErr($"WTF Happened resetting magic health values? {mode.Key}");
                        break;
                }
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
