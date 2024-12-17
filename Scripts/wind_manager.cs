using Godot;
using System;

public partial class wind_manager : Node2D
{
	public Vector2 wind;
	public bool inWindZone;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(wind);
		//GD.Print(inWindZone);
	}
}
