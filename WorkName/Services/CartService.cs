using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkName.Models;

namespace WorkName.Services
{
    public static class CartService
    {
        public static List<CartItems> CartItems { get; private set; } = new();
        //method for adding items to cart
        public static int AddToCart(Listings listing, Product product)
        {
            var item = new CartItems
            {
                listing_id = listing.listing_id,
                rozmiar = listing.rozmiar,
                stan = listing.stan,
                cena_oferowana = listing.cena_oferowana,

                nazwa_modelu = product.nazwa_modelu,
                kolorystyka = product.kolorystyka,
                brand = product.brand_id switch
                {
                    1 => "Nike",
                    2 => "Adidas",
                    3 => "Jordan",
                    4 => "Yeezy",
                    5 => "New Balance",
                    _ => "Inna"
                }
            };
            //simple check so one listing cannot be added twice or more
            // returns 0 if succes -1 if listing is already in cart
            if (!CartItems.Contains(item))
            {
                CartItems.Add(item);
                return 0;
            }
            else return -1;
        }
        //Method for clearing cart
        public static void ClearCart()
        {
            CartItems.Clear();
        }

        public static void RemoveFromCart(CartItems itemToDelete)
        {
            CartItems.Remove(itemToDelete);
        }
    }
}
