using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// [Export] membuat variabel ini muncul di panel Inspector Godot
	// Jadi kamu bisa mengubah kecepatannya tanpa buka script lagi
	[Export] public float WalkSpeed = 150.0f;
	[Export] public float RunSpeed = 250.0f;
	[Export] public float JumpVelocity = -350.0f;

	// Mengambil nilai gravitasi default dari Project Settings Godot
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;
	private bool _isAttacking = false; // Penanda agar karakter tidak lari saat menyerang

	public override void _Ready()
	{
		// Mengambil referensi node AnimatedSprite2D
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		// Mendaftarkan event/signal untuk mendeteksi kapan animasi selesai
		_animatedSprite.AnimationFinished += OnAnimationFinished;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// 1. Memicu Serangan
		// Syarat: Tombol ditekan, sedang tidak menyerang, dan karakter menginjak tanah
		if (Input.IsActionJustPressed("serang") && !_isAttacking)
		{
			_isAttacking = true;
			_animatedSprite.Play("attack");
		}

		// 2. Menerapkan Gravitasi (jika karakter melayang)
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		// 3. Memicu Lompat
		// Syarat: Tombol ditekan, menginjak tanah, dan sedang tidak menyerang
		if (Input.IsActionJustPressed("lompat") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// 4. Logika Pergerakan Kiri/Kanan
		// GetAxis otomatis menghasilkan -1 (kiri), 1 (kanan), atau 0 (diam)
		float direction = Input.GetAxis("gerak_kiri", "gerak_kanan");

		if (!_isAttacking) // Hanya bisa bergerak kalau tidak sedang menyerang
		{
			if (direction != 0)
			{
				// Cek apakah tombol lari ditahan
				float currentSpeed = Input.IsActionPressed("lari") ? RunSpeed : WalkSpeed;
				velocity.X = direction * currentSpeed;

				// Membalikkan arah gambar (Flip) sesuai arah gerak
				_animatedSprite.FlipH = direction < 0;
			}
			else
			{
				// Mengerem perlahan jika tombol dilepas
				velocity.X = Mathf.MoveToward(Velocity.X, 0, WalkSpeed);
			}
		}
		else
		{
			// Jika sedang menyerang, hentikan pergerakan X secara instan
			velocity.X = Mathf.MoveToward(Velocity.X, 0, WalkSpeed);
		}

		// Terapkan kecepatan ke sistem fisika Godot
		Velocity = velocity;
		MoveAndSlide();

		// 5. Update Animasi
		UpdateAnimation();
	}

	// Fungsi khusus untuk mengatur perpindahan animasi
	private void UpdateAnimation()
	{
		// Jika sedang menyerang, biarkan animasi attack selesai (jangan ditimpa animasi lain)
		if (_isAttacking) return;

		if (!IsOnFloor())
		{
			_animatedSprite.Play("jump");
		}
		else if (Velocity.X != 0)
		{
			// Jika ada kecepatan berjalan/lari, mainkan animasi run
			_animatedSprite.Play("run");
		}
		else
		{
			// Jika diam di lantai
			_animatedSprite.Play("idle");
		}
	}

	// Fungsi ini terpanggil otomatis saat animasi apapun mencapai frame terakhir
	private void OnAnimationFinished()
	{
		if (_animatedSprite.Animation == "attack")
		{
			// Bebaskan karakter setelah animasi serangan selesai
			_isAttacking = false; 
		}
	}
}
