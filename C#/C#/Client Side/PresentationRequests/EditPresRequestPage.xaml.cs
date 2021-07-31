// Contributor: Zachary Tennant
// The purpose of this code is to handle when someone "clicks" the buttons from EditPresRequestPage.xaml
// When the submit button is "clicked", the fields are obtained from the EditPresRequestPage.xaml. 
// Required fields are checked to see if they are null. If they are null, then a message is displayed and you remain on the 
// same EditPresRequest page until they are not null or the cancel button is "clicked". Once the required fields are entered,
// they are validated for correct input (numbers are actual numbers, phone numbers resemble phone numbers, etc), if they don't, 
// a message is displayed and you remain on the same EditPresRequest page until the validation is satisfied or the cancel button is 
// "clicked". Once the validation is satisfied, and the submit button is "clicked", the fields are sent to the RestService.cs method 
// EditPresRequest() to be changed in the database. You are then redirected to the HomePage. When the cancel 
// button is "clicked", you are taken to the RequestsPage, where you originally "clicked" to select the presentation to edit.

using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels;
using System;
using System.Diagnostics;

namespace SPOT_App
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPresRequestPage : ContentPage
	{
        readonly RestService restService;
        readonly RequestViewModel rvm;
        readonly User user;

        long phoneNumber;
        long altPhoneNumber;
        int numStudents;
        int numRotations;
        public EditPresRequestPage (RequestViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }

        public EditPresRequestPage (RequestViewModel selectedRVM, RestService restService, User user)
		{
			InitializeComponent();
            this.restService = restService;
            rvm = selectedRVM;
            this.user = user;

            //System.Diagnostics.Debug.WriteLine("*****EditPresRequestPage class and display rvmContents:********\n"+ GetContents());

            // The following line allows us to implicitly bind to the data members of the RequestViewModel 
            // object "selectedRVM" in the corresponding xaml file "EditRequestPage.xaml"
            BindingContext = selectedRVM;
        }

        async private void Edit_Pres_Request_Button_Clicked(object sender, System.EventArgs e)
        {
            string submit = "No";

            if (String.IsNullOrEmpty(Name.Text) || String.IsNullOrEmpty(OrgName.Text) || String.IsNullOrEmpty(TeacherEmail.Text) ||
                String.IsNullOrEmpty(PhoneNumber.Text) || String.IsNullOrEmpty(AltPhoneNumber.Text) || String.IsNullOrEmpty(ContactDayTime.Text) ||
                String.IsNullOrEmpty(PresLocation.Text) || String.IsNullOrEmpty(PresRotations.Text) || String.IsNullOrEmpty(GradeLevels.Text) ||
                String.IsNullOrEmpty(NumberStudents.Text) || String.IsNullOrEmpty(ProposedDateTime.Text) || String.IsNullOrEmpty(RequestedPres.Text) || String.IsNullOrEmpty(Supplies.Text))
                await DisplayAlert("Submit", "Fields can't be empty. Please fill in user information.", "Ok").ConfigureAwait(true);

            else if (PhoneNumber.Text.Length != 10 || !long.TryParse(PhoneNumber.Text, out phoneNumber))
                await DisplayAlert("Submit", "Phone Number must be a number 10 digits long, no dashes.", "Ok").ConfigureAwait(true);

            else if (AltPhoneNumber.Text.Length != 10 || !long.TryParse(AltPhoneNumber.Text, out altPhoneNumber))
                await DisplayAlert("Submit", "Alternative Phone Number must be a number 10 digits long, no dashes.", "Ok").ConfigureAwait(true);

            else if (!int.TryParse(NumberStudents.Text, out numStudents))
                await DisplayAlert("Submit", "Number of students must be a number.", "Ok").ConfigureAwait(true);

            else if (!int.TryParse(PresRotations.Text, out numRotations))
                await DisplayAlert("Submit", "Number of presentation rotations must be a number.", "Ok").ConfigureAwait(true);

            else if (!TeacherEmail.Text.Contains("@") || (!TeacherEmail.Text.Contains(".com") && !TeacherEmail.Text.Contains(".edu") && !TeacherEmail.Text.Contains(".gov")))
                await DisplayAlert("Submit", "Teacher email must have an appropriate email address (contain @ and email domain like .com).", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(AmbassadorEmail.Text))
            {
                if (!AmbassadorEmail.Text.Contains("@") || (!AmbassadorEmail.Text.Contains(".com") && !AmbassadorEmail.Text.Contains(".edu") && !AmbassadorEmail.Text.Contains(".gov")))
                    await DisplayAlert("Submit", "Ambassador email must have an appropriate email address (contain @ and email domain like .com).", "Ok").ConfigureAwait(true);
                else
                    submit = "Yes";
            }

            if (submit == "Yes")
            {
                Debug.WriteLine("OtherAmbassadorEmails: " + OtherAmbassadorEmails.Text);
                restService.EditPresRequest(PresentationID.Text, Name.Text, OrgName.Text, TeacherEmail.Text, AmbassadorEmail.Text, OtherAmbassadorEmails.Text, phoneNumber.ToString(),
                    altPhoneNumber.ToString(), ContactDayTime.Text, PresLocation.Text, RequestedPres.Text, numRotations.ToString(), HandsOnActivities.Text, GradeLevels.Text, numStudents.ToString(),
                    ProposedDateTime.Text, Supplies.Text, TravelFee.Text, Concerns.Text, AltPres.Text);
                await Navigation.PushAsync(new RequestsPage(restService, user)).ConfigureAwait(true);
            }
        }

        async private void Cancel_Button_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RequestsPage(restService, user)).ConfigureAwait(true);
        }
    }
}