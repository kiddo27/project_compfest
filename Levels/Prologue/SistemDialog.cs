using Godot;
using System;

public partial class SistemDialog : Node
{
	// --- SLOT UI DIALOG ---
	[Export] public Control KotakCowok;
	[Export] public Control KotakCewek;
	[Export] public Label LabelCowok;
	[Export] public Label LabelCewek;

	// --- SLOT ANIMASI MULUT ---
	[Export] public TextureRect MulutCowok;
	[Export] public TextureRect MulutCewek;
	
	// Array Gambar Mulut (Wajib: Index 0 = Mingkem, Index 1 dst = Mangap)
	[Export] public Texture2D[] FrameMulutCowok; 
	[Export] public Texture2D[] FrameMulutCewek; 

	// INI KUNCI GANTI-GANTIANNYA! (Teks buatanmu)
	private int[] giliranBicara = { 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1 }; 
	
	// INI TEKS OBROLANNYA (Teks buatanmu)
	private string[] daftarDialog = {
		"Heuhhhh, Cape banget kuliah hari ini",
		"Iya ya, mana tadi materi RL susah banget lagi",
		"Bener banget lagi 🙁",
		"Untung kampus kita punya danau secantik ini", 
		"jadi bisa refreshing kesini tiap lagi stres wkwk",
		"haha, iya gapernah bosen nongkrong disini", 
		"walaupun hampir tiap hari kesini wkwk",
		"wkwk iyakann",
		"Eh udah mau maghrib nih, pulang yok",
		"Yahhh masih pengen disinii:((",
		"Yeee udah ayo pulang", 
		"nanti aku kena marah papamu lagi kalau pulang kemaleman",
        "wkwk iyadehh, ayokk"
	};

	private int indexDialog = 0;
	private int karakterDitampilkan = 0;
	private float timerKetik = 0.0f;
	private float kecepatanKetik = 0.04f;
	private bool sedangNgetik = false;
	
	private Label teksAktif;
	private int pembicaraAktif = 0; 

	// Variabel Timer Animasi Mulut
	private float timerMulut = 0.0f;
	private float kecepatanMulut = 0.25f; // Makin kecil makin cepet ngomongnya
	private int frameMulutSaatIni = 0;

	public override void _Ready()
	{
		KotakCowok.Visible = false;
		KotakCewek.Visible = false;
		MulaiDialog(0);
	}

	public override void _Input(InputEvent @event)
	{
		// MENDETEKSI TOMBOL 'J' DI KEYBOARD (Klik Mouse sudah dimatikan)
		if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.J && !keyEvent.Echo)
		{
			if (sedangNgetik)
			{
				// Skip ketikan biar teks langsung beres
				teksAktif.VisibleCharacters = daftarDialog[indexDialog].Length;
				sedangNgetik = false;
				MingkemkanKarakter(); // Kalo di-skip, langsung tutup mulut
			}
			else
			{
				// Lanjut ke obrolan berikutnya
				indexDialog++;
				if (indexDialog < daftarDialog.Length)
				{
					MulaiDialog(indexDialog);
				}
				else
				{
					KotakCowok.Visible = false;
					KotakCewek.Visible = false;
					MingkemkanKarakter();
					GD.Print("Obrolan Tamat!");
				}
			}
		}
	}

	private void MulaiDialog(int index)
	{
		pembicaraAktif = giliranBicara[index];
		MingkemkanKarakter(); // Pastikan mulai dari posisi mulut tertutup

		if (pembicaraAktif == 0) // Giliran Cowok
		{
			KotakCowok.Visible = true;
			KotakCewek.Visible = false;
			teksAktif = LabelCowok;
		}
		else // Giliran Cewek
		{
			KotakCewek.Visible = true;
			KotakCowok.Visible = false;
			teksAktif = LabelCewek;
		}

		teksAktif.Text = daftarDialog[index];
		teksAktif.VisibleCharacters = 0;
		karakterDitampilkan = 0;
		sedangNgetik = true;
	}

	public override void _Process(double delta)
	{
		if (sedangNgetik)
		{
			// 1. Proses Ngetik Teks
			timerKetik += (float)delta;
			if (timerKetik >= kecepatanKetik)
			{
				timerKetik = 0.0f;
				karakterDitampilkan++;
				teksAktif.VisibleCharacters = karakterDitampilkan;

				if (karakterDitampilkan >= teksAktif.Text.Length)
				{
					sedangNgetik = false;
					MingkemkanKarakter(); // Teks beres = tutup mulut
				}
			}

			// 2. Proses Mulut Komat-Kamit
			timerMulut += (float)delta;
			if (timerMulut >= kecepatanMulut)
			{
				timerMulut = 0.0f;
				frameMulutSaatIni++;

				// Cek siapa yang lagi ngomong, lalu gerakkan mulutnya
				if (pembicaraAktif == 0 && FrameMulutCowok != null && FrameMulutCowok.Length > 0)
				{
					if (frameMulutSaatIni >= FrameMulutCowok.Length) frameMulutSaatIni = 1; // Kembali ke mangap (lewati mingkem)
					if (MulutCowok != null) MulutCowok.Texture = FrameMulutCowok[frameMulutSaatIni];
				}
				else if (pembicaraAktif == 1 && FrameMulutCewek != null && FrameMulutCewek.Length > 0)
				{
					if (frameMulutSaatIni >= FrameMulutCewek.Length) frameMulutSaatIni = 1;
					if (MulutCewek != null) MulutCewek.Texture = FrameMulutCewek[frameMulutSaatIni];
				}
			}
		}
	}

	private void MingkemkanKarakter()
	{
		// Kembalikan ke gambar Index 0 (Mingkem)
		if (MulutCowok != null && FrameMulutCowok != null && FrameMulutCowok.Length > 0)
		{
			MulutCowok.Texture = FrameMulutCowok[0];
		}
		if (MulutCewek != null && FrameMulutCewek != null && FrameMulutCewek.Length > 0)
		{
			MulutCewek.Texture = FrameMulutCewek[0];
		}
	}
}
