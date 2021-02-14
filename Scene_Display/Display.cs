using Godot;
using System;

public class Display : Panel
{
	public Board board;
	public override void _Ready()
	{
		board = GetNode<Board>("Board");
	}
}
