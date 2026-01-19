using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkName.Models;
using System.Threading.Tasks;
using WorkName.Services;

namespace WorkName
{
    public partial class ProductPage : ContentPage
    {
        private Product _product;
        private string? _selectedSize;
        private readonly DatabaseService _databaseService = new DatabaseService();

        private Listings _selectedListing = null;
        private bool _cartLock = false;

        List<Listings> ProductListings = new();

        // ProductPage ctor
        public ProductPage(Product product)
        {
            InitializeComponent();
            _product = product;

            try
            {
                LoadProductData();
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd strony", ex.Message, "OK");
            }
        }

        //Method for loading specified product data
        private void LoadProductData()
        {
            ProductNameLabel.Text = _product.nazwa_modelu;
            ColorwayLabel.Text = _product.kolorystyka;
            ProductImage.Source = "dotnet_bot.png";

        }

        //Method for showing Listings when size is clicked
        private async void OnSizeClicked(object sender, EventArgs e)
        {
            var clickedButton = (Button)sender;

            // Reseting buttons colors to primary
            foreach (var child in SizesLayout.Children)
            {
                if (child is Button btn)
                {
                    btn.BackgroundColor = Colors.White;
                    btn.TextColor = Colors.Black;
                }
            }

            // backlight
            clickedButton.BackgroundColor = Colors.Black;
            clickedButton.TextColor = Colors.White;

            _selectedSize = clickedButton.Text;
            await RefreshData();
        }
        //asynchronous task refreshing data by selected size
        private async Task RefreshData()
        {
            ProductListings = await _databaseService.GetListingsByProductIdAsync(_product.product_id);

            if (!string.IsNullOrEmpty(_selectedSize))
            {
                FilterListingsBySize(_selectedSize);
            }
        }

        //Method for updating listView by selected size
        private void FilterListingsBySize(string size)
        {
            var expected = $"US {size}".Trim();


            ProductsListView.ItemsSource = ProductListings
                .Where(l => string.Equals(
                    l.rozmiar?.Trim(),
                    expected,
                    StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.cena_oferowana)
                .ToList();
        }
        private async void OnMakeOfferClicked(object sender, EventArgs e)
        {

        }
        //Method for adding selected listing to cart
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            //checking is there any selection
            if (_cartLock) {
                if(CartService.AddToCart(_selectedListing, _product) == 0) { 
                await DisplayAlert("Koszyk", "Produkt został dodany do koszyka!", "OK");
                }
                else await DisplayAlert("Koszyk", "Produkt już jest w koszyku!", "OK");
                ProductsListView.SelectedItem = null;
                _selectedListing = null;
                _cartLock = false;
            }
            
        }

        private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as Listings;

            _selectedListing = selected;

            _cartLock = true;
        }
    }
}
