using Godot;
using System;

public partial class player : CharacterBody2D
{
	public float Speed = 100.0f;
	
	public float JumpVelocity = -200.0f;
	float coyoteTime = 0.1f;
	float coyoteTimeCounter;
	float jumpBufferTime = 0.1f;
	float jumpBufferTimeCounter;
	bool JumpReleased;

	public float gravity = 980;
	public float highGravity = 1380;

	public Vector2 velocity;

	public Vector2 facingDirection = Vector2.Right;
	public Vector2 aimDirection;
	bool AimLocked = false;
	Vector2 currAimDiretion;

	public bool windMode = true;
	public bool shootMode = false;

	WindZone wz;

	Node2D pivot;

	AnimatedSprite2D bodySprite;
	AnimatedSprite2D armSprite;
	bool armActive = false;
	bool bodyActive = false;

	[Export] GpuParticles2D hSuckPart;
	[Export] GpuParticles2D hBlowPart;
	[Export] GpuParticles2D uSuckPart;
	[Export] GpuParticles2D uBlowPart;
	[Export] GpuParticles2D dBlowPart;
	[Export] GpuParticles2D dSuckPart;

    public override void _Ready()
    {
		pivot = GetNode<Node2D>("Pivot");
		wz = GetNode<WindZone>("Pivot/Area2D");

		bodySprite = GetNode<AnimatedSprite2D>("Body Sprite");
		armSprite = GetNode<AnimatedSprite2D>("Arm Sprite");
    }

