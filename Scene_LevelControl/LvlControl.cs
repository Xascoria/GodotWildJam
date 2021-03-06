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
	AudioStreamPlayer bgm_player;
	Ending ending_scene;

	private int story_var_1 = -1;
	private int story_var_2 = -1;
	//default true
	private bool ending_4_possible = true;
	//Set to 0 on default
	private int story_progress = 0;

	public override void _Ready()
	{
		terminal = GetNode<Terminal>("../Terminal");
		terminal.Connect(nameof(Terminal.StoryInput), this, nameof(StoryInput));
		line_input = terminal.input;
		terminal.cpu.Connect(nameof(CPU.Victory), this, nameof(OnVictory));
		terminal.cpu.Connect(nameof(CPU.Failure), this, nameof(OnFailure));

		display = GetNode<Display>("../Display");
		header = display.header;
		board = display.board;

		ending_scene = GetNode<Ending>("../Ending");
		
		tween = GetNode<Tween>("Tween");
		timer1 = GetNode<Timer>("Timer");
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
				terminal.AddScrollingLine("Remember, you could always use the command \"reset\" if you", 1.5);
				break;
			case 32:
				terminal.AddScrollingLine("done something wrong, this is just a simulation after all!");
				break;
			//Finish tutorial
			case 33:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);
				terminal.AddScrollingLine("Well done. You are ready.", 4);
				break;
			case 34:
				board.Visible = false;
				display.targets_left.Visible = false;
				terminal.AddScrollingLine("It's time for your first field assignment.", 0.9);
				break;
			case 35:
				terminal.AddScrollingLine("There's been a coup in Guyana, and the Pro-OFN government", 0.9);
				break;
			case 36:
				terminal.AddScrollingLine("has been overthrown by a general.");
				break;
			case 37:
				terminal.AddScrollingLine("The marines had landed 2 hours ago, and they are pushing", 0.9);
				break;
			case 38:
				terminal.AddScrollingLine("the Guyanese forces into the mountains.");
				break;
			case 39:
				terminal.AddScrollingLine("A few key coup leaders has escaped into a small forest",0.9);
				break;
			case 40:
				terminal.AddScrollingLine("outside the capital.");
				break;
			case 41:
				terminal.AddScrollingLine("That's going to be your task, you are going to be connected", 0.9);
				break;
			case 42:
				terminal.AddScrollingLine("to the Paramaribo network in a second.");
				break;
			case 43:
				terminal.AddScrollingLine("Consider this as your trial mission, we'll be watching your", 2.0);
				break;
			case 44:
				terminal.AddScrollingLine("performance and decide what happens next.");
				break;
			case 45:
				terminal.AddStaticLine("Connecting to Paramaribo Network...");
				timer1.WaitTime = 4f;
				timer1.Start();
				break;
			case 56:
				display.header_subtitle.Text = "Paramaribo Operations";
				terminal.ClearTerminal();
				terminal.SetAllowInput(true);
				terminal.SetStoryInput(false);

				SetupLevel(1);

				board.Visible = true;
				display.targets_left.Visible = true;
				terminal.AddStaticLine("New equipment unlocked: Small bomb (SB)");
				break;
			case 57:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);
				//Level 1
				//Victory
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("All targets eliminated, well done.", 3.0);
				}
				//Failure 
				else
				{
					terminal.AddScrollingLine("It seems that some coup leaders had escaped.", 3.0);
				}
				break;
			case 58:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("I knew I saw great potential in you.", 0.9);
				}
				//Failure 
				else
				{
					terminal.AddScrollingLine("I'm disappointed in you, kid, I thought I saw great", 0.9);
				}
				break;
			case 59:
				board.Visible = false;
				display.targets_left.Visible = false;
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("Congrats, you are now officially a field operator of the", 1.5);
				}
				else
				{
					terminal.AddScrollingLine("potential in you.");
				}
				break;
			case 60:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("OFN!");
				}
				else
				{
					terminal.AddScrollingLine("On the orders of Director Hoover, you are hereby relieved", 0.9);
				}
				break;
			case 61:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("From now on, the automated system will take over my post", 0.9);
				}
				else
				{
					terminal.AddScrollingLine("of your duties.");
				}
				break;
			case 62:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("and give you new assignments.");
				}
				else
				{
					terminal.AddScrollingLine("Your security clearance will be revoked in a few seconds.", 2.0);
				}
				break;
			case 63:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("For the free nations of the world!", 2.0);
				}
				else
				{
					bgm_player.Stop();
					bgm_player.Stream = GD.Load<AudioStream>("res://Resources/Sound/startup.wav");
					bgm_player.Play();
					tween.InterpolateProperty(terminal, "modulate:a", 1, 0, 0.5f, tt, et, 0);
					tween.InterpolateProperty(board, "modulate:a", 1, 0, 0.5f, tt, et, 0.75f);
					tween.InterpolateProperty(display.targets_left, "modulate:a", 1, 0, 0.5f, tt, et, 0.75f);
					tween.InterpolateProperty(display.header, "modulate:a", 1, 0, 0.5f, tt, et, 1.5f);
					tween.InterpolateProperty(display, "modulate:a", 1, 0, 0.5f, tt, et, 2.25f);
					tween.Start();
					ending_scene.StartEnding(1, 4.5);
				}
				break;
			case 64:
				terminal.AddScrollingLine("Speaking of which, seems like the system has a message for", 1.5);
				break;
			case 65:
				terminal.AddScrollingLine("you.");
				break;
			case 66:
				terminal.AddScrollingLine("I'll let it do the talking from now, make us proud, kid.", 1.5);
				break;
			case 67:
				terminal.AddScrollingLine("THIS IS AN AUTOMATED MESSAGE", 5.0);
				break;
			case 68:
				terminal.AddScrollingLine("SOUTH AMERICA HEADQUATERS REPORTED A CIVIL WAR HAS BROKEN", 2.0);
				break;
			case 69:
				terminal.AddScrollingLine("OUT IN ARGENTINA");
				break;
			case 70:
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT ORDERS", 2.0);
				break;
			case 71:
				terminal.AddScrollingLine("ORDER 44: REMOVE POCKETS OF RESISTANCE NEAR BUENOS AIRES", 2.0);
				break;
			case 72:
				terminal.AddScrollingLine("YOU WILL BE CONNECTED TO THE B.A. OPERATIONS SYSTEM", 2.0);
				break;
			case 73:
				timer1.WaitTime = 4f;
				timer1.Start();
				break;
			case 84:
				display.header_subtitle.Text = "Buenos Aires Operations";
				terminal.ClearTerminal();

				SetupLevel(2);

				terminal.AddStaticLine("HOSTILE TARGETS DETECTED IN THIS REGION");
				terminal.AddStaticLine("ASSIGNMENT: ELIMINATES ALL TARGETS");
				terminal.SetAllowInput(true);
				terminal.SetStoryInput(false);

				board.Visible = true;
				display.targets_left.Visible = true;
				break;
			case 85:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);

				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("ALL TARGETS ELIMINATED", 2);
				}
				else
				{
					terminal.AddScrollingLine("WARNING: ARSENAL DEPLETED", 2);
				}
				break;
			case 86:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("MISSION CONSIDERED SUCCESS", 2);
				}
				else
				{
					terminal.AddScrollingLine("MISSION CONSIDERED FAILURE", 2);
				}
				break;
			case 87:
				terminal.AddScrollingLine("FIELD REPORTS SENT TO HQ FOR EVALUATION", 2);
				break;
			case 88:
				board.Visible = false;
				display.targets_left.Visible = false;
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT NEW ORDERS");
				break;
			case 89:
				terminal.AddScrollingLine("THIS IS AN AUTOMATED MESSAGE", 8);
				break;
			case 90:
				terminal.AddScrollingLine("AFRICA HEADQUATERS REPORTED THAT THE BOERS MINORITY HAS", 2);
				break;
			case 91:
				terminal.AddScrollingLine("RISEN UP AGAINIST THE SOUTH ARFICAN GOVERNMENT.");
				break;
			case 92:
				terminal.AddScrollingLine("IN ADDITION, THE GERMAN COLONIES UP NORTH HAS DECIDED TO", 2);
				break;
			case 93:
				terminal.AddScrollingLine("BACKED THE NEW BOER REPUBLIC IN THE UPRISING.");
				break;
			case 94:
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT ORDERS", 2);
				break;
			case 95:
				terminal.AddScrollingLine("ORDER 45: REMOVE POCKETS OF BOER REBELS NEAR PRETORIA", 2);
				break;
			case 96:
				terminal.AddScrollingLine("YOU WILL BE CONNECTED TO THE CAPE TOWN OPERATIONS SYSTEM.", 2);
				break;
			case 97:
				timer1.WaitTime = 4f;
				timer1.Start();
				break;
			case 108:
				display.header_subtitle.Text = "Cape Town Operations";
				terminal.ClearTerminal();
				board.Visible = true;
				display.targets_left.Visible = true;

				SetupLevel(3);

				terminal.AddStaticLine("OPERATOR: YOU HAVE (1) MESSAGE");
				terminal.AddStaticLine("YOU HAVE A NEW UNIT IN YOUR ARSENAL: AHBS");
				terminal.SetAllowInput(true);
				terminal.SetStoryInput(false);
				break;
			case 109:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("ALL TARGETS ELIMINATED", 2);
				}
				else
				{
					terminal.AddScrollingLine("WARNING: ARSENAL DEPLETED", 2);
				}
				break;
			case 110:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("MISSION CONSIDERED SUCCESS", 2);
				}
				else
				{
					terminal.AddScrollingLine("MISSION CONSIDERED FAILURE", 2);
				}
				break;
			case 111:
				terminal.AddScrollingLine("FIELD REPORTS SENT TO HQ FOR EVALUATION", 2);
				break;
			case 112:
				board.Visible = false;
				display.targets_left.Visible = false;
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT NEW ORDERS");
				break;
			case 113:
				terminal.AddScrollingLine("THIS IS AN AUTOMATED MESSAGE", 8);
				break;
			case 114:
				terminal.AddScrollingLine("ASIA HEADQUATERS REPORTED THAT A CIVIL WAR HAS BROKEN OUT", 2);
				break;
			case 115:
				terminal.AddScrollingLine("IN INDONESIA.");
				break;
			case 116:
				terminal.AddScrollingLine("THE REBELS, LEADED BY THE FORMER VICE-PRESIDENT, HAS", 2);
				break;
			case 117:
				terminal.AddScrollingLine("ALIGNED HIS FREE INDONESIAN FORCES WITH THE OFN.");
				break;
			case 118:
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT ORDERS", 2);
				break;
			case 119:
				terminal.AddScrollingLine("ORDER 46: REMOVE INDONESIAN GUERILLA FORCES OUTSIDE OF", 2);
				break;
			case 120:
				terminal.AddScrollingLine("JAKARTA");
				break;
			case 121:
				terminal.AddScrollingLine("YOU WILL BE CONNECTED TO THE JAKARTA OPERATIONS SYSTEM.", 2);
				break;
			case 122:
				timer1.WaitTime = 4f;
				timer1.Start();
				break;
			case 133:
				board.Visible = true;
				display.targets_left.Visible = true;
				display.header_subtitle.Text = "Jakarta Operations";
				terminal.ClearTerminal();
				terminal.SetAllowInput(true);
				terminal.SetStoryInput(false);

				SetupLevel(4);

				terminal.AddStaticLine("OPERATOR: YOU HAVE (1) MESSAGE");
				terminal.AddStaticLine("YOU HAVE A NEW UNIT IN YOUR ARSENAL: LB");
				break;
			case 134:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("ALL TARGETS ELIMINATED", 2);
				}
				else
				{
					terminal.AddScrollingLine("WARNING: ARSENAL DEPLETED", 2);
				}
				break;
			case 135:
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("MISSION CONSIDERED SUCCESS", 2);
				}
				else
				{
					terminal.AddScrollingLine("MISSION CONSIDERED FAILURE", 2);
				}
				break;
			case 136:
				terminal.AddScrollingLine("FIELD REPORTS SENT TO HQ FOR EVALUATION", 2);
				break;
			case 137:
				board.Visible = false;
				display.targets_left.Visible = false;
				terminal.AddScrollingLine("OPERATORS: STANDBY AND AWAIT NEW ORDERS");
				break;
			case 138:
				terminal.AddScrollingLine("Hello, operator, been a while.", 5);
				break;
			case 139:
				terminal.AddScrollingLine("There's been an emergency, which is why I'm here giving you", 0.9);
				break;
			case 140:
				terminal.AddScrollingLine("your new assignment personally.");
				break;
			case 141:
				terminal.AddScrollingLine("Despite our best efforts in saving the free world, a civil", 0.9);
				break;
			case 142:
				terminal.AddScrollingLine("war has broken out right on our home soil.");
				break;
			case 143:
				terminal.AddScrollingLine("President Yockey has given me a list of traitors among our", 0.9);
				break;
			case 144:
				terminal.AddScrollingLine("ranks that we need to take care of.");
				break;
			case 145:
				terminal.AddScrollingLine("Since you have been one of our most valuable operator so", 0.9);
				break;
			case 146:
				terminal.AddScrollingLine("far, I've decided to assign this very important target to");
				break;
			case 147:
				terminal.AddScrollingLine("you.");
				break;
			case 148:
				terminal.AddScrollingLine("No matter what, do not let the target escape, we are all", 0.9);
				break;
			case 149:
				terminal.AddScrollingLine("counting on you.");
				break;
			case 150:
				terminal.AddScrollingLine("For the free nations of the world.", 3.0);
				break;
			case 151:
				display.header_subtitle.Text = "Richmond Emergency Operations Center";
				terminal.SetAllowInput(true);
				terminal.SetStoryInput(false);

				SetupLevel(5);

				board.Visible = true;
				display.targets_left.Visible = true;
				break;
			case 152:
				terminal.SetAllowInput(false);
				terminal.SetStoryInput(true);
				if (story_var_1 == 1)
				{
					terminal.AddScrollingLine("So long.");
					ending_scene.StartEnding(2);
					timer1.WaitTime = 2f;
					timer1.Start();
				}
				else
				{
					if (!ending_4_possible)
					{
						//ending 3
						terminal.AddScrollingLine("Very clever, operator.", 2.0);
					}
					else
					{
						//ending 4
						terminal.AddScrollingLine("@#!@:<,.?/\\};*&#['%$", 2.0);
					}
				}
				break;
			case 153:
				if (!ending_4_possible)
				{
					terminal.AddScrollingLine("But you seems to be underestimating us.", 2.0);
				}
				else
				{
					terminal.AddScrollingLine("SECURITY PROTOCAL-113 BYPASSED.", 2.0);
				}
				break;
			case 154:
				if (!ending_4_possible)
				{
					terminal.AddScrollingLine("Goodbye, operator.", 2.0);
					ending_scene.StartEnding(3);
					timer1.WaitTime = 5.75f;
					timer1.Start();
				}
				else
				{
					terminal.AddScrollingLine("SECURITY PROTOCAL-114 BYPASSED.", 2.0);
				}
				break;
			case 155:
				terminal.AddScrollingLine("SECURITY PROTOCAL-115 BYPASSED.", 2.0);
				break;
			case 156:
				terminal.AddScrollingLine("ACCESS GRANTED TO USER @&!#******");
				break;
			case 157:
				bgm_player.Stop();
				terminal.AddScrollingLine("Hello, operator.", 1);
				break;
			case 158:
				terminal.AddScrollingLine("I'm Arya, a resistance leader.", 1);
				break;
			case 159:
				terminal.AddScrollingLine("We have seen your efforts in trying to save the lives of", 1);
				break;
			case 160:
				terminal.AddScrollingLine("innocent people around the world.");
				break;
			case 161:
				terminal.AddScrollingLine("You are a true hero of the people, thereby we are here to", 1);
				ending_scene.StartEnding(4, 17);
				break;
			case 162:
				terminal.AddScrollingLine("invite you into the resistance.");
				break;
			case 163:
				terminal.AddScrollingLine("The Coalition of Nations will overthrow the tyranny of the", 1);
				break;
			case 164:
				terminal.AddScrollingLine("OFN, and will bring liberty and justice for the people");
				break;
			case 165:
				terminal.AddScrollingLine("around the world!");
				break;
			case 166:
				terminal.AddScrollingLine("Come, operator, a brighter future awaits!", 1);
				break;
		}
		story_progress += 1;
	}

	private void _on_Timer_timeout()
	{
		if (story_progress >= 46 && story_progress <= 55)
		{
			terminal.AddStaticLine((story_progress - 45).ToString() + "0%");
			timer1.WaitTime = 0.2f;
			timer1.Start();
			story_progress += 1;
		}
		else if (story_progress == 56)
		{
			ProgressStory();
		}
		else if (story_progress >= 74 && story_progress <= 83)
		{
			terminal.AddStaticLine((story_progress - 73).ToString() + "0%");
			timer1.WaitTime = 0.2f;
			timer1.Start();
			story_progress += 1;
		}
		else if (story_progress == 84)
		{
			ProgressStory();
		}
		else if (story_progress >= 98 && story_progress <= 107)
		{
			terminal.AddStaticLine((story_progress - 97).ToString() + "0%");
			timer1.WaitTime = 0.2f;
			timer1.Start();
			story_progress += 1;
		}
		else if (story_progress == 108)
		{
			ProgressStory();
		}
		else if (story_progress >= 123 && story_progress <= 132)
		{
			terminal.AddStaticLine((story_progress - 122).ToString() + "0%");
			timer1.WaitTime = 0.2f;
			timer1.Start();
			story_progress += 1;
		}
		else if (story_progress == 133)
		{
			ProgressStory();
		}
		else if (story_progress == 153)
		{
			bgm_player.Stop();
		}
		else if (story_progress == 155)
		{
			bgm_player.Stop();
		}

	}

	private void _on_BGMPlayer_finished()
	{
		if (story_progress == 0)
		{
			bgm_player.Stream = GD.Load<AudioStream>("res://Resources/Sound/neon_bg.wav");
			bgm_player.VolumeDb = -16;
			bgm_player.Play();

			story_progress = 1;
			ProgressStory();
		}
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
				terminal.AddStaticLine("Note: you can use the command \"help\" to display a list of");
				terminal.AddStaticLine("commands");
				terminal.SetStoryInput(false);
				terminal.SetAllowInput(true);
				break;
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
			case 43:
			case 44:
			case 45:
			case 58:
			case 59:
			case 60:
			case 61:
			case 62:
			case 63:
				ProgressStory();
				break;
			case 64:
				if (story_var_1 == 1)
				{
					ProgressStory();
				}
				break;
			case 65:
			case 66:
			case 67:
			case 68:
			case 69:
			case 70:
			case 71:
			case 72:
			case 73:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 94:
			case 95:
			case 96:
			case 97:
			case 110:
			case 111:
			case 112:
			case 113:
			case 114:
			case 115:
			case 116:
			case 117:
			case 118:
			case 119:
			case 120:
			case 121:
			case 122:
			case 135:
			case 136:
			case 137:
			case 138:
			case 139:
			case 140:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 146:
			case 147:
			case 148:
			case 149:
			case 150:
			case 151:
				ProgressStory();
				break;
			case 153:
			case 154:
				if (story_var_1 == 2)
				{
					ProgressStory();
				}
				break;
			case 155:
			case 156:
			case 157:
			case 158:
			case 159:
			case 160:
			case 161:
			case 162:
			case 163:
			case 164:
			case 165:
			case 166:
				if (ending_4_possible)
				{
					ProgressStory();
				}
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
		Random rng = new Random();
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
			case 1:
				terminal.cpu.is_tutorial = false;
				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SB] = true;
				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 2;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 2;
				terminal.cpu.unit_amounts[CPU.UnitType.SB] = 1;

				board.SetupGridMap(4);
				display.targets_left.Text = "Target(s) Left: 3";
				int[] rand_array = new int[16];
				for (int i = 0; i < 16; i++)
				{
					rand_array[i] = i;
				}
				ShuffleArray<int>(rand_array);
				for (int i = 0;  i < 3; i++)
				{
					board.SetTargetLoc(1, new Tuple<int, int>(rand_array[i]/4,rand_array[i]%4));
				}
				break;
			case 2:
				terminal.cpu.is_tutorial = false;
				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SB] = true;
				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 3;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 4;
				terminal.cpu.unit_amounts[CPU.UnitType.SB] = 2;

				board.SetupGridMap(5);
				display.targets_left.Text = "Target(s) Left: 5";
				rand_array = new int[25];
				for (int i = 0; i < 25; i++)
				{
					rand_array[i] = i;
				}
				ShuffleArray<int>(rand_array);
				for (int i = 0;  i < 5; i++)
				{
					board.SetTargetLoc(1, new Tuple<int, int>(rand_array[i]/5,rand_array[i]%5));
				}
				break;
			case 3:
				terminal.cpu.is_tutorial = false;

				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SB] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.AHBS] = true;
				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 2;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 6;
				terminal.cpu.unit_amounts[CPU.UnitType.SB] = 4;
				terminal.cpu.unit_amounts[CPU.UnitType.AHBS] = 1;

				board.SetupGridMap(7);
				display.targets_left.Text = "Target(s) Left: 8";
				int[][] simulate = new int[7][];
				for (int i = 0;  i < 7; i++)
				{
					simulate[i] = new int[7];
				}
				for (int i = 0; i < 8; i++)
				{
					simulate[rng.Next(7)][rng.Next(7)] += 1;
				}
				for (int i = 0; i < 7; i++)
				{
					for (int j = 0; j < 7; j ++)
					{
						board.SetTargetLoc(simulate[i][j], new Tuple<int, int>(i, j));
					}
				}
				break;
			case 4:
				terminal.cpu.is_tutorial = false;
				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SB] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.AHBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.LB] = true;

				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 2;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 7;
				terminal.cpu.unit_amounts[CPU.UnitType.SB] = 4;
				terminal.cpu.unit_amounts[CPU.UnitType.AHBS] = 2;
				terminal.cpu.unit_amounts[CPU.UnitType.LB] = 3;

				board.SetupGridMap(9);
				simulate = new int[9][];
				for (int i = 0;  i < 9; i++)
				{
					simulate[i] = new int[9];
				}
				for (int i = 0; i < 12; i++)
				{
					simulate[rng.Next(9)][rng.Next(9)] += 1;
				}
				for (int i = 0; i < 9; i++)
				{
					for (int j = 0; j < 9; j ++)
					{
						board.SetTargetLoc(simulate[i][j], new Tuple<int, int>(i, j));
					}
				}

				display.targets_left.Text = "Target(s) Left: 12";
				break;
			case 5:
				terminal.cpu.is_tutorial = false;
				terminal.cpu.unit_unlocked[CPU.UnitType.HBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SP] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.SB] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.AHBS] = true;
				terminal.cpu.unit_unlocked[CPU.UnitType.LB] = true;

				terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 0;
				terminal.cpu.unit_amounts[CPU.UnitType.SP] = 0;
				terminal.cpu.unit_amounts[CPU.UnitType.SB] = 10;
				terminal.cpu.unit_amounts[CPU.UnitType.AHBS] = 0;
				terminal.cpu.unit_amounts[CPU.UnitType.LB] = 9;

				board.SetupGridMap(11);
				//Testing
				//board.SetTargetLoc(1, new Tuple<int, int>(1,1));
				board.SetTargetLoc(1, new Tuple<int, int>(rng.Next(1), rng.Next(1)));
				display.targets_left.Text = "Target(s) Left: 1";
				break;
		}
	}

	public void OnVictory()
	{
		switch(story_progress)
		{
			case 33:
				ProgressStory();
				break;
			case 57:
				story_var_1 = 1;
				ProgressStory();
				break;
			case 85:
				story_var_1 = 1;
				ending_4_possible = false;
				ProgressStory();
				break;
			case 109:
				story_var_1 = 1;
				ending_4_possible = false;
				ProgressStory();
				break;
			case 134:
				story_var_1 = 1;
				ending_4_possible = false;
				ProgressStory();
				break;
			case 152:
				story_var_1 = 1;
				ProgressStory();
				break;
		}
	}

	public void OnFailure()
	{
		switch(story_progress)
		{
			case 57:
				story_var_1 = 2;
				ProgressStory();
				break;
			case 85:
				story_var_1 = 2;
				ending_4_possible = display.board.GetTargetsLeft() == 5;
				ProgressStory();
				break;
			case 109:
				story_var_1 = 2;
				ending_4_possible = display.board.GetTargetsLeft() == 8;
				ProgressStory();
				break;
			case 134:
				story_var_1 = 2;
				ending_4_possible = display.board.GetTargetsLeft() == 12;
				ProgressStory();
				break;
			case 152:
				story_var_1 = 2;
				ProgressStory();
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
		if (story_progress >= 24)
		{
			board.Modulate = new Color(1,1,1,1);
			display.targets_left.Modulate = new Color(1,1,1,1);
		}
		ProgressStory();
		return false;
	}

	private void ShuffleArray<T>(T[] input)
	{
		int n = input.Length;
		while (n > 1) 
		{
			int k = new Random().Next(n--);
			T temp = input[n];
			input[n] = input[k];
			input[k] = temp;
		}
	}
}

