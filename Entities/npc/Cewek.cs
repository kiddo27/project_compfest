using Godot;
using System;

public partial class Cewek : CharacterBody2D
{
	private bool _isPlayerInZone = false;
	private Node2D _playerNode; // Menyimpan referensi ke node pemain

	public override void _Ready()
	{
		Area2D interactionArea = GetNode<Area2D>("Area2D");

		// Kita biarkan logikanya menggunakan tipe data 'Node' biasa agar lebih fleksibel
		interactionArea.BodyEntered += OnBodyEntered;
		interactionArea.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		if (_isPlayerInZone && Input.IsActionJustPressed("interact"))
		{
			MulaiDialog();
		}
	}

	// Perhatikan tipe parameternya: Node, bukan Node2D
	private void OnBodyEntered(Node body)
	{
		GD.Print("Ada objek menyentuh zona NPC bernama: ", body.Name); // Pelacak utama

		if (body.Name == "mc") 
		{
			_isPlayerInZone = true;
			_playerNode = (Node2D)body;
			GD.Print("MC masuk area! Tekan tombol interaksi.");
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body.Name == "mc")
		{
			_isPlayerInZone = false;
			_playerNode = null;
			GD.Print("MC pergi.");
		}
	}

	private void MulaiDialog()
	{
		GD.Print("NPC: Halo! Semangat bikin gamenya!");
	}
}
