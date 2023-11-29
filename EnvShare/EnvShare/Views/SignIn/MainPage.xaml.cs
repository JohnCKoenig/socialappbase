namespace EnvShare.Views.SignIn;


public partial class MainPage : ContentPage
{


	public MainPage()
	{
		InitializeComponent();
    
    }

	private async void OnSignInClicked(object sender, EventArgs e)
	{
        // App.Current.MainPage = new NavigationPage(new SignIn());
        await Navigation.PushAsync(new SignIn());
    }

}

