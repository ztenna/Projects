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
	public partial class SignupPage : ContentPage
	{
        readonly RestService rs;
        long phoneNumber;
        int applicantCount = 0;
        int genderCount = 0;
        int raceCount = 0;
        int communityCount = 0;
        string applicantGoodToGo = "";
        string genderGoodToGo = "";
        string raceGoodToGo = "";
        string communityGoodToGo = "";
        readonly string PrivacyPolicy;
        public SignupPage (RestService rs, string PrivacyPolicy)
		{
            this.rs = rs;
            this.PrivacyPolicy = PrivacyPolicy;
            InitializeComponent();
		}
        private async void Signup_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Name.Text) || String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(PhoneNumber.Text) ||
                String.IsNullOrEmpty(Address.Text) || String.IsNullOrEmpty(School.Text) || String.IsNullOrEmpty(Major.Text) ||
                String.IsNullOrEmpty(HearAboutSpot.Text) || String.IsNullOrEmpty(WhySpotPresenter.Text) || String.IsNullOrEmpty(ExpWithStudents.Text) ||
                String.IsNullOrEmpty(NotAsPlanned.Text) || String.IsNullOrEmpty(DietaryRestrictions.Text))
                await DisplayAlert("Signup", "Fields can't be empty. Please fill in user information.", "Ok").ConfigureAwait(true);

            else if (PhoneNumber.Text.Length != 10 || !long.TryParse(PhoneNumber.Text, out phoneNumber))
                await DisplayAlert("Signup", "Phone Number must be a number 10 digits long, no dashes.", "Ok").ConfigureAwait(true);

            else if (!Email.Text.Contains("@") || (!Email.Text.Contains(".com") && !Email.Text.Contains(".edu") && !Email.Text.Contains(".gov")))
                await DisplayAlert("Signup", "Your email must have an appropriate email address (contain @ and email domain like .com).", "Ok").ConfigureAwait(true);

            else if (!Monday.IsToggled && !Tuesday.IsToggled && !Wednesday.IsToggled && !Thursday.IsToggled && !Friday.IsToggled && !FallBreak.IsToggled &&
                !WinterBreak.IsToggled && !SpringBreak.IsToggled && String.IsNullOrEmpty(AvailableVisitOther.Text))
                await DisplayAlert("Signup", "You must be available to visit a school.", "Ok").ConfigureAwait(true);

            else if (!AgreeToStatement.IsToggled || !AgreeToDoComponents.IsToggled)
                await DisplayAlert("Signup", "You must agree to the statement and to do components.", "Ok").ConfigureAwait(true);

            else
            {
                if (!String.IsNullOrEmpty(RecommendationLetterEmail.Text))
                {
                    if (!RecommendationLetterEmail.Text.Contains("@") || (!RecommendationLetterEmail.Text.Contains(".com") &&
                    !RecommendationLetterEmail.Text.Contains(".edu") && !RecommendationLetterEmail.Text.Contains(".gov")))
                        await DisplayAlert("Signup", "Your professors email must have an appropriate email address (contain @ and email domain like .com).", "Ok").ConfigureAwait(true);
                }
                if (applicantCount == 0)
                {
                    if (IsNewApplicant.IsToggled)
                        applicantCount++;
                    if (IsCurrentAmbassador.IsToggled)
                        applicantCount++;
                    if (applicantCount == 0)
                        await DisplayAlert("Signup", "You must select new applicant or current ambassador.", "Ok").ConfigureAwait(true);
                    else if (applicantCount > 1)
                        await DisplayAlert("Signup", "You can only select one, new applicant or current ambassador.", "Ok").ConfigureAwait(true);
                    else
                        applicantGoodToGo = "Yes";
                    applicantCount = 0;
                }

                if (genderCount == 0)
                {
                    if (Male.IsToggled)
                        genderCount++;
                    if (Female.IsToggled)
                        genderCount++;
                    if (!String.IsNullOrEmpty(GenderOther.Text))
                        genderCount++;
                    if (genderCount == 0)
                        await DisplayAlert("Signup", "You must pick a gender even if you want to describe it in your own words.", "Ok").ConfigureAwait(true);
                    else if (genderCount > 1)
                        await DisplayAlert("Signup", "You can only choose one gender.", "Ok").ConfigureAwait(true);
                    else
                        genderGoodToGo = "Yes";
                    genderCount = 0;
                }

                if (raceCount == 0)
                {
                    if (AfricanAmerican.IsToggled)
                        raceCount++;
                    if (AmericanIndian_Or_AlaskanNative.IsToggled)
                        raceCount++;
                    if (PacificIslander.IsToggled)
                        raceCount++;
                    if (Asian.IsToggled)
                        raceCount++;
                    if (Caucasian.IsToggled)
                        raceCount++;
                    if (!String.IsNullOrEmpty(RaceOther.Text))
                        raceCount++;
                    if (raceCount == 0)
                        await DisplayAlert("Signup", "You can only choose one race, if more than one applies, use the 'other' line.", "Ok").ConfigureAwait(true);
                    else if (raceCount > 1)
                        await DisplayAlert("Signup", "You must choose a race, if more than one applies, use the 'other' line.", "Ok").ConfigureAwait(true);
                    else
                        raceGoodToGo = "Yes";
                    raceCount = 0;
                }

                if (communityCount == 0)
                {
                    if (RuralCommunity.IsToggled)
                        communityCount++;
                    if (SuburbanCommunity.IsToggled)
                        communityCount++;
                    if (UrbanCommunity.IsToggled)
                        communityCount++;
                    if (communityCount == 0)
                        await DisplayAlert("Signup", "You must pick what type of community your hometown is.", "Ok").ConfigureAwait(true);
                    else if (communityCount > 1)
                        await DisplayAlert("Signup", "You can only pick one type of community.", "Ok").ConfigureAwait(true);
                    else
                        communityGoodToGo = "Yes";
                    communityCount = 0;
                }

                if (applicantGoodToGo.Equals("Yes") && genderGoodToGo.Equals("Yes") && raceGoodToGo.Equals("Yes") && communityGoodToGo.Equals("Yes"))
                {
                    rs.AddPotentialUser(Name.Text, Email.Text, phoneNumber.ToString(), Address.Text, School.Text, Major.Text, HearAboutSpot.Text, WhySpotPresenter.Text, ExpWithStudents.Text,
                        NotAsPlanned.Text, RecommendationLetterName.Text, RecommendationLetterEmail.Text, DietaryRestrictions.Text, Convert.ToInt32(IsNewApplicant.IsToggled).ToString(),
                        Convert.ToInt32(IsCurrentAmbassador.IsToggled).ToString(), Convert.ToInt32(Monday.IsToggled).ToString(), Convert.ToInt32(Tuesday.IsToggled).ToString(),
                        Convert.ToInt32(Wednesday.IsToggled).ToString(), Convert.ToInt32(Thursday.IsToggled).ToString(), Convert.ToInt32(Friday.IsToggled).ToString(),
                        Convert.ToInt32(FallBreak.IsToggled).ToString(), Convert.ToInt32(WinterBreak.IsToggled).ToString(), Convert.ToInt32(SpringBreak.IsToggled).ToString(),
                        AvailableVisitOther.Text, Convert.ToInt32(HaveLicense.IsToggled).ToString(), Convert.ToInt32(HaveCar.IsToggled).ToString(), Convert.ToInt32(AgreeToStatement.IsToggled).ToString(),
                        Convert.ToInt32(AgreeToDoComponents.IsToggled).ToString(), Convert.ToInt32(CanAttendTraining.IsToggled).ToString(), Convert.ToInt32(Male.IsToggled).ToString(),
                        Convert.ToInt32(Female.IsToggled).ToString(), GenderOther.Text, Convert.ToInt32(FirstToCollege.IsToggled).ToString(), Convert.ToInt32(OfOrigin.IsToggled).ToString(),
                        Convert.ToInt32(AfricanAmerican.IsToggled).ToString(), Convert.ToInt32(AmericanIndian_Or_AlaskanNative.IsToggled).ToString(), Convert.ToInt32(PacificIslander.IsToggled).ToString(),
                        Convert.ToInt32(Asian.IsToggled).ToString(), Convert.ToInt32(Caucasian.IsToggled).ToString(), RaceOther.Text, Convert.ToInt32(RuralCommunity.IsToggled).ToString(),
                        Convert.ToInt32(SuburbanCommunity.IsToggled).ToString(), Convert.ToInt32(UrbanCommunity.IsToggled).ToString(), PrivacyPolicy);
                    await Navigation.PopToRootAsync().ConfigureAwait(true);
                }
            }
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PopToRootAsync().ConfigureAwait(true);
        }
	}
}