namespace ProdavnicaTehnikeBekend.Models.DTOs.PorudzbinaProizvodDto
{
    public class PorudzbinaProizvodDto
    {

        public virtual Porudzbina Porudzbina { get; set; } = null!;

        public virtual Proizvod Proizvod { get; set; } = null!;

    }
}
