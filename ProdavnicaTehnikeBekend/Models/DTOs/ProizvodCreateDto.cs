namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class ProizvodCreateDto
    {

     

        public string NazivProizvoda { get; set; } = null!;

        public decimal CenaProizvoda { get; set; }

        public string TipProizvoda { get; set; } = null!;

        public string OpisProizvoda { get; set; } = null!;

        public int Kolicina { get; set; }

        public string StatusProizvoda { get; set; } = null!;


    }
}
