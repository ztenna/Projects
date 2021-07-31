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
    public partial class TitlePage : ContentPage
    {
        readonly RestService restService;
        public TitlePage(RestService restService)
        {
            this.restService = restService;
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                GoToMenuPage();

                return false; // True = Repeat again, False = Stop the timer
            });


        }

        //Loads menu page after you tap on the image button (will try to get this to be the whole screen)
        private async void OnImageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage(restService)).ConfigureAwait(true);
        }
        //For the timer made a seperate function that opens login page after timer
        //The timer one didnt need arguments, so seperate function
        private async void GoToMenuPage()
        {
            await Navigation.PushAsync(new MenuPage(restService)).ConfigureAwait(true);

        }
    }
}