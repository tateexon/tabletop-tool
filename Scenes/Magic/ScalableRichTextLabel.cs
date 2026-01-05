using Godot;

public partial class ScalableRichTextLabel : RichTextLabel
{
	private Font currentFont;
	private string baseText;
	private int fontSize;
	private int charCount;
	private Vector2 previousSize;

	private int resizeCount = 0;

	public override void _Ready()
	{
		currentFont = this.GetThemeFont("normal_font");
		Resized += OnResized;
	}

	private void OnResized()
	{
		this.SetLabel(this.baseText);
	}

	public void SetLabel(string text)
	{
		if (text == null)
		{
			return;
		}
		this.baseText = text;
		int charCount = text.ToCharArray().Length;

		if (charCount == this.charCount && this.previousSize == this.Size)
		{
			return;
		}

		GD.Print("TEXT: find size");
		string tmpChars = "";
		for (int i = 0; i < charCount; i++)
		{
			tmpChars += "0";
		}
		this.fontSize = this.FindBestFitFontSize(tmpChars, this.Size);
		this.charCount = charCount;
		this.previousSize = this.Size;

		// Build formatted text with BBCode
		var formattedText = "";

		formattedText += $"[u][font_size={fontSize}]";
		formattedText += text;
		formattedText += "[/font_size][/u]";

		this.Text = formattedText;
	}

	private int FindBestFitFontSize(string text, Vector2 containerSize)
	{
		if (this.currentFont == null)
		{
			return 32;
		}

		int minSize = 8;
		int maxSize = 1024;
		int bestSize = minSize;

		// Allow 5% margin
		float widthMargin = containerSize.X * 0.95f;
		float heightMargin = containerSize.Y * 0.95f;

		for (int size = maxSize; size >= minSize; size--)
		{
			var textSize = this.currentFont.GetStringSize(text, HorizontalAlignment.Left, -1, size);

			if (textSize.X <= widthMargin && textSize.Y <= heightMargin)
			{
				bestSize = size;
				break;
			}

			// If we're at min size and it still doesn't fit, use min size anyway
			if (size == minSize)
			{
				bestSize = minSize;
			}
		}

		return bestSize;
	}
}
