using Godot;
using Save;

public partial class MagicSection : Control
{
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
    public RichTextLabel RichHealthLabel;

    public override void _Ready()
    {
        var screenSize = GetViewport().GetVisibleRect().Size;
        screenSize.Y -= 25;
        var thisSizeY = Size.Y - 25;
        BackgroundRectangle.Color = BackgroundColor;

        switch (WhichSection)
        {
            case Section.TopLeft:
                Size = new Vector2(thisSizeY / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = 90;
                Position = new Vector2(screenSize.X / 2.0f, 0);
                break;
            case Section.TopRight:
                Size = new Vector2(thisSizeY / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = -90;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotLeft:
                Size = new Vector2(thisSizeY / 2.0f, screenSize.X / 2.0f);
                RotationDegrees = 90f;
                Position = new Vector2(screenSize.X / 2.0f, screenSize.Y / 2.0f);
                break;
            case Section.BotRight:
                Size = new Vector2(thisSizeY / 2.0f, screenSize.X / 2.0f);
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
        SetHealthLabel(Data.GetMagicHealth(this.WhichSection).ToString());
    }

    public override void _ExitTree()
    {
        AddButton.Pressed -= AddPressed;
        SubtractButton.Pressed -= SubtractPressed;
    }

    private void SetHealthLabel(string text)
    {
        this.RichHealthLabel.Text = $"[u]{text}[/u]";
    }

    private void ButtonPressed(bool healed)
    {
        int health = Data.GetMagicHealth(this.WhichSection);
        health += healed ? 1 : -1;
        Data.SetMagicHealth(this.WhichSection, health);
        SetHealthLabel(Data.GetMagicHealth(this.WhichSection).ToString());
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
