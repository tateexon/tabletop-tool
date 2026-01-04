using Godot;
using Save;
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
		CountdownTimer,
		PlayerColorEditR,
		PlayerColorEditG,
		PlayerColorEditB,
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.TextChanged += OnTextChanged;
		var playerData = this.Data.GetMagicPlayer(this.Data.ActiveSection);
		Color c = Colors.Black;
		if (playerData != null)
		{
			c = playerData.Color;
		}
		switch (Box)
		{
			case WhichBox.NumberOfDice:
				this.Text = $"{this.Data.NumberOfDice}";
				break;
			case WhichBox.SizeOfDice:
				this.Text = $"{this.Data.DiceSize}";
				break;
			case WhichBox.CountdownTimer:
				if (this.Data.TimerSeconds <= 0)
				{
					this.Data.TimerSeconds = 600;
				}
				this.Text = $"{this.Data.TimerSeconds}";
				break;
			case WhichBox.PlayerColorEditR:
				this.Text = $"{c.R}";
				break;
			case WhichBox.PlayerColorEditG:
				this.Text = $"{c.G}";
				break;
			case WhichBox.PlayerColorEditB:
				this.Text = $"{c.B}";
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
		this.Text = this.TrimLeadingZeros(this.LimitToDigits(this.Text));
		if (caretColumn > this.Text.Length)
		{
			caretColumn = this.Text.Length;
		}

		this.SetCaretColumn(caretColumn);
		var c = this.Data.GetMagicPlayer(this.Data.ActiveSection).Color;
		var t = 0;
		if (!string.IsNullOrEmpty(this.Text))
		{
			t = int.Parse(this.Text);
		}

		switch (this.Box)
		{
			case WhichBox.NumberOfDice:
				this.Data.NumberOfDice = t;
				break;
			case WhichBox.SizeOfDice:
				this.Data.DiceSize = t;
				break;
			case WhichBox.CountdownTimer:
				this.Data.TimerSeconds = t;
				break;
			case WhichBox.PlayerColorEditR:
				c.R = t / 255.0f;
				this.Data.GetMagicPlayer(this.Data.ActiveSection).Color = c;
				break;
			case WhichBox.PlayerColorEditG:
				c.G = t / 255.0f;;
				this.Data.GetMagicPlayer(this.Data.ActiveSection).Color = c;
				break;
			case WhichBox.PlayerColorEditB:
				c.B = t / 255.0f;;
				this.Data.GetMagicPlayer(this.Data.ActiveSection).Color = c;
				break;
			default:
				GD.PrintErr("WTF TextEditOnlyNumbers");
				break;
		}
	}

}
