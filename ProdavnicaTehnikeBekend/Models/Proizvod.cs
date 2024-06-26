using System;
using System.Collections.Generic;

namespace ProdavnicaTehnikeBekend.Models;

public partial class Proizvod
{
    public int ProizvodId { get; set; }

    public string NazivProizvoda { get; set; }

    public decimal CenaProizvoda { get; set; }

    public string TipProizvoda { get; set; }

    public string OpisProizvoda { get; set; }

    public int Kolicina { get; set; }

    public string StatusProizvoda { get; set; }


}
