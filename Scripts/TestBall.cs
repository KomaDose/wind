using Godot;
using System;

public partial class TestBall : RigidBody2D
{
	wind_manager wm;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		wm = GetNode<wind_manager>("/root/WindManager");
		//this.ApplyCentralForce(new Vector2(10050, 0));
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (wm.inWindZone)
		{
			//ApplyCentralForce(wm.wind);
		}
		//GD.Print(LinearVelocity);
	}
}
