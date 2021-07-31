using System;
using System.Collections.Generic;
using System.Text;

namespace SPOT_App
{

public class User
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string IsAmbassador { get; set; }
        public string IsAdmin { get; set; }

        public string GetContents() // Utility function to quickly get the contents of the current LoginResponse
        {
            return
                "Email = " + Email + "\n" +
                "FirstName = " + FirstName + "\n" +
                "LastName = " + LastName + "\n" +
                "PrimaryPhoneNumber = " + PrimaryPhoneNumber + "\n" +
                "IsAmbassador = " + IsAmbassador + "\n" +
                "IsAdmin = " + IsAdmin + "\n";
        }
    }//end user class
}
