using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkName.Models;
using WorkName.Services;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace WorkName 
{
    public partial class RegisterPage : ContentPage
    {
        private readonly DatabaseService _databaseService = new DatabaseService();
        List<Users> allUsers = new();
        public RegisterPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // getting Users
            allUsers = await _databaseService.GetUsersAsync();
        }

        
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string email = UseremailEntry.Text;
            string password = PasswordEntry.Text;

            var foundUsernameOrEmail = allUsers.FirstOrDefault(u => u.username == username || u.email == username);
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|pl)$";
            //Checking using regex for valid email format
            if (!Regex.IsMatch(email, emailPattern))
            {
                await DisplayAlert("Błąd", "Adres e-mail musi być poprawny i kończyć się na .com lub .pl", "OK");
                return;
            }
            if (foundUsernameOrEmail == null)
            {
                // creating new user object
                var newUser = new Users
                {
                    username = username,
                    email = email,
                    password = password,
                    data_rejestracji = DateTime.Now.ToString("yyyy-MM-dd"),
                    rola = "User"

                };
                try
                {
                    //trying to add new user into database
                    await _databaseService.AddUserAsync(newUser);
                    await DisplayAlert("Sukces", "Utworzony nowego użytkownika!", "OK");

                    await Navigation.PushAsync(new LoginPage());
                }
                catch (Exception ex)
                {
                     await DisplayAlert("Błąd", "Nie udało się użytkownika:" +ex.Message, "OK");
                }
            }
            else await DisplayAlert("Błąd", "Znaleziono takiego użytkownika!", "OK");
        }
    }
}
