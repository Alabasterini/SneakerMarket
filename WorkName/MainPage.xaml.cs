using WorkName.Services;
using WorkName.Models;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
namespace WorkName
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService = new DatabaseService();

       
        List<Product> allProducts = new List<Product>();

        public MainPage(bool isAdmin)
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            
            allProducts = await _databaseService.GetProductsAsync();

            if (allProducts != null)
            {
                foreach (var p in allProducts)
                {
                    // simple switch for brands id
                    p.nazwa_marki = p.brand_id switch
                    {
                        1 => "Nike",
                        2 => "Adidas",
                        3 => "Jordan",
                        4 => "Yeezy",
                        5 => "New Balance",
                        _ => "Inna"
                    };
                }
                ProductsListView.ItemsSource = allProducts;
            }
        }
        // method for searchbar
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (allProducts == null || !allProducts.Any()) return;

            var searchTerm = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ProductsListView.ItemsSource = allProducts;
            }
            else
            {
                ProductsListView.ItemsSource = allProducts
                    .Where(p => (p.nazwa_modelu?.ToLower().Contains(searchTerm) ?? false) ||
                                (p.kolorystyka?.ToLower().Contains(searchTerm) ?? false))
                    .ToList();
            }
        }
        //method for brand filter
        private void OnBrandClicked(object sender, EventArgs e)
        {
            if (allProducts == null || !allProducts.Any()) return;

            var clickedButton = (Button)sender;

            // Reseting buttons colors to primary
            foreach (var child in BrandsLayout.Children)
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

            // filtring trough brands
            string brand = clickedButton.Text.Trim();

            if (brand == "Wszystkie")
            {
                ProductsListView.ItemsSource = allProducts;
            }
            else
            {
                ProductsListView.ItemsSource = allProducts
                    .Where(p => p.nazwa_marki == brand)
                    .ToList();
            }
        }

        //method for getting into profile
        private async void OnProfileClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Profil", "Kliknales w profil", "OK");
        }

        //method for getting into product page
        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            // gettin selected product
            var selectedProduct = e.CurrentSelection.FirstOrDefault() as Product;

            if (selectedProduct == null)
                return;

            if (sender is CollectionView cv)
            {
                cv.SelectedItem = null;
            }

            await Navigation.PushAsync(new ProductPage(selectedProduct));

           
        }
        // method for getting into cart
        private async void OnCartClicked(object sender, EventArgs e)
        {
            
            await DisplayAlert("Koszyk", "Twój koszyk jest obecnie pusty.", "OK");
        }
    }
}