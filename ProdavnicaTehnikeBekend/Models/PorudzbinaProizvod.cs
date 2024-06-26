using System;
using System.Collections.Generic;

namespace ProdavnicaTehnikeBekend.Models;

public partial class PorudzbinaProizvod
{
    public int PorudzbinaProizvodId { get; set; }
    public int ProizvodId { get; set; }

    public int PorudzbinaId { get; set; }

    public virtual Porudzbina Porudzbina { get; set; }

    public virtual Proizvod Proizvod { get; set; }
}
