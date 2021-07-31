using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SPOT_App
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrivacyPolicyPage : ContentPage
	{
        readonly RestService rs;
        string PrivacyPolicy;

        public PrivacyPolicyPage (RestService rs)
		{
            this.rs = rs;
            InitializeComponent();
		}
        private async void Agree_Button_Clicked(object sender, EventArgs e)
        {
            PrivacyPolicy = "Agree";
            await Navigation.PushAsync(new SignupPage(rs, PrivacyPolicy)).ConfigureAwait(true);
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) 
        {
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }
	}
}