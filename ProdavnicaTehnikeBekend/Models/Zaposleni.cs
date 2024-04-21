using System;
using System.Collections.Generic;

namespace ProdavnicaTehnikeBekend.Models;

public partial class Zaposleni
{
    public int ZaposleniId { get; set; }

    public string KorisnickoImeZaposlenog { get; set; }

    public string SifraZaposlenog { get; set; }

    public string KontaktZaposlenog { get; set; }

    public int? PorudzbinaId { get; set; }

    public virtual Porudzbina Porudzbina { get; set; }
}
