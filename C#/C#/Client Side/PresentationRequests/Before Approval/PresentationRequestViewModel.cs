//using Xamarin.Forms;
//using System.ComponentModel;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace SPOT_App.ViewModels
{
    // This class defines an object that can be used to store all the information for a single request.
    // It is called "PresentationRequestViewModel" instead of "PresentationRequest" because I was trying (perhaps unsuccessfully) to follow the View <-> ViewModel <-> Model design.
    // See this link for more information: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#the-mvvm-pattern
    public class PresentationRequestViewModel
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
        public string AltPhoneNumber {get; set; }
        public string ContactDayTime { get; set; }
        public string OrgStreet { get; set; }
        public string OrgCity { get; set; }
        public string OrgState { get; set; }
        public string OrgZip { get; set; }
        public string PresLocation { get; set; }
        public string AnyPres { get; set; }
        public string InvisiblePres { get; set; }
        public string MentorPres { get; set; }
        public string StarPres { get; set; }
        public string WaterPres { get; set; }
        public string PlanetPres { get; set; }
        public string MarsPres { get; set; }
        public string StationPres { get; set; }
        public string TelescopePres { get; set; }
        public string NANOGravPres { get; set; }
        public string RequestedPres {get; set; }
        public string PresRotations { get; set; }
        public string NoHandsOn { get; set; }
        public string BuildMolecule { get; set; }
        public string DesignAlien { get; set; }
        public string PocketSolSys { get; set; }
        public string SizeUpMoon { get; set; }
        public string TelescopeDesign { get; set; }
        public string RoboticArm { get; set; }
        public string ElectroWar { get; set; }
        public string BuildWatershed { get; set; }
        public string DropInBucket { get; set; }
        public string HandsOnActivities {get; set; }
        public string GradeLevels { get; set; }
        public string NumberStudents { get; set; }
        public string ProposedDateTime { get; set; }
        public string ProjectSupplies { get; set; }
        public string SpeakerSupplies { get; set; }
        public string CompSupplies { get; set; }
        public string CordSupplies { get; set; }
        public string MicSupplies { get; set; }
        public string OtherSupplies { get; set; }
        public string Supplies { get; set; }
        public string TravelFee { get; set; }
        public string AgreeStatement { get; set; }
        public string Concerns { get; set; }
        public string CollegePres { get; set; }
        public string SkypePres { get; set; }
        public string AltOtherPres { get; set; }
        public string AltPres { get; set; }

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
                "OrgStreet:" + OrgStreet + "\n" +
                "OrgCity:" + OrgCity + "\n" +
                "OrgState:" + OrgState + "\n" +
                "OrgZip:" + OrgZip + "\n" +
                "AnyPres:" + AnyPres + "\n" +
                "InvisiblePres:" + InvisiblePres + "\n" +
                "MentorPres:" + MentorPres + "\n" +
                "StarPres:" + StarPres + "\n" +
                "WaterPres:" + WaterPres + "\n" +
                "PlanetPres:" + PlanetPres + "\n" +
                "MarsPres:" + MarsPres + "\n" +
                "StationPres:" + StationPres + "\n" +
                "TelescopePres:" + TelescopePres + "\n" +
                "NANOGravPres:" + NANOGravPres + "\n" +
                "PresRotations:" + PresRotations + "\n" +
                "NoHandsOn:" + NoHandsOn + "\n" +
                "BuildMolecule:" + BuildMolecule + "\n" +
                "DesignAlien:" + DesignAlien + "\n" +
                "PocketSolSys:" + PocketSolSys + "\n" +
                "SizeUpMoon:" + SizeUpMoon + "\n" +
                "TelescopeDesign:" + TelescopeDesign + "\n" +
                "RoboticArm:" + RoboticArm + "\n" +
                "ElectroWar:" + ElectroWar + "\n" +
                "BuildWatershed:" + BuildWatershed + "\n" +
                "DropInBucket:" + DropInBucket + "\n" +
                "GradeLevels:" + GradeLevels + "\n" +
                "NumberStudents:" + NumberStudents + "\n" +
                "ProposedDateTime:" + ProposedDateTime + "\n" +
                "ProjectSupplies:" + ProjectSupplies + "\n" +
                "SpeakerSupplies:" + SpeakerSupplies + "\n" +
                "CompSupplies:" + CompSupplies + "\n" +
                "CordSupplies:" + CordSupplies + "\n" +
                "MicSupplies:" + MicSupplies + "\n" +
                "OtherSupplies:" + OtherSupplies + "\n" +
                "TravelFee:" + TravelFee + "\n" +
                "AgreeStatement:" + AgreeStatement + "\n" +
                "Concerns:" + Concerns + "\n" +
                "CollegePres:" + CollegePres + "\n" +
                "SkypePres:" + SkypePres + "\n" +
                "AltOtherPres:" + AltOtherPres + "\n";
        }

        public void ChangeVariables()
        {
            RequestedPres = "";

            if (AnyPres.Equals("1"))
                RequestedPres += "Any Presentation\n";
            if (InvisiblePres.Equals("1"))
                RequestedPres += "The Invisible Unvierse 2.0\n";
            if (MentorPres.Equals("1"))
                RequestedPres += "Mountaineer Mentors\n";
            if (StarPres.Equals("1"))
                RequestedPres += "The Star of Our World\n";
            if (WaterPres.Equals("1"))
                RequestedPres += "Water: The Source of Life\n";
            if (PlanetPres.Equals("1"))
                RequestedPres += "How to Make a Planet...With Life!\n";
            if (MarsPres.Equals("1"))
                RequestedPres += "Mars: Past, Present, Future\n";
            if (StationPres.Equals("1"))
                RequestedPres += "The International Space Station\n";
            if (TelescopePres.Equals("1"))
                RequestedPres += "Space Telescope: Searching for Other Worlds\n";
            if (NANOGravPres.Equals("1"))
                RequestedPres += "NANOGrav: Tuning in to Einstein's Universe";

            HandsOnActivities = "";

            if (NoHandsOn != null && NoHandsOn.Equals("1"))
                HandsOnActivities += "No hands-on component requested";
            if (BuildMolecule != null && BuildMolecule.Equals("1"))
                HandsOnActivities += "Build a Molecule\n";
            if (DesignAlien != null && DesignAlien.Equals("1"))
                HandsOnActivities += "Design an Alien\n";
            if (PocketSolSys != null && PocketSolSys.Equals("1"))
                HandsOnActivities += "Pocket Solar System\n";
            if (SizeUpMoon != null && SizeUpMoon.Equals("1"))
                HandsOnActivities += "Sizing Up the Moon\n";
            if (TelescopeDesign != null && TelescopeDesign.Equals("1"))
                HandsOnActivities += "Green Bank Telescope Engineering Design Challenge\n";
            if (RoboticArm != null && RoboticArm.Equals("1"))
                HandsOnActivities += "Programming a Robotic Arm\n";
            if (ElectroWar != null && ElectroWar.Equals("1"))
                HandsOnActivities += "Electromagnetic War\n";
            if (BuildWatershed != null && BuildWatershed.Equals("1"))
                HandsOnActivities += "Build-Your-Own Watershed\n";
            if (DropInBucket != null && DropInBucket.Equals("1"))
                HandsOnActivities += "A Drop in the Bucket";

            Supplies = "Have: ";

            if (ProjectSupplies.Equals("1"))
                Supplies += "Projector, blank screen or wall, and dark room\n";
            if (SpeakerSupplies.Equals("1"))
                Supplies += "Speakers with laptop plug-in\n";
            if (CompSupplies.Equals("1"))
                Supplies += "Computer with flash drive plug-in\n";
            if (CordSupplies.Equals("1"))
                Supplies += "Extension Cord\n";
            if (MicSupplies.Equals("1"))
                Supplies += "Microphone\n";
            if (OtherSupplies != null)
                Supplies += OtherSupplies;

            if (TravelFee.Equals("1"))
                TravelFee = "Can pay ambassador travel fee";
            else
                TravelFee = "Can't pay ambassador travel fee";

            if (AgreeStatement.Equals("1"))
                AgreeStatement = "Agree";

            AltPres = "";
            
            if (CollegePres != null && CollegePres.Equals("1"))
                AltPres += "Field trip to WV college campus\n";
            if (SkypePres != null && SkypePres.Equals("1"))
                AltPres += "Skype virtual presentation\n";
            if (AltOtherPres != null)
                AltPres += AltOtherPres;
        }
    }
}