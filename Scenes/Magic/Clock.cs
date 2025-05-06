using Godot;
using Save;
using System;

public partial class Clock : Button
{
	[Export]
	public SaveData Data;

	[Export]
	private Timer timer;

	[Export]
	private Section activeSection = Section.None;

	[Export]
	private AudioStreamPlayer audioStreamPlayer;

	public static Action<Section> StartTimerForSection;
	public static Action PauseTimer;

	private double jeopardyLengthToBell = 33.6d;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.activeSection = Section.None;
		this.timer.Paused = true;
		timer.Timeout += this.OnTimeout;
		StartTimerForSection += this.OnSectionStartPressed;
		PauseTimer += this.OnPauseTimerPressed;
		this.Pressed += this.OnPressed;

		this.UpdateTimerDisplay();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _ExitTree()
	{
		timer.Timeout -= this.OnTimeout;
		StartTimerForSection -= this.OnSectionStartPressed;
		PauseTimer -= this.OnPauseTimerPressed;
		this.Pressed -= this.OnPressed;
	}

	public override void _Process(double delta)
	{
		if (this.timer.Paused || this.timer.TimeLeft <= 0d)
		{
			return;
		}

		this.UpdateTimerDisplay();
		this.CheckIfStartPlayingJeopardy();
	}

	private void OnPressed()
	{
		PauseTimer?.Invoke();
	}

	private void OnTimeout()
	{
		// do something on timeout
	}

	private void OnSectionStartPressed(Section sectionSendingSignal)
	{
		if (!this.timer.Paused && this.timer.TimeLeft > 0d)
		{
			return;
		}

		if (this.activeSection.Equals(sectionSendingSignal))
		{
			return;
		}

		this.activeSection = sectionSendingSignal;
		this.timer.Paused = false;
		this.timer.Start(this.Data.TimerSeconds);
	}

	private void OnPauseTimerPressed()
	{
		if (this.activeSection.Equals(Section.None))
		{
			return;
		}

		if (this.timer.Paused)
		{
			if (this.Data.AudioEnabled && this.timer.TimeLeft < this.jeopardyLengthToBell)
			{
				this.audioStreamPlayer.StreamPaused = false;
			}

			this.timer.Paused = false;
			this.timer.Start(this.timer.TimeLeft);
		}
		else
		{
			this.timer.Paused = true;
			if (this.Data.AudioEnabled)
			{
				this.audioStreamPlayer.StreamPaused = true;
			}
		}
	}

	private void UpdateTimerDisplay()
	{
		TimeSpan time = TimeSpan.FromSeconds(this.timer.TimeLeft);
		string formattedTime = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

		this.Text = formattedTime;
	}

	private void CheckIfStartPlayingJeopardy()
	{
		if (!this.Data.AudioEnabled)
		{
			return;
		}

		if (this.audioStreamPlayer.Playing)
		{
			return;
		}

		if (this.timer.TimeLeft <= this.jeopardyLengthToBell)
		{
			this.audioStreamPlayer.Play();
		}
	}
}
