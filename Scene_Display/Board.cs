using Godot;
using System;

public class Board : Control
{
	private int width_height = 536;
	private Tile[][] tile_refs;
	private int[][] target_counts;
	private bool[][] revealed;
	private PackedScene tile_ref = GD.Load<PackedScene>("res://Scene_Display/Tile.tscn");
	public override void _Ready()
	{
		width_height = (int) RectSize.x;

		// Testing
		//SetupGridMap(11);
	}

	public int GetBoardSize() { return tile_refs.Length; }

	public void SetupGridMap(int dimension)
	{
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}

		int working_wh = (width_height / dimension) * dimension;
		double size_per_sqr = working_wh/ ((double) dimension);
		tile_refs = new Tile[dimension][];
		target_counts = new int[dimension][];
		revealed = new bool[dimension][];

		for (int i = 0; i < dimension; i++)
		{
			tile_refs[i] = new Tile[dimension];
			target_counts[i] = new int[dimension];
			revealed[i] = new bool[dimension];
			for (int j = 0; j < dimension; j++)
			{
				Tile new_tile = (Tile) tile_ref.Instance();
				AddChild(new_tile);

				//TODO: more font_size
				switch(dimension)
				{
					case 3:
						new_tile.SetFontSize(40);
						break;
					case 4:
						new_tile.SetFontSize(32);
						break;
					case 6:
						new_tile.SetFontSize(24);
						break;
				}

				new_tile.coord_text.Text = i + "," + j;
				new_tile.content_text.Text = "?";
				new_tile.coord_text.Visible = false;
				new_tile.RectSize = new Vector2((float)size_per_sqr, (float)size_per_sqr);
				tile_refs[i][j] = new_tile;
				new_tile.SetPosition(new Vector2((float)size_per_sqr * j, (float)size_per_sqr * i));

			}
		}
	}

	public void ToggleBoard(bool show_coord)
	{
		if (show_coord)
		{
			for (int i = 0; i < tile_refs.Length; i++)
			{
				for (int j = 0; j < tile_refs.Length; j++)
				{
					tile_refs[i][j].coord_text.Visible = true;
					tile_refs[i][j].content_text.Visible = false;
				}
			}
		}
		else
		{
			for (int i = 0; i < tile_refs.Length; i++)
			{
				for (int j = 0; j < tile_refs.Length; j++)
				{
					tile_refs[i][j].coord_text.Visible = false;
					tile_refs[i][j].content_text.Visible = true;
				}
			}
		}
		
	}

	public void SetTargetLoc(int target_num, Tuple<int, int> coord)
	{
		target_counts[coord.Item1][coord.Item2] = target_num;
	}

	private void UpdateBoard()
	{
		for (int i = 0; i < tile_refs.Length; i++)
		{
			for (int j = 0; j < tile_refs.Length; j++)
			{
				if (revealed[i][j])
				{
					tile_refs[i][j].content_text.Text = target_counts[i][j].ToString();
				}
				else
				{
					tile_refs[i][j].content_text.Text = "?";
				}
			}
		}
	}

	public int GetTargetsLeft()
	{
		int output = 0;
		for (int i = 0; i < target_counts.Length; i++)
		{
			for (int j = 0; j < target_counts.Length; j++)
			{
				output += target_counts[i][j];
			}
		}
		return output;
	}

	//Return bool: if the loc is revealed b4, int: target killed
	public Tuple<bool, int> KillOne(Tuple<int,int> coord)
	{
		int target_kill = 0;
		if (target_counts[coord.Item1][coord.Item2] > 0)
		{
			target_counts[coord.Item1][coord.Item2] -= 1;
			target_kill = 1;
		}
		UpdateBoard();
		return new Tuple<bool, int>(revealed[coord.Item1][coord.Item2],target_kill);
	}

	//Return bool: if the loc is revealed b4, int: target killed
	public Tuple<bool, int> KillAll(Tuple<int,int> coord)
	{
		int target_kill = 0;
		if (target_counts[coord.Item1][coord.Item2] > 0)
		{
			target_kill = target_counts[coord.Item1][coord.Item2];
			target_counts[coord.Item1][coord.Item2] = 0;
		}
		UpdateBoard();
		return new Tuple<bool, int>(revealed[coord.Item1][coord.Item2],target_kill);
	}

	public int RevealTarget(Tuple<int,int> coord)
	{
		revealed[coord.Item1][coord.Item2] = true;
		UpdateBoard();
		return target_counts[coord.Item1][coord.Item2];
	}

}
