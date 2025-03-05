using Godot;

public partial class RollDiceButton : Button
{
	public override void _Ready()
	{
		Pressed += OnButtonPressed;
	}
	public override void _ExitTree()
	{
		Pressed -= OnButtonPressed;
	}

	public void OnButtonPressed()
	{
		DiceGridContainer.GenerateDiceEvent?.Invoke();
	}
}
