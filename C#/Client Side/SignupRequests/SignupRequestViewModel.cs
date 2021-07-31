//using Xamarin.Forms;
//using System.ComponentModel;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace SPOT_App.ViewModels
{
    // This class defines an object that can be used to store all the information for a single request.
    // It is called "SignupRequestViewModel" instead of "SignupRequest" because I was trying (perhaps unsuccessfully) to follow the View <-> ViewModel <-> Model design.
    // See this link for more information: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#the-mvvm-pattern
    public class SignupRequestViewModel
    {
        // As I understand it, these "get set" functions are used by the View of the MVVM pattern to be able to access the data members of this class.
        // Without them, it would not be possible to use something like:
        // <Label Text="{Binding OrganizationName}" FontSize="{StaticResource contentFontSize}" TextColor="{StaticResource contentTextColor}" HorizontalOptions="Start" VerticalOptions="Center"/>
        // in a .xaml file because the "{Binding OrganizationName}" would not be able to "get" the information.
        // Take this with some salt though -- I'm not comfortable with this yet.
        //public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string School { get; set; }
        public string Major { get; set; }
        public string HearAboutSpot { get; set; }
        public string WhySpotPresenter { get; set; }
        public string ExpWithStudents { get; set; }
        public string NotAsPlanned { get; set; }
        public string RecommendationLetterName { get; set; }
        public string RecommendationLetterEmail { get; set; }
        public string DietaryRestrictions { get; set; }
        public string IsNewApplicant { get; set; }
        public string IsCurrentAmbassador { get; set; }
        public string CurrentApplicantStatus { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string FallBreak { get; set; }
        public string WinterBreak { get; set; }
        public string SpringBreak { get; set; }
        public string AvailableVisitOther { get; set; }
        public string AvailableToVisit { get; set; }
        public string HaveLicense { get; set; }
        public string HaveCar { get; set; }
        public string AgreeToStatements { get; set; }
        public string AgreeToDoComponents { get; set; }
        public string CanAttendTraining { get; set; }
        public string Male { get; set; }
        public string Female { get; set; }
        public string GenderOther { get; set; }
        public string Gender { get; set; }
        public string FirstToCollege { get; set; }
        public string OfOrigin { get; set; }
        public string AfricanAmerican { get; set; }
        public string AmericanIndianOrAlaskanNative { get; set; }
        public string PacificIslander { get; set; }
        public string Asian { get; set; }
        public string Caucasian { get; set; }
        public string RaceOther { get; set; }
        public string Race { get; set; }
        public string RuralCommunity { get; set; }
        public string SuburbanCommunity { get; set; }
        public string UrbanCommunity { get; set; }
        public string Community { get; set; }
        public string PrivacyPolicy { get; set; }

        public string GetContents() // Utility function to quickly get contents of all data members.
        {
            return
                "Name:" + Name + "\n" +
                "Email:" + Email + "\n" +
                "PhoneNumber:" + PhoneNumber + "\n" +
                "Address:" + Address + "\n" +
                "School:" + School + "\n" +
                "Major:" + Major + "\n" +
                "HearAboutSpot:" + HearAboutSpot + "\n" +
                "WhySpotPresenter:" + WhySpotPresenter + "\n" +
                "ExpWithStudents:" + ExpWithStudents + "\n" +
                "NotAsPlanned:" + NotAsPlanned + "\n" +
                "RecommendationLetterName:" + RecommendationLetterName + "\n" +
                "RecommendationLetterEmail:" + RecommendationLetterEmail + "\n" +
                "DietaryRestrictions:" + DietaryRestrictions + "\n" +
                "IsNewApplicant:" + IsNewApplicant + "\n" +
                "IsCurrentAmbassador:" + IsCurrentAmbassador + "\n" +
                "Monday:" + Monday + "\n" +
                "Tuesday:" + Tuesday + "\n" +
                "Wednesday:" + Wednesday + "\n" +
                "Thursday:" + Thursday + "\n" +
                "Friday:" + Friday + "\n" +
                "FallBreak:" + FallBreak + "\n" +
                "WinterBreak:" + WinterBreak + "\n" +
                "SpringBreak:" + SpringBreak + "\n" +
                "AvailableVisitOther:" + AvailableVisitOther + "\n" +
                "HaveLicense:" + HaveLicense + "\n" +
                "HaveCar:" + HaveCar + "\n" +
                "AgreeToStatements:" + AgreeToStatements + "\n" +
                "AgreeToDoComponents:" + AgreeToDoComponents + "\n" +
                "CanAttendTraining:" + CanAttendTraining + "\n" +
                "Male:" + Male + "\n" +
                "Female:" + Female + "\n" +
                "GenderOther:" + GenderOther + "\n" +
                "FirstToCollege:" + FirstToCollege + "\n" +
                "OfOrigin:" + OfOrigin + "\n" +
                "AfricanAmerican:" + AfricanAmerican + "\n" +
                "AmericanIndianOrAlaskanNative:" + AmericanIndianOrAlaskanNative + "\n" +
                "PacificIslander:" + PacificIslander + "\n" +
                "Asian:" + Asian + "\n" +
                "Caucasian:" + Caucasian + "\n" +
                "RaceOther:" + RaceOther + "\n" +
                "RuralCommunity:" + RuralCommunity + "\n" +
                "SuburbanCommunity:" + SuburbanCommunity + "\n" +
                "UrbanCommunity:" + UrbanCommunity + "\n" +
                "PrivacyPolicy:" + PrivacyPolicy;
        }

        public void ChangeVariables()
        {
            AvailableToVisit = "";

            if (IsNewApplicant.Equals("1"))
                CurrentApplicantStatus = "New Applicant";
            else
                CurrentApplicantStatus = "Current Ambassador";

            if (Monday.Equals("1"))
                AvailableToVisit += "Monday\n";
            if (Tuesday.Equals("1"))
                AvailableToVisit += "Tuesday\n";
            if (Wednesday.Equals("1"))
                AvailableToVisit += "Wednesday\n";
            if (Thursday.Equals("1"))
                AvailableToVisit += "Thursday\n";
            if (Friday.Equals("1"))
                AvailableToVisit += "Friday\n";
            if (FallBreak.Equals("1"))
                AvailableToVisit += "Fall Break\n";
            if (WinterBreak.Equals("1"))
                AvailableToVisit += "Winter Break\n";
            if (SpringBreak.Equals("1"))
                AvailableToVisit += "Spring Break\n";
            if (AvailableVisitOther != null)
                AvailableToVisit += AvailableVisitOther;

            if (HaveLicense.Equals("1"))
                HaveLicense = "Yes";
            else
                HaveLicense = "No";

            if (HaveCar.Equals("1"))
                HaveCar = "Yes";
            else
                HaveCar = "No";

            if (CanAttendTraining.Equals("1"))
                CanAttendTraining = "Yes";
            else
                CanAttendTraining = "No";

            if (Male.Equals("1"))
                Gender = "Male";
            else if (Female.Equals("1"))
                Gender = "Female";
            else 
                Gender = GenderOther;

            if (FirstToCollege != null)
            {
                if (FirstToCollege.Equals("1"))
                    FirstToCollege = "Yes";
                else
                    FirstToCollege = "No";
            }

            if (OfOrigin != null)
            {
                if (OfOrigin.Equals("1"))
                    OfOrigin = "Yes";
                else
                    OfOrigin = "No";
            }

            if (AfricanAmerican != null && AfricanAmerican.Equals("1"))
                Race = "African American";
            else if (AmericanIndianOrAlaskanNative != null && AmericanIndianOrAlaskanNative.Equals("1"))
                Race = "American Indian or Alaskan Native";
            else if (PacificIslander != null && PacificIslander.Equals("1"))
                Race = "Pacific Islander";
            else if (Asian != null && Asian.Equals("1"))
                Race = "Asian";
            else if (Caucasian != null && Caucasian.Equals("1"))
                Race = "Caucasian";
            else if (RaceOther != null)
                Race = RaceOther;

            if (RuralCommunity != null && RuralCommunity.Equals("1"))
                Community = "Rural Community";
            else if (SuburbanCommunity != null && SuburbanCommunity.Equals("1"))
                Community = "Suburban Community";
            else if (UrbanCommunity != null && UrbanCommunity.Equals("1"))
                Community = "Urban Community";
        }
    }
}