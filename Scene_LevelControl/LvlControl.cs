using Godot;
using System;

public class LvlControl : Node
{
	Terminal terminal;
	Display display;

	private int story_progress = -1;

	public override void _Ready()
	{
		terminal = GetNode<Terminal>("../Terminal");
		display = GetNode<Display>("../Display");
	}

	private void SetupInitial()
	{

	}
}
