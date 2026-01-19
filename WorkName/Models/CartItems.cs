using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkName.Models
{
    public class CartItems
    {
        public int listing_id { get; set; }
        public string rozmiar { get; set; }
        public string stan { get; set; }
        public decimal cena_oferowana { get; set; }


        public string nazwa_modelu{ get; set; }
        public string kolorystyka { get; set; }
        public string brand { get; set; }
    }
}
