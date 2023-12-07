using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;



namespace MobileApp.Views.SignIn;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();

	}
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string name = txtName.Text;
        string email = txtEmail.Text;
        string password = txtPassword.Text;

        bool isRegistered = await RegisterUser(name, email, password);

        if (isRegistered)
        {
            await DisplayAlert("Success", "Registration successful!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Registration failed.", "OK");
        }
    }
    private async Task<bool> RegisterUser(string name, string email, string password)
    {
        HttpClient req = new HttpClient();
        try
        {
            var endpoint = "placeholder";
            var requestData = new { Name = name, Email = email, Password = password };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await req.PostAsync(endpoint, content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

}