using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the RequestViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Collections.Generic;
using System;

namespace SPOT_App
{
    // This class defines how the list of RequestViewModel objects will be shown in the GUI.
    // Temporarily, it constructs 20 RequestViewModel objects and stores them in an "ObservableCollection".
    // This ObservableCollection is used by the ListView "requests" (which is declared in the RequestsPage.xaml file NOT in this file -- 
    // I think it is also possible to define requests in this file, but I didn't have time to figure it out) to display the RequestViewModel objects.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        //============================================================
        //Data Memebers
        ObservableCollection<RequestViewModel> RequestCollection { get; set; } 
        readonly RestService restService;
        readonly User user;
        private int firstNumOfRequests=0;

        //============================================================
        public RequestsPage(RestService restService, User user)
        {
            InitializeComponent();
            this.restService = restService;
            this.user = user;

            ConstructRequestView();
        }
        private async void ConstructRequestView()
        {
            //List that stores all of the cells that is being displayed.
            RequestCollection = new ObservableCollection<RequestViewModel> { }; // This variable will contain the RequestViewModel objects that store request data.

            //A list that contains all the data that needs to be displayed.
            List<RequestViewModel> rvmList= await restService.GetRequestData(10, firstNumOfRequests).ConfigureAwait(true);
            
            //Filling out the cells using rvmList that is also being filled in inside this loop.
            foreach (RequestViewModel rvm in rvmList)
            {
                RequestCollection.Add(new RequestViewModel
                {
                    PresentationID = rvm.PresentationID,
                    TimeDateCreated = rvm.TimeDateCreated,
                    Name = rvm.Name,
                    OrgName = rvm.OrgName,
                    TeacherEmail = rvm.TeacherEmail,
                    AmbassadorEmail = rvm.AmbassadorEmail,
                    OtherAmbassadorEmails = rvm.OtherAmbassadorEmails,
                    PhoneNumber = rvm.PhoneNumber,
                    AltPhoneNumber = rvm.AltPhoneNumber,
                    ContactDayTime = rvm.ContactDayTime,
                    PresLocation = rvm.PresLocation,
                    RequestedPres = rvm.RequestedPres,
                    PresRotations = rvm.PresRotations,
                    HandsOnActivities = rvm.HandsOnActivities,
                    GradeLevels = rvm.GradeLevels,
                    NumberStudents = rvm.NumberStudents,
                    ProposedDateTime = rvm.ProposedDateTime,
                    Supplies = rvm.Supplies,
                    TravelFee = rvm.TravelFee,
                    AgreeStatement = rvm.AgreeStatement,
                    Concerns = rvm.Concerns,
                    AltPres = rvm.AltPres
                });
            }
            
            requests.ItemTemplate = new DataTemplate(typeof(RequestCell)); // The "RequestCell" defines how the RequestViewModel objects will be displayed by the GUI (the View).
            requests.HasUnevenRows = true;
            requests.SeparatorColor = Color.Black;
            requests.ItemsSource = RequestCollection; // "requests" is the ListView defined in the RequestPage.xaml file -- the ItemsSource property tells this ListView where it should get information for its list items from.
            
            //This lane just make the list go back to the top. (mainly for when you press next and back)
            requests.ScrollTo(RequestCollection[0], ScrollToPosition.Start, true);
        }
          
        async void Handle_RequestTapped(object sender, ItemTappedEventArgs e)
        {
           //sender.GetType();

            if (e.Item == null)
                return;
            
            //await DisplayAlert("Item Tapped", "An item was tapped." + "the type of the sender was: " + sender.GetType() + " " + sender.GetHashCode(), "OK");

            RequestViewModel selectedRequestViewModel = (RequestViewModel)((ListView)sender).SelectedItem; // This line lets me store the RequestViewModel object that was tapped by the user.

            //await DisplayAlert("x", "x is " + selectedRequestViewModel.GetType(), "ok");

            //await DisplayAlert("************", selectedRequestViewModel.GetContents(), "ok");

            await Navigation.PushAsync(new RequestDetailPage(selectedRequestViewModel, restService, user)).ConfigureAwait(true); // Here I am passing the RequestViewModel object to the RequestDetailPage (this page will then display ALL the contents of the RequestViewModel object).

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        // The following class extends the ViewCell class to create a custom GUI for each RequestViewModel in the "requests" ListView.
        // Changing the contents of this class will affect how ALL RequestViewModels in the ListView are displayed.
        // This code -- most likely -- could have all been written in the RequestsPage.xaml file.
      
        //Function behind the "Back" button on the RequestsPage.xaml. It queries the db in increments of 10. 
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
        //Function behind the "Next" button on the RequestsPage.xaml 
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

        private async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(restService, user)).ConfigureAwait(true);
        }
    }
    public class RequestCell : ViewCell
        {
            public RequestCell()
            {
                var grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var nameHeader = new Label();
                var organizationHeader = new Label();
                var locationHeader = new Label();
                var whoHasClaimedHeader = new Label();

                var name = new Label();
                var organization = new Label();
                var location = new Label();
                var whoHasClaimed = new Label();

                nameHeader.Text = "Name:";
                organizationHeader.Text = "Organization:";
                locationHeader.Text = "Location:";
                whoHasClaimedHeader.Text = "Claimed By:";

                nameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                nameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
                organizationHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                organizationHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
                locationHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                locationHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
                whoHasClaimedHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                whoHasClaimedHeader.HorizontalOptions = LayoutOptions.StartAndExpand;

                nameHeader.FontSize = 14;
                organizationHeader.FontSize = 14;
                locationHeader.FontSize = 14;
                whoHasClaimedHeader.FontSize = 14;

                name.SetBinding(Label.TextProperty, new Binding("Name"));
                organization.SetBinding(Label.TextProperty, new Binding("OrgName"));
                location.SetBinding(Label.TextProperty, new Binding("PresLocation"));
                whoHasClaimed.SetBinding(Label.TextProperty, new Binding("AmbassadorEmail"));

                name.FontSize = 14;
                organization.FontSize = 14;
                location.FontSize = 14;
                whoHasClaimed.FontSize = 14;

                name.TextColor = Color.Black;
                organization.TextColor = Color.Black;
                location.TextColor = Color.Black;
                whoHasClaimed.TextColor = Color.Black;

                name.VerticalOptions = LayoutOptions.CenterAndExpand;
                name.HorizontalOptions = LayoutOptions.StartAndExpand;
                organization.VerticalOptions = LayoutOptions.CenterAndExpand;
                organization.HorizontalOptions = LayoutOptions.StartAndExpand;
                location.VerticalOptions = LayoutOptions.CenterAndExpand;
                location.HorizontalOptions = LayoutOptions.StartAndExpand;
                whoHasClaimed.VerticalOptions = LayoutOptions.CenterAndExpand;
                whoHasClaimed.HorizontalOptions = LayoutOptions.StartAndExpand;

                grid.Children.Add(nameHeader, 0, 0);
                grid.Children.Add(name, 1, 0);
                grid.Children.Add(organizationHeader, 0, 1);
                grid.Children.Add(organization, 1, 1);
                grid.Children.Add(locationHeader, 0, 2);
                grid.Children.Add(location, 1, 2);
                grid.Children.Add(whoHasClaimedHeader, 0, 3);
                grid.Children.Add(whoHasClaimed, 1, 3);

                View = grid; // This sets the View of the parent object (which I think is the ViewCell I am extending -- not quite sure yet).
                }
              
        }

}
