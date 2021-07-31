//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the SignupRequestViewModel is defined in the test_xamarin_app.ViewModels namespace

namespace SPOT_App
{
    // This class defines how the "SignupRequestDetailPage" (which is pushed onto the stack of pages when the user clicks on a request in the request list looks.
    // In SignupRequestDetailPage's constructor, a SignupRequestViewModel object is passed as an argument from the Handle_RequestTapped method of the SignupRequestsPage.xaml.cs file.
    // This SignupRequestViewModel is then set as the "BindingContext" of this page. This means that I can now implicitly bind properties of the that SignupRequestViewModel to GUI components of this page.

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupRequestDetailPage : ContentPage
	{
        //public SignupRequestViewModel selectedRequestViewModel;
        readonly RestService restService;
        readonly SignupRequestViewModel rvm;
        readonly User user;
        public SignupRequestDetailPage (SignupRequestViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }

        public SignupRequestDetailPage (SignupRequestViewModel selectedRVM, RestService restService, User user)
		{
			InitializeComponent();
            this.restService = restService;
            this.user = user;
            rvm = selectedRVM;
            //System.Diagnostics.Debug.WriteLine("*****SignupRequestDetailPage class and display rvmContents:********\n"+ rvm.GetContents());


            // The following line allows us to implicitly bind to the data members of the SignupRequestViewModel 
            // object "selectedRVM" in the corresponding xaml file "SignupRequestDetailPage.xaml"
            BindingContext = selectedRVM; //selectedRequestViewModel;


        }

        async private void AcceptSignupRequest_Button_Clicked(object sender, System.EventArgs e)
        {
            // The following DisplayAlert is a placeholder
            //System.Diagnostics.Debug.WriteLine("inside SignupRequestDetailPage.xaml.cs , AcceptSignupRequest_Button_Clicked(): Email:" + rvm.Email);
            await Navigation.PushAsync(new AddUserPage(restService, rvm, user)).ConfigureAwait(true);
        }

        async private void DeclineSignupRequest_Button_Clicked(object sender, System.EventArgs e)
        {
            string emailBody = "We regret to inform you that your application has been declined.\nSorry.";

            // The following DisplayAlert is a placeholder
            //System.Diagnostics.Debug.WriteLine("inside SignupRequestDetailPage.xaml.cs , DeclineSignupRequest_Button_Clicked(): Email:" + rvm.Email);
            restService.RemovePotentialUser(rvm.Email);
            RestService.SendEmail("WV-SPOT", emailBody, rvm.Email);
            await Navigation.PushAsync(new SignupRequestsPage(restService, user)).ConfigureAwait(true);
        }
    }
}