using Godot;
using System;

public class LvlControl : Node
{
	Terminal terminal;
	Display display;
	LineEdit line_input;
	Node2D header;

	Tween tween;
	private Tween.TransitionType tt = Tween.TransitionType.Linear;
	private Tween.EaseType et = Tween.EaseType.InOut;
	Timer timer1;
	Timer timer2;
	AudioStreamPlayer bgm_player;

	//Set to 0 on default
	private int story_progress = 15;

	public override void _Ready()
	{
		terminal = GetNode<Terminal>("../Terminal");
		terminal.Connect(nameof(Terminal.StoryInput), this, nameof(StoryInput));
		line_input = terminal.input;

		display = GetNode<Display>("../Display");
		header = display.header;
		
		tween = GetNode<Tween>("Tween");
		timer1 = GetNode<Timer>("Timer");
		timer2 = GetNode<Timer>("Timer2");
		bgm_player = GetNode<AudioStreamPlayer>("BGMPlayer");
		terminal.Connect(nameof(Terminal.FinishedAnimation), this, nameof(TerminalAnimationEnded));

		if (DebugState())
		{
			SetupInitial();
		}
		
	}

	private void SetupInitial()
	{
		terminal.Modulate = new Color(1,1,1,0);
		terminal.SetStoryInput(true);
		terminal.SetAllowInput(false);
		display.Modulate = new Color(1,1,1,0);
		line_input.Visible = false;
		display.targets_left.Modulate = new Color(1,1,1,0);
		
		bgm_player.Stream = GD.Load<AudioStream>("res://Resources/Sound/startup.wav");
		bgm_player.VolumeDb = -4;
		bgm_player.Play();

		tween.InterpolateProperty(terminal, "modulate:a", 0, 1, 0.2f, tt, et, 0.5f);
		tween.Start();
	}

	private void ProgressStory()
	{
		switch(story_progress)
		{
			case 1:
				terminal.AddScrollingLine("Hello, Newbie!");
				story_progress += 1;
				break;
			case 2:
				terminal.AddScrollingLine("Congratulations on your new internship position in the OFN!", 1.5);
				story_progress += 1;
				tween.InterpolateProperty(display, "modulate:a", 0, 1, 0.6f, tt, et, 4f);
				tween.Start();
				break;
			case 3:
				terminal.AddScrollingLine("I believe you have already read the memo, so let's get", 1.5);
				story_progress += 1;
				break;
			case 4:
				terminal.AddScrollingLine("right down into business!");
				story_progress += 1;
				break;
			case 5:
				terminal.AddScrollingLine("While the system is still connecting with the servers,", 1.5);
				story_progress += 1;
				break;
			case 6:
				terminal.AddScrollingLine("let's get some standard procedure out of the way.");
				story_progress += 1;
				break;
			case 7:
				terminal.AddScrollingLine("\"I pledge allegiance to the Organization of Free Nations,", 1.5);
				story_progress += 1;
				break;
			case 8:
				terminal.AddScrollingLine("and her cause of liberating the oppressed people around the");
				story_progress += 1;
				break;
			case 9:
				terminal.AddScrollingLine("world, with the goal of bringing liberty and justice for");
				story_progress += 1;
				break;
			case 10:
				terminal.AddScrollingLine("all\".");
				story_progress += 1;
				break;
			case 11:
				terminal.AddScrollingLine("If you agree with the above statement, type anything into", 1.5);
				story_progress += 1;
				break;
			case 12:
				terminal.AddScrollingLine("the console below, then press enter.");
				story_progress += 1;
				line_input.Visible = true;
				line_input.Modulate = new Color(1,1,1,0);
				tween.InterpolateProperty(line_input, "modulate:a", 0, 1, 0.6f, tt, et, 0.2f);
				tween.Start();
				break;
			case 13:
				terminal.AddScrollingLine("Great, welcome onboard!", 3.0);
				story_progress += 1;
				break;
			case 14:
				terminal.AddScrollingLine("Now, let's get you familiar with the system.", 0.9);
				story_progress += 1;
				break;
			case 15:
				terminal.AddScrollingLine("Let's start by showing you the tools you can access, type", 0.9);
				story_progress += 1;
				break;
			case 16:
				terminal.AddScrollingLine("\"list\" into the console.");
				story_progress += 1;
				break;
			case 17:
				terminal.AddScrollingLine("So these are the units you currently have in the system's", 0.9);
				story_progress += 1;
				break;
			case 18:
				terminal.AddScrollingLine("arsenal.");
				story_progress += 1;
				break;
		}
	}

	private void _on_Tween_tween_all_completed()
	{
		// Replace with function body.
	}

	private void _on_Timer_timeout()
	{
		// Replace with function body.
	}

	private void _on_BGMPlayer_finished()
	{
		bgm_player.Stream = GD.Load<AudioStream>("res://Resources/Sound/neon_bg.wav");
		bgm_player.VolumeDb = -12;
		bgm_player.Play();

		story_progress = 1;
		ProgressStory();
	}

	public void TerminalAnimationEnded()
	{
		switch(story_progress)
		{
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				ProgressStory();
				break;
			case 13:
				terminal.SetAllowInput(true);
				break;
			case 14:
			case 15:
			case 16:
				ProgressStory();
				break;
			case 17:
				terminal.SetAllowInput(true);
				break;
			case 18:
				ProgressStory();
				break;
		}
	}

	public void StoryInput(string new_line)
	{
		switch(story_progress)
		{
			case 13:
				ProgressStory();
				terminal.SetAllowInput(false);
				break;
			case 17:
				if (new_line.ToLower().Equals("list"))
				{
					terminal.AddStaticLine("You current have:");
					terminal.AddStaticLine("1 Unit(s) of : HBS");
					terminal.AddStaticLine("1 Unit(s) of : SP");

					ProgressStory();
					terminal.SetAllowInput(false);
				}
				break;
		}
	}

	private bool DebugState()
	{
		if (story_progress == 0)
		{
			return true;
		}
		if (story_progress >= 13)
		{
			terminal.Modulate = new Color(1,1,1,1);
			display.Modulate = new Color(1,1,1,1);
			display.targets_left.Modulate = new Color(1,1,1,0);
			terminal.SetAllowInput(false);
			terminal.SetStoryInput(true);
		}
		ProgressStory();
		return false;
	}
}








