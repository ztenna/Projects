using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the PresentationReportViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Collections.Generic;
using System;

namespace SPOT_App
{
    // This class defines how the list of PresentationReportViewModel objects will be shown in the GUI.
    // Temporarily, it constructs 20 PresentationReportViewModel objects and stores them in an "ObservableCollection".
    // This ObservableCollection is used by the ListView "PresentationReports" (which is declared in the PresentationReportReviewPage.xaml file NOT in this file -- 
    // I think it is also possible to define Reports in this file, but I didn't have time to figure it out) to display the PresentationReportViewModel objects.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresentationReportReviewPage : ContentPage
    {
        //============================================================
        //Data Memebers
        ObservableCollection<PresentationReportViewModel> ReportCollection { get; set; } 
        readonly RestService restService;
        readonly User user;
        private int firstNumOfReports=0;

        //============================================================
        public PresentationReportReviewPage(RestService restService, User user)
        {
            InitializeComponent();
            this.restService = restService;
            this.user = user;
   
            ConstructReportView();
        }
        private async void ConstructReportView()
        {
            //List that stores all of the cells that is being displayed.
            ReportCollection = new ObservableCollection<PresentationReportViewModel> { }; // This variable will contain the PresentationReportViewModel objects that store Report data.

            //A list that contains all the data that needs to be displayed.
            List<PresentationReportViewModel> rvmList= await restService.GetPresentationReportData(10, firstNumOfReports).ConfigureAwait(true);
            
            //Filling out the cells using rvmList that is also being filled in inside this loop.
            foreach (PresentationReportViewModel rvm in rvmList)
            {
                rvm.ChangeVariables();
                ReportCollection.Add(new PresentationReportViewModel
                {
                    ReportID = rvm.ReportID,
                    Email = rvm.Email,
                    Name = rvm.Name,
                    Driver = rvm.Driver,
                    OrgName = rvm.OrgName,
                    ContactPerson = rvm.ContactPerson,
                    TravelFee = rvm.TravelFee,
                    PresDate = rvm.PresDate,
                    NumPres = rvm.NumPres,
                    NumActs = rvm.NumActs,
                    SameEverything = rvm.SameEverything,
                    AnythingDiff = rvm.AnythingDiff,
                    FirstTime = rvm.FirstTime,
                    Audience = rvm.Audience,
                    EngagedRating = rvm.EngagedRating,
                    AppropriateRating = rvm.AppropriateRating,
                    AskedQuestions = rvm.AskedQuestions,
                    AnswerQuestions = rvm.AnswerQuestions,
                    AspectsWorkBest = rvm.AspectsWorkBest,
                    PresSuggestions = rvm.PresSuggestions
                });

            }

            presentationReports.ItemTemplate = new DataTemplate(typeof(PresentationReportCell)); // The "PresentationReportCell" defines how the PresentationReportViewModel objects will be displayed by the GUI (the View).
            presentationReports.HasUnevenRows = true;
            presentationReports.SeparatorColor = Color.Black;
            presentationReports.ItemsSource = ReportCollection; // "PresentationReports" is the ListView defined in the PresentationReportPage.xaml file -- the ItemsSource property tells this ListView where it should get information for its list items from.
            
            //This lane just make the list go back to the top. (mainly for when you press next and back)
            //presentationReports.ScrollTo(ReportCollection[0], ScrollToPosition.Start, true);
            
        }
          
       

        async void Handle_ReportTapped(object sender, ItemTappedEventArgs e)
        {
           //sender.GetType();

            if (e.Item == null)
                return;
            
            //await DisplayAlert("Item Tapped", "An item was tapped." + "the type of the sender was: " + sender.GetType() + " " + sender.GetHashCode(), "OK");

            PresentationReportViewModel selectedReportViewModel = (PresentationReportViewModel)((ListView)sender).SelectedItem; // This line lets me store the PresentationReportViewModel object that was tapped by the user.

            //await DisplayAlert("x", "x is " + selectedReportViewModel.GetType(), "ok");

            //await DisplayAlert("************", selectedReportViewModel.GetContents(), "ok");

            await Navigation.PushAsync(new PresentationReportDetailPage(selectedReportViewModel)).ConfigureAwait(true); // Here I am passing the PresentationReportViewModel object to the PresentationReportDetailPage (this page will then display ALL the contents of the PresentationReportViewModel object).

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        // The following class extends the ViewCell class to create a custom GUI for each PresentationReportViewModel in the "Reports" ListView.
        // Changing the contents of this class will affect how ALL PresentationReportViewModels in the ListView are displayed.
        // This code -- most likely -- could have all been written in the PresentationReportReviewPage.xaml file.
      
        //Function behind the "Back" button on the PresentationReportReviewPage.xaml. It queries the db in increments of 10. 
        //When pressed: grabs and displays previous 10 Reports in db.
        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            //Making sure that it does not go under 0
            if (firstNumOfReports - 10 >= 0)
            {
                firstNumOfReports -= 10;
                ConstructReportView();
            }
        }
        //Function behind the "Next" button on the PresentationReportReviewPage.xaml 
        //When pressed: grabs and displays the next 10 Reports.
        private void Next_Button_Clicked(object sender, EventArgs e)
        {
            if(ReportCollection.Count >= 10)
            {
                firstNumOfReports += 10;
                ConstructReportView();
            }
            else
            {
                DisplayAlert("End of the list.", "Sorry, but there are no more Reports to display.", "Close");
            }
        }

        private async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(restService, user)).ConfigureAwait(true);
        }
    }
    public class PresentationReportCell : ViewCell
    {
        public PresentationReportCell()
        {
            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var nameHeader = new Label();
            var orgNameHeader = new Label();
            var presDateHeader = new Label();

            var name = new Label();
            var orgName = new Label();
            var presDate = new Label();

            nameHeader.Text = "Name:";
            orgNameHeader.Text = "Organization Name:";
            presDateHeader.Text = "Presentation Date: ";

            nameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            nameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
            orgNameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            orgNameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
            presDateHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
            presDateHeader.HorizontalOptions = LayoutOptions.StartAndExpand;

            nameHeader.FontSize = 14;
            orgNameHeader.FontSize = 14;
            presDateHeader.FontSize = 14;

            name.SetBinding(Label.TextProperty, new Binding("Name"));
            orgName.SetBinding(Label.TextProperty, new Binding("OrgName"));
            presDate.SetBinding(Label.TextProperty, new Binding("PresDate"));

            name.FontSize = 14;
            orgName.FontSize = 14;
            presDate.FontSize = 14;

            name.TextColor = Color.Black;
            orgName.TextColor = Color.Black;
            presDate.TextColor = Color.Black;

            name.VerticalOptions = LayoutOptions.CenterAndExpand;
            name.HorizontalOptions = LayoutOptions.StartAndExpand;
            orgName.VerticalOptions = LayoutOptions.CenterAndExpand;
            orgName.HorizontalOptions = LayoutOptions.StartAndExpand;
            presDate.VerticalOptions = LayoutOptions.CenterAndExpand;
            presDate.HorizontalOptions = LayoutOptions.StartAndExpand;

            grid.Children.Add(nameHeader, 0, 0);
            grid.Children.Add(name, 1, 0);
            grid.Children.Add(orgNameHeader, 0, 1);
            grid.Children.Add(orgName, 1, 1);
            grid.Children.Add(presDateHeader, 0, 2);
            grid.Children.Add(presDate, 1, 2);

            View = grid; // This sets the View of the parent object (which I think is the ViewCell I am extending -- not quite sure yet).
        }
              
    }

}