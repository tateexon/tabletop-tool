using System;
using Godot;
using Save;

public partial class MagicHBoxContainer : HBoxContainer
{
    [Export]
    public SaveData Data;

    [Export]
    public Button OneVOneButton;

    [Export]
    public Button OneVOneVOneVOneButton;

    [Export]
    public GameMode GameMode;

    public override void _Ready()
    {
        OneVOneButton.Pressed += OneVOneButtonPressed;
        OneVOneVOneVOneButton.Pressed += OneVOneVOneVOneButtonPressed;
    }

    public override void _ExitTree()
    {
        OneVOneButton.Pressed -= OneVOneButtonPressed;
        OneVOneVOneVOneButton.Pressed -= OneVOneVOneVOneButtonPressed;
    }

    private void OneVOneButtonPressed()
    {
        this.Data.GameMode = this.GameMode;
        GetTree().ChangeSceneToFile("res://Scenes/Magic/1v1.tscn");
    }

    private void OneVOneVOneVOneButtonPressed()
    {
        this.Data.GameMode = this.GameMode;
        GetTree().ChangeSceneToFile("res://Scenes/Magic/1v1v1v1.tscn");
    }
}
