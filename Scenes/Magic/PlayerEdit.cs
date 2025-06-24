using Godot;
using Save;

public partial class PlayerEdit : Control
{
	[Export]
	private SaveData data;

	[Export]
	private ColorRect backgroundColor;

	[Export]
	private TextEditOnlyNumbers r;
	[Export]
	private TextEditOnlyNumbers g;
	[Export]
	private TextEditOnlyNumbers b;
	[Export]
	private Button backButton;

	public override void _Ready()
	{
		Color c = data.GetMagicColor(data.ActiveSection);
		this.r.Text = ((int)c.R8).ToString();
		this.g.Text = ((int)c.G8).ToString();
		this.b.Text = ((int)c.B8).ToString();
		this.backgroundColor.Color = c;
		this.r.TextChanged += this.OnColorChanged;
		this.g.TextChanged += this.OnColorChanged;
		this.b.TextChanged += this.OnColorChanged;
		this.backButton.Pressed += this.OnBackPressed;
	}

	public override void _ExitTree()
	{
		this.r.TextChanged -= this.OnColorChanged;
		this.g.TextChanged -= this.OnColorChanged;
		this.b.TextChanged -= this.OnColorChanged;
		this.backButton.Pressed -= this.OnBackPressed;
	}

    public override void _Process(double delta)
    {
        this.backgroundColor.Color = data.GetMagicColor(data.ActiveSection);
    }


	private void OnColorChanged()
	{
		this.backgroundColor.Color = this.data.GetMagicColor(this.data.ActiveSection);
	}

	private void OnBackPressed()
	{
		GetTree().ChangeSceneToFile(this.data.SentFromScene);
	}

}
