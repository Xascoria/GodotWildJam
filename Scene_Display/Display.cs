using Godot;
using System;

public class Display : Panel
{
	public Board board;
	public Node2D header;
	public Label header_title;
	public Label header_subtitle;
	public Label targets_left;

	public override void _Ready()
	{
		board = GetNode<Board>("Board");
		header = GetNode<Node2D>("Header");
		header_title = GetNode<Label>("Header/Title");
		header_subtitle = GetNode<Label>("Header/Subtitle");
		targets_left = GetNode<Label>("TargetsLeft");
	}
}
