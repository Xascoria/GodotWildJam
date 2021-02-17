using Godot;
using System;

public class LvlControl : Node
{
	Terminal terminal;
	Display display;
	Board board;
	LineEdit line_input;
	Node2D header;

	Tween tween;
	private Tween.TransitionType tt = Tween.TransitionType.Linear;
	private Tween.EaseType et = Tween.EaseType.InOut;
	Timer timer1;
	Timer timer2;
	AudioStreamPlayer bgm_player;

	//Set to 0 on default
	private int story_progress = 0;

	public override void _Ready()
	{
		terminal = GetNode<Terminal>("../Terminal");
		terminal.Connect(nameof(Terminal.StoryInput), this, nameof(StoryInput));
		line_input = terminal.input;

		display = GetNode<Display>("../Display");
		header = display.header;
		board = display.board;
		
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
		board.Modulate = new Color(1,1,1,0);
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
				break;
			case 2:
				terminal.AddScrollingLine("Congratulations on your new internship position in the OFN!", 1.5);
				tween.InterpolateProperty(display, "modulate:a", 0, 1, 0.6f, tt, et, 4f);
				tween.Start();
				break;
			case 3:
				terminal.AddScrollingLine("I believe you have already read the memo, so let's get", 1.5);
				break;
			case 4:
				terminal.AddScrollingLine("right down into business.");
				break;
			case 5:
				terminal.AddScrollingLine("While the system is still connecting with the servers,", 1.5);
				break;
			case 6:
				terminal.AddScrollingLine("let's get some standard procedure out of the way.");
				break;
			case 7:
				terminal.AddScrollingLine("\"I pledge allegiance to the Organization of Free Nations,", 1.5);
				break;
			case 8:
				terminal.AddScrollingLine("and her cause of liberating the oppressed people around the");
				break;
			case 9:
				terminal.AddScrollingLine("world, with the goal of bringing liberty and justice for");
				break;
			case 10:
				terminal.AddScrollingLine("all\".");
				break;
			case 11:
				terminal.AddScrollingLine("If you agree with the above statement, type anything into", 1.5);
				break;
			case 12:
				terminal.AddScrollingLine("the console below, then press enter.");
				line_input.Visible = true;
				line_input.Modulate = new Color(1,1,1,0);
				tween.InterpolateProperty(line_input, "modulate:a", 0, 1, 0.6f, tt, et, 0.2f);
				tween.Start();
				break;
			case 13:
				terminal.AddScrollingLine("Great, welcome onboard!", 3.0);
				break;
			case 14:
				terminal.AddScrollingLine("Now, let's get you familiar with the system.", 0.9);
				break;
			case 15:
				terminal.AddScrollingLine("Let's start by showing you the tools you can access, type", 0.9);
				break;
			case 16:
				terminal.AddScrollingLine("\"list\" into the console.");
				break;
			case 17:
				terminal.AddScrollingLine("So these are the units you currently have control over in", 0.9);
				break;
			case 18:
				terminal.AddScrollingLine("the system's arsenal.");
				break;
			case 19:
				terminal.AddScrollingLine("Type \"help [unit name]\" into the console to get the details", 0.9);
				break;
			case 20:
				terminal.AddScrollingLine("regarding the units.");
				break;
			case 21:
				terminal.AddScrollingLine("Great, looks like you got the hang of it.", 0.9);
				break;
			case 22:
				terminal.AddScrollingLine("Now let me set up a simulation that you can try out the", 0.9);
				break;
			case 23:
				terminal.AddScrollingLine("toys on.");
				break;
			case 24:
				terminal.AddScrollingLine("So on the left, you'll see a simulated operation field.", 0.9);
				SetupLevel(0);
				tween.InterpolateProperty(board, "modulate:a", 0, 1, 2f, tt, et, 3);
				tween.InterpolateProperty(display.targets_left, "modulate:a", 0, 1, 2f, tt, et, 3);
				tween.Start();
				break;
			case 25:
				terminal.AddScrollingLine("You can use the commands \"toggle s\" and \"toggle c\" to", 0.9);
				break;
			case 26:
				terminal.AddScrollingLine("toggle between coordinates and the situation on the field.");
				break;
			case 27:
				terminal.AddScrollingLine("When you want to use an unit, just type in the command:", 0.9);
				break;
			case 28:
				terminal.AddScrollingLine("\"[unit_name] [coord]\".");
				break;
			case 29:
				terminal.AddScrollingLine("Anyway, that is all. Now take out all the targets on the", 0.9);
				break;
			case 30:
				terminal.AddScrollingLine("field to finish the tutorial.");
				break;
			case 31:
				terminal.AddScrollingLine("Remember, you could always use the command \"reset\" if you");
				break;
			case 32:
				terminal.AddScrollingLine("done something wrong, this is just a simulation after all!");
				break;
		}
		story_progress += 1;
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
		bgm_player.VolumeDb = -16;
		bgm_player.Play();

