using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Xamarin.Forms;
using System.Security.Cryptography; // This library gives us access to several different hashing algorithms. Hopefully one of those algorithms will match whatever is available in PHP (for the create request web page).
using Newtonsoft.Json;
using SPOT_App.Models; // This is necessary because the LoginResponse class is defined in the SPOT_App.Models namespace.
using SPOT_App.ViewModels; // This is necessary because the RequestViewModel class is defined in the "SPOT_App.ViewModels" namespace.
using Xamarin.Essentials;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
// Without this line, attempting to declare/initialize a RequestViewModel object would fail.

// ##### REPLACE wvspot.myddns.me WITH YOUR OWN IP ADDRESS!!! #####

namespace SPOT_App
{
    // This class will serve as the middleman between the application and the PHP web service that is hosted on the Apache server.
    // The idea is that this class will be instantiated once and then used to communicated to the web service.
    // Depending on the type of communication we want to do (maybe we want to pull request data from the database, maybe we want to log someone in, etc.),
    // we will need to define individual functions for this RestService class.
    // Currently, there are two such functions "test_getData()" and "test_setData()".
    // Each of these functions serves a different purpose and, as a consequence, connects to a different PHP web service file on the Apache server.
    // See the comments on/in these functions for more information.
    
    public class RestService
    {
        // This class will serve as our method of communication to the PHP web service and, by extension, the SQL database.
        readonly HttpClient client;
        User user;
        LoginResponse loginResponse;
        bool isAccepted;
        readonly string domain = "http://35.245.208.30/application_files/";

        // Default constructor.
        public RestService()
        {
            client = new HttpClient();
        }

        public async Task<List<SignupRequestViewModel>> GetSignupRequestData(int maxNumRowsToGet, int startRowOffset)
        {
            //Debug.WriteLine("********** RestService.GetSignupRequestData() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_signup_request_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                
                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.GetSignupRequestData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.GetSignupRequestData(): RESPONSE CONTENT END");

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create a list of RequestViewModel objects (which I've called rvmList).
                List<SignupRequestViewModel> rvmList = JsonConvert.DeserializeObject<List<SignupRequestViewModel>>(responseContent);

                // To make sure that the JsonConvert.DeserializeObject() function was successful, loop through the list and call the GetContents() function on each object.
                // If the JsonConvert.DeserializeObject() was successful, then the values of some of properties of the RequestViewModel objects should match what is stored in the SQL database.
                foreach (SignupRequestViewModel rvm in rvmList)
                {
                    //Debug.WriteLine(rvm.GetContents());
                }
                //Debug.WriteLine("********** RestService.GetSignupRequestData() END **********");
                return rvmList;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.GetSignupRequestData(): something went wrong while testing connection to web service!");
                return null;
            }

        }

        // This function sends a POST request to the "get_user_data.php" file on the Apache server.
        // It uses the email of the loginResponse to load the rest of the user data
        public async Task<User> GetUserData(string email)
        {
            //Debug.WriteLine("** RestService.GetUserData() START **");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_user_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("email", email),

                });
                //Debug.WriteLine("FormUrlEncodedContent: " + formUrlEncodedContent);
                //Debug.WriteLine("URI: " + uri);
                //Debug.WriteLine("Email: " + email);
                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create LoginResponse object.
                //Debug.WriteLine("Hello: " + responseContent);
                user = JsonConvert.DeserializeObject<User>(responseContent);

                //Debug.WriteLine("RestService.GetUserData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.GetUserData(): RESPONSE CONTENT END");

