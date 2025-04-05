using Godot;

public partial class AudioStreamPlayerPoolItem : AudioStreamPlayer
{
	public void PlayThisSound(AudioStreamOggVorbis soundToPlay)
	{
		Stream = soundToPlay;
		Play();
	}
}
