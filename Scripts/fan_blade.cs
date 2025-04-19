using Godot;
using System;

public partial class fan_blade : RigidBody2D
{
	public float speed = 100;
	public Vector2 direction;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) {
		LinearVelocity = direction * speed;
	}
}
