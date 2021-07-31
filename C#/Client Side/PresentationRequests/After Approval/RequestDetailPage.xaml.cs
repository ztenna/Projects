using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels;
using System.Diagnostics;

namespace SPOT_App
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestDetailPage : ContentPage
	{
        readonly RestService restService;
        readonly RequestViewModel rvm;
        readonly User user;
        public RequestDetailPage (RequestViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }

        public RequestDetailPage (RequestViewModel selectedRVM, RestService restService, User user)
		{
			InitializeComponent();
            this.restService = restService;
            rvm = selectedRVM;
            this.user = user;

            if (user.IsAdmin == "1")
                edit.IsVisible = true;

            //System.Diagnostics.Debug.WriteLine("*****RequestDetailPage class and display rvmContents:********\n"+ rvm.GetContents());

            // The following line allows us to implicitly bind to the data members of the RequestViewModel 
            // object "selectedRVM" in the corresponding xaml file "RequestDetailPage.xaml"
            BindingContext = selectedRVM;
        }

        async private void AcceptRequest_Button_Clicked(object sender, System.EventArgs e)
        {
            bool hasTheRequestBeenAccepted = await restService.CheckAcceptanceOfRequest(rvm.PresentationID, rvm.TeacherEmail).ConfigureAwait(true);
            //Since false means that the request has not been accepted then accept the request.
            if (!hasTheRequestBeenAccepted)
            {
                //Do the php thing to accept the request.
                bool acceptRequest = await restService.AcceptRequest(rvm.PresentationID, rvm.TeacherEmail).ConfigureAwait(true);
                //System.Diagnostics.Debug.WriteLine("acceptRequest:" + acceptRequest);
                if(acceptRequest)
                {
                    await DisplayAlert("Request Accepted", "You accepted this request", "OK").ConfigureAwait(true);
                }
                    
                else
                {
                    await DisplayAlert("Request Declined.", "Sorry, but something went wrong trying to accept this request.", "Close").ConfigureAwait(true);
                }
            }
            else
            {
                string emailBody = user.FirstName + " " + user.LastName + " (" + user.Email + ") would like to be added to the following presentation request:\nName: " + rvm.Name + "\nOrganization: " + rvm.OrgName + "\nPresentation ID: " + rvm.PresentationID;

                //Reject the acceptance request.
                bool answer = await DisplayAlert("Request Declined.", "Sorry, but the request was already accepted.\nWould you like to request to be added to this presentation?", "Yes", "No").ConfigureAwait(true);
                // have name, orgName, and presentationID in email to identify specific request along with user email and user name
                if (answer == true)
                {
                    RestService.SendEmail("Presentation Claim", emailBody, "wvspotapp@gmail.com");
                }
            }
            await Navigation.PushAsync(new RequestsPage(restService, user)).ConfigureAwait(true);
        }

        async private void Maps_Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new LocationPage(restService, rvm, user)).ConfigureAwait(true);
        }

        async private void Cancel_Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RequestsPage(restService, user)).ConfigureAwait(true);
        }

        async private void Edit_Button_Clicked(object sender, System.EventArgs e)
        {
            Debug.WriteLine("OtherAmbassadorEmails: " + rvm.OtherAmbassadorEmails);
            if (rvm.OtherAmbassadorEmails == null)
                Debug.WriteLine("It is null");
            else if (rvm.OtherAmbassadorEmails.Equals(""))
                Debug.WriteLine("It is ''");
            await Navigation.PushAsync(new EditPresRequestPage(rvm, restService, user)).ConfigureAwait(true);
        }
    }
}