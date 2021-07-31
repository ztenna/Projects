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
	public partial class PresentationReportingPage : ContentPage
	{
        readonly RestService rs;
        readonly User user;
        int travelFeeCount = 0;
        int numUniversePres;
        int numWaterPres;
        int numStarPres;
        int numPlanetPres;
        int numMarsPres;
        int numStationPres;
        int numTelescopePres;
        int numNANOGravPres;
        int numMentorPres;
        int numSolarActs;
        int numMoonActs;
        int numEngDesignActs;
        int numRobArmActs;
        int numElectroWarActs;
        int numAlienActs;
        int numMoleculeActs;
        int numBucketActs;
        int numWatershedActs;
        int num_KTo3_Stu;
        int num_4To5_Stu;
        int num_6To8_Stu;
        int num_9To12_Stu;
        int numCollegeStu;
        int numTeachers;
        int engagedRating;
        int appropriateRating;
        string travelFeeGoodToGo = "";

        public PresentationReportingPage (RestService rs, User user)
		{
            this.rs = rs;
            this.user = user;
            InitializeComponent();
		}
        private async void Submit_Button_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Name.Text) || String.IsNullOrEmpty(Driver.Text) || String.IsNullOrEmpty(OrgName.Text) ||
                String.IsNullOrEmpty(PresDate.Text) || String.IsNullOrEmpty(FirstTime.Text) || String.IsNullOrEmpty(AskedQuestions.Text) ||
                String.IsNullOrEmpty(AnswerQuestions.Text))
                await DisplayAlert("Submit", "Fields can't be empty. Please fill in user information.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumUniversePres.Text) && (NumUniversePres.Text.Length != 1 || !int.TryParse(NumUniversePres.Text, out numUniversePres) || numUniversePres < 1 || numUniversePres > 8))
                await DisplayAlert("Submit", "The number of 'The Invisible Universe 2.0' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumWaterPres.Text) && (NumWaterPres.Text.Length != 1 || !int.TryParse(NumWaterPres.Text, out numWaterPres) || numWaterPres < 1 || numWaterPres > 8))
                await DisplayAlert("Submit", "The number of 'Water: The Source of Life' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumStarPres.Text) && (NumStarPres.Text.Length != 1 || !int.TryParse(NumStarPres.Text, out numStarPres) || numStarPres < 1 || numStarPres > 8))
                await DisplayAlert("Submit", "The number of 'The Star in Our Lives' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumPlanetPres.Text) && (NumPlanetPres.Text.Length != 1 || !int.TryParse(NumPlanetPres.Text, out numPlanetPres) || numPlanetPres < 1 || numPlanetPres > 8))
                await DisplayAlert("Submit", "The number of 'How to Make a Planet... with Life!' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumMarsPres.Text) && (NumMarsPres.Text.Length != 1 || !int.TryParse(NumMarsPres.Text, out numMarsPres) || numMarsPres < 1 || numMarsPres > 8))
                await DisplayAlert("Submit", "The number of 'Mars: Past, Present, Future' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumStationPres.Text) && (NumStationPres.Text.Length != 1 || !int.TryParse(NumStationPres.Text, out numStationPres) || numStationPres < 1 || numStationPres > 8))
                await DisplayAlert("Submit", "The number of 'The International Space Station' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumTelescopePres.Text) && (NumTelescopePres.Text.Length != 1 || !int.TryParse(NumTelescopePres.Text, out numTelescopePres) || numTelescopePres < 1 || numTelescopePres > 8))
                await DisplayAlert("Submit", "The number of 'Space Telescopes: Searching for Other Worlds' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumNANOGravPres.Text) && (NumNANOGravPres.Text.Length != 1 || !int.TryParse(NumNANOGravPres.Text, out numNANOGravPres) || numNANOGravPres < 1 || numNANOGravPres > 8))
                await DisplayAlert("Submit", "The number of 'NANOGrav: Tuning in to Einstein's Universe' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumMentorPres.Text) && (NumMentorPres.Text.Length != 1 || !int.TryParse(NumMentorPres.Text, out numMentorPres) || numMentorPres < 1 || numMentorPres > 8))
                await DisplayAlert("Submit", "The number of 'Mountaineer Mentors' presentations must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumSolarActs.Text) && (NumSolarActs.Text.Length != 1 || !int.TryParse(NumSolarActs.Text, out numSolarActs) || numSolarActs < 1 || numSolarActs > 8))
                await DisplayAlert("Submit", "The number of 'Pocket Solar System' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumMoonActs.Text) && (NumMoonActs.Text.Length != 1 || !int.TryParse(NumMoonActs.Text, out numMoonActs) || numMoonActs < 1 || numMoonActs > 8))
                await DisplayAlert("Submit", "The number of 'Sizing up the Moon' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumEngDesignActs.Text) && (NumEngDesignActs.Text.Length != 1 || !int.TryParse(NumEngDesignActs.Text, out numEngDesignActs) || numEngDesignActs < 1 || numEngDesignActs > 8))
                await DisplayAlert("Submit", "The number of 'GBT Engineering Design Challenge' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumRobArmActs.Text) && (NumRobArmActs.Text.Length != 1 || !int.TryParse(NumRobArmActs.Text, out numRobArmActs) || numRobArmActs < 1 || numRobArmActs > 8))
                await DisplayAlert("Submit", "The number of 'Programming a Robotic Arm' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumElectroWarActs.Text) && (NumElectroWarActs.Text.Length != 1 || !int.TryParse(NumElectroWarActs.Text, out numElectroWarActs) || numElectroWarActs < 1 || numElectroWarActs > 8))
                await DisplayAlert("Submit", "The number of 'Electromagnetic War' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumAlienActs.Text) && (NumAlienActs.Text.Length != 1 || !int.TryParse(NumAlienActs.Text, out numAlienActs) || numAlienActs < 1 || numAlienActs > 8))
                await DisplayAlert("Submit", "The number of 'Design an Alien' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumMoleculeActs.Text) && (NumMoleculeActs.Text.Length != 1 || !int.TryParse(NumMoleculeActs.Text, out numMoleculeActs) || numMoleculeActs < 1 || numMoleculeActs > 8))
                await DisplayAlert("Submit", "The number of 'Build a Molecule' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumBucketActs.Text) && (NumBucketActs.Text.Length != 1 || !int.TryParse(NumBucketActs.Text, out numBucketActs) || numBucketActs < 1 || numBucketActs > 8))
                await DisplayAlert("Submit", "The number of 'A Drop in the Bucket' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumWatershedActs.Text) && (NumWatershedActs.Text.Length != 1 || !int.TryParse(NumWatershedActs.Text, out numWatershedActs) || numWatershedActs < 1 || numWatershedActs > 8))
                await DisplayAlert("Submit", "The number of 'Build-Your-Own Watershed' activities must be numbers between 1 and 8.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(Num_KTo3_Stu.Text) && !int.TryParse(Num_KTo3_Stu.Text, out num_KTo3_Stu))
                await DisplayAlert("Submit", "The number of K-3 students must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(Num_4To5_Stu.Text) && !int.TryParse(Num_4To5_Stu.Text, out num_4To5_Stu))
                await DisplayAlert("Submit", "The number of 4-5 students must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(Num_6To8_Stu.Text) && !int.TryParse(Num_6To8_Stu.Text, out num_6To8_Stu))
                await DisplayAlert("Submit", "The number of 6-8 students must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(Num_9To12_Stu.Text) && !int.TryParse(Num_9To12_Stu.Text, out num_9To12_Stu))
                await DisplayAlert("Submit", "The number of 9-12 students must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumCollegeStu.Text) && !int.TryParse(NumCollegeStu.Text, out numCollegeStu))
                await DisplayAlert("Submit", "The number of college students must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(NumTeachers.Text) && !int.TryParse(NumTeachers.Text, out numTeachers))
                await DisplayAlert("Submit", "The number of teachers must be a number.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(EngagedRating.Text) && (EngagedRating.Text.Length != 1 || !int.TryParse(EngagedRating.Text, out engagedRating) || engagedRating < 0 || engagedRating > 4))
                await DisplayAlert("Submit", "The rating of how engaged and interested the students were in the show must be a number between 1 and 4.", "Ok").ConfigureAwait(true);

            else if (!String.IsNullOrEmpty(AppropriateRating.Text) && (AppropriateRating.Text.Length != 1 || !int.TryParse(AppropriateRating.Text, out appropriateRating) || appropriateRating < 0 || appropriateRating > 4))
                await DisplayAlert("Submit", "The rating of how appropriate the presentation appeared to be for the students must be a number between 1 and 4.", "Ok").ConfigureAwait(true);

            else if (travelFeeCount == 0)
            {
                if (TravelFeeRecDir.IsToggled)
                    travelFeeCount++;
                if (TravelFeeNoNeed.IsToggled)
                    travelFeeCount++;
                if (TravelFeeFollowUp.IsToggled)
                    travelFeeCount++;
                if (travelFeeCount > 1)
                    await DisplayAlert("Submit", "You can only choose one travel fee option.", "Ok").ConfigureAwait(true);
                else
                    travelFeeGoodToGo = "Yes";
                travelFeeCount = 0;
            }

            if (travelFeeGoodToGo.Equals("Yes"))
            {
                rs.SubmitPresReport(Name.Text, Driver.Text, OrgName.Text, ContactPerson.Text, Convert.ToInt32(TravelFeeRecDir.IsToggled).ToString(), Convert.ToInt32(TravelFeeNoNeed.IsToggled).ToString(),
                    Convert.ToInt32(TravelFeeFollowUp.IsToggled).ToString(), PresDate.Text, numUniversePres.ToString(), numWaterPres.ToString(), numStarPres.ToString(), numPlanetPres.ToString(),
                    numMarsPres.ToString(), numStationPres.ToString(), numTelescopePres.ToString(), numNANOGravPres.ToString(), numMentorPres.ToString(), numSolarActs.ToString(), numMoonActs.ToString(),
                    numEngDesignActs.ToString(), numRobArmActs.ToString(), numElectroWarActs.ToString(), numAlienActs.ToString(), numMoleculeActs.ToString(), numBucketActs.ToString(), numWatershedActs.ToString(),
                    Convert.ToInt32(SameEverything.IsToggled).ToString(), AnythingDiff.Text, FirstTime.Text, num_KTo3_Stu.ToString(), num_4To5_Stu.ToString(), num_6To8_Stu.ToString(), num_9To12_Stu.ToString(),
                    numCollegeStu.ToString(), numTeachers.ToString(), OtherAudience.Text, engagedRating.ToString(), appropriateRating.ToString(), AskedQuestions.Text, AnswerQuestions.Text, AspectsWorkBest.Text,
                    PresSuggestions.Text);
                await Navigation.PushAsync(new HomePage(rs, user)).ConfigureAwait(true);
            }
        }
        private async void Cancel_Button_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new HomePage(rs, user)).ConfigureAwait(true);
        }
	}
}