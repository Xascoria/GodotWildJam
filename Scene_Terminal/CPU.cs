using Godot;
using System;

//Deals with terminal input
public class CPU : Node
{
	private string invalid_com = "Invalid Command.";
	private string invalid_arg = "Invalid argument(s) for command.";
	public Terminal terminal;

	[Signal] public delegate void ShowCoord();
	[Signal] public delegate void ShowContent();
	public override void _Ready()
	{
	}

	public void CommandInputed(string input)
	{
		string lower_in = input.ToLower();
		string[] splitted_coms = lower_in.Split(" ");

		if (splitted_coms.Length == 0)
		{
			//No commands
			return;
		}

		switch(splitted_coms[0])
		{
			case "toggle":
				//No args
				if (splitted_coms.Length == 1)
				{
					terminal.AddScrollingLine(invalid_arg);
					return;
				}
				if (splitted_coms[1][0] == 'c')
				{
					EmitSignal(nameof(ShowCoord));
				}
				else if (splitted_coms[1][0] == 's')
				{
					EmitSignal(nameof(ShowContent));
				}
				else
				{
					terminal.AddScrollingLine(invalid_arg);
				}
				return;
			case "reset":
				return;
			case "help":

				return;
		}

		terminal.AddScrollingLine(invalid_com);
	}
}
