namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class PorudzbinaDto
    {

 

        public DateOnly DatumPorudzbine { get; set; }

        public string AdresaPorudzbine { get; set; } = null!;

        public DateOnly DatumPlacanja { get; set; }

        public virtual ICollection<Kupac> Kupacs { get; set; } = new List<Kupac>();

        public virtual ICollection<Zaposleni> Zaposlenis { get; set; } = new List<Zaposleni>();


    }
}
