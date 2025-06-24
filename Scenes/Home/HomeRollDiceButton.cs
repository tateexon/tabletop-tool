using Godot;

public partial class HomeRollDiceButton : Button
{
	public override void _Ready()
    {
        this.Pressed += OnButtonPressed;
    }

    public override void _ExitTree()
    {
        this.Pressed -= OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/RollDice/RollDice.tscn");
    }
}
