using Godot;
using System;

public class Tile : Panel
{
	public Label content_text;
	public Label coord_text;
	public override void _Ready()
	{
		content_text = GetNode<Label>("Content");
		coord_text = GetNode<Label>("Coord");
	}
}
