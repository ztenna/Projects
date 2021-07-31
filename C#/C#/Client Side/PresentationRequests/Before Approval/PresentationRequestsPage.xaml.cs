using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the PresentationRequestViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Collections.Generic;
using System;

namespace SPOT_App
{
    // This class defines how the list of PresentationRequestViewModel objects will be shown in the GUI.
    // Temporarily, it constructs 20 PresentationRequestViewModel objects and stores them in an "ObservableCollection".
    // This ObservableCollection is used by the ListView "PresentationRequests" (which is declared in the PresentationRequestsPage.xaml file NOT in this file -- 
    // I think it is also possible to define requests in this file, but I didn't have time to figure it out) to display the PresentationRequestViewModel objects.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresentationRequestsPage : ContentPage
    {
        //============================================================
        //Data Memebers
        ObservableCollection<PresentationRequestViewModel> RequestCollection { get; set; } 
        readonly RestService restService;
        readonly User user;
        private int firstNumOfRequests=0;

        //============================================================
        public PresentationRequestsPage(RestService restService, User user)
        {
            InitializeComponent();
            this.restService = restService;
            this.user = user;
   
            ConstructRequestView();
        }
        private async void ConstructRequestView()
        {
            //List that stores all of the cells that is being displayed.
            RequestCollection = new ObservableCollection<PresentationRequestViewModel> { }; // This variable will contain the PresentationRequestViewModel objects that store request data.

            //A list that contains all the data that needs to be displayed.
            List<PresentationRequestViewModel> rvmList= await restService.GetPresentationRequestData(10, firstNumOfRequests).ConfigureAwait(true);
            
            //Filling out the cells using rvmList that is also being filled in inside this loop.
            foreach (PresentationRequestViewModel rvm in rvmList)
            {
                rvm.ChangeVariables();
                RequestCollection.Add(new PresentationRequestViewModel
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
                    PresLocation = rvm.OrgStreet + " " + rvm.OrgCity + " " + rvm.OrgState + " " + rvm.OrgZip,
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

            presentationRequests.ItemTemplate = new DataTemplate(typeof(PresentationRequestCell)); // The "PresentationRequestCell" defines how the PresentationRequestViewModel objects will be displayed by the GUI (the View).
            presentationRequests.HasUnevenRows = true;
            presentationRequests.SeparatorColor = Color.Black;
            presentationRequests.ItemsSource = RequestCollection; // "PresentationRequests" is the ListView defined in the PresentationRequestPage.xaml file -- the ItemsSource property tells this ListView where it should get information for its list items from.
            
            //This lane just make the list go back to the top. (mainly for when you press next and back)
            //presentationRequests.ScrollTo(requestCollection[0], ScrollToPosition.Start, true);
        }
          
        async void Handle_RequestTapped(object sender, ItemTappedEventArgs e)
        {
           //sender.GetType();

            if (e.Item == null)
                return;
            
            //await DisplayAlert("Item Tapped", "An item was tapped." + "the type of the sender was: " + sender.GetType() + " " + sender.GetHashCode(), "OK");

            PresentationRequestViewModel selectedRequestViewModel = (PresentationRequestViewModel)((ListView)sender).SelectedItem; // This line lets me store the PresentationRequestViewModel object that was tapped by the user.

            //await DisplayAlert("x", "x is " + selectedRequestViewModel.GetType(), "ok");

            //await DisplayAlert("************", selectedRequestViewModel.GetContents(), "ok");

            await Navigation.PushAsync(new PresentationRequestDetailPage(selectedRequestViewModel, restService, user)).ConfigureAwait(true); // Here I am passing the PresentationRequestViewModel object to the PresentationRequestDetailPage (this page will then display ALL the contents of the PresentationRequestViewModel object).

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        // The following class extends the ViewCell class to create a custom GUI for each PresentationRequestViewModel in the "requests" ListView.
        // Changing the contents of this class will affect how ALL PresentationRequestViewModels in the ListView are displayed.
        // This code -- most likely -- could have all been written in the PresentationRequestsPage.xaml file.
      
        //Function behind the "Back" button on the PresentationRequestsPage.xaml. It queries the db in increments of 10. 
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
        //Function behind the "Next" button on the PresentationRequestsPage.xaml 
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
    public class PresentationRequestCell : ViewCell
    {
        public PresentationRequestCell()
        {
            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var nameHeader = new Label();
            var timeDateCreatedHeader = new Label();
            var orgNameHeader = new Label();
            var requestedPresHeader = new Label();

            var name = new Label();
            var timeDateCreated = new Label();
            var orgName = new Label();
            var requestedPres = new Label();

            nameHeader.Text = "Name:";
            timeDateCreatedHeader.Text = "Time and Date Created:";
            orgNameHeader.Text = "Organization Name:";
            requestedPresHeader.Text = "Requested Presentation(s): ";

            nameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            nameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
            timeDateCreatedHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            timeDateCreatedHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
            orgNameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            orgNameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
            requestedPresHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            requestedPresHeader.HorizontalOptions = LayoutOptions.StartAndExpand;

            nameHeader.FontSize = 14;
            timeDateCreatedHeader.FontSize = 14;
            orgNameHeader.FontSize = 14;
            requestedPresHeader.FontSize = 14;

            name.SetBinding(Label.TextProperty, new Binding("Name"));
            timeDateCreated.SetBinding(Label.TextProperty, new Binding("TimeDateCreated"));
            orgName.SetBinding(Label.TextProperty, new Binding("OrgName"));
            requestedPres.SetBinding(Label.TextProperty, new Binding("RequestedPres"));

            name.FontSize = 14;
            timeDateCreated.FontSize = 14;
            orgName.FontSize = 14;
            requestedPres.FontSize = 14;

            name.TextColor = Color.Black;
            timeDateCreated.TextColor = Color.Black;
            orgName.TextColor = Color.Black;
            requestedPres.TextColor = Color.Black;

            name.VerticalOptions = LayoutOptions.CenterAndExpand;
            name.HorizontalOptions = LayoutOptions.StartAndExpand;
            timeDateCreated.VerticalOptions = LayoutOptions.CenterAndExpand;
            timeDateCreated.HorizontalOptions = LayoutOptions.StartAndExpand;
            orgName.VerticalOptions = LayoutOptions.CenterAndExpand;
            orgName.HorizontalOptions = LayoutOptions.StartAndExpand;
            requestedPres.VerticalOptions = LayoutOptions.CenterAndExpand;
            requestedPres.HorizontalOptions = LayoutOptions.StartAndExpand;

            grid.Children.Add(nameHeader, 0, 0);
            grid.Children.Add(name, 1, 0);
            grid.Children.Add(timeDateCreatedHeader, 0, 1);
            grid.Children.Add(timeDateCreated, 1, 1);
            grid.Children.Add(orgNameHeader, 0, 2);
            grid.Children.Add(orgName, 1, 2);
            grid.Children.Add(requestedPresHeader, 0, 3);
            grid.Children.Add(requestedPres, 1, 3);

            View = grid; // This sets the View of the parent object (which I think is the ViewCell I am extending -- not quite sure yet).
        }
              
    }

}