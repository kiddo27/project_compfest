using Godot;
using System;

public partial class Kedips : TextureRect
{
	// Sekarang ada 3 slot gambar!
	[Export] public Texture2D FrameTerbuka;
	[Export] public Texture2D FrameSetengah; 
	[Export] public Texture2D FrameMerem;
	
	// Rentang waktu antar kedipan (dalam detik)
	[Export] public float JedaMin = 2.0f;
	[Export] public float JedaMax = 5.0f;
	
	// Kecepatan perpindahan antar frame saat ngedip (makin kecil makin cepet kedipnya)
	[Export] public float KecepatanKedip = 0.05f; 

	private float timer = 0.0f;
	private float waktuTungguSelanjutnya = 0.0f;
	
	// Status untuk melacak pergerakan mata (0 = Melek, 1 = Setengah, 2 = Merem, 3 = Setengah mau buka)
	private int statusKedip = 0; 
	private Random random = new Random();

	public override void _Ready()
	{
		this.Texture = FrameTerbuka;
		TentukanWaktuTunggu();
	}

	public override void _Process(double delta)
	{
		timer += (float)delta;

		// Logika urutan ngedip
		if (statusKedip == 0) // Lagi melek, nunggu waktu ngedip
		{
			if (timer >= waktuTungguSelanjutnya)
			{
				statusKedip = 1;
				timer = 0.0f;
				this.Texture = FrameSetengah;
			}
		}
		else if (statusKedip == 1) // Bergerak ke setengah merem
		{
			if (timer >= KecepatanKedip)
			{
				statusKedip = 2;
				timer = 0.0f;
				this.Texture = FrameMerem;
			}
		}
		else if (statusKedip == 2) // Tertutup rapat
		{
			if (timer >= KecepatanKedip)
			{
				statusKedip = 3;
				timer = 0.0f;
				this.Texture = FrameSetengah;
			}
		}
		else if (statusKedip == 3) // Buka mata setengah
		{
			if (timer >= KecepatanKedip)
			{
				statusKedip = 0;
				timer = 0.0f;
				this.Texture = FrameTerbuka;
				TentukanWaktuTunggu(); // Acak lagi waktu untuk kedipan berikutnya
			}
		}
	}

	private void TentukanWaktuTunggu()
	{
		waktuTungguSelanjutnya = (float)(random.NextDouble() * (JedaMax - JedaMin) + JedaMin);
	}
}
