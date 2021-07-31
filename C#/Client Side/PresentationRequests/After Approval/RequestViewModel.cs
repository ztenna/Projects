//using Xamarin.Forms;
//using System.ComponentModel;
using System.Net.Http;
using System.Collections.Generic;

namespace SPOT_App.ViewModels
{
    // This class defines an object that can be used to store all the information for a single request.
    // It is called "RequestViewModel" instead of "Request" because I was trying (perhaps unsuccessfully) to follow the View <-> ViewModel <-> Model design.
    // See this link for more information: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#the-mvvm-pattern
    public class RequestViewModel
	{
        // As I understand it, these "get set" functions are used by the View of the MVVM pattern to be able to access the data members of this class.
        // Without them, it would not be possible to use something like:
        // <Label Text="{Binding OrganizationName}" FontSize="{StaticResource contentFontSize}" TextColor="{StaticResource contentTextColor}" HorizontalOptions="Start" VerticalOptions="Center"/>
        // in a .xaml file because the "{Binding OrganizationName}" would not be able to "get" the information.
        // Take this with some salt though -- I'm not comfortable with this yet.
        //public string ID { get; set; }
        public string PresentationID { get; set; }
        public string TimeDateCreated { get; set; }
        public string Name { get; set; }
        public string OrgName { get; set; }
        public string TeacherEmail { get; set; }
        public string AmbassadorEmail { get; set; }
        public string OtherAmbassadorEmails { get; set; }
        public string PhoneNumber { get; set; }
        public string AltPhoneNumber { get; set; }
        public string ContactDayTime { get; set; }
        public string PresLocation { get; set; }
        public string RequestedPres { get; set; }
        public string PresRotations { get; set; }
        public string HandsOnActivities { get; set; }
        public string GradeLevels { get; set; }
        public string NumberStudents { get; set; }
        public string ProposedDateTime { get; set; }
        public string Supplies { get; set; }
        public string TravelFee { get; set; }
        public string AgreeStatement { get; set; }
        public string Concerns { get; set; }
        public string AltPres { get; set; }
        // This function returns a "FormUrlEncodedContent" object constructed from an array of KeyValuePair objects.
        // Each KeyValuePair object is constructed from the name and contents of each get/set function of this RequestViewModel class.
        // The object returned by this function is then send (by the RestService class) as part of POST request to the PHP web service.
        public FormUrlEncodedContent GetFormUrlEncodedContent()
        {            
            // This is "shorthand" code that constructs/initializes the FormUrlEncodedContent object -- see the slightly-longer-but-clearer commented out equivalent code below if you are confused.
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("PresentationID", this.PresentationID),
                new KeyValuePair<string, string>("TimeDateCreated", this.TimeDateCreated),
                new KeyValuePair<string, string>("Name", this.Name),
                new KeyValuePair<string, string>("OrgName", this.OrgName),
                new KeyValuePair<string, string>("TeacherEmail", this.TeacherEmail),
                new KeyValuePair<string, string>("AmbassadorEmail", this.AmbassadorEmail), 
                new KeyValuePair<string, string>("OtherAmbassadorEmails", this.OtherAmbassadorEmails),
                new KeyValuePair<string, string>("PhoneNumber", this.PhoneNumber),
                new KeyValuePair<string, string>("AltPhoneNumber", this.AltPhoneNumber),
                new KeyValuePair<string, string>("ContactDayTime", this.ContactDayTime),
                new KeyValuePair<string, string>("PresLocation", this.PresLocation),
                new KeyValuePair<string, string>("RequestedPres", this.RequestedPres),
                new KeyValuePair<string, string>("PresRotations", this.PresRotations),
                new KeyValuePair<string, string>("HandsOnActivities", this.HandsOnActivities),
                new KeyValuePair<string, string>("GradeLevels", this.GradeLevels),
                new KeyValuePair<string, string>("NumberStudents", this.NumberStudents),
                new KeyValuePair<string, string>("ProposedDateTime", this.ProposedDateTime),
                new KeyValuePair<string, string>("Supplies", this.Supplies),
                new KeyValuePair<string, string>("TravelFee", this.TravelFee),
                new KeyValuePair<string, string>("Concerns", this.Concerns),
                new KeyValuePair<string, string>("AltPres", this.AltPres)
            }); 

            return formUrlEncodedContent;
        }

        public string GetContents() // Utility function to quickly get contents of all data members.
        {
            return
                "PresentationID:" + PresentationID + "\n" +
                "TimeDateCreated:" + TimeDateCreated + "\n" +
                "Name:" + Name + "\n" +
                "OrgName:" + OrgName + "\n" +
                "Email:" + TeacherEmail + "\n" +
                "AmbassadorEmail:" + AmbassadorEmail + "\n" +
                "OtherAmbassadorEmails:" + OtherAmbassadorEmails + "\n" +
                "PhoneNumber:" + PhoneNumber + "\n" +
                "AltPhoneNumber:" + AltPhoneNumber + "\n" +
                "ContactDayTime:" + ContactDayTime + "\n" +
                "PresLocation:" + PresLocation + "\n" +
                "RequestedPres:" + RequestedPres + "\n" +
                "PresRotations:" + PresRotations + "\n" +
                "HandsOnActivities:" + HandsOnActivities + "\n" +
                "GradeLevels:" + GradeLevels + "\n" +
                "NumberStudents:" + NumberStudents + "\n" +
                "ProposedDateTime:" + ProposedDateTime + "\n" +
                "Supplies:" + Supplies + "\n" +
                "TravelFee:" + TravelFee + "\n" +
                "AgreeStatement:" + AgreeStatement + "\n" +
                "Concerns:" + Concerns + "\n" +
                "AltPres:" + AltPres + "\n";
        }
    }   
}