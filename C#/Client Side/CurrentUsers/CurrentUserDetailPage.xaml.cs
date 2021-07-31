//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the CurrentUserViewModel is defined in the test_xamarin_app.ViewModels namespace

namespace SPOT_App
{
    // This class defines how the "CurrentUserDetailPage" (which is pushed onto the stack of pages when the user clicks on a user in the user list looks.
    // In CurrentUserDetailPage's constructor, a CurrentUserViewModel object is passed as an argument from the Handle_userTapped method of the CurrentUsersPage.xaml.cs file.
    // This CurrentUserViewModel is then set as the "BindingContext" of this page. This means that I can now implicitly bind properties of the that CurrentUserViewModel to GUI components of this page.

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CurrentUserDetailPage : ContentPage
	{
        readonly RestService restService;
        readonly CurrentUserViewModel rvm;
        readonly User user;
        public CurrentUserDetailPage (CurrentUserViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }

        public CurrentUserDetailPage(CurrentUserViewModel selectedRVM, RestService restService, User user)
		{
			InitializeComponent();
            this.restService = restService;
            rvm = selectedRVM;
            this.user = user;
            //System.Diagnostics.Debug.WriteLine("*****CurrentUserDetailPage class and display rvmContents:********\n"+ rvm.GetContents());


            // The following line allows us to implicitly bind to the data members of the CurrentUserViewModel 
            // object "selectedRVM" in the corresponding xaml file "CurrentUserDetailPage.xaml"
            BindingContext = selectedRVM; //selectedCurrentUserViewModel;


        }

        async private void DeleteUser_Button_Clicked(object sender, System.EventArgs e)
        {
            string emailBody = "We regret to inform you that your account has been deleted.\nSorry.";

            // The following DisplayAlert is a placeholder
            //System.Diagnostics.Debug.WriteLine("inside CurrentUserDetailPage.xaml.cs , DeleteUser_Button_Clicked(): Email:" + rvm.Email);
            restService.RemoveUser(rvm.Email);
            RestService.SendEmail("WV-SPOT", emailBody, rvm.Email);
            await Navigation.PushAsync(new CurrentUsersPage(restService, user)).ConfigureAwait(true);
        }
    }
}