using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the CurrentUserViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Collections.Generic;
using System;

namespace SPOT_App
{
    // This class defines how the list of CurrentUserViewModel objects will be shown in the GUI.
    // Temporarily, it constructs 20 CurrentUserViewModel objects and stores them in an "ObservableCollection".
    // This ObservableCollection is used by the ListView "CurrentUsers" (which is declared in the CurrentUsersPage.xaml file NOT in this file -- 
    // I think it is also possible to define Users in this file, but I didn't have time to figure it out) to display the CurrentUserViewModel objects.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentUsersPage : ContentPage
    {
        //============================================================
        //Data Memebers
        ObservableCollection<CurrentUserViewModel> UserCollection { get; set; } 
        readonly RestService restService;
        readonly User user;
        private int firstNumOfUsers=0;

        //============================================================
        public CurrentUsersPage(RestService restService, User user)
        {
            InitializeComponent();
            this.restService = restService;
            this.user = user;

            ConstructUserView();
        }
        private async void ConstructUserView()
        {
            //List that stores all of the cells that is being displayed.
            UserCollection = new ObservableCollection<CurrentUserViewModel> { }; // This variable will contain the CurrentUserViewModel objects that store User data.

            //A list that contains all the data that needs to be displayed.
            List<CurrentUserViewModel> rvmList= await restService.GetCurrentUserData(10, firstNumOfUsers).ConfigureAwait(true);
            
            //Filling out the cells using rvmList that is also being filled in inside this loop.
            foreach (CurrentUserViewModel rvm in rvmList)
            {
                rvm.ChangeVariables();
                UserCollection.Add(new CurrentUserViewModel
                {
                    Name = rvm.Name,
                    Email = rvm.Email,
                    PhoneNumber = rvm.PhoneNumber,
                    CurrentStatus = rvm.CurrentStatus,
                    PrivacyPolicy = rvm.PrivacyPolicy
                });

            }

            currentUsers.ItemTemplate = new DataTemplate(typeof(CurrentUserCell)); // The "CurrentUserCell" defines how the CurrentUserViewModel objects will be displayed by the GUI (the View).
            currentUsers.HasUnevenRows = true;
            currentUsers.SeparatorColor = Color.Black;
            currentUsers.ItemsSource = UserCollection; // "CurrentUsers" is the ListView defined in the CurrentUserPage.xaml file -- the ItemsSource property tells this ListView where it should get information for its list items from.
            
            //This lane just make the list go back to the top. (mainly for when you press next and back)
            currentUsers.ScrollTo(UserCollection[0], ScrollToPosition.Start, true);
            
        }
          
       

        async void Handle_UserTapped(object sender, ItemTappedEventArgs e)
        {
           //sender.GetType();

            if (e.Item == null)
                return;
            
            //await DisplayAlert("Item Tapped", "An item was tapped." + "the type of the sender was: " + sender.GetType() + " " + sender.GetHashCode(), "OK");

            CurrentUserViewModel selectedUserViewModel = (CurrentUserViewModel)((ListView)sender).SelectedItem; // This line lets me store the CurrentUserViewModel object that was tapped by the user.

            //await DisplayAlert("x", "x is " + selectedUserViewModel.GetType(), "ok");

            //await DisplayAlert("************", selectedUserViewModel.GetContents(), "ok");

            await Navigation.PushAsync(new CurrentUserDetailPage(selectedUserViewModel, restService, user)).ConfigureAwait(true); // Here I am passing the CurrentUserViewModel object to the CurrentUserDetailPage (this page will then display ALL the contents of the CurrentUserViewModel object).

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        // The following class extends the ViewCell class to create a custom GUI for each CurrentUserViewModel in the "Users" ListView.
        // Changing the contents of this class will affect how ALL CurrentUserViewModels in the ListView are displayed.
        // This code -- most likely -- could have all been written in the CurrentUsersPage.xaml file.
      
        //Function behind the "Back" button on the CurrentUsersPage.xaml. It queries the db in increments of 10. 
        //When pressed: grabs and displays previous 10 Users in db.
        private void Back_Button_Clicked(object sender, EventArgs e)
        {
            //Making sure that it does not go under 0
            if (firstNumOfUsers - 10 >= 0)
            {
                firstNumOfUsers -= 10;
                ConstructUserView();
            }
        }
        //Function behind the "Next" button on the CurrentUsersPage.xaml 
        //When pressed: grabs and displays the next 10 Users.
        private void Next_Button_Clicked(object sender, EventArgs e)
        {
            if(UserCollection.Count >= 10)
            {
                firstNumOfUsers += 10;
                ConstructUserView();
            }
            else
            {
                DisplayAlert("End of the list.", "Sorry, but there are no more users to display.", "Close");
            }        
        }

        private async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(restService, user)).ConfigureAwait(true);
        }
    }

    public class CurrentUserCell : ViewCell
        {
            public CurrentUserCell()
            {
                var grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var nameHeader = new Label();
                var userHeader = new Label();

                var name = new Label();
                var userStatus = new Label();

                nameHeader.Text = "Name:";
                userHeader.Text = "User Is:";

                nameHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                nameHeader.HorizontalOptions = LayoutOptions.StartAndExpand;
                userHeader.VerticalOptions = LayoutOptions.CenterAndExpand;
                userHeader.HorizontalOptions = LayoutOptions.StartAndExpand;

                nameHeader.FontSize = 14;
                userHeader.FontSize = 14;

                name.SetBinding(Label.TextProperty, new Binding("Name"));
                userStatus.SetBinding(Label.TextProperty, new Binding("CurrentStatus"));

                name.FontSize = 14;
                userStatus.FontSize = 14;

                name.TextColor = Color.Black;
                userStatus.TextColor = Color.Black;

                name.VerticalOptions = LayoutOptions.CenterAndExpand;
                name.HorizontalOptions = LayoutOptions.StartAndExpand;
                userStatus.VerticalOptions = LayoutOptions.CenterAndExpand;
                userStatus.HorizontalOptions = LayoutOptions.StartAndExpand;

                grid.Children.Add(nameHeader, 0, 0);
                grid.Children.Add(name, 1, 0);
                grid.Children.Add(userHeader, 0, 1);
                grid.Children.Add(userStatus, 1, 1);

                View = grid; // This sets the View of the parent object (which I think is the ViewCell I am extending -- not quite sure yet).
                }     
        }
}