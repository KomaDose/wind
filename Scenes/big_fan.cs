using Godot;
using System;

public partial class big_fan : AnimatableBody2D
{
	[Export] public Vector2 facingDirection;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Rotation = facingDirection.Angle();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
