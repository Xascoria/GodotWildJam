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
	Guiana coup d'etat (done)
	Argentinian civil war (done)
	Indonesian uprising 
	South African War - Boer Uprising
	American Civil War

	https://www.youtube.com/watch?v=uSk1j0H5gAg
	https://www.youtube.com/watch?v=1jPEZ9ytZvM
	https://www.youtube.com/watch?v=PLFVGwGQcB0
	https://www.youtube.com/watch?v=921z4LAHvak&t=49s

	In trying to defend everything he defended nothing. - Frederick the Great
	Everything in nature is lyrical in its ideal essence, tragic in its fate, and comic in its existence. - George Santayana
	Fortunes can turn into misfortunes, and misfortunes can turn into fortunes. 
	The changes are elusive and unpredictable. - Chinese Fable
	*/

	private void _on_Button_pressed()
	{
		GetTree().ChangeScene("res://Scene_Main/Main.tscn");
	}

}


