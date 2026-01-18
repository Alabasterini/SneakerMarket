using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Libsql.Client;
using WorkName.Models;

namespace WorkName.Services
{
    public class DatabaseService
    {
        
        private IDatabaseClient _dbClient;

        private async Task Init()
        {
            if (_dbClient is not null)
                return;
            //Database connection i suppose it should be encrypted or smth but idk
            _dbClient = await DatabaseClient.Create(opts =>
            {
                opts.Url = "https://sneakermarket-alabasterini.aws-eu-west-1.turso.io";
                opts.AuthToken = "eyJhbGciOiJFZERTQSIsInR5cCI6IkpXVCJ9.eyJhIjoicnciLCJpYXQiOjE3Njg3MzY1MzUsImlkIjoiMmVhM2JkZjYtOTc4ZC00ZDg2LTk1ZmEtYjc2MGM5NWE4Nzk2IiwicmlkIjoiMjg1ZGNhMDQtNTMzYi00OWExLTk1M2UtMDliMjhmNDNlMDMzIn0.FBWyzWKTfnfVC75OxmO22qyPrr9YmoLhqysGtCmRuOG768r_gSWMiOd8fz6JiU0C74db52vZfXVVdTjj-ZZUBw";          
                opts.UseHttps = true;
            });
        }

        // Method for getting products
        public async Task<List<Product>> GetProductsAsync()
        {
            await Init();
            var resultSet = await _dbClient.Execute("SELECT * FROM PRODUCTS");
            var products = new List<Product>();
            // strange casting made by copilot bcs wtf is that why cant i cast like a normal person
            foreach (var row in resultSet.Rows)
            {
                var cols = row.ToArray();
                var idValue = cols[0];

                int productId = idValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idValue.ToString() ?? "0")
                };
                var idBrandValue = cols[1];

                int brandId = idBrandValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idBrandValue.ToString() ?? "0")
                };
                var priceValue = cols[5];
                decimal cena_retail = priceValue switch
                {
                    Libsql.Client.Real r => Convert.ToDecimal(r.Value),
                    Libsql.Client.Integer i => Convert.ToDecimal(i.Value),
                    null => 0m,
                    _ => decimal.TryParse(priceValue?.ToString(), out var d) ? d : 0m
                };
                // adding values from rows into Product class
                products.Add(new Product
                {
                    product_id = productId,
                    brand_id = brandId,
                    nazwa_modelu = cols[2]?.ToString(),
                    kolorystyka = cols[3]?.ToString(),
                    data_premiery = cols[4]?.ToString(),
                    cena_retail = cena_retail
                });
            }
            return products;
        }

        // Method for adding products
        // WIP
        public async Task AddProductAsync(Product product)
        {
            await Init();
            await _dbClient.Execute(
                "INSERT INTO PRODUCTS (Name, Price) VALUES (?, ?)"
                /*product.Name, product.Price*/);
        }

        // Method for getting users
        public async Task<List<Users>> GetUsersAsync()
        {
            await Init();
            var resultSet = await _dbClient.Execute("SELECT * FROM Users");
            var users = new List<Users>();
            // same casting as produtcs
            foreach (var row in resultSet.Rows)
            {
                var cols = row.ToArray(); 
                var idValue = cols[0];

                int userId = idValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idValue.ToString() ?? "0")
                };
                // adding values from rows into Users class
                users.Add(new Users
                {
                    user_id = userId,
                    username = cols[1]?.ToString(),
                    email = cols[2]?.ToString(),
                    password = cols[3]?.ToString(),
                    data_rejestracji = cols[4]?.ToString(),
                    rola = cols[5]?.ToString()
                });
            }
            return users;
        }

        // Method for adding users
        public async Task AddUserAsync(Users User)
        {
            await Init();
            await _dbClient.Execute(
                "INSERT INTO Users (username, email, password,data_rejestracji, rola) VALUES (?, ?, ?, ?, ?)",
                User.username, User.email, User.password,User.data_rejestracji, User.rola);
        }

        public async Task<List<Listings>> GetListingsAsync()
        {
            await Init();
            var resultSet = await _dbClient.Execute("SELECT * FROM Listings");

            var listings = new List<Listings>();

            foreach(var row in resultSet.Rows){
                var cols = row.ToArray();
                var idListingValue = cols[0];
                var idProductValue = cols[1];
                var idUserValue = cols[2];
                var idPriceValue = cols[4];

                int listingId = idListingValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idListingValue.ToString() ?? "0")
                };

                int productId = idProductValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idProductValue.ToString() ?? "0")
                };

                int userId = idUserValue switch
                {
                    Libsql.Client.Integer i => i.Value,
                    null => 0,
                    _ => int.Parse(idUserValue.ToString() ?? "0")
                };

                decimal cena = idPriceValue switch
                {
                    Libsql.Client.Real r => Convert.ToDecimal(r.Value),
                    Libsql.Client.Integer i => Convert.ToDecimal(i.Value),
                    null => 0m,
                    _ => decimal.TryParse(idPriceValue?.ToString(), out var d) ? d : 0m
                };

                listings.Add(new Listings
                {
                    listing_id = listingId,
                    product_id = productId,
                    user_id = userId,
                    rozmiar = cols[3].ToString(),
                    cena_oferowana = cena,
                    stan = cols[5].ToString(),
                    status = cols[6].ToString()
                });
            }


            return listings;
        }
    }
}
