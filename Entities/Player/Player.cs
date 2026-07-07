using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public float WalkSpeed = 150.0f;
	[Export] public float SprintSpeed = 250.0f;

	// SAKELAR BARU: Menentukan apakah MC boleh dikendalikan pemain
	public bool CanMove = true; 

	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private AnimatedSprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// 1. GRAVITASI (Tetap berjalan agar tidak melayang saat ngobrol di udara/turunan)
		if (!IsOnFloor())
		{
			velocity.Y += Gravity * (float)delta;
		}

		// 2. CEK SAKELAR PERGERAKAN (KUNCI KAKI)
		if (!CanMove)
		{
			// Jika tidak boleh bergerak (sedang ngobrol), paksa kecepatan X jadi 0
			velocity.X = Mathf.MoveToward(Velocity.X, 0, WalkSpeed);
			_sprite.Play("idle"); // Paksa putar animasi diam
			
			Velocity = velocity;
			MoveAndSlide();
			return; // 'return' akan menghentikan pembacaan kode di bawahnya
		}

		// ========================================================
		// KODE DI BAWAH INI HANYA JALAN JIKA CanMove == true
		// ========================================================

		bool isSprinting = Input.IsActionPressed("lari");
		float currentSpeed = isSprinting ? SprintSpeed : WalkSpeed;
		float direction = Input.GetAxis("gerak_kiri", "gerak_kanan");

		if (direction != 0)
		{
			velocity.X = direction * currentSpeed;

			if (direction > 0)
			{
				_sprite.FlipH = true; 
			}
			else if (direction < 0)
			{
				_sprite.FlipH = false; 
			}
			
			if (isSprinting)
			{
				_sprite.Play("run"); 
			}
			else
			{
				_sprite.Play("walk"); 
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			_sprite.Play("idle"); 
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
