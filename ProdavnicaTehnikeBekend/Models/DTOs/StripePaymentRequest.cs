namespace ProdavnicaTehnikeBekend.Models.DTOs
{
    public class StripePaymentRequest
    {
        public List<Product> Products { get; set; } = new();

        public String? ProizvodId { get; set; }
        public string KorisnickoImeKupca { get; set; } = null!;
    }

    

}
