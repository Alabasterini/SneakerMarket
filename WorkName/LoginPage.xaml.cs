namespace WorkName; 
using WorkName.Services;
using WorkName.Models;
using Microsoft.Maui.Graphics;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _databaseService = new DatabaseService();
    List<Users> allUsers = new();
    public LoginPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Getting users
        allUsers = await _databaseService.GetUsersAsync();
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string user = UserEntry.Text;
        string password = PasswordEntry.Text;
        //checking for empty space
        if (string.IsNullOrWhiteSpace(user))
        {
            await DisplayAlert("Błąd", "Wpisz swoją nazwę!", "OK");
        }
        //found if either username or email is there
        var foundUser = allUsers.FirstOrDefault(u => u.username == user || u.email == user);
 
        if (foundUser != null)
        {
            //checking for admin
            bool isAdmin = foundUser.rola == "admin";
            if(foundUser.password == password)
            { await Navigation.PushAsync(new MainPage(isAdmin));
                CartService.loggedUser = foundUser;
            }
                
            
            else await DisplayAlert("Błąd", "Błedne hasło!", "OK");
        }
        else
        {
            await DisplayAlert("Błąd", "Nie znaleziono takiego użytkownik!", "OK");
        }

        
    }
    //method for navigating to registerPage
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}