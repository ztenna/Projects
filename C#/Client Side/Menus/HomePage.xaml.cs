using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SPOT_App
{
    public partial class HomePage : ContentPage
    {
        readonly RestService restService;
        readonly User user;
        public HomePage(RestService restService, User user)
        {
            this.restService = restService;
            this.user = user;
            InitializeComponent();
            if (user.IsAdmin == "1")
            {
                signup.IsVisible = true;
                currentUser.IsVisible = true;
                presentationRequest.IsVisible = true;
                presentationReport.IsVisible = true;
                report.IsVisible = false;
            }
        }

        // The following "_Button_Clicked" methods catch events from the buttons created in the "HomePage.xaml" file.
        private async void Signup_RequestsPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupRequestsPage(restService, user)).ConfigureAwait(true);
        }
        private async void CurrentUsersPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CurrentUsersPage(restService, user)).ConfigureAwait(true);
        }
        private async void PresentationRequestsPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PresentationRequestsPage(restService, user)).ConfigureAwait(true);
        }
        private async void PresentationReportsPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PresentationReportReviewPage(restService, user)).ConfigureAwait(true);
        }
        private async void PresentationReportingPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PresentationReportingPage(restService, user)).ConfigureAwait(true);
        }
        private async void RequestsPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RequestsPage(restService, user)).ConfigureAwait(true);
        }

        private async void Change_Password_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage(restService, user)).ConfigureAwait(true);
        }

        // This function causes the application to go back to the root page by popping all non-root pages off the stack of pages.
        // The root page is defined by the following line:
        // MainPage = new NavigationPage(new LoginPage());
        // which is located in the App.xaml.cs file.
        private async void Logout_Button_Clicked(object sender, EventArgs e)
        {
            restService.Logout();
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }
    }
}
