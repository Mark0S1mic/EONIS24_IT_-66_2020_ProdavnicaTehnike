namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class KupacCreateDto
    {

        public string KorisnickoImeKupca { get; set; }
        public string SifraKupca { get; set; }
        public DateOnly? DatumRodjenjaKupca { get; set; }
        public string AdresaKupca { get; set; }
        public string GradKupca { get; set; }
        public string KontaktKupca { get; set; }
        public string BrojTelefonaKupca { get; set; }

        



    }
}
