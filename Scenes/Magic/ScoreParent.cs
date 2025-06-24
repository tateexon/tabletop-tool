using Godot;
using Save;

public partial class ScoreParent : Node2D
{
	[Export]
	private SaveData data;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.data.SentFromScene = this.GetTree().CurrentScene.SceneFilePath;
	}
}
