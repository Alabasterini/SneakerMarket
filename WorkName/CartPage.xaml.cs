using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkName.Services;
using WorkName.Models;

namespace WorkName
{
    public partial class CartPage : ContentPage
    {
        private readonly DatabaseService _databaseService = new DatabaseService();
        //ctor for initializing component and showing TotalCartValue
        public CartPage()
        {
            InitializeComponent();
            TotalCartValue.Text = TotalValue().ToString();
        }
        private static List<CartItems> _cartItems = CartService.CartItems;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CartListView.ItemsSource = _cartItems;
        }
        //Method for payment 
        //Wont be implemented
        private async void OnPayClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Płatność", "Przechodzenie do wyboru metody płatności", "OK");
        }
        //method for calculating total cart value
        private static decimal TotalValue()
        {
            decimal value = 0;
            foreach(CartItems items in _cartItems)
            {
                value += items.cena_oferowana;
            }
            return value;
        }
    }
}
