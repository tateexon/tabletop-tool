using System;
using Godot;

public partial class MagicHBoxContainer : HBoxContainer
{
    [Export]
    public Button OneVOneButton;

    [Export]
    public Button OneVOneVOneVOneButton;

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
        GetTree().ChangeSceneToFile("res://Scenes/Magic/1v1.tscn");
    }

    private void OneVOneVOneVOneButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Magic/1v1v1v1.tscn");
    }
}
