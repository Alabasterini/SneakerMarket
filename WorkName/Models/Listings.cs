using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkName.Models
{

    [Table("Listings")]
    public class Listings
    {
        [PrimaryKey, AutoIncrement]
        public int listing_id { get; set; }

        public int product_id { get; set; }

        public int user_id { get; set; }

        public string rozmiar { get; set; }

        public decimal cena_oferowana { get; set; }

        public string stan { get; set; }
        public string status { get; set; }


    }

}
