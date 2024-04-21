namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class ZaposleniDto
    {    

    public string KorisnickoImeZaposlenog { get; set; } = null!;

    public string SifraZaposlenog { get; set; } = null!;

    public string KontaktZaposlenog { get; set; } = null!;

    public int? PorudzbinaId { get; set; }

    public virtual Porudzbina? Porudzbina { get; set; } 


    }
}
