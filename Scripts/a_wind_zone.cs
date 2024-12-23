using Godot;
using System;
using System.Collections.Generic;

public partial class a_wind_zone : Area2D
{
	player player; 
	public Vector2 wind;
	float windPower = 800; //200 Default

	public bool blowInput;
	public bool suckInput;
	public bool inWindMode;

	[Export] CollisionShape2D hWindZone;
	[Export] CollisionShape2D uWindZone;
	[Export] CollisionShape2D dWindZone;


	List<RigidBody2D> bodies = new List<RigidBody2D>();
	RigidBody2D[] daBodies;

	public override void _Ready() {
		player = GetNode<player>(GetPathTo(GetParent().GetParent()));
	}

	public override void _PhysicsProcess(double delta) {
		wind = player.aimDirection * windPower;

		if (player.aimDirection == Vector2.Down) { //Down
			hWindZone.Disabled = true;
			uWindZone.Disabled = true;
			dWindZone.Disabled = false;
		}
		else if (player.aimDirection == Vector2.Up) { //Up
			hWindZone.Disabled = true;
			uWindZone.Disabled = false;
			dWindZone.Disabled = true;
		}
		else {
			hWindZone.Disabled = false;
			uWindZone.Disabled = true;
			dWindZone.Disabled = true;
		}
		
		daBodies = bodies.ToArray();

		if (blowInput && inWindMode) {
			foreach (var Index in daBodies) {
				Index.ApplyCentralForce(wind);
			}
		}
		else if (suckInput && inWindMode) {
			foreach (var Index in daBodies) {
				Index.ApplyCentralForce(-wind);
			}
		}
	}

	void _on_body_entered(RigidBody2D body) {
		bodies.Add(body);
	}

	void _on_body_exited(RigidBody2D body) {
		bodies.Remove(body);
	}

}
