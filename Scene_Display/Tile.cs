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

		//Testing
		SetFontSize(20);
	}

	public void SetFontSize(int size)
	{
		((Font) content_text.Get("custom_fonts/font")).Set("size", size);
		((Font) coord_text.Get("custom_fonts/font")).Set("size", size);
	}
}
