using Godot;
using Godot.Collections;
using System;

[Tool]
[GlobalClass]
public partial class SaveData : Resource
{
    [Export]
    public bool AudioEnabled;

    [Export]
    public Dictionary<MagicSection.Section, int> MagicHealth;

    // roll dice
    [Export]
    public int NumberOfDice;

    [Export]
    public int DiceSize;

    public void Reset()
    {
        ResetMagicHealth();
        ResetDice();
    }

    public void ResetMagicHealth()
    {
        MagicHealth = new Dictionary<MagicSection.Section, int>();
        System.Array values = Enum.GetValues(typeof(MagicSection.Section));
        foreach (MagicSection.Section n in values)
        {
            MagicHealth[n] = 20;
        }
    }

    public void ResetDice()
    {
        NumberOfDice = 2;
        DiceSize = 6;
    }
}
