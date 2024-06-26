using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Repositories;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;
using System.Threading.Tasks;
using System.Xml;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IPorudzbinaRepository _porudzbinaRepository;
        private IKupacRepository _kupacRepository;
        private IProizvodRepository _proizvodRepository;
        private IPorudzbinaProizvodRepository _porudzbinaProizvodRepository;

        public PaymentsController(IConfiguration configuration, IPorudzbinaRepository porudzbinaRepository, IKupacRepository kupacRepository
            , IProizvodRepository proizvodRepository, IPorudzbinaProizvodRepository porudzbinaProizvodRepository)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            _porudzbinaRepository = porudzbinaRepository;
            _kupacRepository = kupacRepository;
            _proizvodRepository = proizvodRepository;
            _porudzbinaProizvodRepository = porudzbinaProizvodRepository;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] StripePaymentRequest request)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var product in request.Products)
            {
                var proizvod = await _proizvodRepository.GetProizvodByNaziv(product.Name);
                if (proizvod == null)
                {
                    return BadRequest($"Proizvod sa nazivom '{product.Name}' nije pronađen.");
                }

                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(product.Amount * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                            Metadata = new Dictionary<string, string>
                    {
                        { "ProizvodId", proizvod.ProizvodId.ToString() }, // Dodaj ProizvodId u Metadata
                        { "korisnickoImeKupca", request.KorisnickoImeKupca }
                    }
                        },
                    },
                    Quantity = product.Quantity,
                });
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = _configuration["Stripe:SuccessUrl"],
                CancelUrl = _configuration["Stripe:CancelUrl"],
                Metadata = new Dictionary<string, string>
        {
            { "korisnickoImeKupca", request.KorisnickoImeKupca }
        }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);


            // await FulfillOrder(session);

            return Ok(new { sessionId = session.Id });
        }










        [HttpPost("Webhook")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _configuration["Stripe:WebhookSecret"]);

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                // Fulfill the order
                await FulfillOrder(session);
            }

            return Ok();
        }







        private async Task FulfillOrder(Session session)
        {
            var options = new SessionGetOptions { Expand = new List<string> { "line_items" } };
            var service = new SessionService();
            var sessionWithLineItems = await service.GetAsync(session.Id, options);

            var lineItems = sessionWithLineItems.LineItems;
            var korisnickoImeKupca = session.Metadata["korisnickoImeKupca"];

            // Dohvatimo Kupca na osnovu korisničkog imena
            var kupac = await _kupacRepository.GetKupacByKorisnickoIme(korisnickoImeKupca);

            // Kreiramo novu porudžbinu
            Porudzbina novaPorudzbina = new Porudzbina
            {
                DatumPorudzbine = DateOnly.FromDateTime(DateTime.Now),
                DatumPlacanja = DateOnly.FromDateTime(DateTime.Now),
                AdresaPorudzbine = kupac.AdresaKupca ?? "Posta",
                KupacId = kupac.KupacId  // Postavljamo KupacId na ID kupca
            };

            // Čuvamo novu porudžbinu u bazi
            await _porudzbinaRepository.CreatePorudzbina(novaPorudzbina);

            // Kreiramo veze između porudžbine i proizvoda (line items)
            foreach (var item in lineItems.Data)
            {
                var proizvodName = item.Description; // Pretpostavka da je item.Description naziv proizvoda
                var proizvod = await _proizvodRepository.GetProizvodByNaziv(proizvodName);
                var proizvodId = proizvod.ProizvodId;

                PorudzbinaProizvod porudzbinaProizvod = new PorudzbinaProizvod
                {
                    PorudzbinaId = novaPorudzbina.PorudzbinaId,
                    ProizvodId = proizvodId,
                };

                await _porudzbinaProizvodRepository.CreatePorudzbinaProizvod(porudzbinaProizvod);
            }

            System.Diagnostics.Debug.WriteLine("Korisnicko ime: " + korisnickoImeKupca);
        }






        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var options = new BalanceTransactionListOptions
                {
                    Limit = 10,
                };

                var service = new BalanceTransactionService();
                var transactions = await service.ListAsync(options);

                var simplifiedTransactions = new List<object>();
                foreach (var transaction in transactions)
                {
                    var simplifiedTransaction = new
                    {
                        Amount = transaction.Amount,
                        Id = transaction.Id,
                        Currency = transaction.Currency,
                        Date = transaction.Created
                    };
                    simplifiedTransactions.Add(simplifiedTransaction);
                }

                return Ok(simplifiedTransactions);
            }
            catch (StripeException e)
            {
                return StatusCode((int)e.HttpStatusCode, new { error = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching transactions." });
            }
        }
    }
}
