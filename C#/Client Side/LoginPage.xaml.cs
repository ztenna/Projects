using SPOT_App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SPOT_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        readonly RestService restService;
        LoginResponse loginResponse;
        User user;
		public LoginPage(RestService restService)
		{
            this.restService = restService;
			InitializeComponent();
		}

        // Then the Login button (which is defined in the LoginPage.xaml file) is clicked, push the HomePage onto the stack of pages.
        private async void Login_Button_Clicked(object sender, EventArgs e)
        {
            string password;
            //check if fields have anything. If they dont, display an alert.
            //If they are full, send them off to be checked
            if (String.IsNullOrEmpty(Username.Text) || String.IsNullOrEmpty(Password.Text))
                await DisplayAlert("Login", "Username and password field can not be empty", "Ok").ConfigureAwait(true);
            else
            {
                password = RestService.GetPasswordHash(Password.Text);
                Password.Text = "";
                loginResponse = await restService.Login(Username.Text, password).ConfigureAwait(true);
                if (loginResponse.Status.Contains("true"))
                {
                    user = await restService.GetUserData(loginResponse.Email).ConfigureAwait(true);
                    loginResponse = null; //delete the login response so that password is not stored anywhere
                    await DisplayAlert("Login", "Login Successful!", "Ok").ConfigureAwait(true);
                    await Navigation.PushAsync(new HomePage(restService,user)).ConfigureAwait(true);
                }
                else
                {
                    await DisplayAlert("Login", "Username or password is incorrect", "Ok").ConfigureAwait(true);
                }

            }
        }        
    }
}