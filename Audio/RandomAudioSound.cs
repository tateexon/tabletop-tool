using Godot;
using Godot.Collections;
using System;

public partial class RandomAudioSound : AudioStreamPlayer
{
    [Export]
    public Array<AudioStreamOggVorbis> HealSounds = new Array<AudioStreamOggVorbis>();
    [Export]
    public Array<AudioStreamOggVorbis> DamageSounds = new Array<AudioStreamOggVorbis>();

    private RandomNumberGenerator rng;

    public static Action<bool> PlayRandomSound;

    public override void _Ready()
    {
        rng = new RandomNumberGenerator();
        PlayRandomSound += this.PlayRandomSounds;
    }

    public override void _ExitTree()
    {
        PlayRandomSound -= this.PlayRandomSounds;
    }

    public void PlayRandomSounds(bool toHeal)
    {
        // get the range for the heal and damage sounds
        int rangeCap = toHeal ? HealSounds.Count : DamageSounds.Count;

        // generate a random number between 0 and the cap -1 inclusive
        int randomNumber = rng.RandiRange(0, rangeCap - 1);

        // get the sound from the list
        var soundToPlay = toHeal ? HealSounds[randomNumber] : DamageSounds[randomNumber];

        // set the stream
        Stream = soundToPlay;

        // Play the sound
        Play();
    }
}
