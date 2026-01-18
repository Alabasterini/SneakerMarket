using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkName.Models
{

        [Table("Products")]
        public class Product
        {
            [PrimaryKey, AutoIncrement]
            public int product_id { get; set; }

            public int brand_id { get; set; }

            public string nazwa_modelu { get; set; }

            public string kolorystyka { get; set; }

            public string data_premiery { get; set; }
            public decimal cena_retail { get; set; }

            [Ignore]
            public string nazwa_marki { get; set; }
        }
    
}
