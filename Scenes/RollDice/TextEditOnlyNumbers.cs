using Godot;
using System.Text.RegularExpressions;

public partial class TextEditOnlyNumbers : TextEdit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextChanged += OnTextChanged;
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
	}


}
