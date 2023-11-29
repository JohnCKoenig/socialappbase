using EnvShare.API.Account;
using EnvShare.DeviceStore.Authentication;
using EnvShare.Views.Home;

namespace EnvShare.Views.SignIn;

public partial class SignIn : ContentPage
{
    private readonly SignInService _signInService;
    public SignIn()
	{
		InitializeComponent();
        _signInService = new SignInService();
    }
    private void OnSwiped(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new MainPage());

    }
    private async void SignInButton_Clicked(object sender, EventArgs e)
    {
        string username = Username.Text;
        string password = Password.Text;

        string jwtToken = await _signInService.SignInAsync(username, password);

        if (!string.IsNullOrEmpty(jwtToken))
        {
            //need to save the refresh token
            await TokenStorage.SaveTokenAsync(jwtToken);
            await Navigation.PushAsync(new HomePage());
        }
        else
        {
            await DisplayAlert("Try Again", "Invalid Username or Password", "OK");
        }
    }
}