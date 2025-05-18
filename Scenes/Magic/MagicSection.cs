using Godot;
using Save;
using System;
using System.Data;

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
    private Color selectedColor;
    [Export]
    private Color unselectedColor;

    [Export]
    private PanelContainer isSelectedPanel;
    public static Action<Section> SetSelected;
    private StyleBoxFlat styleBox;

    [Export]
    public Button AddButton;

    [Export]
    public Button SubtractButton;

    [Export]
    public RichTextLabel RichHealthLabel;

    [Export]
    public Button CenterButton;

    [Export]
    private Timer doubleClickTimer;
    [Export]
    private float doubleClickDelay = 0.3f;

    private int clickCount = 0;

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

        this.RichHealthLabel.MouseFilter = MouseFilterEnum.Ignore;
        this.AddButton.Pressed += this.AddPressed;
        this.SubtractButton.Pressed += this.SubtractPressed;
        this.CenterButton.Pressed += this.OnUserButtonPressed;
        SetHealthLabel(this.Data.GetMagicHealth(this.WhichSection).ToString());

        this.doubleClickTimer.WaitTime = this.doubleClickDelay;
        this.doubleClickTimer.OneShot = true;
        this.doubleClickTimer.Timeout += this.OnDoubleClickTimerTimeout;

        this.styleBox = this.isSelectedPanel.GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
        this.SetSelectedBorder(Section.None);
        SetSelected += this.SetSelectedBorder;
    }

    public override void _ExitTree()
    {
        this.AddButton.Pressed -= this.AddPressed;
        this.SubtractButton.Pressed -= this.SubtractPressed;
        this.CenterButton.Pressed -= this.OnUserButtonPressed;
        this.doubleClickTimer.Timeout -= this.OnDoubleClickTimerTimeout;
        SetSelected -= this.SetSelectedBorder;
    }

    private void SetHealthLabel(string text)
    {
        this.RichHealthLabel.Text = $"[u]{text}[/u]";
    }

    private void ButtonPressed(bool healed)
    {
        int health = Data.GetMagicHealth(this.WhichSection);
        health += healed ? 1 : -1;
        if (health < 0)
        {
            health = 0;
            this.CallDeferred(nameof(this.StopHover), healed ? AddButton : SubtractButton);
            return;
        }

        Data.SetMagicHealth(this.WhichSection, health);
        SetHealthLabel(Data.GetMagicHealth(this.WhichSection).ToString());
        if (Data.AudioEnabled)
        {
            AudioStreamPool.SoundType typeToPlay = AudioStreamPool.SoundType.Heal;
            if (!healed)
            {
                typeToPlay = AudioStreamPool.SoundType.Damage;
            }

            if (health <= 0)
            {
                typeToPlay = AudioStreamPool.SoundType.KO;
            }

            AudioStreamPool.PlaySound?.Invoke(typeToPlay);
        }
        this.CallDeferred(nameof(this.StopHover), healed ? this.AddButton : this.SubtractButton);
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

    private void OnUserButtonPressed()
    {
        this.clickCount++;

        if (this.clickCount == 1)
        {
            this.doubleClickTimer.Start();
        }
        else if (this.clickCount == 2)
        {
            if (!this.doubleClickTimer.IsStopped())
            {
                this.doubleClickTimer.Stop();
                this.HandleDoubleTap();
                this.clickCount = 0;
            }
            else
            {
                // timer was stopped so we entered a race conditions like place where we already did the single click send,
                // start this back up as a single click.
                // i mean realistically I don't think we should end up here but just in case
                this.clickCount = 1;
                this.doubleClickTimer.Start();
            }
        }
    }

    private void HandleDoubleTap()
    {
        SetSelected?.Invoke(this.WhichSection);
        Clock.StartTimerForSection?.Invoke(this.WhichSection, 2);
    }

    private void HandleSingleTap()
    {
        Clock.StartTimerForSection?.Invoke(this.WhichSection, 1);
    }

    private void OnDoubleClickTimerTimeout()
    {
        if (this.clickCount == 1)
        {
            this.HandleSingleTap();
        }

        this.clickCount = 0;
    }

    private void SetSelectedBorder(Section whichSection)
    {
        if (this.WhichSection.Equals(whichSection))
        {
            // set this one to have a gold border
            this.styleBox.BgColor = this.selectedColor;
        }
        else
        {
            // clear out border color
            this.styleBox.BgColor = this.unselectedColor;
        }

        this.isSelectedPanel.AddThemeStyleboxOverride("panel", styleBox);
    }
}
