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

        List<Listings> ProductListings = new();
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RefreshData();
        }
        private void LoadProductData()
        {
            ProductNameLabel.Text = _product.nazwa_modelu;
            ColorwayLabel.Text = _product.kolorystyka;
            ProductImage.Source = "dotnet_bot.png";

        }

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
        private async Task RefreshData()
        {
            ProductListings = await _databaseService.GetListingsByProductIdAsync(_product.product_id);

            if (!string.IsNullOrEmpty(_selectedSize))
            {
                FilterListingsBySize(_selectedSize);
            }
        }
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
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Koszyk", "Produkt został dodany do koszyka!", "OK");
        }

        private async void OnListingSelected(object sender, EventArgs e)
        {

        }
    }
}
