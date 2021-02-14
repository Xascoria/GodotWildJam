using Godot;
using System;

public class Main : Node2D
{
	Terminal terminal;
	CPU cpu;
	Display display;
	Board board;
	public override void _Ready()
	{
		terminal = GetNode<Terminal>("Terminal");
		cpu = terminal.cpu;
		display = GetNode<Display>("Display");
		board = display.board;
	
		cpu.Connect(nameof(CPU.ShowContent), this, nameof(CPU_ShowContent));
		cpu.Connect(nameof(CPU.ShowCoord), this, nameof(CPU_ShowCoord));
	}

	public void CPU_ShowContent()
	{
		board.ToggleBoard(false);
	}

	public void CPU_ShowCoord()
	{
		board.ToggleBoard(true);
	}
}
