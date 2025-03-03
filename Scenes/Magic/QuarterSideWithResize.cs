using Godot;

public partial class QuarterSideWithResize : Control
{
    public enum Section
    {
        TopLeft,
        TopRight,
        BotLeft,
        BotRight,
        Top,
        Bot,
    }

    [Export]
    public Section WhichSection;

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

        Size = new Vector2(Size.Y / 2.0f, screenSize.X / 2.0f);

        switch (WhichSection)
        {
            case Section.TopLeft:
                RotationDegrees = 90;
                Position = new Vector2(screenSize.X / 2.0f, 0);
                break;
            case Section.TopRight:
                RotationDegrees = -90;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotLeft:
                RotationDegrees = 90f;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotRight:
                RotationDegrees = -90f;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y);
                break;
            default:
                GD.PrintErr("WTF QuarterSideWithResize");
                break;
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
