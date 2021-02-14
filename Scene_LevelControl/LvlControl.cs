using Godot;
using System;

public class LvlControl : Node
{
	Terminal terminal;
	Display display;

	Tween tween;
	Timer timer1;
	Timer timer2;

	private int story_progress = 0;

	public override void _Ready()
	{
		terminal = GetNode<Terminal>("../Terminal");
		display = GetNode<Display>("../Display");
		tween = GetNode<Tween>("Tween");
		timer1 = GetNode<Timer>("Timer");
		timer2 = GetNode<Timer>("Timer2");

		SetupInitial();
	}

	private void SetupInitial()
	{
		terminal.Modulate = new Color(1,1,1,0);
		display.Modulate = new Color(1,1,1,0);
	}

	private void _on_Tween_tween_all_completed()
	{
		// Replace with function body.
	}

	private void _on_Timer_timeout()
	{
		// Replace with function body.
	}
}





