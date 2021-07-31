//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SPOT_App.ViewModels; // We need this using statement because the PresentationReportViewModel is defined in the test_xamarin_app.ViewModels namespace
using System.Diagnostics;

namespace SPOT_App
{
    // This class defines how the "PresentationReportDetailPage" (which is pushed onto the stack of pages when the user clicks on a Report in the Report list looks.
    // In PresentationReportDetailPage's constructor, a PresentationReportViewModel object is passed as an argument from the Handle_ReportTapped method of the PresentationReportsPage.xaml.cs file.
    // This PresentationReportViewModel is then set as the "BindingContext" of this page. This means that I can now implicitly bind properties of the that PresentationReportViewModel to GUI components of this page.

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresentationReportDetailPage : ContentPage
    {
        //public PresentationReportViewModel selectedReportViewModel;
        public PresentationReportDetailPage(PresentationReportViewModel selectedRVM)
        {
            InitializeComponent();

            BindingContext = selectedRVM;
        }
    }
}