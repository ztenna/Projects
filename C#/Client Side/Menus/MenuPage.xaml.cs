using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SPOT_App
{
    public partial class MenuPage : ContentPage
    {
        readonly RestService restService;

        public MenuPage(RestService restService)
        {
            this.restService = restService;
            InitializeComponent();
        }

        // The following "_Button_Clicked" methods catch events from the buttons created in the "MenuPage.xaml" file.
        private async void LoginPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage(restService)).ConfigureAwait(true);
        }

        private async void Signup_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PrivacyPolicyPage(restService)).ConfigureAwait(true);
        }

        /*private async void Maps_Button_Clicked(object sender, EventArgs e)
        {
            string sourceAddress = "401 Guffey Street Fairmont WV";
            string destinationAddress = "1325 Locust Avenue Fairmont WV";
            string uri = "http://maps.google.com/?daddr=" + restService.ConvertToGoogleMapsUriFormat(destinationAddress) + ",&saddr=" + restService.ConvertToGoogleMapsUriFormat(sourceAddress);
            await Launcher.OpenAsync(uri);//"http://maps.google.com/?daddr=San+Francisco,+CA&saddr=Mountain+View");
        }*/

        private async void Teacher_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeacherMenuPage(restService)).ConfigureAwait(true);
        }
    }
}