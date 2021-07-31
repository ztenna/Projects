namespace SPOT_App.Models
{
    // This class defines a LoginResponse object.
    // The purpose of this class is to be able to easily (via the JsonConvert.DeserializeObject() function) create an object that holds information as to whether a login was successful.
    // See the test_login() function in the RestService file for an example where this class is used.
    public class LoginResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public string GetContents() // Utility function to quickly get the contents of the current LoginResponse
        {
            return 
                "Status = " + Status + "\n" + 
                "Message = " + Message + "\n" +
                "Email = " + Email + "\n";
        }
    }
}