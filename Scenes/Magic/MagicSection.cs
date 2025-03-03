using Godot;

public partial class MagicSection : Control
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
    public SaveData Data;

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

    public override void _Ready()
    {
        var screenSize = GetViewport().GetVisibleRect().Size;
        BackgroundRectangle.Color = BackgroundColor;

        switch (WhichSection)
        {
            case Section.TopLeft:
                Size = new Vector2(Size.Y / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = 90;
                Position = new Vector2(screenSize.X / 2.0f, 0);
                break;
            case Section.TopRight:
                Size = new Vector2(Size.Y / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = -90;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotLeft:
                Size = new Vector2(Size.Y / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = 90f;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotRight:
                Size = new Vector2(Size.Y / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = -90f;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y);
                break;
            case Section.Top:
                Size = new Vector2(Size.X, screenSize.Y / 2.0f);
                RotationDegrees = 180f;
                Position = new Vector2(Size.X, screenSize.Y / 2.0f);
                break;
            case Section.Bot:
                Size = new Vector2(Size.X, screenSize.Y / 2.0f);
                Position = new Vector2(0, screenSize.Y / 2.0f);
                break;
            default:
                GD.PrintErr("WTF MagicSection");
                break;
        }

        AddButton.Pressed += AddPressed;
        SubtractButton.Pressed += SubtractPressed;
        HealthLabel.Text = Data.MagicHealth[this.WhichSection].ToString();
    }

    public override void _ExitTree()
    {
        AddButton.Pressed -= AddPressed;
        SubtractButton.Pressed -= SubtractPressed;
    }

    private void ButtonPressed(bool healed)
    {
        Data.MagicHealth[this.WhichSection] += healed ? 1 : -1;
        HealthLabel.Text = Data.MagicHealth[this.WhichSection].ToString();
        if (Data.AudioEnabled)
        {
            RandomAudioSound.PlayRandomSound?.Invoke(healed);
        }
        this.CallDeferred(nameof(this.StopHover), healed ? AddButton : SubtractButton);
    }

    private void SubtractPressed()
    {
        ButtonPressed(false);
    }
    private void AddPressed()
    {
        ButtonPressed(true);
    }

    private void StopHover(Button button)
    {
        button.ReleaseFocus();

        // Temporarily disable mouse filtering
        button.MouseFilter = MouseFilterEnum.Ignore;

        // Re-enable mouse filtering (reset to the original state)
        button.MouseFilter = MouseFilterEnum.Stop;
    }

}