                // Print the contents of the UserResponse to see if the user object was loaded correctly
                //Debug.WriteLine(user.GetContents());
                return user;
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
                Debug.Fail("RestService.GetUserData(): something went wrong while testing connection to web service!");
                return null;
            }
        }

        public async Task<List<PresentationRequestViewModel>> GetPresentationRequestData(int maxNumRowsToGet, int startRowOffset)
        {
            //Debug.WriteLine("********** RestService.GetPresentationRequestData() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_presentation_request_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.GetPresentationRequestData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.GetPresentationRequestData(): RESPONSE CONTENT END");

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create a list of RequestViewModel objects (which I've called rvmList).
                List<PresentationRequestViewModel> rvmList = JsonConvert.DeserializeObject<List<PresentationRequestViewModel>>(responseContent);

                // To make sure that the JsonConvert.DeserializeObject() function was successful, loop through the list and call the GetContents() function on each object.
                // If the JsonConvert.DeserializeObject() was successful, then the values of some of properties of the RequestViewModel objects should match what is stored in the SQL database.
                foreach (PresentationRequestViewModel rvm in rvmList)
                {
                    //Debug.WriteLine(rvm.GetContents());
                }
                //Debug.WriteLine("********** RestService.GetPresentationRequestData() END **********");
                return rvmList;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.GetPresentationRequestData(): something went wrong while testing connection to web service!");
                return null;
            }

        }

        // This function sends a POST request to the "get_request_data.php" file on the Apache server.
        // It uses the integer arguments "startRow" and "endRow" to tell the "get_request_data.php" file which rows (effectively which requests) should be queried from the database.
        public async Task<List<RequestViewModel>> GetRequestData(int maxNumRowsToGet, int startRowOffset)
        {
            //Debug.WriteLine("********** RestService.GetRequestData() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_request_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                
                //Debug.WriteLine("RestService.GetRequestData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.GetRequestData(): RESPONSE CONTENT END");

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create a list of RequestViewModel objects (which I've called rvmList).
                List<RequestViewModel> rvmList = JsonConvert.DeserializeObject<List<RequestViewModel>>(responseContent);

                // To make sure that the JsonConvert.DeserializeObject() function was successful, loop through the list and call the GetContents() function on each object.
                // If the JsonConvert.DeserializeObject() was successful, then the values of some of properties of the RequestViewModel objects should match what is stored in the SQL database.
                foreach (RequestViewModel rvm in rvmList)
                {
                    Debug.WriteLine(rvm.GetContents());
                }
                //Debug.WriteLine("********** RestService.GetRequestData() END **********");
                return rvmList;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.GetRequestData(): something went wrong while testing connection to web service!");
                return null;
            }
        }

        public async Task<List<PresentationReportViewModel>> GetPresentationReportData(int maxNumRowsToGet, int startRowOffset)
        {
            //Debug.WriteLine("********** RestService.GetPresentationReportData() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_presentation_report_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.GetPresentationReportData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.GetPresentationReportData(): RESPONSE CONTENT END");

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create a list of ReportViewModel objects (which I've called rvmList).
                List<PresentationReportViewModel> rvmList = JsonConvert.DeserializeObject<List<PresentationReportViewModel>>(responseContent);

                // To make sure that the JsonConvert.DeserializeObject() function was successful, loop through the list and call the GetContents() function on each object.
                // If the JsonConvert.DeserializeObject() was successful, then the values of some of properties of the ReportViewModel objects should match what is stored in the SQL database.
                foreach (PresentationReportViewModel rvm in rvmList)
                {
                    //Debug.WriteLine(rvm.GetContents());
                }
                //Debug.WriteLine("********** RestService.GetPresentationReportData() END **********");
                return rvmList;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.GetPresentationReportData(): something went wrong while testing connection to web service!");
                return null;
            }
        }

        public async Task<List<CurrentUserViewModel>> GetCurrentUserData(int maxNumRowsToGet, int startRowOffset)
        {
            //Debug.WriteLine("********** RestService.GetCurrentUserData() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "get_currentUser_data.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                // This content will be used by the PHP file to determine which rows should be queried from the database.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.GetCurrentUserData(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.GetCurrentUserData(): RESPONSE CONTENT END");

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create a list of CurrentUserViewModel objects (which I've called rvmList).
                List<CurrentUserViewModel> rvmList = JsonConvert.DeserializeObject<List<CurrentUserViewModel>>(responseContent);

                // To make sure that the JsonConvert.DeserializeObject() function was successful, loop through the list and call the GetContents() function on each object.
                // If the JsonConvert.DeserializeObject() was successful, then the values of some of properties of the CurrentUserViewModel objects should match what is stored in the SQL database.
                foreach (CurrentUserViewModel rvm in rvmList)
                {
                    //Debug.WriteLine(rvm.GetContents());
                }
                //Debug.WriteLine("********** RestService.GetCurrentUserData() END **********");
                return rvmList;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.GetUserRequestData(): something went wrong while testing connection to web service!");
                return null;
            }
        }

        // This is a TEST login function -- it will send the argument email and password combination to the login.php file on the Apache server.
        // That login.php file will return one of two associative arrays:
        // 1: A "login successful" array that will have the following key/value pairs (note: the format is "key" -> "value"):
        //      - "Status" -> "true" // This is a boolean in the PHP file but gets converted to a string when the LoginResponse is constructed.
        //      - "Message" -> "Login successful" // This is a string
        //      - "Email" -> *Email* // This the Email of the user as a string
        //
        // 2: A "login unsuccessful" array that will have the following key/value pairs:
        //      - "Status" -> "false" // This is a boolean in the PHP file but gets converted to a string when the LoginResponse is constructed.
        //      - "Message" -> "Login unsuccessful: invalid email and password combination" // This is a string
        //
        // These associative arrays are then converted into a LoginResponse object via a call to JsonConvert.DeserializeObject().
        // A LoginResponse object has get/set Status, Message, and Email functions -- these allow you to easily see if the attempted login was successful.
        public async Task<LoginResponse> Login(string email, string password)
        {
            //Debug.WriteLine("********** RestService.test_login() START **********");

            // Identify the target PHP file.
            var uri = new Uri(domain + "login.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                //Debug.WriteLine("Response Content: " + responseContent);

                // Because the PHP file sends back a JSON encoded associative array, we can use the "Newtonsoft.Json" package to deserialize the JSON string and, thereby, create LoginResponse object.
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                //Debug.WriteLine("RestService.test_login(): RESPONSE CONTENT AS FOLLOWS:");

                // This will print the JSON string itself -- this is what is "deserialized" by the JsonConvert.DeserializeObject() function.
                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.test_login(): RESPONSE CONTENT END");

                // Print the contents of the LoginResponse to see if the attempted login was successful.
                // In this case "successful" just means that the email/password combination given to this test_login() function was in the SQL database.
                //Debug.WriteLine(loginResponse.GetContents());
                return loginResponse;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_login(): something went wrong!");
                return null;
            }
        }

        // This method will generate a SHA-512 hash of your password so your password isn't transmitted in plaintext.
        // Input: the password you want to generate a hash for.
        // Output: the hashed password is returned to where ever the method was called from.
        public static string GetPasswordHash(string password)
        {
            // Choose which hashing algorithm to use and store it in a variable.
            var hashingAlgorithm = SHA512.Create();            
            
            // Compute the hash of the password and store the resulting byte array in a variable.
            byte[] hashedPasswordBytes = hashingAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Construct the StringBuilder instance that we will use to convert the byte array form of the hashed password into a hex string.
            StringBuilder stringBuilder = new StringBuilder();

            // Individually convert each byte in the password byte array into a lowercase hex string an append the result to the StringBuilder instance.
            foreach (byte hashByte in hashedPasswordBytes)
            {
                // "hashByte.ToString("x2")" converts the byte into a lowercase hexadecimal number (indicated by the format string "x2").
                // That converted byte is then appended onto the StringBuilder instance.
                // For more information on "x2" see: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
                stringBuilder.Append(hashByte.ToString("x2")); 
            }

            // Now that the StringBuilder contains the complete hex string of the hashed password, call ToString() again and return the resulting string hash of the password.
            return stringBuilder.ToString();
        }

        public async void AddPotentialUser(string name, string email, string phoneNumber, string address, string school, string major, string hearAboutSpot, string whySpotPresenter,
            string expWithStudents, string notAsPlanned, string recommendationLetterName, string recommendationLetterEmail, string dietaryRestrictions, string isNewApplicant, 
            string isCurrentAmbassador, string monday, string tuesday, string wednesday, string thursday, string friday, string fallBreak, string winterBreak, string springBreak, 
            string availableVisitOther, string haveLicense, string haveCar, string agreeToStatement, string agreeToDoComponents, string canAttendTraining, string male, string female, 
            string genderOther, string firstToCollege, string ofOrigin, string africanAmerican, string americanIndianOrAlaskanNative, string pacificIslander, string asian, string caucasian, 
            string raceOther, string ruralCommunity, string suburbanCommunity, string urbanCommunity, string privacyPolicy)
        {
            var uri = new Uri(domain + "add_potential_user.php");
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(name);
            //Debug.WriteLine(email);
            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("phoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("address", address),
                    new KeyValuePair<string, string>("school", school),
                    new KeyValuePair<string, string>("major", major),
                    new KeyValuePair<string, string>("hearAboutSpot", hearAboutSpot),
                    new KeyValuePair<string, string>("whySpotPresenter", whySpotPresenter),
                    new KeyValuePair<string, string>("expWithStudents", expWithStudents),
                    new KeyValuePair<string, string>("notAsPlanned", notAsPlanned),
                    new KeyValuePair<string, string>("recommendationLetterName", recommendationLetterName),
                    new KeyValuePair<string, string>("recommendationLetterEmail", recommendationLetterEmail),
                    new KeyValuePair<string, string>("dietaryRestrictions", dietaryRestrictions),
                    new KeyValuePair<string, string>("isNewApplicant", isNewApplicant),
                    new KeyValuePair<string, string>("isCurrentAmbassador", isCurrentAmbassador),
                    new KeyValuePair<string, string>("monday", monday),
                    new KeyValuePair<string, string>("tuesday", tuesday),
                    new KeyValuePair<string, string>("wednesday", wednesday),
                    new KeyValuePair<string, string>("thursday", thursday),
                    new KeyValuePair<string, string>("friday", friday),
                    new KeyValuePair<string, string>("fallBreak", fallBreak),
                    new KeyValuePair<string, string>("winterBreak", winterBreak),
                    new KeyValuePair<string, string>("springBreak", springBreak),
                    new KeyValuePair<string, string>("availableVisitOther", availableVisitOther),
                    new KeyValuePair<string, string>("haveLicense", haveLicense),
                    new KeyValuePair<string, string>("haveCar", haveCar),
                    new KeyValuePair<string, string>("agreeToStatement", agreeToStatement),
                    new KeyValuePair<string, string>("agreeToDoComponents", agreeToDoComponents),
                    new KeyValuePair<string, string>("canAttendTraining", canAttendTraining),
                    new KeyValuePair<string, string>("male", male),
                    new KeyValuePair<string, string>("female", female),
                    new KeyValuePair<string, string>("genderOther", genderOther),
                    new KeyValuePair<string, string>("firstToCollege", firstToCollege),
                    new KeyValuePair<string, string>("ofOrigin", ofOrigin),
                    new KeyValuePair<string, string>("africanAmerican", africanAmerican),
                    new KeyValuePair<string, string>("americanIndianOrAlaskanNative", americanIndianOrAlaskanNative),
                    new KeyValuePair<string, string>("pacificIslander", pacificIslander),
                    new KeyValuePair<string, string>("asian", asian),
                    new KeyValuePair<string, string>("caucasian", caucasian),
                    new KeyValuePair<string, string>("raceOther", raceOther),
                    new KeyValuePair<string, string>("ruralCommunity", ruralCommunity),
                    new KeyValuePair<string, string>("suburbanCommunity", suburbanCommunity),
                    new KeyValuePair<string, string>("urbanCommunity", urbanCommunity),
                    new KeyValuePair<string, string>("privacyPolicy", privacyPolicy)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.AddPotentialUser(): RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.AddPotentialUser(): something went wrong!");
            }
        }

        public async void AddUser(string email, string password, string firstName, string lastName, string phoneNumber, string address, string school, string major, string hearAboutSpot, string whySpotPresenter,
                    string expWithStudents, string notAsPlanned, string recommendationLetterName, string recommendationLetterEmail, string dietaryRestrictions, string isAmbassador,
                    string isAdmin, string availableToVisit, string haveLicense, string haveCar, string canAttendTraining, string gender, string firstToCollege,
                    string ofOrigin, string race, string community, string privacyPolicy)
        {
            var uri = new Uri(domain + "add_user.php");
            string hashedPassword = GetPasswordHash(password);
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(isAmbassador);
            //Debug.WriteLine(isAdmin);

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", hashedPassword),
                    new KeyValuePair<string, string>("firstName", firstName),
                    new KeyValuePair<string, string>("lastName", lastName),
                    new KeyValuePair<string, string>("phoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("address", address),
                    new KeyValuePair<string, string>("school", school),
                    new KeyValuePair<string, string>("major", major),
                    new KeyValuePair<string, string>("hearAboutSpot", hearAboutSpot),
                    new KeyValuePair<string, string>("whySpotPresenter", whySpotPresenter),
                    new KeyValuePair<string, string>("expWithStudents", expWithStudents),
                    new KeyValuePair<string, string>("notAsPlanned", notAsPlanned),
                    new KeyValuePair<string, string>("recommendationLetterName", recommendationLetterName),
                    new KeyValuePair<string, string>("recommendationLetterEmail", recommendationLetterEmail),
                    new KeyValuePair<string, string>("dietaryRestrictions", dietaryRestrictions),
                    new KeyValuePair<string, string>("isAmbassador", isAmbassador),
                    new KeyValuePair<string, string>("isAdmin", isAdmin),
                    new KeyValuePair<string, string>("availableToVisit", availableToVisit),
                    new KeyValuePair<string, string>("haveLicense", haveLicense),
                    new KeyValuePair<string, string>("haveCar", haveCar),
                    new KeyValuePair<string, string>("canAttendTraining", canAttendTraining),
                    new KeyValuePair<string, string>("gender", gender),
                    new KeyValuePair<string, string>("firstToCollege", firstToCollege),
                    new KeyValuePair<string, string>("ofOrigin", ofOrigin),
                    new KeyValuePair<string, string>("race", race),
                    new KeyValuePair<string, string>("community", community),
                    new KeyValuePair<string, string>("privacyPolicy", privacyPolicy)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine(responseContent);
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.AddUser(): something went wrong!");
            }
        }

        public async void RemovePotentialUser(string email)
        { 
            var uri = new Uri(domain + "remove_potential_user.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.RemovePotentialUser(): RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.RemovePotentialUser(): something went wrong!");
            }
        }

        public async void RemoveUser(string email)
        {
            var uri = new Uri(domain + "remove_user.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.RemoveUser(): RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.RemoveUser(): something went wrong!");
            }
        }

        public async void SubmitPresRequest(string name, string orgName, string email, string phoneNumber, string altPhoneNumber, string contactDayTime, string orgStreet, string orgCity, 
            string orgState, string orgZip, string anyPres, string invisiblePres, string mentorPres, string starPres, string waterPres, string planetPres, string marsPres, string stationPres, 
            string telescopePres, string NANOGravPres, string presRotations, string noHandsOn, string buildMolecule, string designAlien, string pocketSolSys, string sizeUpMoon,
            string telescopeDesign, string roboticArm, string electroWar, string buildWatershed, string dropInBucket, string gradeLevels, string numberStudents, string proposedDateTime,
            string projectSupplies, string speakerSupplies, string compSupplies, string cordSupplies, string micSupplies, string otherSupplies, string travelFee, string agreeStatement, 
            string concerns, string collegePres, string skypePres, string altOtherPres)
        {
            var uri = new Uri(domain + "submit_pres_request.php");
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(name);
            //Debug.WriteLine(email);
            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("orgName", orgName),
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("phoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("altPhoneNumber", altPhoneNumber),
                    new KeyValuePair<string, string>("contactDayTime", contactDayTime),
                    new KeyValuePair<string, string>("orgStreet", orgStreet),
                    new KeyValuePair<string, string>("orgCity", orgCity),
                    new KeyValuePair<string, string>("orgState", orgState),
                    new KeyValuePair<string, string>("orgZip", orgZip),
                    new KeyValuePair<string, string>("anyPres", anyPres),
                    new KeyValuePair<string, string>("invisiblePres", invisiblePres),
                    new KeyValuePair<string, string>("mentorPres", mentorPres),
                    new KeyValuePair<string, string>("starPres", starPres),
                    new KeyValuePair<string, string>("waterPres", waterPres),
                    new KeyValuePair<string, string>("planetPres", planetPres),
                    new KeyValuePair<string, string>("marsPres", marsPres),
                    new KeyValuePair<string, string>("stationPres", stationPres),
                    new KeyValuePair<string, string>("telescopePres", telescopePres),
                    new KeyValuePair<string, string>("NANOGravPres", NANOGravPres),
                    new KeyValuePair<string, string>("presRotations", presRotations),
                    new KeyValuePair<string, string>("noHandsOn", noHandsOn),
                    new KeyValuePair<string, string>("buildMolecule", buildMolecule),
                    new KeyValuePair<string, string>("designAlien", designAlien),
                    new KeyValuePair<string, string>("pocketSolSys", pocketSolSys),
                    new KeyValuePair<string, string>("sizeUpMoon", sizeUpMoon),
                    new KeyValuePair<string, string>("telescopeDesign", telescopeDesign),
                    new KeyValuePair<string, string>("roboticArm", roboticArm),
                    new KeyValuePair<string, string>("electroWar", electroWar),
                    new KeyValuePair<string, string>("buildWatershed", buildWatershed),
                    new KeyValuePair<string, string>("dropInBucket", dropInBucket),
                    new KeyValuePair<string, string>("gradeLevels", gradeLevels),
                    new KeyValuePair<string, string>("numberStudents", numberStudents),
                    new KeyValuePair<string, string>("proposedDateTime", proposedDateTime),
                    new KeyValuePair<string, string>("projectSupplies", projectSupplies),
                    new KeyValuePair<string, string>("speakerSupplies", speakerSupplies),
                    new KeyValuePair<string, string>("compSupplies", compSupplies),
                    new KeyValuePair<string, string>("cordSupplies", cordSupplies),
                    new KeyValuePair<string, string>("micSupplies", micSupplies),
                    new KeyValuePair<string, string>("otherSupplies", otherSupplies),
                    new KeyValuePair<string, string>("travelFee", travelFee),
                    new KeyValuePair<string, string>("agreeStatement", agreeStatement),
                    new KeyValuePair<string, string>("concerns", concerns),
                    new KeyValuePair<string, string>("collegePres", collegePres),
                    new KeyValuePair<string, string>("skypePres", skypePres),
                    new KeyValuePair<string, string>("altOtherPres", altOtherPres)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.SubmitPresRequest(): RESPONSE CONTENT END");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.SubmitPresRequest(): something went wrong!");
            }
        }

        public async void AcceptPresentationRequest(string presentationID, string timeDateCreated, string name, string orgName, string teacherEmail, string ambassadorEmail, string otherAmbassadorEmails, string phoneNumber,
                    string altPhoneNumber, string contactDayTime, string presLocation, string requestedPres, string presRotations, string handsOnActivities, string gradeLevels, string numberStudents,
                    string proposedDateTime, string supplies, string travelFee, string agreeStatement, string concerns, string altPres)
        {
            var uri = new Uri(domain + "accept_presentation_request.php");
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(presentationID);
            //Debug.WriteLine(teacherEmail);

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("presentationID", presentationID),
                    new KeyValuePair<string, string>("timeDateCreated", timeDateCreated),
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("orgName", orgName),
                    new KeyValuePair<string, string>("teacherEmail", teacherEmail),
                    new KeyValuePair<string, string>("ambassadorEmail", ambassadorEmail),
                    new KeyValuePair<string, string>("otherAmbassadorEmails", otherAmbassadorEmails),
                    new KeyValuePair<string, string>("phoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("altPhoneNumber", altPhoneNumber),
                    new KeyValuePair<string, string>("contactDayTime", contactDayTime),
                    new KeyValuePair<string, string>("presLocation", presLocation),
                    new KeyValuePair<string, string>("requestedPres", requestedPres),
                    new KeyValuePair<string, string>("presRotations", presRotations),
                    new KeyValuePair<string, string>("handsOnActivities", handsOnActivities),
                    new KeyValuePair<string, string>("gradeLevels", gradeLevels),
                    new KeyValuePair<string, string>("numberStudents", numberStudents),
                    new KeyValuePair<string, string>("proposedDateTime", proposedDateTime),
                    new KeyValuePair<string, string>("supplies", supplies),
                    new KeyValuePair<string, string>("travelFee", travelFee),
                    new KeyValuePair<string, string>("agreeStatement", agreeStatement),
                    new KeyValuePair<string, string>("concerns", concerns),
                    new KeyValuePair<string, string>("altPres", altPres)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("Response Content: " + responseContent);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.AddUser(): something went wrong!");
            }
        }

        public async void RemovePresentationRequest(string presentationID, string teacherEmail)
        {
            //Debug.WriteLine("presentationID in removePresentationRequest(): " + presentationID);
            var uri = new Uri(domain + "delete_presentation_request.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("presentationID", presentationID),
                    new KeyValuePair<string, string>("email", teacherEmail)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("Response Content: " + responseContent);

                //Debug.WriteLine("RestService.removePresentationRequest(): RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.removePresentationRequest(): something went wrong!");
            }
        }

        public async Task<bool> CheckAcceptanceOfRequest(String presentationID, String email)
        {
            //Debug.WriteLine("********* CheckAcceptanceOfRequest() START ***************" + "\n\npresentationID:" + presentationID);

            Uri uri = new Uri(domain + "check_acceptance.php");

            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string,string>("presentationID",presentationID),
                     new KeyValuePair<string, string>("email",email)
                 });

                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);

                //Debug.WriteLine("Response: " + response);
                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("responseContent:" + responseContent);
                //Make sure the string is not null and wont crash the program.
                if (String.IsNullOrEmpty(responseContent) || String.IsNullOrWhiteSpace(responseContent))
                {
                    //Returning true because nothing will happen if this returns true.\
                    //Debug.WriteLine("inside the condition to check if null or empty....");
                    isAccepted = true;
                }
                //Check if check_acceptance.php returned false, which means no one has accpeted the request yet.
                else if (responseContent.Equals("false"))
                {
                    //Debug.WriteLine("responsContent is FALSE!!!");
                    isAccepted = false;
                }
                //Check if check_acceptance.php returned true, which means someone has accepted the request.
                else if (responseContent.Equals("true"))
                {
                    //Debug.WriteLine("responsContent is TRUE!!!");
                    isAccepted = true;
                }

                //Debug.WriteLine("********* CheckAcceptanceOfRequest() END ***************");
                return isAccepted;
            }
            catch (Exception e)
            {
                Debug.Fail("Error in checkAcceptanceOfRequest()");
                Debug.WriteLine(e);
                //Since returing true does nothing this does no harm.
                return true;
            }
        }

        public async Task<bool> AcceptRequest(String presentationID, String email)
        {
            //Debug.WriteLine("***************** AcceptRequest() START ****************");

            var uri = new Uri(domain + "accept_request.php");
            string userEmail = user.Email;
            //Debug.WriteLine("userEmail:" + userEmail);
            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string,string>("presentationID",presentationID),
                     new KeyValuePair<string, string>("teacherEmail",email),
                     new KeyValuePair<string, string>("ambassadorEmail",userEmail)
                });

                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);

                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("responseContent:" + responseContent);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR INSIDE AcceptRequest()");
                Debug.WriteLine(e);
                return false;
            }
        }

        // This is method will send the arguments to the edit_pres_request.php file on the Apache server.
        // The response variable is to make sure a connection was established and working correctly.
        // The response content is to relay what the "php file has to say", any echos in the php file.
        // Input: the fields you are changing and the presentationID and teacherEmail to identify the presentation being updated.
        // Output: the presentation is changed in the database.
        // Exceptions: the client.PostAsync() method throws an exception when there is a connection issue
        //             or an input validation error occurs.
        public async void EditPresRequest(string presentationID, string name, string orgName, string teacherEmail, string ambassadorEmail, string otherAmbassadorEmails, string phoneNumber,
                    string altPhoneNumber, string contactDayTime, string presLocation, string requestedPres, string presRotations, string handsOnActivities, string gradeLevels, string numberStudents,
                    string proposedDateTime, string supplies, string travelFee, string concerns, string altPres)
        {
            var uri = new Uri(domain + "edit_pres_request.php");
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(presentationID);
            //Debug.WriteLine(teacherEmail);
            //Debug.WriteLine(otherAmbassadorEmails);

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("presentationID", presentationID),
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("orgName", orgName),
                    new KeyValuePair<string, string>("teacherEmail", teacherEmail),
                    new KeyValuePair<string, string>("ambassadorEmail", ambassadorEmail),
                    new KeyValuePair<string, string>("otherAmbassadorEmails", otherAmbassadorEmails),
                    new KeyValuePair<string, string>("phoneNumber", phoneNumber),
                    new KeyValuePair<string, string>("altPhoneNumber", altPhoneNumber),
                    new KeyValuePair<string, string>("contactDayTime", contactDayTime),
                    new KeyValuePair<string, string>("presLocation", presLocation),
                    new KeyValuePair<string, string>("requestedPres", requestedPres),
                    new KeyValuePair<string, string>("presRotations", presRotations),
                    new KeyValuePair<string, string>("handsOnActivities", handsOnActivities),
                    new KeyValuePair<string, string>("gradeLevels", gradeLevels),
                    new KeyValuePair<string, string>("numberStudents", numberStudents),
                    new KeyValuePair<string, string>("proposedDateTime", proposedDateTime),
                    new KeyValuePair<string, string>("supplies", supplies),
                    new KeyValuePair<string, string>("travelFee", travelFee),
                    new KeyValuePair<string, string>("concerns", concerns),
                    new KeyValuePair<string, string>("altPres", altPres)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("Response: " + response);

                // Read the HttpResponseMessage's Content property as a string.                
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("Response Content: " + responseContent);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.EditPresRequest(): something went wrong!");
            }
        }

        public async void SubmitPresReport(string name, string driver, string orgName, string contactPerson, string travelFeeRecDir, string travelFeeNoNeed,
                    string travelFeeFollowUp, string presDate, string numUniversePres, string numWaterPres, string numStarPres, string numPlanetPres,
                    string numMarsPres, string numStationPres, string numTelescopePres, string numNANOGravPres, string numMentorPres, string numSolarActs, string numMoonActs,
                    string numEngDesignActs, string numRobArmActs, string numElectroWarActs, string numAlienActs, string numMoleculeActs, string numBucketActs, string numWatershedActs,
                    string sameEverything, string anythingDiff, string firstTime, string num_KTo3_Stu, string num_4To5_Stu, string num_6To8_Stu, string num_9To12_Stu,
                    string numCollegeStu, string numTeachers, string otherAudience, string engagedRating, string appropriateRating, string askedQuestions, string answerQuestions, string aspectsWorkBest,
                    string presSuggestions)
        {
            var uri = new Uri(domain + "submit_pres_report.php");

            string email = user.Email;
            
            //Debug.WriteLine("Values are as follows:");
            //Debug.WriteLine(name);
            //Debug.WriteLine(orgName);
            //Debug.WriteLine(email);
            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("name", name),
                    new KeyValuePair<string, string>("driver", driver),
                    new KeyValuePair<string, string>("orgName", orgName),
                    new KeyValuePair<string, string>("contactPerson", contactPerson),
                    new KeyValuePair<string, string>("travelFeeRecDir", travelFeeRecDir),
                    new KeyValuePair<string, string>("travelFeeNoNeed", travelFeeNoNeed),
                    new KeyValuePair<string, string>("travelFeeFollowUp", travelFeeFollowUp),
                    new KeyValuePair<string, string>("presDate", presDate),
                    new KeyValuePair<string, string>("numUniversePres", numUniversePres),
                    new KeyValuePair<string, string>("numWaterPres", numWaterPres),
                    new KeyValuePair<string, string>("numStarPres", numStarPres),
                    new KeyValuePair<string, string>("numPlanetPres", numPlanetPres),
                    new KeyValuePair<string, string>("numMarsPres", numMarsPres),
                    new KeyValuePair<string, string>("numStationPres", numStationPres),
                    new KeyValuePair<string, string>("numTelescopePres", numTelescopePres),
                    new KeyValuePair<string, string>("numNANOGravPres", numNANOGravPres),
                    new KeyValuePair<string, string>("numMentorPres", numMentorPres),
                    new KeyValuePair<string, string>("numSolarActs", numSolarActs),
                    new KeyValuePair<string, string>("numMoonActs", numMoonActs),
                    new KeyValuePair<string, string>("numEngDesignActs", numEngDesignActs),
                    new KeyValuePair<string, string>("numRobArmActs", numRobArmActs),
                    new KeyValuePair<string, string>("numElectroWarActs", numElectroWarActs),
                    new KeyValuePair<string, string>("numAlienActs", numAlienActs),
                    new KeyValuePair<string, string>("numMoleculeActs", numMoleculeActs),
                    new KeyValuePair<string, string>("numBucketActs", numBucketActs),
                    new KeyValuePair<string, string>("numWatershedActs", numWatershedActs),
                    new KeyValuePair<string, string>("sameEverything", sameEverything),
                    new KeyValuePair<string, string>("anythingDiff", anythingDiff),
                    new KeyValuePair<string, string>("firstTime", firstTime),
                    new KeyValuePair<string, string>("num_KTo3_Stu", num_KTo3_Stu),
                    new KeyValuePair<string, string>("num_4To5_Stu", num_4To5_Stu),
                    new KeyValuePair<string, string>("num_6To8_Stu", num_6To8_Stu),
                    new KeyValuePair<string, string>("num_9To12_Stu", num_9To12_Stu),
                    new KeyValuePair<string, string>("numCollegeStu", numCollegeStu),
                    new KeyValuePair<string, string>("numTeachers", numTeachers),
                    new KeyValuePair<string, string>("otherAudience", otherAudience),
                    new KeyValuePair<string, string>("engagedRating", engagedRating),
                    new KeyValuePair<string, string>("appropriateRating", appropriateRating),
                    new KeyValuePair<string, string>("askedQuestions", askedQuestions),
                    new KeyValuePair<string, string>("answerQuestions", answerQuestions),
                    new KeyValuePair<string, string>("aspectsWorkBest", aspectsWorkBest),
                    new KeyValuePair<string, string>("presSuggestions", presSuggestions)
                }) ;

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("response: " + response);

                // Read the HttpResponseMessage's Content property as a string. 
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                //Debug.WriteLine("responseContent: " + responseContent);

                //Debug.WriteLine("RestService.SubmitPresReport(): RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.SubmitPresReport(): something went wrong!");
            }
        }

        // This method is called by the DisplayGoogleMapsDirections() function of the RestService class.
        // It converts an argument string into a format that can be inserted into a Google Maps URI.
        // Input: a location string
        // Output: google maps formated location string
        public string ConvertToGoogleMapsUriFormat(String str)
        {
            // Remove white space characters from beginning and end of the string.
            str = str.Trim();
            //Debug.WriteLine("RestService.ConvertToGoogleMapsUriFormat(): " + str);

            // Replace " " and "," characters with a "+" character.
            str = str.Replace(' ', '+');
            str = str.Replace(',', '+');
            //Debug.WriteLine("RestService.ConvertToGoogleMapsUriFormat(): " + str);

            // Remove all white space characters from the string.
            str = this.RemoveWhiteSpace(str);
            //Debug.WriteLine("RestService.ConvertToGoogleMapsUriFormat(): " + str);

            // Remove any extra "+" characters from the string.
            str = this.RemoveExtraPlusCharacters(str);
            //Debug.WriteLine("RestService.ConvertToGoogleMapsUriFormat(): " + str);

            return str;
        }

        // This method uses a Google Maps URI to display how far away the presentation location is from the user's current location or other location they input.
        // It assumes that the location string stored in the PresentationLocation() get/set function of the argument RequestViewModel object uses a "," character as a delimiter.
        // Note that, on an emulator, this function will not be able to identify the user's location -- this will need to be tested on an actual phone.
        // Input: source and destination locations
        // Output: Both locations displayed on Google Maps with directions on getting from source to destination.
        // Exceptions: Launcher.OpenAsync() method throws exception if uri is not valid
        public async Task DisplayGoogleMapsDirections(string sourceLocation, string destinationLocation)
        {
            //Debug.WriteLine("********** RestService.DisplayGoogleMapsDirections() START **********");

            try
            {
                Uri uri;
                // Create the URI that will be opened with Google Maps for IOS.
                if (Device.RuntimePlatform == Device.iOS)
                {
                    uri = new Uri("http://maps.apple.com/?daddr=" + this.ConvertToGoogleMapsUriFormat(destinationLocation) + ",&saddr=" + this.ConvertToGoogleMapsUriFormat(sourceLocation));
                    await Launcher.OpenAsync(uri).ConfigureAwait(true);
                }

                // Create the URI that will be opened with Google Maps for IOS.
                else if (Device.RuntimePlatform == Device.Android)
                {
                    uri = new Uri("http://maps.google.com/?daddr=" + this.ConvertToGoogleMapsUriFormat(destinationLocation) + ",&saddr=" + this.ConvertToGoogleMapsUriFormat(sourceLocation));
                    await Launcher.OpenAsync(uri).ConfigureAwait(true);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.SubmitPresReport(): something went wrong!");
            }

            //Debug.WriteLine("********** RestService.DisplayGoogleMapsDirections() END **********");
        }

        // This method will send the arguments password and email to the change_password.php file on the Apache server.
        // The response variable is to make sure a connection was established and working correctly.
        // The response content is to relay what the "php file has to say", any echos in the php file.
        // Input: the password you are changing and the email to identify the user being updated.
        // Output: the password is changed in the database.
        // Exceptions: the client.PostAsync() method throws an exception when there is a connection issue
        //             or an input validation error occurs.
        public async void ChangePassword(string password, string email)
        {
            var uri = new Uri(domain + "change_password.php");

            try
            {
                // Create the content that will be posted to the target PHP file.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("email", email)
                });

                // Send the POST request to the PHP file at the specified URI.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                //Debug.WriteLine("response: " + response);

                // Read the HttpResponseMessage's Content property as a string. 
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                //Debug.WriteLine("responseContent: " + responseContent);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.ChangePassword(): something went wrong!");
            }
        }

        //==============================================================================================================================================================================//
        // This function will log the user out, destroying the php session.
        public async void Logout() 
        {
            var uri = new Uri(domain + "logout.php");

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.Logout: LOGOUT: RESPONSE CONTENT AS FOLLOWS:");
                //Debug.WriteLine(responseContent);
                //Debug.WriteLine("RestService.Logout(): LOGOUT: RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.Logout(): LOGOUT: something went wrong while attempting to logout!");
            }
        }

        // This is method will send an email from the default email address (wvspotapp@gmail.com) to the desired recipient.
        // Input: the subject, body, and recipient of the email.
        // Output: an email is sent from wvspotapp@gmail.com to the recipient containing the specified content set elsewhere in the app.
        // Exceptions: ArgumentNullException if mail is null
        //             InvalidOperationException if SmtpServer already has a SendAsync call in progress, From is null, no recipients, 
        //                                       DeliveryMethod is set to Network and Host is null, DeliveryMethod is set to Network and
        //                                       Host is equal to empty string (""), DeliveryMethod is set to Network and Port is zero or
        //                                       negative or greater than 65535
        //             ObjectDisposedException if this object has been disposed
        //             SmtpException if connection to SMTP server failed, authentication failed, operation timed out, EnableSsl set to true
        //                           but DeliveryMethod set to SpecifiedPickupDirectory or PickupDirectoryFromlis, EnableSsl set to true but
        //                           SMTP server didn't advertise STARTTLS in response to EHLO command
        //             SmtpFailedRecipientException if message could not be delivered to one of the recipients
        //             SmtpFailedRecipientsException if mail couldn't be delivered to two or more recipients
        public static void SendEmail(string subject, string body, string recipient)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                
                // Set the sender
                mail.From = new MailAddress("wvspotapp@gmail.com");
                
                // Set the recipient
                mail.To.Add(recipient);
                
                // Set the subject
                mail.Subject = subject;
                
                // Set the message
                mail.Body = body;
                
                // Set the port
                SmtpServer.Port = 587;
                
                // Set the sender credentials
                SmtpServer.Credentials = new System.Net.NetworkCredential("wvspotapp@gmail.com", "0Gravity");
                
                // Encrypt the connection to sender account
                SmtpServer.EnableSsl = true;
                
                // Connect to sender account
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender1, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                // Send mail
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        //==============================================================================================================================================================================// 

        // Utility function -- accepts an argument string and returns the same string with white space characters removed.
        public string RemoveWhiteSpace(String str)
        {
            string stringWithNoWhiteSpace = "";

            foreach (Char character in str)
            {
                if (!Char.IsWhiteSpace(character))
                    stringWithNoWhiteSpace += character.ToString();
            }

            return stringWithNoWhiteSpace;
        }

        // Utility function -- accepts an argument string and returns the same string with extra "+" characters removed.
        // As an example, the argument string "a++++b+c" would become "a+b+c".
        public string RemoveExtraPlusCharacters(String str)
        {
            string formattedString;

            // For documentation see: https://docs.microsoft.com/en-us/dotnet/api/system.string.split?view=netframework-4.7.2#System_String_Split_System_Char___System_StringSplitOptions_
            string[] strArray = str.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            formattedString = string.Join("+", strArray);

            return formattedString;
        }

        // This function tests the compatibility of the hash algorithm used in the getPasswordHash() function of this RestService class and the other hash algorithm used in the "test_password_hashing.php" file.
        // It takes a password and computes the corresponding hash of that password by calling the getPasswordHash() function of the RestService.
        // It then sends (in a POST request) a FormUrlEncodedObject with key/value pairs of the email, the password, and the hashed password to the "test_password_hashing.php" file.
        // As a response, the "test_password_hashing.php" file echos:
        // 1: the unaltered email and unaltered password.
        // 2: the unaltered email and the password hashed by the getPasswordHash() function of the RestService.
        // 3: the unaltered email and the password hashed by an algorithm in the "test_password_hashing.php" file.
        // 4: the result of comparing the Xamarin hashed password with the PHP hashed password.
        public async void TestPasswordHashing()
        {
            //Debug.WriteLine("********** RestService.testPasswordHashing() START **********");

            // Set the email/password combination that will be hashed and tested.
            string email = "testEmail";
            string password = "testPassword";

            // Hash the password with an algorithm defined in the hashPassword() function.
            // Subsequently store the string hash of the password in the hashedPassword variable.
            string hashedPassword = GetPasswordHash(password);

            var uri = new Uri(domain + "test_password_hashing.php");

            try
            {
                // Construct the FormUrlEncodedContent object with the email, password, and hashed password as key/value pairs.
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("hashedPassword", hashedPassword)
                });

                // Send the FormUrlEncodedContent object to the PHP file in a POST request and await a response.
                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);

                // Read the response's content as a string.
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.testPasswordHashing(): RESPONSE CONTENT AS FOLLOWS:");

                // Print the response from the PHP file.
                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.testPasswordHashing(): RESPONSE CONTENT END");
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.testPasswordHashing(): something went wrong!");
            }

            //Debug.WriteLine("********** RestService.testPasswordHashing() END **********");
        }

        // This function will send a GET request to the "test_get_data.php" file on the Apache server.
        // The function will then wait for a response from the PHP file -- the contents of the response and whether we get a response at all
        // is defined entirely by the PHP file.
        public async void Test_GetData()
        {
            // Print a message to the Debug console in Visual Studio to indicate that the test_getData() function has been called.
            //Debug.WriteLine("********** RestService.test_getData() START **********");

            // Create a "Uniform Resource Identifier" that holds the IP, port number, and directory location of the PHP web service file "test_get_data.php".
            var uri = new Uri(domain + "test_get_data.php");

            try
            {
                // Send a GET request to the "test_get_data.php" file (which it identified by the argument "uri" variable) on the Apache server and "await" a reponse.
                // The response will be stored in the "response" string.
                string response = await client.GetStringAsync(uri).ConfigureAwait(true);

                // Print the type of the response to the console -- it should print "System.String".
                //Debug.WriteLine(response.GetType());

                //Debug.WriteLine("RestService.test_getData(): RESPONSE CONTENT AS FOLLOWS:");

                // Print the contents of the response string to the console.
                // The the format and contents of the response is defined by code in the "test_get_data.php" file.
                //Debug.WriteLine(response);

                //Debug.WriteLine("RestService.test_getData(): RESPONSE CONTENT END");
            }

            // If something went wrong: catch the Exception, print the Exception, and print an error message.
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_getData(): something went wrong while testing connection to web service!");
            }

            //Debug.WriteLine("********** RestService.test_getData() END **********");
        }

        // This function will send a POST request to the "test_set_data.php" file on the Apache server.
        // The function will then wait for a response from the PHP file -- the contents of the response and whether we get a response at all
        // is defined entirely by the PHP file.
        public async void Test_SetData(RequestViewModel requestViewModel)
        {
            // Print a message to the Debug console in Visual Studio to indicate that the test_setData() function has been called.
            //Debug.WriteLine("********** RestService.test_setData() START **********");

            // Create a "Uniform Resource Identifier" that holds the IP, port number, and directory location of the PHP web service file "test_set_data.php".
            var uri = new Uri(domain + "test_set_data.php");

            try
            {
                // Send a POST request to the "test_get_data.php" file (which it identified by the argument "uri" variable) on the Apache server and "await" a reponse.
                // The response will be stored in the "response" HttpResponseMessage variable.
                // The contents of the POST request is defined by the GetFormUrlEncodedContent() method of the RequestViewModel object class.
                HttpResponseMessage response = await client.PostAsync(uri, requestViewModel.GetFormUrlEncodedContent()).ConfigureAwait(true);

                //Print the type of the response to the console -- it should be "Xamarin.Android.Net.AndroidHttpResponseMessage".
                //Debug.WriteLine(response.GetType());

                // Because the response is an HttpResponseMessage object, it has a property called "Content".
                // Print the type of the "Content" property to the console -- it should be "System.Net.Http.StreamContent".
                //Debug.WriteLine(response.Content.GetType());

                // Read the contents of the Content property as a string and store it in the "responseContent" string.
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_setData(): RESPONSE CONTENT AS FOLLOWS:");

                // Print the contents of the responseContent string to the console.
                // The the format and contents of the response is defined by code in the "test_set_data.php" file. 
                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("RestService.test_setData(): RESPONSE CONTENT END");
            }

            // If something went wrong: catch the Exception, print the Exception, and print an error message.
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_setData(): something went wrong while testing connection to web service!");
            }

            //Debug.WriteLine("********** RestService.test_setData() END **********");
        }

        // This test function will send POST request to the echo_POST.php file.
        // That PHP file will echo the contents of the POST back to this function.
        // The results are printed to the console.
        public async void Test_POST(RequestViewModel requestViewModel)
        {
            //Debug.WriteLine("********** RestService.test_POST() START **********");

            var uri = new Uri(domain + "echo_POST.php");

            try
            {
                HttpContent httpContent = requestViewModel.GetFormUrlEncodedContent();
                HttpResponseMessage response = await client.PostAsync(uri, httpContent).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_POST(): RESPONSE CONTENT AS FOLLOWS:");
                //Debug.WriteLine(responseContent);
                //Debug.WriteLine("RestService.test_POST(): RESPONSE CONTENT END");
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_POST(): something went wrong!");
            }

            //Debug.WriteLine("********** RestService.test_POST() END **********");
        }

        // This is a test function. It is meant to connect to a PHP web service that remembers the username of the user who logs in (this is done via $_SESSION in the PHP file).
        // This function will perform four actions:
        // 1: it will attempt to login.
        // 2: it will attempt to get request data -- this should only succeed if the previous login attempt succeeded.
        // 3: it will attempt to logout.
        // 4: it will attempt to get request data again -- this should fail provided the previous logout was successful.
        public async void Test_Login_GetRequestData_Logout_GetRequestData_WithSession()
        {
            //Debug.WriteLine("********** RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession() START **********");

            // The user to login as.
            string email = "testuseremail1@test.com";
            string password = "password1";

            // Get at most two rows from the database.
            int maxNumRowsToGet = 2;

            // Skip the first two rows.
            int startRowOffset = 2;

            //----------------------------------------------------------------------------------------------------------//
            // Attempt login.

            var uri = new Uri(domain + "login.php");

            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password)
                });

                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGIN: RESPONSE CONTENT AS FOLLOWS:");

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("-----------------------------------------------------------------------------------------");

                try
                {
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);

                    //Debug.WriteLine(loginResponse.GetContents());
                }
                catch (Exception e)
                {
                    Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGIN: something went wrong while deserializing \"responseContent\":");
                    Debug.WriteLine(e);
                }

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGIN: RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGIN: something went wrong while logging in!");
            }

            //----------------------------------------------------------------------------------------------------------//
            // Attempt to get request data.

            uri = new Uri(domain + "get_request_data.php");

            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: RESPONSE CONTENT AS FOLLOWS:");

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("-----------------------------------------------------------------------------------------");

                try
                {
                    List<RequestViewModel> rvmList = JsonConvert.DeserializeObject<List<RequestViewModel>>(responseContent);

                    foreach (RequestViewModel rvm in rvmList)
                    {
                        //Debug.WriteLine(rvm.GetContents());
                    }
                }
                catch (Exception e)
                {
                    Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: something went wrong while deserializing \"responseContent\":");
                    Debug.WriteLine(e);
                }

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: something went wrong while trying to get request data!");
            }

            //----------------------------------------------------------------------------------------------------------//
            // Attempt to logout.

            uri = new Uri(domain + "logout.php");

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGOUT: RESPONSE CONTENT AS FOLLOWS:");
                //Debug.WriteLine(responseContent);
                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGOUT: RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): LOGOUT: something went wrong while attempting to logout!");
            }

            //----------------------------------------------------------------------------------------------------------//
            // Attempt to get request data again. This should fail since we've already logged out.

            uri = new Uri(domain + "get_request_data.php");

            try
            {
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("maxNumRowsToGet", maxNumRowsToGet.ToString()),
                    new KeyValuePair<string, string>("startRowOffset", startRowOffset.ToString())
                });

                HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent).ConfigureAwait(true);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: RESPONSE CONTENT AS FOLLOWS:");

                //Debug.WriteLine(responseContent);

                //Debug.WriteLine("-----------------------------------------------------------------------------------------");

                try
                {
                    List<RequestViewModel> rvmList = JsonConvert.DeserializeObject<List<RequestViewModel>>(responseContent);

                    foreach (RequestViewModel rvm in rvmList)
                    {
                        //Debug.WriteLine(rvm.GetContents());
                    }
                }
                catch (Exception e)
                {
                    Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: something went wrong while deserializing \"responseContent\":");
                    Debug.WriteLine(e);
                }

                //Debug.WriteLine("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: RESPONSE CONTENT END");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.Fail("RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession(): GET REQUEST DATA: something went wrong while trying to get request data!");
            }

            //----------------------------------------------------------------------------------------------------------//

            //Debug.WriteLine("********** RestService.test_Login_GetRequestData_Logout_GetRequestData_WithSession() END **********");
        }
    }
}