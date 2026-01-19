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
        //ctor for initializing component
        public CartPage()
        {
            InitializeComponent();
        }
        private static List<CartItems> _cartItems = CartService.CartItems;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshView();
        }
        //Method for payment 
        //Wont be implemented
        private async void OnPayClicked(object sender, EventArgs e)
        {
            List<CartItems> itemsToRemove = new();
            foreach (Listings item in CartService.CartListings)
            {
                await _databaseService.RemoveListingAndCreateOrderAsync(item, CartService.loggedUser);
            }
            CartService.ClearCart();

            await DisplayAlert("Płatność", "Przechodzenie do wyboru metody płatności", "OK");
            RefreshView();
        }
        //method for calculating total cart value
        private static string TotalValue()
        {

            decimal value = 0;
            foreach (CartItems items in _cartItems)
            {
                value += items.cena_oferowana;
            }
            string valueString = $"{value} USD";
            return valueString;
        }
        //method for removing item from cart
        private  void OnRemoveFromCartClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var itemToDelete = button.BindingContext as CartItems;
            if (itemToDelete == null) return;

            CartService.RemoveFromCart(itemToDelete);
            RefreshView();
        }
        //method for refreshing cart view
        private void RefreshView()
        {

            CartListView.ItemsSource = null;
            CartListView.ItemsSource = CartService.CartItems;


            TotalCartValue.Text = TotalValue();
        }
    }
}
