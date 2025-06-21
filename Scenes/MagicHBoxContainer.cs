using System;
using Godot;
using Save;

public partial class MagicHBoxContainer : HBoxContainer
{
    [Export]
    public SaveData Data;

    [Export]
    public Button P2;

    [Export]
    public Button P4;

    [Export]
    public Button P6;

    [Export]
    public GameMode GameMode;

    public override void _Ready()
    {
        this.P2.Pressed += this.P2Pressed;
        this.P4.Pressed += this.P4Pressed;
        this.P6.Pressed += this.P6Pressed;
    }

    public override void _ExitTree()
    {
        this.P2.Pressed -= this.P2Pressed;
        this.P4.Pressed -= this.P4Pressed;
        this.P6.Pressed -= this.P6Pressed;
    }

    private void P2Pressed()
    {
        this.Data.GameMode = this.GameMode;
        GetTree().ChangeSceneToFile("res://Scenes/Magic/2Player.tscn");
    }

    private void P4Pressed()
    {
        this.Data.GameMode = this.GameMode;
        GetTree().ChangeSceneToFile("res://Scenes/Magic/4Player.tscn");
    }

    private void P6Pressed()
    {
        this.Data.GameMode = this.GameMode;
        GetTree().ChangeSceneToFile("res://Scenes/Magic/6Player.tscn");
    }
}
