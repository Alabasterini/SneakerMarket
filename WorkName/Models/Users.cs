using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace WorkName.Models
{
    [Table("Users")]
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int user_id { get; set; }

        [Unique]
        public string email { get; set; }
        public string username { get; set; }

        public string password { get; set; }
        public string data_rejestracji { get; set; }
        public string rola { get; set; }
    }
}
