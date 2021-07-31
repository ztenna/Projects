//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the PresentationRequestViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Diagnostics;

namespace SPOT_App
{
    // This class defines how the "PresentationRequestDetailPage" (which is pushed onto the stack of pages when the user clicks on a request in the request list looks.
    // In PresentationRequestDetailPage's constructor, a PresentationRequestViewModel object is passed as an argument from the Handle_RequestTapped method of the PresentationRequestsPage.xaml.cs file.
    // This PresentationRequestViewModel is then set as the "BindingContext" of this page. This means that I can now implicitly bind properties of the that PresentationRequestViewModel to GUI components of this page.

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PresentationRequestDetailPage : ContentPage
	{
        readonly RestService restService;
        readonly PresentationRequestViewModel rvm;
        readonly User user;
        public PresentationRequestDetailPage (PresentationRequestViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }

        public PresentationRequestDetailPage(PresentationRequestViewModel selectedRVM, RestService restService, User user)
		{
			InitializeComponent();
            this.restService = restService;
            rvm = selectedRVM;
            this.user = user;
            //System.Diagnostics.Debug.WriteLine("*****PresentationRequestDetailPage class and display rvmContents:********\n"+ rvm.GetContents());


            // The following line allows us to implicitly bind to the data members of the PresentationRequestViewModel 
            // object "selectedRVM" in the corresponding xaml file "PresentationRequestDetailPage.xaml"
            BindingContext = selectedRVM; //selectedRequestViewModel;


        }

        async private void AcceptPresentationRequest_Button_Clicked(object sender, System.EventArgs e)
        {
            string emailBody = "Your presentation request has been approved and an ambassador will be in touch.\nCongratulations.";

            // The following DisplayAlert is a placeholder
            //Debug.WriteLine("Accepted Presentation Request");
            //System.Diagnostics.Debug.WriteLine("inside PresentationRequestDetailPage.xaml.cs , AcceptPresentationRequest_Button_Clicked(): PresID and Email:" + rvm.PresentationID + " " + rvm.TeacherEmail);
            restService.AcceptPresentationRequest(rvm.PresentationID, rvm.TimeDateCreated, rvm.Name, rvm.OrgName, rvm.TeacherEmail, rvm.AmbassadorEmail, rvm.OtherAmbassadorEmails, rvm.PhoneNumber,
                    rvm.AltPhoneNumber, rvm.ContactDayTime, rvm.PresLocation, rvm.RequestedPres, rvm.PresRotations, rvm.HandsOnActivities, rvm.GradeLevels, rvm.NumberStudents,
                    rvm.ProposedDateTime, rvm.Supplies, rvm.TravelFee, rvm.AgreeStatement, rvm.Concerns, rvm.AltPres);
            restService.RemovePresentationRequest(rvm.PresentationID, rvm.TeacherEmail);
            RestService.SendEmail("WV-SPOT", emailBody, rvm.TeacherEmail);
            await Navigation.PushAsync(new PresentationRequestsPage(restService, user)).ConfigureAwait(true);
        }

        async private void DeclinePresentationRequest_Button_Clicked(object sender, System.EventArgs e)
        {
            string emailBody = "We regret to inform you that your presentation request has been declined.\nSorry.";

            // The following DisplayAlert is a placeholder
            //System.Diagnostics.Debug.WriteLine("inside PresentationRequestDetailPage.xaml.cs , DeclinePresentationRequest_Button_Clicked(): PresID and Email:" + rvm.PresentationID + " " + rvm.TeacherEmail);
            restService.RemovePresentationRequest(rvm.PresentationID, rvm.TeacherEmail);
            RestService.SendEmail("WV-SPOT", emailBody, rvm.TeacherEmail);
            await Navigation.PushAsync(new PresentationRequestsPage(restService, user)).ConfigureAwait(true);
        }
    }
}