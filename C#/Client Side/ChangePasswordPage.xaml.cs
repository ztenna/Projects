// Contributor: Zachary Tennant
// The purpose of this code is to handle when someone "clicks" the buttons from ChangePasswordPage.xaml
// When the submit button is "clicked", the Password and ConfirmPassword fields are obtained from the ChangePassword.xaml. 
// Both of these are checked to see if they are null. If they are null, then a message is displayed and you remain on the 
// same ChangePassword page until they are not null or the cancel button is "clicked". Once a password is entered and re-entered,
// the two passwords are compared to make sure they match, if they don't, a message is displayed and you remain on the same
// ChangePassword page until they match or the cancel button is "clicked". Once they match, and the submit button is "clicked", the
// password is sent to the RestService.cs method ChangePassword() to be changed in the database. An email is also sent to the email
// attached to your account letting you know your password was changed. You are then redirected to the HomePage. When the cancel 
// button is "clicked", you are taken to the HomePage, where you originally "clicked" to change your password.

using SPOT_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SPOT_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePasswordPage : ContentPage
	{
        readonly RestService rs;
        readonly User user;
       
		public ChangePasswordPage (RestService rs, User user)
		{
            this.rs = rs;
            this.user = user;
            InitializeComponent();
        }
        private async void Change_Password_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Password.Text) && String.IsNullOrEmpty(ConfirmPassword.Text))
                await DisplayAlert("Password Change", "Password can't be empty. Please fill in new password.", "Ok").ConfigureAwait(true);

            else if (Password.Text != ConfirmPassword.Text)
                await DisplayAlert("Password Change", "Passwords do not match. Please retry confirm password.", "Ok").ConfigureAwait(true);

            else
            {
                string emailBody = "Your password has been changed.\nIf this was not you, contact an admin.";
                string password = RestService.GetPasswordHash(Password.Text);

                rs.ChangePassword(password, user.Email);

                RestService.SendEmail("WV-SPOT", emailBody, user.Email);
                await Navigation.PushAsync(new HomePage(rs, user)).ConfigureAwait(true);
            }
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) 
        {
            await Navigation.PushAsync(new HomePage(rs, user)).ConfigureAwait(true);
        }
	}
}