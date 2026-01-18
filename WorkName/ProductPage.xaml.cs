using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkName.Models;
using System.Threading.Tasks;

namespace WorkName
{
    public partial class ProductPage : ContentPage
    {
        private Product _product;
        private string _selectedSize;

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

        private void LoadProductData()
        {
            ProductNameLabel.Text = _product.nazwa_modelu;
            ColorwayLabel.Text = _product.kolorystyka;
            ProductImage.Source = "dotnet_bot.png"; // Dynamicznie: _product.image_url

            // Generowanie przycisków rozmiarów
            string[] sizes = { "7", "8", "9", "10", "11", "12" };
            foreach (var size in sizes)
            {
                var btn = new Button { Text = size, WidthRequest = 60, Margin = 5, BackgroundColor = Color.FromArgb("#EEE") };
                btn.Clicked += (s, e) => SelectSize(size);
                SizesContainer.Children.Add(btn);
            }
        }

        private async void SelectSize(string size)
        {
            _selectedSize = size;
            // Tutaj pobierasz z bazy najtańszą ofertę dla tego rozmiaru
            LowestPriceLabel.Text = "$1250";

        }
        private async void OnMakeOfferClicked(object sender, EventArgs e)
        {

        }
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Koszyk", "Produkt został dodany do koszyka!", "OK");
        }
    }
}
