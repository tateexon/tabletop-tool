using Godot;
using System;
using System.Collections.Generic;

public partial class DiceGridContainer : GridContainer
{
	[Export]
	public PackedScene GridContainerLabelScene;

	[Export]
	public TextEdit numberOfDiceTextEdit;

	[Export]
	public TextEdit diceSizeTextEdit;

	[Export]
	public Label RollDiceTotalLabel;

	public static Action GenerateDiceEvent;

	public override void _Ready()
	{
		GenerateDiceEvent += OnGenerateCalled;
	}
	public override void _ExitTree()
	{
		GenerateDiceEvent -= OnGenerateCalled;
	}

	public void OnGenerateCalled()
	{
		var l = GenerateDice();
		SpawnLabels(l);
		var total = 0;
		foreach (var i in l)
		{
			total += i;
		}
		RollDiceTotalLabel.Text = $"Total: {total}";
	}

	public List<int> GenerateDice()
	{
		if (string.IsNullOrEmpty(numberOfDiceTextEdit.Text) || string.IsNullOrEmpty(diceSizeTextEdit.Text))
		{
			GD.PrintErr("need numbers in the text edit fields");
		}

		int n = int.Parse(numberOfDiceTextEdit.Text);
		int d = int.Parse(diceSizeTextEdit.Text);

		var rng = new Random();
		var list = new List<int>();

		for (int i = 0; i < n; i++)
		{
			list.Add(rng.Next(1, d + 1));
		}

		return list;
	}

	public void SpawnLabels(List<int> numbers)
    {
        if (GridContainerLabelScene == null)
        {
            GD.PrintErr("LabelScene is not assigned!");
            return;
        }

		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}

        for (int i = 0; i < numbers.Count; i++)
        {
            // Instantiate the Label scene
            Label labelInstance = (Label)GridContainerLabelScene.Instantiate();

            // Set the text of the label (optional)
            labelInstance.Text = $"{numbers[i]}";

            // Add the label to the GridContainer
            AddChild(labelInstance);
        }
    }
}
