using Godot;
using System;
using System.Collections.Generic;

public class Ending : Node2D
{
	private Panel background;
	private Label title;
	private Label quote;
	private Label quoter;
	private Button restart_but;
	private Label ending_num_label;
	private Tween tween;
	private AudioStreamPlayer music_player;
	private Timer timer;

	private int ending_num = -1;
	private Dictionary<int, string> restart_txt = new Dictionary<int, string>()
	{
		{1, "Perhaps it's for the best?"},
	};

	public override void _Ready()
	{
		//On-ready
		/* #region */
		background = GetNode<Panel>("BG");
		title = GetNode<Label>("Title");
		quote = GetNode<Label>("Quote");
		quoter = GetNode<Label>("Quoter");
		restart_but = GetNode<Button>("Button");
		ending_num_label = GetNode<Label>("EndingNum");
		tween = GetNode<Tween>("Tween");
		music_player = GetNode<AudioStreamPlayer>("MusicPlayer");
		timer = GetNode<Timer>("Timer");
		/* #endregion */

		Setup();
		//StartEnding(1);
	}

	private void Setup()
	{
		background.Modulate = new Color(1,1,1,0);
		title.Modulate = new Color(1,1,1,0);
		quote.Modulate = new Color(1,1,1,0);
		quoter.Modulate = new Color(1,1,1,0);
		restart_but.Visible = false;
		ending_num_label.Visible = false;
		music_player.VolumeDb = -80;
	}


	public void StartEnding(int ending_num, double time_before_fade = 0)
	{
		this.ending_num = ending_num;
		switch(ending_num)
		{
			case 1:
			
			title.Text = "Nothing Much Happened";
			quote.Text = "Fortunes can turn into misfortunes, and misfortunes can turn into fortunes.\nThe changes are elusive and unpredictable.";
			quoter.Text = "- Chinese Fable";
			restart_but.Text = restart_txt[ending_num];
			music_player.Stream = GD.Load<AudioStream>("res://Resources/Sound/ending_1.wav");
			music_player.Play();

			break;

			case 2:

			break;
		}
		ending_num_label.Text = "Ending " + ending_num + " of 4";
		tween.InterpolateProperty(background, "modulate:a", 0, 1, 4.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, (float) time_before_fade);
		tween.InterpolateProperty(music_player, "volume_db", -80, 0, 6);
		tween.Start();
	}

	private int progress_num = 0;
	private void _on_Tween_tween_all_completed()
	{
		switch(progress_num)
		{
			case 0:
				tween.InterpolateProperty(title, "modulate:a", 0, 1, 2.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 1.5f);
				break;
			case 1:
				tween.InterpolateProperty(quote, "modulate:a", 0, 1, 3.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 1.5f);
				break;
			case 2:
				tween.InterpolateProperty(quoter, "modulate:a", 0, 1, 1.0f, Tween.TransitionType.Linear, Tween.EaseType.InOut, 4f);
				timer.WaitTime = 7.5f;
				timer.Start();
				break;
		}	
		if (progress_num < 3)
		{
			tween.Start();
			progress_num += 1;
		}	
	}

	private void _on_Timer_timeout()
	{
		switch(progress_num)
		{
			case 3:
				restart_but.Visible = true;
				ending_num_label.Visible = true;
				break;
		}
	}

	private void _on_Button_pressed()
	{
		GetTree().ReloadCurrentScene();
	}

	private void _on_Button_mouse_entered()
	{
		restart_but.Text = "RESTART";
	}


	private void _on_Button_mouse_exited()
	{
		restart_but.Text = restart_txt[ending_num];
	}

}








