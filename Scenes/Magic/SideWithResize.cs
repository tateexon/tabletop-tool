using Godot;

public partial class SideWithResize : Control
{

    [Export]
    public bool IsTop;

    [Export]
    public Color BackgroundColor;

    [Export]
    public ColorRect BackgroundRectangle;

    [Export]
    public Button AddButton;

    [Export]
    public Button SubtractButton;

    [Export]
    public Label HealthLabel;

    private int Health = 20;

    public override void _Ready()
    {
        var screenSize = GetViewport().GetVisibleRect().Size;
        BackgroundRectangle.Color = BackgroundColor;

        if (IsTop)
        {
            Size = new Vector2(Size.X, screenSize.Y / 2.0f);
            RotationDegrees = 180f;
            Position = new Vector2(Size.X, screenSize.Y / 2.0f);
        }
        else
        {
            Size = new Vector2(Size.X, screenSize.Y / 2.0f);
            Position = new Vector2(0, screenSize.Y / 2.0f);
        }

        AddButton.ToggleMode = false;
        SubtractButton.ToggleMode = false;

        AddButton.Pressed += AddPressed;
        SubtractButton.Pressed += SubtractPressed;
        HealthLabel.Text = Health.ToString();
    }

    public override void _ExitTree()
    {
        AddButton.Pressed -= AddPressed;
        SubtractButton.Pressed -= SubtractPressed;
    }

    private void SubtractPressed()
    {
        Health -= 1;
        HealthLabel.Text = Health.ToString();
        RandomAudioSound.PlayRandomSound?.Invoke(false);
        this.CallDeferred(nameof(this.StopHover), SubtractButton);
    }
    private void AddPressed()
    {
        Health += 1;
        HealthLabel.Text = Health.ToString();
        RandomAudioSound.PlayRandomSound?.Invoke(true);
        this.CallDeferred(nameof(this.StopHover), AddButton);
    }

    private void StopHover(Button button)
    {
        button.ReleaseFocus();
        
        // Temporarily disable mouse filtering
        button.MouseFilter = Control.MouseFilterEnum.Ignore;

        // Re-enable mouse filtering (reset to the original state)
        button.MouseFilter = Control.MouseFilterEnum.Stop;
    }

}
