using Godot;
using System;

public class Terminal : Panel
{
	private const int max_line_nums = 24;
	private int line_taken = 0;
	private int char_per_sec = 15; 

	private bool is_animating = false;
	private bool allow_input = true;
	private bool story_special_input = false;

	private Label output;
	public LineEdit input;
	private Tween text_tween;
	public CPU cpu;

	[Signal] public delegate void FinishedAnimation();
	[Signal] public delegate void StoryInput(string input);

	public bool GetIsAnimating() { return is_animating; }
	public void SetAllowInput(bool x) { allow_input = x; }
	public bool GetAllowInput() { return allow_input; }
	public void SetStoryInput(bool x) { story_special_input = x; }
	public bool GetStoryInput() { return story_special_input; }

	public override void _Ready()
	{
		output = GetNode<Label>("Output");
		input = GetNode<LineEdit>("Input");
		text_tween = GetNode<Tween>("Output/Tween");
		cpu = GetNode<CPU>("CPU");
		cpu.terminal = this;

		output.VisibleCharacters = 0;
		output.Text = "";
		input.Text = "";
	}

	public void AddStaticLine(string new_line)
	{
		if (line_taken == 0)
		{
			output.Text = new_line;
			line_taken = 1;
		}
		else if (line_taken < max_line_nums)
		{
			output.Text += "\n" + new_line;
			line_taken += 1;
		}
		else
		{
			int index = 0;
			while (true)
			{
				if (output.Text[index] == '\n')
				{
					break;
				}
				index += 1;
			}
			output.Text = output.Text.Substring(index + 1);
			output.Text += "\n" + new_line;
		}
		output.VisibleCharacters = LengthWithoutSpace(output.Text);
	}

	public void AddScrollingLine(string new_line, double pause_before_load = 0)
	{
		if (line_taken == 0)
		{
			output.Text = new_line;
			line_taken = 1;
		}
		else if (line_taken < max_line_nums)
		{
			output.Text += "\n" + new_line;
			line_taken += 1;
		}
		else
		{
			int index = 0;
			while (true)
			{
				if (output.Text[index] == '\n')
				{
					break;
				}
				index += 1;
			}
			output.Text = output.Text.Substring(index + 1);
			output.VisibleCharacters = LengthWithoutSpace(output.Text);
			output.Text += "\n" + new_line;
		}
		double new_text_len = LengthWithoutSpace(new_line);
		Tween.TransitionType tr = Tween.TransitionType.Linear;
		Tween.EaseType et = Tween.EaseType.InOut;
		text_tween.InterpolateProperty(
			output, "visible_characters", output.VisibleCharacters, LengthWithoutSpace(output.Text), (float) new_text_len/char_per_sec,
			tr, et, (float) pause_before_load
		);
		text_tween.Start();
		is_animating = true;
	}

	private void _on_Input_text_entered(String new_text)
	{
		if (is_animating || !allow_input)
		{
			//Does not allow user to input
			return;
		}
		if (new_text.Length == 0)
		{
			//No input
			return;
		}
		input.Text = "";
		AddStaticLine(">" + new_text);
		if (!story_special_input)
		{
			cpu.CommandInputed(new_text);
		}
		else
		{
			EmitSignal(nameof(StoryInput), new_text);
		}
	}

	private void _on_Tween_tween_all_completed()
	{
		is_animating = false;
		text_tween.StopAll();
		EmitSignal(nameof(FinishedAnimation));
	}

	public int LengthWithoutSpace(string str)
	{
		int output = 0;
		for (int i = 0; i < str.Length; i++)
		{
			output += str[i] == ' ' || str[i] == '\n' ? 0 : 1;
		}
		return output;
	}

	public void ClearTerminal()
	{
		output.Text = "";
		output.VisibleCharacters = 0;
		line_taken = 0;
	}

}








