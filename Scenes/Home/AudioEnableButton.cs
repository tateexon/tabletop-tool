using Godot;
using Save;

public partial class AudioEnableButton : Button
{
    [Export]
    public SaveData Data;

    public override void _Ready()
    {
        this.Pressed += OnPressed;
        this.SetText();
    }

    public override void _ExitTree()
    {
        this.Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        Data.AudioEnabled = !Data.AudioEnabled;
        this.SetText();
    }

    private void SetText()
    {
        this.Text = Data.AudioEnabled ? "Audio Enabled" : "Audio Disabled";
    }
}
