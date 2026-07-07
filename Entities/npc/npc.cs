using Godot;
using System;

public partial class npc : CharacterBody2D
{
	private bool _isPlayerInZone = false;

	// Sekarang kita menyambungkannya langsung ke DialogManager
	[Export] private DialogManager _dialogManager;
	
	// Array ini akan menampung naskah panjang!
	[Export] public string[] NaskahObrolan; 

	public override void _Ready()
	{
		Area2D interactionArea = GetNode<Area2D>("Area2D");
		interactionArea.BodyEntered += OnBodyEntered;
		interactionArea.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		// Panggil dialog jika pemain di area, tekan tombol, dan dialog belum aktif
		if (_isPlayerInZone && Input.IsActionJustPressed("interact"))
		{
			if (_dialogManager != null && !_dialogManager.IsDialogActive)
			{
				_dialogManager.StartDialog(NaskahObrolan);
			}
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body.Name == "mc") 
		{
			_isPlayerInZone = true;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body.Name == "mc")
		{
			_isPlayerInZone = false;
		}
	}
}
