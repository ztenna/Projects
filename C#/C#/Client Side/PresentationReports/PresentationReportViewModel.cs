//using Xamarin.Forms;
//using System.ComponentModel;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;

namespace SPOT_App.ViewModels
{
    // This class defines an object that can be used to store all the information for a single Report.
    // It is called "PresentationReportViewModel" instead of "PresentationReport" because I was trying (perhaps unsuccessfully) to follow the View <-> ViewModel <-> Model design.
    // See this link for more information: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#the-mvvm-pattern
    public class PresentationReportViewModel
    {
        // As I understand it, these "get set" functions are used by the View of the MVVM pattern to be able to access the data members of this class.
        // Without them, it would not be possible to use something like:
        // <Label Text="{Binding OrganizationName}" FontSize="{StaticResource contentFontSize}" TextColor="{StaticResource contentTextColor}" HorizontalOptions="Start" VerticalOptions="Center"/>
        // in a .xaml file because the "{Binding OrganizationName}" would not be able to "get" the information.
        // Take this with some salt though -- I'm not comfortable with this yet.
        //public string ID { get; set; }
        public string ReportID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
        public string OrgName { get; set; }
        public string ContactPerson { get; set; }
        public string TravelFeeRecDir { get; set; }
        public string TravelFeeNoNeed {get; set; }
        public string TravelFeeFollowUp { get; set; }
        public string TravelFee { get; set; }
        public string PresDate { get; set; }
        public string NumUniversePres { get; set; }
        public string NumWaterPres { get; set; }
        public string NumStarPres { get; set; }
        public string NumPlanetPres { get; set; }
        public string NumMarsPres { get; set; }
        public string NumStationPres { get; set; }
        public string NumTelescopePres { get; set; }
        public string NumNANOGravPres { get; set; }
        public string NumMentorPres { get; set; }
        public string NumPres { get; set; }
        public string NumSolarActs { get; set; }
        public string NumMoonActs { get; set; }
        public string NumEngDesignActs { get; set; }
        public string NumRobArmActs { get; set; }
        public string NumElectroWarActs { get; set; }
        public string NumAlienActs {get; set; }
        public string NumMoleculeActs { get; set; }
        public string NumBucketActs { get; set; }
        public string NumWatershedActs { get; set; }
        public string NumActs { get; set; }
        public string SameEverything { get; set; }
        public string AnythingDiff { get; set; }
        public string FirstTime { get; set; }
        public string Num_KTo3_Stu { get; set; }
        public string Num_4To5_Stu { get; set; }
        public string Num_6To8_Stu { get; set; }
        public string Num_9To12_Stu { get; set; }
        public string NumCollegeStu {get; set; }
        public string NumTeachers { get; set; }
        public string OtherAudience { get; set; }
        public string Audience { get; set; }
        public string EngagedRating { get; set; }
        public string AppropriateRating { get; set; }
        public string AskedQuestions { get; set; }
        public string AnswerQuestions { get; set; }
        public string AspectsWorkBest { get; set; }
        public string PresSuggestions { get; set; }

        public string GetContents() // Utility function to quickly get contents of all data members.
        {
            return
                "ReportID:" + ReportID + "\n" +
                "Email:" + Email + "\n" +
                "Name:" + Name + "\n" +
                "Driver:" + Driver + "\n" +
                "OrgName:" + OrgName + "\n" +
                "ContactPerson:" + ContactPerson + "\n" +
                "TravelFeeRecDir:" + TravelFeeRecDir + "\n" +
                "TravelFeeNoNeed:" + TravelFeeNoNeed + "\n" +
                "TravelFeeFollowUp:" + TravelFeeFollowUp + "\n" +
                "PresDate:" + PresDate + "\n" +
                "NumUniversePres:" + NumUniversePres + "\n" +
                "NumWaterPres:" + NumWaterPres + "\n" +
                "NumStarPres:" + NumStarPres + "\n" +
                "NumPlanetPres:" + NumPlanetPres + "\n" +
                "NumMarsPres:" + NumMarsPres + "\n" +
                "NumStationPres:" + NumStationPres + "\n" +
                "NumTelescopePres:" + NumTelescopePres + "\n" +
                "NumNANOGravPres:" + NumNANOGravPres + "\n" +
                "NumMentorPres:" + NumMentorPres + "\n" +
                "NumSolarActs:" + NumSolarActs + "\n" +
                "NumMoonActs:" + NumMoonActs + "\n" +
                "NumEngDesignActs:" + NumEngDesignActs + "\n" +
                "NumRobArmActs:" + NumRobArmActs + "\n" +
                "NumElectroWarActs:" + NumElectroWarActs + "\n" +
                "NumAlienActs:" + NumAlienActs + "\n" +
                "NumMoleculeActs:" + NumMoleculeActs + "\n" +
                "NumBucketActs:" + NumBucketActs + "\n" +
                "NumWatershedActs:" + NumWatershedActs + "\n" +
                "SameEverything:" + SameEverything + "\n" +
                "AnythingDiff:" + AnythingDiff + "\n" +
                "FirstTime:" + FirstTime + "\n" +
                "Num_KTo3_Stu:" + Num_KTo3_Stu + "\n" +
                "Num_4To5_Stu:" + Num_4To5_Stu + "\n" +
                "Num_6To8_Stu:" + Num_6To8_Stu + "\n" +
                "Num_9To12_Stu:" + Num_9To12_Stu + "\n" +
                "NumCollegeStu:" + NumCollegeStu + "\n" +
                "NumTeachers:" + NumTeachers + "\n" +
                "OtherAudience:" + OtherAudience + "\n" +
                "EngagedRating:" + EngagedRating + "\n" +
                "AppropriateRating:" + AppropriateRating + "\n" +
                "AskedQuestions:" + AskedQuestions + "\n" +
                "AnswerQuestions:" + AnswerQuestions + "\n" +
                "AspectsWorkBest:" + AspectsWorkBest + "\n" +
                "PresSuggestions:" + PresSuggestions;
        }

