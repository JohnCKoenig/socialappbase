using EnvShare.API.Account;
using EnvShare.DeviceStore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using EnvShare.Views.SignIn;

namespace EnvShare;

public partial class App : Application
{

    public App()
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Dark;
        //MainPage = new AppShell();
        MainPage = new NavigationPage(new MainPage());

    }
    protected override async void OnStart()
    {
        // Handle when your app starts
        string token = await TokenStorage.GetRefreshTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            // Check token expiration
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var expiration = jwtToken.ValidTo;

            if (expiration < DateTime.UtcNow.AddMinutes(5)) // Refresh if token will expire within 5 minutes
            {
                SignInService siService = new SignInService();
                string refreshToken = await TokenStorage.GetRefreshTokenAsync(); // Retrieve refresh token
                string newToken = await siService.RefreshTokenAsync(refreshToken);

                if (!string.IsNullOrEmpty(newToken))
                {
                    await TokenStorage.SaveTokenAsync(newToken);
                }
                else
                {
                    // Handle refresh failure
                }
            }
        }
    }
}
