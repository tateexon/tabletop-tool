using Godot;

public partial class ExitButton : Button
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
        GetTree().ChangeSceneToFile("res://Scenes/Home/HomeScreen.tscn");
    }
}