        public void ChangeVariables()
        {
            TravelFee = "";

            if (TravelFeeRecDir != null && TravelFeeRecDir.Equals("1"))
                TravelFee = "Received directly";
            else if (TravelFeeNoNeed != null && TravelFeeNoNeed.Equals("1"))
                TravelFee = "No money is needed for this trip";
            else if (TravelFeeFollowUp != null && TravelFeeFollowUp.Equals("1"))
                TravelFee = "Teacher needs more information";

            if (SameEverything.Equals("1"))
                SameEverything = "Yes";
            else
                SameEverything = "No";

            NumPres = "";

            if (NumUniversePres != null && !NumUniversePres.Equals("0"))
                NumPres = NumUniversePres + " The Invisible Universe 2.0\n";
            if (NumWaterPres != null && !NumWaterPres.Equals("0"))
                NumPres = NumPres + NumWaterPres + " Water: The Source of Life\n";
            if (NumStarPres != null && !NumStarPres.Equals("0"))
                NumPres = NumPres + NumStarPres + " The Star in Our Lives\n";
            if (NumPlanetPres != null && !NumPlanetPres.Equals("0"))
                NumPres = NumPres + NumPlanetPres + " How to Make a Planet... with Life!\n";
            if (NumMarsPres != null && !NumMarsPres.Equals("0"))
                NumPres = NumPres + NumMarsPres + " Mars: Past, Present, Future\n";
            if (NumStationPres != null && !NumStationPres.Equals("0"))
                NumPres = NumPres + NumStationPres + " The International Space Station\n";
            if (NumTelescopePres != null && !NumTelescopePres.Equals("0"))
                NumPres = NumPres + NumTelescopePres + " Space Telescopes: Searching for Other Worlds\n";
            if (NumNANOGravPres != null && !NumNANOGravPres.Equals("0"))
                NumPres = NumPres + NumNANOGravPres + " NANOGrav: Tuning in to Einstein's Universe\n";
            if (NumMentorPres != null && !NumMentorPres.Equals("0"))
                NumPres = NumPres + NumMentorPres + " Mountaineer Mentors";

            NumActs = "";

            if (NumSolarActs != null && !NumSolarActs.Equals("0"))
                NumActs = NumSolarActs + " Pocket Solar System\n";
            if (NumMoonActs != null && !NumMoonActs.Equals("0"))
                NumActs = NumActs + NumMoonActs + " Sizing up the Moon\n";
            if (NumEngDesignActs != null && !NumEngDesignActs.Equals("0"))
                NumActs = NumActs + NumEngDesignActs + " GBT Engineering Design Challenge\n";
            if (NumRobArmActs != null && !NumRobArmActs.Equals("0"))
                NumActs = NumActs + NumRobArmActs + " Programming a Robotic Arm\n";
            if (NumElectroWarActs != null && !NumElectroWarActs.Equals("0"))
                NumActs = NumActs + NumElectroWarActs + " Electromagnetic War\n";
            if (NumAlienActs != null && !NumAlienActs.Equals("0"))
                NumActs = NumActs + NumAlienActs + " Design an Alien\n";
            if (NumMoleculeActs != null && !NumMoleculeActs.Equals("0"))
                NumActs = NumActs + NumMoleculeActs + " Build a Molecule\n";
            if (NumBucketActs != null && !NumBucketActs.Equals("0"))
                NumActs = NumActs + NumBucketActs + " A Drop in the Bucket\n";
            if (NumWatershedActs != null && !NumWatershedActs.Equals("0"))
                NumActs = NumActs + NumWatershedActs + " Build-Your-Own Watershed";

            Audience = "";

            if (Num_KTo3_Stu != null && !Num_KTo3_Stu.Equals("0"))
                Audience = Num_KTo3_Stu + " Grade K-3 Students\n";
            if (Num_4To5_Stu != null && !Num_4To5_Stu.Equals("0"))
                Audience = Audience + Num_4To5_Stu + " Grade 4-5 Students\n";
            if (Num_6To8_Stu != null && !Num_6To8_Stu.Equals("0"))
                Audience = Audience + Num_6To8_Stu + " Grade 6-8 Students\n";
            if (Num_9To12_Stu != null && !Num_9To12_Stu.Equals("0"))
                Audience = Audience + Num_9To12_Stu + " Grade 9-12 Students\n";
            if (NumCollegeStu != null && !NumCollegeStu.Equals("0"))
                Audience = Audience + NumCollegeStu + " College Students\n";
            if (NumTeachers != null && !NumTeachers.Equals("0"))
                Audience = Audience + NumTeachers + " Teachers\n";
            if (OtherAudience != null)
                Audience += OtherAudience;
        }
    }
}