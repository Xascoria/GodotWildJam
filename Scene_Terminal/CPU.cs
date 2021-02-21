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
		HBS, SP, SB, LB, AHBS
	}
	public Dictionary<UnitType, int> unit_amounts = new Dictionary<UnitType, int>
	{
		{UnitType.HBS, 0},
		{UnitType.SP, 0},
		{UnitType.SB, 0},
		{UnitType.LB, 0},
		{UnitType.AHBS, 0},
	};
	public Dictionary<UnitType, bool> unit_unlocked = new Dictionary<UnitType, bool>
	{
		{UnitType.HBS, false},
		{UnitType.SP, false},
		{UnitType.SB, false},
		{UnitType.LB, false},
		{UnitType.AHBS, false},
	};
	public bool is_tutorial = false;

	[Signal] public delegate void ShowCoord();
	[Signal] public delegate void ShowContent();
	[Signal] public delegate void Victory();
	[Signal] public delegate void Failure();
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
					terminal.cpu.unit_amounts[CPU.UnitType.HBS] = 1;
					terminal.cpu.unit_amounts[CPU.UnitType.SP] = 1;
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
					//TODO get help section here
					//terminal.AddStaticLine(invalid_arg);
					terminal.AddStaticLine("List of Commands:");
					terminal.AddStaticLine("list: Provide a list of units currently in your arsenal");
					terminal.AddStaticLine("[unit_name] [coord]: Execute a unit at that location");
					terminal.AddStaticLine("help [unit_name]: Gives information about that unit");
					terminal.AddStaticLine("toggle s/toggle c: Toggle between views of coords and");
					terminal.AddStaticLine("situation of the field");
					terminal.AddStaticLine("reset: Reset simulated field operation");
					return;
				}
				else
				{
					if (splitted_coms[1].Equals("hbs"))
					{
						terminal.AddStaticLine("HBS: Heartbeat Scanner");
						terminal.AddStaticLine("Scan for human heartbeats, reveals their locations.");
						terminal.AddStaticLine("Range: 3x3 tiles from the center tile.");
					}
					else if (splitted_coms[1].Equals("sp"))
					{
						terminal.AddStaticLine("SP: Sniper");
						terminal.AddStaticLine("Kill one target on the given coord.");
						terminal.AddStaticLine("Range: 1x1 tile.");
					}
					else if (splitted_coms[1].Equals("sb"))
					{
						terminal.AddStaticLine("SB: Small Bomb");
						terminal.AddStaticLine("Kills all target in its range.");
						terminal.AddStaticLine("Range: a cross from the center.");
						terminal.AddStaticLine("|_|x|_|");
						terminal.AddStaticLine("|x|X|x|");
						terminal.AddStaticLine("|_|x|_|");
					}
					else if (splitted_coms[1].Equals("ahbs"))
					{
						terminal.AddStaticLine("AHBS: Advanced Heartbeat Scanner");
						terminal.AddStaticLine("Scan for human heartbeats, reveals their locations.");
						terminal.AddStaticLine("Range: 5x5 tiles from the center tile.");
					}
					else if (splitted_coms[1].Equals("lb"))
					{
						terminal.AddStaticLine("LB: Large Bomb");
						terminal.AddStaticLine("Kills all target in its range.");
						terminal.AddStaticLine("Range: a 2x2 cross from the upper center.");
						terminal.AddStaticLine("|_|x|x|_|");
						terminal.AddStaticLine("|x|X|x|x|");
						terminal.AddStaticLine("|x|x|x|x|");
						terminal.AddStaticLine("|_|x|x|_|");
					}
					else
					{
						terminal.AddStaticLine(invalid_arg);
					}
				}
				return;
			case "list":
				terminal.AddStaticLine("You currently have:");
				foreach (UnitType type in unit_unlocked.Keys)
				{
					if (unit_unlocked[type])
					{
						int amount = unit_amounts[type];
						string unit_name = "";
						switch(type)
						{
							case UnitType.HBS:
								unit_name = "HBS";
								break;
							case UnitType.SP:
								unit_name = "SP";
								break;
							case UnitType.SB:
								unit_name = "SB";
								break;
							case UnitType.AHBS:
								unit_name = "AHBS";
								break;
							case UnitType.LB:
								unit_name = "LB";
								break;
						}
						terminal.AddStaticLine(amount + " unit(s) of : " + unit_name);
					}
				}
				return;
			case "hbs":
				ExecuteUnit(UnitType.HBS, lower_in);
				return;
			case "sp":
				ExecuteUnit(UnitType.SP, lower_in);
				return;
			case "sb":
				ExecuteUnit(UnitType.SB, lower_in);
				return;
			case "ahbs":
				ExecuteUnit(UnitType.AHBS, lower_in);
				return;
			case "lb":
				ExecuteUnit(UnitType.LB, lower_in);
				return;
		}

		terminal.AddStaticLine(invalid_com);
	}

	private void ExecuteUnit(UnitType unit_type, string line_input)
	{
		string[] splitted_coms = line_input.ToLower().Split(" ");
		if (splitted_coms.Length == 1)
		{
			terminal.AddStaticLine(invalid_arg);
			return;
		}
		if (!unit_unlocked[UnitType.HBS])
		{
			terminal.AddStaticLine(invalid_com);
			return;
		}
		if (unit_amounts[unit_type] == 0)
		{
			terminal.AddStaticLine(not_enough_unit);
			return;
		}
		Tuple<bool, int, int> coord_info = IsValidCoord(splitted_coms[1]);
		if (!coord_info.Item1)
		{
			terminal.AddStaticLine(invalid_arg);
			return;
		}
		switch (unit_type)
		{
			case UnitType.HBS:
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < 2; j++)
					{
						if (CoordInRange(coord_info.Item2 + i, coord_info.Item3 + j))
						{
							int x = coord_info.Item2 + i;
							int y = coord_info.Item3 + j;
							int target_num = display.board.RevealTarget(new Tuple<int, int>(x, y));
							if (target_num != 0 && target_num != -1)
							{
								terminal.AddStaticLine(target_num + " target(s) revealed at: " + x.ToString() + "," + y.ToString());
							}
						}
					}
				}
				break;
			case UnitType.SP:
				Tuple<bool, int> info = display.board.KillOne(new Tuple<int, int>(coord_info.Item2, coord_info.Item3));
				if (info.Item1 && info.Item2 != 0)
				{
					terminal.AddStaticLine("Sniper killed a target on " + coord_info.Item2.ToString() + "," + coord_info.Item3.ToString());
				}
				break;
			case UnitType.SB:
				int[][] int_trans = new int[][]{new int[]{0,0}, new int[]{0,1}, new int[]{0,-1}, new int[]{1,0}, new int[]{-1,0}};
				for (int i = 0; i < int_trans.Length; i++)
				{
					int x = coord_info.Item2 + int_trans[i][0];
					int y = coord_info.Item3 + int_trans[i][1];
					if (CoordInRange(x,y))
					{
						Tuple<bool, int> info_sb = display.board.KillAll(new Tuple<int,int>(x,y));
						if (info_sb.Item2 != 0)
						{
							if (info_sb.Item1)
							{
								terminal.AddStaticLine("A small bomb has killed " + info_sb.Item2 + " target(s) at " + x + "," + y);
							}
							else
							{
								terminal.AddStaticLine("A small bomb has killed " + info_sb.Item2 + " target(s) at an unknown location.");
							}
						}
					}
				}
				break;
			case UnitType.AHBS:
				for (int i = -2; i < 3; i++)
				{
					for (int j = -2; j < 3; j++)
					{
						if (CoordInRange(coord_info.Item2 + i, coord_info.Item3 + j))
						{
							int x = coord_info.Item2 + i;
							int y = coord_info.Item3 + j;
							int target_num = display.board.RevealTarget(new Tuple<int, int>(x, y));
							if (target_num != 0 && target_num != -1)
							{
								terminal.AddStaticLine(target_num + " target(s) revealed at: " + x.ToString() + "," + y.ToString());
							}
						}
					}
				}

				break;
			case UnitType.LB:
				for (int i = -1; i < 3; i++)
				{
					for (int j = -1; j < 3; j++)
					{
						if ( !( (i == 2 && j == 2)||(i == 2 && j == -1)||(i == -1 && j == 2)||(i == -1 && j == -1) ) )
						{
							int x = coord_info.Item2 + i;
							int y = coord_info.Item3 + j;
							if (CoordInRange(x,y))
							{
								Tuple<bool, int> info_sb = display.board.KillAll(new Tuple<int,int>(x,y));
								if (info_sb.Item1 && info_sb.Item2 != 0)
								{
									terminal.AddStaticLine("A large bomb has killed " + info_sb.Item2 + " target(s) at " + x + "," + y);
								}
								else if (info_sb.Item2 != 0)
								{
									terminal.AddStaticLine("A large bomb has killed " + info_sb.Item2 + " target(s) at an unknown location.");
								}
							}
						}
					}
				}
				break;
		}
		unit_amounts[unit_type] -= 1;
		display.targets_left.Text = "Target(s) Left: " + display.board.GetTargetsLeft();
		if (display.board.GetTargetsLeft() == 0)
		{
			EmitSignal(nameof(Victory));
		}
		else if (UnitsLeft() == 0)
		{
			EmitSignal(nameof(Failure));
		}
	}

	//return true, x, y if the coord is valid, else return false
	private Tuple<bool, int, int> IsValidCoord(string coord_input)
	{
		Tuple<bool, int, int> output = new Tuple<bool, int, int>(false, -1, -1);
		string[] coord_str = coord_input.Split(",");
		int[] final_coord = new int[2];
		//Not enough nums to be coord
		if (coord_str.Length < 2)
		{
			return output;
		}
		for (int i = 0; i < 2; i++)
		{
			try
			{
				final_coord[i] = int.Parse(coord_str[i]);
			}
			catch (Exception)
			{
				return output;
			}
			if (final_coord[i] < 0 || final_coord[i] >= display.board.GetBoardSize())
			{
				return output;
			}
		}
		return new Tuple<bool, int, int>(true, final_coord[0], final_coord[1]);
	}

	private int UnitsLeft()
	{
		int output = 0;
		foreach (int value in unit_amounts.Values)
		{
			output += value;
		}
		return output;
	}

	private bool CoordInRange(int x, int y)
	{
		return 
			x >= 0 && x < display.board.GetBoardSize() && 
			y >= 0 && y < display.board.GetBoardSize();
	}
}