		story_progress = 1;
		ProgressStory();
	}

	private int story_var_1 = -1;
	private int story_var_2 = -1;

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
			case 19:
			case 20:
				ProgressStory();
				break;
			case 21:
				terminal.SetAllowInput(true);
				break;
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
				ProgressStory();
				break;
			case 27:
				terminal.SetAllowInput(true);
				break;
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
				ProgressStory();
				break;
			case 33:
				terminal.SetStoryInput(false);
				terminal.SetAllowInput(true);
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
					terminal.AddStaticLine("1 unit(s) of : HBS");
					terminal.AddStaticLine("1 unit(s) of : SP");

					ProgressStory();
					terminal.SetAllowInput(false);
				}
				break;
			case 21:
			//using story var 1 and 2 to detect is user check all the conditions
				string[] inputs = new_line.ToLower().Split(" ");
				if (inputs.Length >= 2)
				{
					if (inputs[0].Equals("help"))
					{
						if (inputs[1].Equals("hbs"))
						{
							terminal.AddStaticLine("HBS: Heartbeat Scanner");
							terminal.AddStaticLine("Scan for human heartbeats, reveals their locations.");
							terminal.AddStaticLine("Range: 3x3 tiles from the center tile.");
							story_var_1 = 1;
						}
						else if (inputs[1].Equals("sp"))
						{
							terminal.AddStaticLine("SP: Sniper");
							terminal.AddStaticLine("Kill one target on the given coord.");
							terminal.AddStaticLine("Range: 1x1 tile.");
							story_var_2 = 1;
						}
					}
				}
				else if (new_line.ToLower().Equals("list"))
				{
					terminal.AddStaticLine("You current have:");
					terminal.AddStaticLine("1 unit(s) of : HBS");
					terminal.AddStaticLine("1 unit(s) of : SP");
				}
				if (story_var_1 == 1 && story_var_2 == 1)
				{
					story_var_1 = -1;
					story_var_2 = -1;
					terminal.SetAllowInput(false);
					ProgressStory();
				}
				break;
			case 27:
				inputs = new_line.ToLower().Split(" ");
				if (inputs.Length >= 2)
				{
					if (inputs[0].Equals("toggle"))
					{
						if (inputs[1][0] == 's')
						{
							board.ToggleBoard(false);
						}
						else if (inputs[1][0] == 'c')
						{
							board.ToggleBoard(true);
							terminal.SetAllowInput(false);
							ProgressStory();
						}
					}
				}
				break;
		}
	}

	private void SetupLevel(int level_num)
	{
		switch(level_num)
		{
			case 0:
				//Tutorial lvl
				terminal.cpu.is_tutorial = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 1;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 1;

				board.SetupGridMap(3);
				display.targets_left.Text = "Target(s) Left: 1";
				board.SetTargetLoc(1, new Tuple<int, int>(1,1));
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
			board.Modulate = new Color(1,1,1,0);
			terminal.SetAllowInput(false);
			terminal.SetStoryInput(true);
		}
		ProgressStory();
		return false;
	}
}