    public override void _PhysicsProcess(double delta) {
		velocity = Velocity;

		//Add the gravity.
		if (!IsOnFloor()){
			if (JumpReleased) {
				velocity.Y += highGravity * (float)delta;
			}
			else {
				velocity.Y += gravity * (float)delta;
			}
		}

		//Coyote Time
		if (IsOnFloor()) {
			//JumpReleased = false;
			coyoteTimeCounter = coyoteTime;
		}
		else {
			coyoteTimeCounter -= (float)delta;
		}

		//Jump Buffer
		if (Input.IsActionJustPressed("jump")) {
			jumpBufferTimeCounter = jumpBufferTime;
		}
		else {
			jumpBufferTimeCounter -= (float)delta;
		}
			
		// Handle Jump.
		if (jumpBufferTimeCounter > 0f && coyoteTimeCounter > 0f) {
			velocity.Y = JumpVelocity;
			coyoteTimeCounter = 0f;
			jumpBufferTimeCounter = 0f;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = (Input.GetVector("move_left", "move_right", "ui_up", "ui_down")).Normalized();

		velocity = new Vector2(direction.X * Speed, velocity.Y);

		//Body Sprite
		if (direction.X == 0) {
			if (armActive) {
				bodySprite.Play("Idle");
			}
			else {
				//sprite.Play("Idle");
				bodySprite.Play("Idle Move");
			}

			bodyActive = false;
		}
		else if (facingDirection != Vector2.Zero) {
			bodySprite.Play("Run");
			bodyActive = true;
		}

		//Arm Sprite
		if ((Input.IsActionPressed("blow") || Input.IsActionPressed("suck")) && windMode) {
			if (aimDirection == facingDirection) {
				armSprite.Play("Hori Wind Active");
			}
			else if (aimDirection == Vector2.Up) {
				armSprite.Play("Up Wind Active");
			}
			else if (aimDirection == Vector2.Down) {
				armSprite.Play("Down Wind Active");
			}

			armActive = true;
		}
		else if (Input.IsActionPressed("blow") && shootMode) {
			armSprite.Play("Shoot Active");
			armActive = true;
		}
		else {
			if (bodyActive) {
				armSprite.Play("Idle");
			}
			else{
				armSprite.Play("Idle Move");
			}
			
			armActive = false;
		}

		//Wind Particles
		if (Input.IsActionPressed("blow") && windMode) {
			if (aimDirection == facingDirection) {
				hBlowPart.Emitting = true;
				hSuckPart.Emitting = false;
				uBlowPart.Emitting = false;
				uSuckPart.Emitting = false;
				dBlowPart.Emitting = false;
				dSuckPart.Emitting = false;
			}
			else if (aimDirection == Vector2.Up) {
				hBlowPart.Emitting = false;
				hSuckPart.Emitting = false;
				uBlowPart.Emitting = true;
				uSuckPart.Emitting = false;
				dBlowPart.Emitting = false;
				dSuckPart.Emitting = false;
			}
			else if (aimDirection == Vector2.Down) {
				hBlowPart.Emitting = false;
				hSuckPart.Emitting = false;
				uBlowPart.Emitting = false;
				uSuckPart.Emitting = false;
				dBlowPart.Emitting = true;
				dSuckPart.Emitting = false;
			}
		}
		else if (Input.IsActionPressed("suck") && windMode) {
			if (aimDirection == facingDirection) {
				hBlowPart.Emitting = false;
				hSuckPart.Emitting = true;
				uBlowPart.Emitting = false;
				uSuckPart.Emitting = false;
				dBlowPart.Emitting = false;
				dSuckPart.Emitting = false;
			}
			else if (aimDirection == Vector2.Up) {
				hBlowPart.Emitting = false;
				hSuckPart.Emitting = false;
				uBlowPart.Emitting = false;
				uSuckPart.Emitting = true;
				dBlowPart.Emitting = false;
				dSuckPart.Emitting = false;
			}
			else if (aimDirection == Vector2.Down) {
				hBlowPart.Emitting = false;
				hSuckPart.Emitting = false;
				uBlowPart.Emitting = false;
				uSuckPart.Emitting = false;
				dBlowPart.Emitting = false;
				dSuckPart.Emitting = true;
			}
		}
		else {
			hBlowPart.Emitting = false;
			hSuckPart.Emitting = false;
			uBlowPart.Emitting = false;
			uSuckPart.Emitting = false;
			dBlowPart.Emitting = false;
			dSuckPart.Emitting = false;
		}

		//Flips and Rotations
		if (direction.X < 0 && !Input.IsActionPressed("blow") && !Input.IsActionPressed("suck")) {
			facingDirection = Vector2.Left;
			bodySprite.FlipH = true;
			armSprite.FlipH = true;
			//pivot.Rotation = Mathf.DegToRad(180);
			pivot.Scale = new Vector2(-1, 1);
		}
		else if (direction.X > 0 && !Input.IsActionPressed("blow") && !Input.IsActionPressed("suck")) {
			facingDirection = Vector2.Right;
			bodySprite.FlipH = false;
			armSprite.FlipH = false;
			//pivot.Rotation = Mathf.DegToRad(0);
			pivot.Scale = new Vector2(1, 1);
		}

		//Aim Direction
		if (Input.IsActionPressed("blow") || Input.IsActionPressed("suck")) {
			AimLocked = true;
		}
		else {
			AimLocked = false;
		}

		if (direction.Y < 0) { //Up
			if (!AimLocked) {
				aimDirection = Vector2.Up;
				currAimDiretion = aimDirection;
			}
			else if (AimLocked) {
				aimDirection = currAimDiretion;
			}
		}
		else if (direction.Y > 0 ) { //Down
			if (!AimLocked) {
				aimDirection = Vector2.Down;
				currAimDiretion = aimDirection;
			}
			else if (AimLocked) {
				aimDirection = currAimDiretion;
			}
		}
		else {
			if (!AimLocked) {
				aimDirection = facingDirection;
				currAimDiretion = aimDirection;
			}
			else if (AimLocked) {
				aimDirection = currAimDiretion;
			}
		}

		//Wind Manager
		wz.blowInput = Input.IsActionPressed("blow");
		wz.suckInput = Input.IsActionPressed("suck");
		wz.inWindMode = windMode;

		//Arm Mode
		/*
		if (Input.IsActionJustPressed("switch") && windMode) {
			windMode = false;
			shootMode = true;
		}
		else if (Input.IsActionJustPressed("switch") && shootMode) {
			windMode = true;
			shootMode = false;
		}
		*/

		Velocity = velocity;
		
		MoveAndSlide();
	}
}
