using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SPOT_App
{
    public partial class TeacherMenuPage : ContentPage
    {
        readonly RestService restService;

        public TeacherMenuPage(RestService restService)
        {
            this.restService = restService;
            InitializeComponent();
        }

        // The following "_Button_Clicked" methods catch events from the buttons created in the "TeacherMenuPage.xaml" file.
        private async void PresentationRequestPage_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PresentationPage(restService)).ConfigureAwait(true);
        }

        private async void Evaluation_Button_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("https://docs.google.com/forms/d/e/1FAIpQLSdalZvIfwjMTpx2IeJ9ZKow-3-UGu4b4uSovZdqRgrmpcDQeQ/viewform")).ConfigureAwait(true);
        }
    }
}