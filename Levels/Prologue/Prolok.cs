using Godot;
using System;

public partial class Prolok : Control
{
	// Atur seberapa cepat frame berganti (dalam detik). Makin kecil makin ngebut.
	private float kecepatanAnimasi = 0.85f; 
	private float timer = 0.0f;
	private int frameSaatIni = 0;

	// Array (kotak penyimpanan) untuk menampung 4 gambar kita
	private Texture2D[] kumpulanGambar = new Texture2D[4];
	
	// Referensi ke node TextureRect kita
	private TextureRect layarBackground;

	public override void _Ready()
	{
		// 1. Ambil node "Bg1" dari editor
		layarBackground = GetNode<TextureRect>("Bg1");

		// 2. Muat (load) keempat gambar dari folder ke dalam memori
		// Pastikan huruf besar/kecil di path ini persis dengan yang ada di panel FileSystem kamu!
		kumpulanGambar[0] = GD.Load<Texture2D>("res://Levels/Prologue/Bg/BG 1.png");
		kumpulanGambar[1] = GD.Load<Texture2D>("res://Levels/Prologue/Bg/BG 2.png");
		kumpulanGambar[2] = GD.Load<Texture2D>("res://Levels/Prologue/Bg/BG 3.png");
		kumpulanGambar[3] = GD.Load<Texture2D>("res://Levels/Prologue/Bg/BG 4.png");
		
		// 3. Pasang gambar pertama saat game baru mulai
		layarBackground.Texture = kumpulanGambar[0];
	}

	public override void _Process(double delta)
	{
		// Hitung waktu yang berlalu
		timer += (float)delta;

		// Kalau waktunya sudah mencapai batas kecepatan yang kita tentukan
		if (timer >= kecepatanAnimasi)
		{
			// Reset stopwatch ke 0
			timer = 0.0f;
			
			// Maju ke urutan gambar berikutnya
			frameSaatIni++;

			// Kalau sudah melewati gambar ke-4 (indeks 3), kembali ke gambar ke-1 (indeks 0)
			if (frameSaatIni >= kumpulanGambar.Length)
			{
				frameSaatIni = 0;
			}

			// Ganti tekstur (gambar) di layar dengan frame yang baru!
			layarBackground.Texture = kumpulanGambar[frameSaatIni];
		}
	}
}
