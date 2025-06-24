using Godot;
using Save;
using System;

public partial class MagicSection : Control
{
    [Export]
    public SaveData Data;

    [Export]
    private int numSections;

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
    private const float left = 90f;
    private const float right = -90f;

    public override void _Ready()
    {
        var screenSize = GetViewport().GetVisibleRect().Size;
        screenSize.Y -= 25;
        var thisSizeY = Size.Y - 25;
        this.BackgroundColor = this.Data.GetMagicColor(this.WhichSection);
        BackgroundRectangle.Color = BackgroundColor;
        Size = this.getSize(screenSize.X, thisSizeY);
        float xLength = screenSize.X / 2;
        float yLength = screenSize.Y / this.numSections;

        switch (this.WhichSection)
        {
            case Section.TopLeft:
                RotationDegrees = left;
                Position = new Vector2(xLength, 0);
                break;
            case Section.TopRight:
                RotationDegrees = right;
                Position = new Vector2(xLength, yLength * (this.numSections == 2 ? 1 : 1));
                break;
            case Section.BotLeft:
                RotationDegrees = left;
                Position = new Vector2(xLength, yLength * (this.numSections == 2 ? 1 : 2));
                break;
            case Section.BotRight:
                RotationDegrees = right;
                Position = new Vector2(xLength, screenSize.Y);
                break;
            case Section.MidLeft:
                RotationDegrees = left;
                Position = new Vector2(xLength, yLength);
                break;
            case Section.MidRight:
                RotationDegrees = right;
                Position = new Vector2(xLength, yLength * 2);
                break;
            case Section.Top:
                RotationDegrees = 180f;
                Position = new Vector2(Size.X, screenSize.Y / 2.0f);
                break;
            case Section.Bot:
                Position = new Vector2(0, screenSize.Y / 2.0f);
                break;
            default:
                GD.PrintErr("WTF MagicSection");
                break;
        }

        if (this.numSections == 3)
        {
            this.RichHealthLabel.AddThemeFontSizeOverride("normal_font_size", 250);
        }
        this.RichHealthLabel.MouseFilter = MouseFilterEnum.Ignore;
        this.AddButton.Pressed += this.AddPressed;
        this.SubtractButton.Pressed += this.SubtractPressed;
        this.CenterButton.Pressed += this.OnUserButtonPressed;
        SetHealthLabel(this.Data.GetMagicHealth(this.WhichSection).ToString());

        this.doubleClickTimer.WaitTime = this.doubleClickDelay;
        this.doubleClickTimer.OneShot = true;
        this.doubleClickTimer.Timeout += this.HandleMultiTap;

        this.styleBox = this.isSelectedPanel.GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;
        this.SetSelectedBorder(Section.None);
        SetSelected += this.SetSelectedBorder;
    }

    public override void _ExitTree()
    {
        this.AddButton.Pressed -= this.AddPressed;
        this.SubtractButton.Pressed -= this.SubtractPressed;
        this.CenterButton.Pressed -= this.OnUserButtonPressed;
        this.doubleClickTimer.Timeout -= this.HandleMultiTap;
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

        if (this.clickCount < 3)
        {
            if (this.doubleClickTimer.IsStopped())
            {
                // timer was stopped so we entered a race conditions like place where we already did the single click send,
                // start this back up as a single click.
                // i mean realistically I don't think we should end up here but just in case
                this.clickCount = 1;
                this.doubleClickTimer.Start();
            }
            else
            {
                this.doubleClickTimer.Stop();
                this.doubleClickTimer.Start();
            }
        }
        else
        {
            this.doubleClickTimer.Stop();
            this.HandleMultiTap();
        }
    }

    private void HandleMultiTap()
    {
        switch (this.clickCount)
        {
            case 1:
                // single tap
                Clock.StartTimerForSection?.Invoke(this.WhichSection, 1);
                break;
            case 2:
                // double tap
                SetSelected?.Invoke(this.WhichSection);
                Clock.StartTimerForSection?.Invoke(this.WhichSection, 2);
                break;
            case 3:
                // triple tap
                SetSelected?.Invoke(this.WhichSection);
                GetTree().ChangeSceneToFile("res://Scenes/Magic/PlayerEdit.tscn");
                break;
            default:
                break;
        }
        this.clickCount = 0;
    }

    private void SetSelectedBorder(Section whichSection)
    {
        this.Data.ActiveSection = whichSection;
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

    private Vector2 getSize(float sizeX, float sizeY)
    {
        if (this.numSections <= 1)
        {
            return new Vector2(sizeX, sizeY / 2.0f);
        }
        else if (this.numSections == 2)
        {
            return new Vector2(sizeY / 2.0f, sizeX / 2.0f);
        }
        else
        {
            return new Vector2(sizeY / 3.0f, sizeX / 2.0f);
        }
    }
}
