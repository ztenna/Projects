// Contributor: Zachary Tennant
// The purpose of this code is to handle when someone "clicks" the buttons from LocationPage.xaml
// When the maps button is "clicked", the destination address is obtained from the RequestViewModel, 
// the source address is obtained from the LocationPage.xaml. Both of these are sent to the RestService.cs
// method DisplayGoogleMapsDirections() to be displayed in google maps. When the cancel button is "clicked",
// you are taken to the RequestDetailPage, where you originally "clicked" to view the location of the presentation.

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
	public partial class LocationPage : ContentPage
	{
        readonly RestService rs;
        readonly RequestViewModel rvm;
        readonly User user;

		public LocationPage (RestService rs, RequestViewModel rvm, User user)
		{
            this.rs = rs;
            this.rvm = rvm;
            this.user = user;
            InitializeComponent();
        }
        private async void Maps_Button_Clicked(object sender, EventArgs e)
        {
            string destinationLocation = rvm.PresLocation;
            string sourceLocation = SourceLocation.Text;

            await rs.DisplayGoogleMapsDirections(sourceLocation, destinationLocation).ConfigureAwait(true);
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) 
        {
            await Navigation.PushAsync(new RequestDetailPage(rvm, rs, user)).ConfigureAwait(true);
        }
	}
}