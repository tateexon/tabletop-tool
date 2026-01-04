namespace Save
{
	using Godot;
	using Godot.Collections;
	using System;

	public partial class MagicPlayerData : Resource
	{
		private Section Section;
		public int Health;
		public int Poison;
		public Dictionary<MagicPlayerData, int> CommanderDamage;
		public Color Color;

		public void Reset(int health, Color color)
		{
			this.Health = health;
			this.Poison = 0;
			this.Color = color;
			this.CommanderDamage = [];

			switch (this.Section)
			{
				case Section.Top:
				case Section.TopLeft:
					this.Color = Colors.Red;
					break;
				case Section.Bot:
				case Section.TopRight:
					this.Color = Colors.Blue;
					break;
				case Section.BotLeft:
					this.Color = Colors.Purple;
					break;
				case Section.BotRight:
					this.Color = Colors.Orange;
					break;
				case Section.MidLeft:
					this.Color = Colors.Green;
					break;
				case Section.MidRight:
					this.Color = Colors.Pink;
					break;
				case Section.None:
				default:
					this.Color = Colors.Black;
					break;
			}
		}

		public MagicPlayerData(Section s, int health)
		{
			this.Section = s;
			this.Reset(health, Colors.Black);
		}

		public void AddCommander(MagicPlayerData m)
		{
			if (m == this)
			{
				return;
			}

			this.CommanderDamage.Add(m, 0);
		}

		public void IncrementHealth(bool healed)
		{
			this.Health += healed ? 1 : -1;
			if (this.Health <= 0)
			{
				this.Health = 0;
			}
		}

		public void AddCommanderDamage(MagicPlayerData from, bool healed)
		{
			this.IncrementHealth(healed);
			int d = this.CommanderDamage[from];
			d += healed ? 1 : -1;
			if (d <= 0)
			{
				d = 0;
			}
			this.CommanderDamage[from] = d;
		}
	}
}