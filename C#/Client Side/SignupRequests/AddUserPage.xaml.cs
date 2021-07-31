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
	public partial class AddUserPage : ContentPage
	{
        readonly RestService rs;
        readonly User user;
        readonly SignupRequestViewModel rvm;
        
		public AddUserPage (RestService rs, SignupRequestViewModel rvm, User user)
		{
            this.rs = rs;
            this.user = user;
            this.rvm = rvm;
            InitializeComponent();
        }
        private async void Add_User_Button_Clicked(object sender, EventArgs e)
        {
            string[] split = rvm.Name.Split(' ');
            string firstName;
            string lastName = "";

            if (rvm.Name.Contains(' '))
            {
                firstName = split[0];
                lastName = split[1];
            }
            else
                firstName = rvm.Name;

            string isAmbassador = "0";

            if (String.IsNullOrEmpty(Password.Text))
                await DisplayAlert("Adding User", "Password can't be empty. Please fill in user password.", "Ok").ConfigureAwait(true);

            else
            {
                string emailBody = "Congratulations! You have been accepted to the WV Science Public Outreach Team.\n\nYour Username: " + rvm.Email + "\nYour password: " + Password.Text + ".";

                if (!IsAdmin.IsToggled)
                    isAmbassador = "1";
                else
                    emailBody += "\n\nYou are an admin.\nCongratulations.";

                rs.AddUser(rvm.Email, Password.Text, firstName, lastName, rvm.PhoneNumber, rvm.Address, rvm.School, rvm.Major, rvm.HearAboutSpot, rvm.WhySpotPresenter, 
                    rvm.ExpWithStudents, rvm.NotAsPlanned, rvm.RecommendationLetterName, rvm.RecommendationLetterEmail, rvm.DietaryRestrictions, isAmbassador, 
                    Convert.ToInt32(IsAdmin.IsToggled).ToString(), rvm.AvailableToVisit, rvm.HaveLicense, rvm.HaveCar, rvm.CanAttendTraining, rvm.Gender, rvm.FirstToCollege,
                    rvm.OfOrigin, rvm.Race, rvm.Community, rvm.PrivacyPolicy);

                rs.RemovePotentialUser(rvm.Email);
                RestService.SendEmail("WV-SPOT", emailBody, rvm.Email);
                await Navigation.PushAsync(new SignupRequestsPage(rs, user)).ConfigureAwait(true);
            }
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }
	}
}