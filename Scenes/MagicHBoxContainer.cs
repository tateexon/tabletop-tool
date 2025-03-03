using Godot;

public partial class MagicHBoxContainer : HBoxContainer
{
    [Export]
    public Button OneVOneButton;

    public override void _Ready()
    {
        OneVOneButton.Pressed += OneVOneButtonPressed;
    }

    public override void _ExitTree()
    {
        OneVOneButton.Pressed -= OneVOneButtonPressed;
    }

    private void OneVOneButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Magic/1v1.tscn");
    }
}
