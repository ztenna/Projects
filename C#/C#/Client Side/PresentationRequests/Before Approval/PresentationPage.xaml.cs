using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SPOT_App
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PresentationPage : ContentPage
	{
        readonly RestService rs;
        long phoneNumber;
        long altPhoneNumber;
        int orgZip;
        int numStudents;
        int numRotations;
        int presCount = 0;
        int activityCount = 0;
        int supplyCount = 0;
        int altPresCount = 0;
        string presGoodToGo = "";
        string activityGoodToGo = "";
        string supplyGoodToGo = "";
        string altPresGoodToGo = "";
        public PresentationPage (RestService rs)
		{
            this.rs = rs;
            InitializeComponent();
		}
        private async void Submit_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Name.Text) || String.IsNullOrEmpty(OrgName.Text) || String.IsNullOrEmpty(Email.Text) ||
                String.IsNullOrEmpty(PhoneNumber.Text) || String.IsNullOrEmpty(AltPhoneNumber.Text) || String.IsNullOrEmpty(ContactDayTime.Text) ||
                String.IsNullOrEmpty(OrgStreet.Text) || String.IsNullOrEmpty(OrgCity.Text) || String.IsNullOrEmpty(OrgState.Text) ||
                String.IsNullOrEmpty(OrgZip.Text) || String.IsNullOrEmpty(PresRotations.Text) || String.IsNullOrEmpty(GradeLevels.Text) ||
                String.IsNullOrEmpty(NumberStudents.Text) || String.IsNullOrEmpty(ProposedDateTime.Text))
                await DisplayAlert("Submit", "Fields can't be empty. Please fill in user information.", "Ok").ConfigureAwait(true);

            else if (PhoneNumber.Text.Length != 10 || !long.TryParse(PhoneNumber.Text, out phoneNumber))
                await DisplayAlert("Submit", "Phone Number must be a number 10 digits long, no dashes.", "Ok").ConfigureAwait(true);

            else if (AltPhoneNumber.Text.Length != 10 || !long.TryParse(AltPhoneNumber.Text, out altPhoneNumber))
                await DisplayAlert("Submit", "Alternative Phone Number must be a number 10 digits long, no dashes.", "Ok").ConfigureAwait(true);

            else if (OrgZip.Text.Length != 5 || !int.TryParse(OrgZip.Text, out orgZip))
                await DisplayAlert("Submit", "Organization zipcode must be a number 5 digits long.", "Ok").ConfigureAwait(true);

            else if (!int.TryParse(NumberStudents.Text, out numStudents))
                await DisplayAlert("Submit", "Number of students must be a number.", "Ok").ConfigureAwait(true);

            else if (!int.TryParse(PresRotations.Text, out numRotations))
                await DisplayAlert("Submit", "Number of presentation rotations must be a number.", "Ok").ConfigureAwait(true);

            else if (!Email.Text.Contains("@") || (!Email.Text.Contains(".com") && !Email.Text.Contains(".edu") && !Email.Text.Contains(".gov")))
                await DisplayAlert("Submit", "Your email must have an appropriate email address (contain @ and email domain like .com).", "Ok").ConfigureAwait(true);

            else if (!AgreeStatement.IsToggled)
                await DisplayAlert("Submit", "You must agree to the statement.", "Ok").ConfigureAwait(true);

            else
            {
                if (presCount == 0)
                {
                    if (AnyPres.IsToggled)
                        presCount++;
                    if (InvisiblePres.IsToggled)
                        presCount++;
                    if (MentorPres.IsToggled)
                        presCount++;
                    if (StarPres.IsToggled)
                        presCount++;
                    if (WaterPres.IsToggled)
                        presCount++;
                    if (PlanetPres.IsToggled)
                        presCount++;
                    if (MarsPres.IsToggled)
                        presCount++;
                    if (StationPres.IsToggled)
                        presCount++;
                    if (TelescopePres.IsToggled)
                        presCount++;
                    if (NANOGravPres.IsToggled)
                        presCount++;
                    if (presCount == 0)
                        await DisplayAlert("Submit", "You must select at least one presentation.", "Ok").ConfigureAwait(true);
                    else if (presCount > 9)
                        await DisplayAlert("Submit", "Don't select all of the presentations (can choose 'Any Presentation').", "Ok").ConfigureAwait(true);
                    else
                        presGoodToGo = "Yes";
                    presCount = 0;
                }

                if (activityCount == 0)
                {
                    if (NoHandsOn.IsToggled)
                        activityCount++;
                    if (BuildMolecule.IsToggled)
                        activityCount++;
                    if (DesignAlien.IsToggled)
                        activityCount++;
                    if (PocketSolSys.IsToggled)
                        activityCount++;
                    if (SizeUpMoon.IsToggled)
                        activityCount++;
                    if (TelescopeDesign.IsToggled)
                        activityCount++;
                    if (RoboticArm.IsToggled)
                        activityCount++;
                    if (ElectroWar.IsToggled)
                        activityCount++;
                    if (BuildWatershed.IsToggled)
                        activityCount++;
                    if (DropInBucket.IsToggled)
                        activityCount++;
                    if (activityCount > 1)
                        await DisplayAlert("Submit", "You can only choose one hands on activity because they take, on average, 30 minutes a piece.", "Ok").ConfigureAwait(true);
                    else
                        activityGoodToGo = "Yes";
                    activityCount = 0;
                }

                if (supplyCount == 0)
                {
                    if (ProjectSupplies.IsToggled)
                        supplyCount++;
                    if (SpeakerSupplies.IsToggled)
                        supplyCount++;
                    if (CompSupplies.IsToggled)
                        supplyCount++;
                    if (CordSupplies.IsToggled)
                        supplyCount++;
                    if (MicSupplies.IsToggled)
                        supplyCount++;
                    if (!String.IsNullOrEmpty(OtherSupplies.Text))
                        supplyCount++;
                    if (supplyCount == 0)
                        await DisplayAlert("Submit", "You must choose at least one supply.", "Ok").ConfigureAwait(true);
                    else
                        supplyGoodToGo = "Yes";
                    supplyCount = 0;
                }

                if (altPresCount == 0)
                {
                    if (CollegePres.IsToggled)
                        altPresCount++;
                    if (SkypePres.IsToggled)
                        altPresCount++;
                    if (!String.IsNullOrEmpty(AltOtherPres.Text))
                        altPresCount++;
                    if (altPresCount > 1)
                        await DisplayAlert("Submit", "You can only pick one alternative presentation format (WV college, Skype, Other).", "Ok").ConfigureAwait(true);
                    else
                        altPresGoodToGo = "Yes";
                    altPresCount = 0;
                }

                if (presGoodToGo.Equals("Yes") && activityGoodToGo.Equals("Yes") && supplyGoodToGo.Equals("Yes") && altPresGoodToGo.Equals("Yes"))
                {
                    rs.SubmitPresRequest(Name.Text, OrgName.Text, Email.Text, phoneNumber.ToString(), altPhoneNumber.ToString(), ContactDayTime.Text, OrgStreet.Text, OrgCity.Text, OrgState.Text,
                        orgZip.ToString(), Convert.ToInt32(AnyPres.IsToggled).ToString(), Convert.ToInt32(InvisiblePres.IsToggled).ToString(), Convert.ToInt32(MentorPres.IsToggled).ToString(),
                        Convert.ToInt32(StarPres.IsToggled).ToString(), Convert.ToInt32(WaterPres.IsToggled).ToString(), Convert.ToInt32(PlanetPres.IsToggled).ToString(),
                        Convert.ToInt32(MarsPres.IsToggled).ToString(), Convert.ToInt32(StationPres.IsToggled).ToString(), Convert.ToInt32(TelescopePres.IsToggled).ToString(),
                        Convert.ToInt32(NANOGravPres.IsToggled).ToString(), numRotations.ToString(), Convert.ToInt32(NoHandsOn.IsToggled).ToString(), Convert.ToInt32(BuildMolecule.IsToggled).ToString(),
                        Convert.ToInt32(DesignAlien.IsToggled).ToString(), Convert.ToInt32(PocketSolSys.IsToggled).ToString(), Convert.ToInt32(SizeUpMoon.IsToggled).ToString(),
                        Convert.ToInt32(TelescopeDesign.IsToggled).ToString(), Convert.ToInt32(RoboticArm.IsToggled).ToString(), Convert.ToInt32(ElectroWar.IsToggled).ToString(),
                        Convert.ToInt32(BuildWatershed.IsToggled).ToString(), Convert.ToInt32(DropInBucket.IsToggled).ToString(), GradeLevels.Text, numStudents.ToString(), ProposedDateTime.Text,
                        Convert.ToInt32(ProjectSupplies.IsToggled).ToString(), Convert.ToInt32(SpeakerSupplies.IsToggled).ToString(), Convert.ToInt32(CompSupplies.IsToggled).ToString(),
                        Convert.ToInt32(CordSupplies.IsToggled).ToString(), Convert.ToInt32(MicSupplies.IsToggled).ToString(), OtherSupplies.Text, Convert.ToInt32(TravelFee.IsToggled).ToString(),
                        Convert.ToInt32(AgreeStatement.IsToggled).ToString(), Concerns.Text, Convert.ToInt32(CollegePres.IsToggled).ToString(), Convert.ToInt32(SkypePres.IsToggled).ToString(), AltOtherPres.Text);
                    await Navigation.PopToRootAsync().ConfigureAwait(true);
                }
            }
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }
	}
}