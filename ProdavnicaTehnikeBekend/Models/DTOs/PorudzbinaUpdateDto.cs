namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class PorudzbinaUpdateDto
    {

        public int PorudzbinaId { get; set; }
        public DateOnly? DatumPorudzbine { get; set; }
        public string AdresaPorudzbine { get; set; }
        public DateOnly? DatumPlacanja { get; set; }
    

    }
}
