using Godot;
using System;
using System.Collections.Generic;

//Deals with terminal input
public class CPU : Node
{
	private string invalid_com = "Invalid Command.";
	private string invalid_arg = "Invalid argument(s) for command.";
	private string not_enough_unit = "Not enough number of specified unit.";
	public Terminal terminal;
	public Display display;
	public enum UnitType 
	{
		HBS, SP, SB, BB
	}
	public Dictionary<UnitType, int> unit_amounts = new Dictionary<UnitType, int>
	{
		{UnitType.HBS, 0},
		{UnitType.SP, 0},
		{UnitType.SB, 0},
		{UnitType.BB, 0},
	};
	public Dictionary<UnitType, bool> unit_unlocked = new Dictionary<UnitType, bool>
	{
		{UnitType.HBS, false},
		{UnitType.SP, false},
		{UnitType.SB, false},
		{UnitType.BB, false},
	};
	public bool is_tutorial = false;

	[Signal] public delegate void ShowCoord();
	[Signal] public delegate void ShowContent();
	public override void _Ready()
	{
		terminal = GetNode<Terminal>("..");
		display = GetNode<Display>("../../Display");
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
				if (is_tutorial)
				{
					display.board.SetupGridMap(3);
					display.targets_left.Text = "Target(s) Left: 1";
					display.board.SetTargetLoc(1, new Tuple<int, int>(1,1));
					terminal.AddStaticLine("Simulation resetted.");
				}
				else
				{
					terminal.AddStaticLine("You cannot reset real field operations.");
				}
				return;
			case "help":
				if (splitted_coms.Length == 1)
				{
					terminal.AddStaticLine(invalid_arg);
					return;
				}
				else
				{
					if (splitted_coms[1].Equals("hbs"))
					{
						
					}
				}
				return;
			case "hbs":
				if (splitted_coms.Length == 1)
				{
					terminal.AddStaticLine(invalid_arg);
					return;
				}
				if (unit_unlocked[UnitType.HBS])
				{
					if (unit_amounts[UnitType.HBS] > 0)
					{
						unit_amounts[UnitType.HBS] -= 1;
					}
					else
					{
						terminal.AddStaticLine(not_enough_unit);
					}
				}
				else
				{
					terminal.AddStaticLine(invalid_com);
				}
				return;
		}

		terminal.AddScrollingLine(invalid_com);
	}
}
