namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class PorudzbinaCreateDto
    {

        public DateOnly? DatumPorudzbine { get; set; }
        public string AdresaPorudzbine { get; set; }
        public DateOnly? DatumPlacanja { get; set; }
       

    }
}
