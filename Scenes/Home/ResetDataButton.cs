using Godot;

public partial class ResetDataButton : Button
{
    [Export]
    public SaveData Data;

    public override void _Ready()
    {
        this.Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        this.Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        Data.ResetMagicHealth();
    }
}
