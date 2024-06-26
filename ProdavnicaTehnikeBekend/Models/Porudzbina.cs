using System;
using System.Collections.Generic;

namespace ProdavnicaTehnikeBekend.Models;

public partial class Porudzbina
{
    public int PorudzbinaId { get; set; }

    public DateOnly DatumPorudzbine { get; set; }

    public string? AdresaPorudzbine { get; set; }

    public DateOnly DatumPlacanja { get; set; }
    public int KupacId { get; set; }

    public virtual Kupac Kupac { get; set; }
  //  public virtual ICollection<Kupac> Kupacs { get; set; } = new List<Kupac>();
    public virtual ICollection<Zaposleni> Zaposlenis { get; set; } = new List<Zaposleni>(); // Dodajte ovu liniju
    public virtual ICollection<PorudzbinaProizvod> PorudzbinaProizvods { get; set; } = new List<PorudzbinaProizvod>();

}
