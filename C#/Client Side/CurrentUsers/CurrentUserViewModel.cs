//using Xamarin.Forms;
//using System.ComponentModel;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace SPOT_App.ViewModels
{
    // This class defines an object that can be used to store all the information for a single user.
    // It is called "CurrentUserViewModel" instead of "CurrentUser" because I was trying (perhaps unsuccessfully) to follow the View <-> ViewModel <-> Model design.
    // See this link for more information: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#the-mvvm-pattern
    public class CurrentUserViewModel
    {
        // As I understand it, these "get set" functions are used by the View of the MVVM pattern to be able to access the data members of this class.
        // Without them, it would not be possible to use something like:
        // <Label Text="{Binding OrganizationName}" FontSize="{StaticResource contentFontSize}" TextColor="{StaticResource contentTextColor}" HorizontalOptions="Start" VerticalOptions="Center"/>
        // in a .xaml file because the "{Binding OrganizationName}" would not be able to "get" the information.
        // Take this with some salt though -- I'm not comfortable with this yet.

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IsAmbassador { get; set; }
        public string IsAdmin { get; set; }
        public string CurrentStatus { get; set; }
        public string PrivacyPolicy { get; set; }

        public string GetContents() // Utility function to quickly get contents of all data members.
        {
            return
                "Name:" + Name + "\n" +
                "FirstName: " + "\n" +
                "LastName: " + "\n" +
                "Email:" + Email + "\n" +
                "PhoneNumber:" + PhoneNumber + "\n" +
                "IsAmbassador: " + "\n" +
                "IsAdmin: " + "\n" +
                "PrivacyPolicy: " + PrivacyPolicy;
        }

        public void ChangeVariables()
        {
            if (LastName == null)
                Name = FirstName;
            else
                Name = FirstName + " " + LastName;

            if (IsAmbassador.Equals("1"))
                CurrentStatus = "Ambassador";
            else
                CurrentStatus = "Admin";
        }
    }
}