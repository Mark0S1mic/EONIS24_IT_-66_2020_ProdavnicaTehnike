using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs.PorudzbinaProizvodDto;
using ProdavnicaTehnikeBekend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.TestHelpers;
using System.Threading.Tasks;
using ProdavnicaTehnikeBekend.Models.DTOs;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private IPorudzbinaRepository _porudzbinaRepository;

        public PaymentController(IConfiguration configuration, IPorudzbinaRepository service)
        {
            _configuration = configuration;
            this._porudzbinaRepository = service;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = request.Amount,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = request.ProductName,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                Metadata = new Dictionary<string, string>
                {
                    { "customerId", request.CustomerId.ToString() },
                    { "price", request.Price.ToString() }
                },
                SuccessUrl = _configuration["Stripe:SuccessUrl"],
                CancelUrl = _configuration["Stripe:CancelUrl"],

            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

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
            // Retrieve the session. If you require line items in the response, you may include them by expanding line_items.
            var customerId = session.Metadata["customerId"];
            var price = session.Metadata["price"];
        


            Porudzbina porudzbina = new Porudzbina
            {
                DatumPlacanja = DateOnly.FromDateTime(DateTime.Today),
                DatumPorudzbine = DateOnly.FromDateTime(DateTime.Today),
               
            };

            await _porudzbinaRepository.CreatePorudzbina(porudzbina);

        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var options = new BalanceTransactionListOptions
                {
                    Limit = 10, // Limit the number of transactions to retrieve
                };

                var service = new BalanceTransactionService();
                var transactions = await service.ListAsync(options);

                var simplifiedTransactions = new List<object>();
                foreach (var transaction in transactions)
                {
                    var simplifiedTransaction = new
                    {
                        Amount = transaction.Amount,
                        Customer = transaction.Id,
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

    public class CreateCheckoutSessionRequest
    {
        public long Amount { get; set; }
        public string ProductName { get; set; }

        public int CustomerId { get; set; }

        public int Price { get; set; }
    }
}
