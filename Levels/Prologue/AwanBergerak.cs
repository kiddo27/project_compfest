using Godot;
using System;

public partial class AwanBergerak : AnimatedSprite2D
{
	// Menggunakan [Export] supaya kecepatannya bisa diubah-ubah lewat Inspector tiap awan
	[Export] public float Kecepatan = 20.0f; 
	
	// Batas koordinat X saat awan sudah benar-benar keluar dari layar sebelah kanan
	// Sesuaikan angka ini dengan lebar resolusi game kamu (misal 1280 atau 1920)
	[Export] public float BatasKanan = 1132.451f;
	
	// Titik mulai kembali di sebelah kiri luar layar agar tidak muncul tiba-tiba
	[Export] public float BatasKiri = -1165.157f;

	public override void _Ready()
	{
		// Memastikan animasi "bernafas" awan kamu otomatis jalan pas game dimulai
		Play();
	}

	public override void _Process(double delta)
	{
		// Geser posisi X awan ke kanan secara konstan setiap frame
		Position += new Vector2(Kecepatan * (float)delta, 0);

		// Jika awan sudah melewati batas kanan layar
		if (Position.X > BatasKanan)
		{
			// Pindahkan posisinya kembali ke ujung kiri luar layar
			Position = new Vector2(BatasKiri, Position.Y);
		}
	}
}
