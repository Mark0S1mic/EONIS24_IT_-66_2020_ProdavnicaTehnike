namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class PorudzbinaDto
    {

 

        public DateOnly DatumPorudzbine { get; set; }

        public string AdresaPorudzbine { get; set; } = null!;

        public DateOnly DatumPlacanja { get; set; }

        public int KupacId { get; set; }

        public virtual Kupac Kupac { get; set; }
        //  public virtual ICollection<Kupac> Kupacs { get; set; } = new List<Kupac>();
        public virtual ICollection<Zaposleni> Zaposlenis { get; set; } = new List<Zaposleni>(); // Dodajte ovu liniju
        public virtual ICollection<PorudzbinaProizvod> PorudzbinaProizvods { get; set; } = new List<PorudzbinaProizvod>();


    }
}
