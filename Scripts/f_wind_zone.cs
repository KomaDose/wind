using Godot;
using System;
using System.Collections.Generic;

public partial class f_wind_zone : Area2D
{
	big_fan bf;
	Vector2 wind;
	float windPower = 800;

	List<RigidBody2D> bodies = new List<RigidBody2D>();
	RigidBody2D[] daBodies;

	public override void _Ready() {
		bf = GetNode<big_fan>(GetPathTo(GetParent()));
	}

	public override void _PhysicsProcess(double delta) {
		wind = bf.facingDirection * windPower;

		daBodies = bodies.ToArray();

		foreach (var Index in daBodies) {
			Index.ApplyCentralForce(wind);
		}
	}

	void _on_body_entered(RigidBody2D body) {
		bodies.Add(body);
	}

	void _on_body_exited(RigidBody2D body) {
		bodies.Remove(body);
	}
}
