namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class KupacDto
    {


        public  string KorisnickoImeKupca { get; set; }
        public  string SifraKupca { get; set; }
        public DateOnly? DatumRodjenjaKupca { get; set; }
        public string AdresaKupca { get; set; }
        public string GradKupca { get; set; }
        public string KontaktKupca { get; set; }
        public string BrojTelefonaKupca { get; set; }
        public int? PorudzbinaId { get; set; }

        public virtual Porudzbina? Porudzbina { get; set; }

    }
}
