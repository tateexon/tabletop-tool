using Godot;
using Godot.Collections;
using System;

public partial class AudioStreamPool : Node
{
	
    [Export]
    public Array<AudioStreamOggVorbis> HealSounds = new Array<AudioStreamOggVorbis>();
    [Export]
    public Array<AudioStreamOggVorbis> DamageSounds = new Array<AudioStreamOggVorbis>();

	[Export]
	public Array<AudioStreamOggVorbis> KOSounds = new Array<AudioStreamOggVorbis>();

	[Export]
	public Array<AudioStreamPlayerPoolItem> StreamPool = new Array<AudioStreamPlayerPoolItem>();

	private int poolIndex = 0;

	private RandomNumberGenerator rng;
	public enum SoundType
	{
		Heal,
		Damage,
		KO,
	}

	public static Action<SoundType> PlaySound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.rng = new RandomNumberGenerator();
		PlaySound += this.PlaySounds;
	}

	public override void _ExitTree()
	{
		PlaySound -= this.PlaySounds;
	}

	private void PlaySounds(SoundType soundType)
	{
		AudioStreamOggVorbis soundToPlay;
		int rangeCap = 0;
		switch (soundType)
		{
			case SoundType.Heal:
				rangeCap = HealSounds.Count;
				soundToPlay = HealSounds[this.rng.RandiRange(0, rangeCap - 1)];
				this.StreamPool[this.NextIndex()].PlayThisSound(soundToPlay);
				break;
			case SoundType.Damage:
				rangeCap = DamageSounds.Count;
				soundToPlay = DamageSounds[this.rng.RandiRange(0, rangeCap - 1)];
				this.StreamPool[this.NextIndex()].PlayThisSound(soundToPlay);
				break;
			case SoundType.KO:
				rangeCap = KOSounds.Count;
				soundToPlay = KOSounds[this.rng.RandiRange(0, rangeCap - 1)];
				this.StreamPool[this.NextIndex()].PlayThisSound(soundToPlay);
				break;
			default:
				break;
		}
	}

	private int NextIndex()
	{
		this.poolIndex += 1;
		if (this.poolIndex >= this.StreamPool.Count)
		{
			this.poolIndex = 0;
		}

		return this.poolIndex;
	}
}
