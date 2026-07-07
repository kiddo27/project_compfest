using Godot;
using System;

public partial class DialogManager : Control // Ubah ke CanvasLayer jika node DialogUI kamu tipe CanvasLayer
{
	[Export] private RichTextLabel _dialogText;
	[Export] private Player _player; 
	
	private string[] _dialogLines;
	private int _currentLineIndex = 0;
	
	// Variabel untuk mengecek apakah dialog sedang berjalan
	public bool IsDialogActive = false; 

	public override void _Ready()
	{
		Visible = false; // Sembunyikan chatbox saat game dimulai
	}

	public override void _Process(double delta)
	{
		// Jika dialog sedang aktif dan tombol interaksi ('J') ditekan
		if (IsDialogActive && Input.IsActionJustPressed("interact"))
		{
			LanjutKeBarisBerikutnya();
		}
	}

	// Fungsi ini akan dipanggil oleh NPC saat MC mengajak ngobrol
	public void StartDialog(string[] lines)
	{
		if (IsDialogActive) return;

		// MATIKAN SAKELAR MC
		if (_player != null)
		{
			_player.CanMove = false; 
		}

		_dialogLines = lines;
		_currentLineIndex = 0;
		IsDialogActive = true;
		Visible = true;

		TampilkanTeks();
	}

	private void LanjutKeBarisBerikutnya()
	{
		_currentLineIndex++;
		
		// Cek apakah masih ada sisa naskah
		if (_currentLineIndex < _dialogLines.Length)
		{
			TampilkanTeks();
		}
		else
		{
			AkhiriDialog();
		}
	}

	private void TampilkanTeks()
	{
		_dialogText.Text = _dialogLines[_currentLineIndex];
	}

	private void AkhiriDialog()
	{
		IsDialogActive = false;
		Visible = false;
		
		// NYALAKAN LAGI SAKELAR MC
		if (_player != null)
		{
			_player.CanMove = true; 
		}
	}
}
