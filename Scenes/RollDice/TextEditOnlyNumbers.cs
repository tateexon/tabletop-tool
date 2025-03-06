using Godot;
using System.Text.RegularExpressions;

public partial class TextEditOnlyNumbers : TextEdit
{
	[Export]
	public SaveData Data;

	[Export]
	public WhichBox Box;

	public enum WhichBox
	{
		NumberOfDice,
		SizeOfDice,
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextChanged += OnTextChanged;
		switch (Box)
		{
			case WhichBox.NumberOfDice:
				Text = $"{Data.NumberOfDice}";
				break;
			case WhichBox.SizeOfDice:
				Text = $"{Data.DiceSize}";
				break;
			default:
				GD.PrintErr("WTF TextEditOnlyNumbers");
				break;
		}
	}
	public override void _ExitTree()
	{
		TextChanged -= OnTextChanged;
	}

	public virtual string LimitToDigits(string input)
	{
		string digitsOnly = Regex.Replace(input, @"[^\d]", "");
		return digitsOnly;
	}

	public virtual string TrimLeadingZeros(string input)
	{
		string trimmedResult = input.TrimStart('0');
		return trimmedResult;
	}

	private void OnTextChanged()
	{
		int caretColumn = GetCaretColumn();
		Text = this.TrimLeadingZeros(this.LimitToDigits(Text));
		if (caretColumn > Text.Length)
		{
			caretColumn = Text.Length;
		}

		SetCaretColumn(caretColumn);

		switch (Box)
		{
			case WhichBox.NumberOfDice:
				Data.NumberOfDice = int.Parse(Text);
				break;
			case WhichBox.SizeOfDice:
				Data.DiceSize = int.Parse(Text);
				break;
			default:
				GD.PrintErr("WTF TextEditOnlyNumbers");
				break;
		}
	}

}
