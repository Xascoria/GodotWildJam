using Godot;
using System;

public class Testing1 : Node2D
{

	public override void _Ready()
	{
		string test_str = "toggle s";
		string[] splitted = test_str.Split(" ");
		foreach (string str in splitted)
		{
			GD.Print(str);
		}
	}

	/*
	Guinea coup d'etat
	Argentinian civil war
	Indonesian uprising
	South African War - Boer Uprising
	American Civil War
	*/
}
