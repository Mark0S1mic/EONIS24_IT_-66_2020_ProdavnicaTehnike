namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class ZaposleniUpdateDto
    {

        public int ZaposleniId { get; set; }
        public string KorisnickoImeZaposlenog { get; set; }
        public string SifraZaposlenog { get; set; }
        public string KontaktZaposlenog { get; set; }
        public int? PorudzbinaId { get; set; }


    }
}
