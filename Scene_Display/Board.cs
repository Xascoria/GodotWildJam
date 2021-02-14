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

		//TODO: Testing
		//SetupGridMap(11);
	}

	public void SetupGridMap(int dimension)
	{
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

}
