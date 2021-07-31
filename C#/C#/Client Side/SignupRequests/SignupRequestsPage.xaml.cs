using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the SignupRequestViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Collections.Generic;
using System;

namespace SPOT_App
{
    // This class defines how the list of SignupRequestViewModel objects will be shown in the GUI.
    // Temporarily, it constructs 20 SignupRequestViewModel objects and stores them in an "ObservableCollection".
    // This ObservableCollection is used by the ListView "signupRequests" (which is declared in the SignupRequestsPage.xaml file NOT in this file -- 
    // I think it is also possible to define requests in this file, but I didn't have time to figure it out) to display the SignupRequestViewModel objects.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupRequestsPage : ContentPage
    {
        //============================================================
        //Data Memebers
        ObservableCollection<SignupRequestViewModel> RequestCollection { get; set; }
        readonly RestService restService;
        readonly User user;
        private int firstNumOfRequests=0;

        //============================================================
        public SignupRequestsPage(RestService restService, User user)
        {
            InitializeComponent();
            this.restService = restService;
            this.user = user;
   
            ConstructRequestView();
        }
        private async void ConstructRequestView()
        {
            //List that stores all of the cells that is being displayed.
            RequestCollection = new ObservableCollection<SignupRequestViewModel> { }; // This variable will contain the SignupRequestViewModel objects that store request data.

            //A list that contains all the data that needs to be displayed.
            List<SignupRequestViewModel> rvmList= await restService.GetSignupRequestData(10, firstNumOfRequests).ConfigureAwait(true);
            
            //Filling out the cells using rvmList that is also being filled in inside this loop.
            foreach (SignupRequestViewModel rvm in rvmList)
            {
                rvm.ChangeVariables();
                RequestCollection.Add(new SignupRequestViewModel
                {
                    Name = rvm.Name,
                    Email = rvm.Email,
                    PhoneNumber = rvm.PhoneNumber,
                    Address = rvm.Address,
                    School = rvm.School,
                    Major = rvm.Major,
                    HearAboutSpot = rvm.HearAboutSpot,
                    WhySpotPresenter = rvm.WhySpotPresenter,
                    ExpWithStudents = rvm.ExpWithStudents,
                    NotAsPlanned = rvm.NotAsPlanned,
                    RecommendationLetterName = rvm.RecommendationLetterName,
                    RecommendationLetterEmail = rvm.RecommendationLetterEmail,
                    DietaryRestrictions = rvm.DietaryRestrictions,
                    CurrentApplicantStatus = rvm.CurrentApplicantStatus,
                    AvailableToVisit = rvm.AvailableToVisit,
                    HaveLicense = rvm.HaveLicense,
                    HaveCar = rvm.HaveCar,
                    CanAttendTraining = rvm.CanAttendTraining,
                    Gender = rvm.Gender,
                    FirstToCollege = rvm.FirstToCollege,
                    OfOrigin = rvm.OfOrigin,
                    Race = rvm.Race,
                    Community = rvm.Community,
                    PrivacyPolicy = rvm.PrivacyPolicy
                });

            }

            signupRequests.ItemTemplate = new DataTemplate(typeof(SignupRequestCell)); // The "SignupRequestCell" defines how the SignupRequestViewModel objects will be displayed by the GUI (the View).
            signupRequests.HasUnevenRows = true;
            signupRequests.SeparatorColor = Color.Black;
            signupRequests.ItemsSource = RequestCollection; // "signupRequests" is the ListView defined in the SignupRequestPage.xaml file -- the ItemsSource property tells this ListView where it should get information for its list items from.
            
            //This lane just make the list go back to the top. (mainly for when you press next and back)
            signupRequests.ScrollTo(RequestCollection[0], ScrollToPosition.Start, true);
        }

        async void Handle_RequestTapped(object sender, ItemTappedEventArgs e)
        {
           //sender.GetType();

            if (e.Item == null)
                return;
            
            //await DisplayAlert("Item Tapped", "An item was tapped." + "the type of the sender was: " + sender.GetType() + " " + sender.GetHashCode(), "OK");

            SignupRequestViewModel selectedRequestViewModel = (SignupRequestViewModel)((ListView)sender).SelectedItem; // This line lets me store the SignupRequestViewModel object that was tapped by the user.

            //await DisplayAlert("x", "x is " + selectedRequestViewModel.GetType(), "ok");

            //await DisplayAlert("************", selectedRequestViewModel.GetContents(), "ok");

            await Navigation.PushAsync(new SignupRequestDetailPage(selectedRequestViewModel, restService, user)).ConfigureAwait(true); // Here I am passing the SignupRequestViewModel object to the SignupRequestDetailPage (this page will then display ALL the contents of the SignupRequestViewModel object).

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        // The following class extends the ViewCell class to create a custom GUI for each SignupRequestViewModel in the "requests" ListView.
        // Changing the contents of this class will affect how ALL SignupRequestViewModels in the ListView are displayed.
        // This code -- most likely -- could have all been written in the SignupRequestsPage.xaml file.
      
        //Function behind the "Back" button on the SignupRequestsPage.xaml. It queries the db in increments of 10. 
        //When pressed: grabs and displays previous 10 requests in db.
        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            //Making sure that it does not go under 0
            if (firstNumOfRequests - 10 >= 0)
            {
                firstNumOfRequests -= 10;
                ConstructRequestView();
            }
        }
        //Function behind the "Next" button on the SignupRequestsPage.xaml 
        //When pressed: grabs and displays the next 10 requests.
        private void Next_Button_Clicked(object sender, EventArgs e)
        {
            if(RequestCollection.Count >= 10)
            {
                firstNumOfRequests += 10;
                ConstructRequestView();
            }
            else
            {
                DisplayAlert("End of the list.", "Sorry, but there are no more requests to display.", "Close");
            } 
        }

        async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(restService, user)).ConfigureAwait(true);
        }
    }
    public class SignupRequestCell : ViewCell
        {
            public SignupRequestCell()
            {
                var grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var nameHeader = new Label();
                var applicantHeader = new Label();

                var name = new Label();
                var applicantStatus = new Label();

                nameHeader.Text = "Name:";
                applicantHeader.Text = "Applicant Is:";

                nameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                nameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
                applicantHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                applicantHeader.HorizontalOptions = LayoutOptions.StartAndExpand;

                nameHeader.FontSize = 14;
                applicantHeader.FontSize = 14;

                name.SetBinding(Label.TextProperty, new Binding("Name"));
                applicantStatus.SetBinding(Label.TextProperty, new Binding("CurrentApplicantStatus"));

                name.FontSize = 14;
                applicantStatus.FontSize = 14;

                name.TextColor = Color.Black;
                applicantStatus.TextColor = Color.Black;

                name.VerticalOptions = LayoutOptions.CenterAndExpand;
                name.HorizontalOptions = LayoutOptions.StartAndExpand;
                applicantStatus.VerticalOptions = LayoutOptions.CenterAndExpand;
                applicantStatus.HorizontalOptions = LayoutOptions.StartAndExpand;

                grid.Children.Add(nameHeader, 0, 0);
                grid.Children.Add(name, 1, 0);
                grid.Children.Add(applicantHeader, 0, 1);
                grid.Children.Add(applicantStatus, 1, 1);

                View = grid; // This sets the View of the parent object (which I think is the ViewCell I am extending -- not quite sure yet).
                }
              
        }

}