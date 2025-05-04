using Godot;
using System;

public partial class Clock : Button
{
	[Export]
	private Timer timer;

	[Export]
	private Section activeSection = Section.None;

	public static Action<Section> StartTimerForSection;
	public static Action PauseTimer;

	private double secondsToWait = 600d;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.activeSection = Section.None;
		this.timer.Paused = true;
		timer.Timeout += this.OnTimeout;
		StartTimerForSection += this.OnSectionStartPressed;
		PauseTimer += this.OnPauseTimerPressed;
		this.Pressed += this.OnPressed;
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

		UpdateTimerDisplay();
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
		if (!this.timer.Paused)
		{
			return;
		}

		if (this.activeSection.Equals(sectionSendingSignal))
		{
			return;
		}

		this.activeSection = sectionSendingSignal;
		this.timer.Paused = false;
		this.timer.Start(this.secondsToWait);
	}

	private void OnPauseTimerPressed()
	{
		if (this.timer.Paused)
		{
			this.timer.Paused = false;
			this.timer.Start(this.timer.TimeLeft);
		}
		else
		{
			this.timer.Paused = true;
		}
	}

	private void UpdateTimerDisplay()
	{
		TimeSpan time = TimeSpan.FromSeconds(this.timer.TimeLeft);
		string formattedTime = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

		this.Text = formattedTime;
	}
}
