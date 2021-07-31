<?php
// Include the PHP files that define objects or functions we need in this file.
include_once '../application_files/database.php';
include_once '../application_files/user.php';  
include_once '../application_files/utility_functions.php';

// In order to check if user is logged in, we have to have access to the $_SESSION variable.
// We can get this by calling the following function.
session_start();

// If the key "logged_in_user_email" is not set or is empty, then the user isn't logged in and shouldn't be able to get request data.
if (!isset($_SESSION['logged_in_user_email']) || empty($_SESSION['logged_in_user_email']))
{       
    exit("get_request_data.php: error: User attempted to get request data but wasn't logged in.");
}

// If the POST request doesn't have values for the keys "startRow" and "endRow", don't do anything.
else if (isset($_POST['maxNumRowsToGet']) && isset($_POST['startRowOffset']))
{    
    // Declare two variables that will define which rows we get from the database.
    $maxNumRowsToGet = $startRowOffset = 0;

    // Set those variables to the values contained in the POST array.
    $maxNumRowsToGet = $_POST['maxNumRowsToGet'];
    $startRowOffset = $_POST['startRowOffset'];  
    //echo "maxNumRowsToGet: " . $maxNumRowsToGet.PHP_EOL;
    //echo "startRowOffset: " . $startRowOffset.PHP_EOL; 

    // This is technically unnecessary, but call the cleanseInput() function to remove whitespace, slashes, and convert HTML special characters.
    //$maxNumRowsToGet = cleanseInput($maxNumRowsToGet);
    //$startRowOffset = cleanseInput($startRowOffset); 
        
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
    // Set the user's email based on the value stored in current session.
    $user->email = $_SESSION['logged_in_user_email'];
        
    // Call the getRequestData() function with the specified start and end row indexes.
    $statement = $user->getSignupRequestData($maxNumRowsToGet, $startRowOffset);
    
    // Set the fetch mode to an associative array.
    $statement->setFetchMode(PDO::FETCH_ASSOC);
    
    // Declare the array that will contain request information arrays.
    $requests = array();
    
    try
    {        
        // Fetch individual rows from the query results and construct a "request" array of key/value pairs for each row.
        while($row = $statement->fetch())
        {            
            // Construct an array that represents a single request from a single row of the query results.
            // NOTE: the name of the key (for example the key "Email") MUST match the name of the property of the SignupRequestViewModel class back in the Xamarin application.
            // This is because, when the JsonConvert.DeserializeObject() function is called in the application, it will look for keys in this array that correspond to properties of the SignupRequestViewModel object.
            // If the array key matches the property of the object, then it will set the property of the object to the value associated with the matching key.
            $request = array(
				"Name" => $row['name'],	
				"Email" => $row['email'],
                "PhoneNumber" => $row['phoneNumber'],
				"Address" => $row['address'],
				"School" => $row['school'],
				"Major" => $row['major'],
                "HearAboutSpot" => $row['hearAboutSpot'],
                "WhySpotPresenter" => $row['whySpotPresenter'],
                "ExpWithStudents" => $row['expWithStudents'],
                "NotAsPlanned" => $row['notAsPlanned'],
                "RecommendationLetterName" => $row['recommendationLetterName'],
                "RecommendationLetterEmail" => $row['recommendationLetterEmail'],
                "DietaryRestrictions" => $row['dietaryRestrictions'],
                "IsNewApplicant" => $row['isNewApplicant'],
				"IsCurrentAmbassador" => $row['isCurrentAmbassador'],
                "Monday" => $row['monday'],
				"Tuesday" => $row['tuesday'],
                "Wednesday" => $row['wednesday'],
                "Thursday" => $row['thursday'],
                "Friday" => $row['friday'],
                "FallBreak" => $row['fallBreak'],
                "WinterBreak" => $row['winterBreak'],
                "SpringBreak" => $row['springBreak'],
                "AvailableVisitOther" => $row['availableVisitOther'],
                "HaveLicense" => $row['haveLicense'],
                "HaveCar" => $row['haveCar'],
                "AgreeToStatement" => $row['agreeToStatement'],
                "AgreeToDoComponents" => $row['agreeToDoComponents'],
                "CanAttendTraining" => $row['canAttendTraining'],
                "Male" => $row['male'],
                "Female" => $row['female'],
                "GenderOther" => $row['genderOther'],
                "FirstToCollege" => $row['firstToCollege'],
                "OfOrigin" => $row['ofOrigin'],
                "AfricanAmerican" => $row['africanAmerican'],
                "AmericanIndianOrAlaskanNative" => $row['americanIndianOrAlaskanNative'],
                "PacificIslander" => $row['pacificIslander'],
                "Asian" => $row['asian'],
                "Caucasian" => $row['caucasian'],
                "RaceOther" => $row['raceOther'],
                "RuralCommunity" => $row['ruralCommunity'],
                "SuburbanCommunity" => $row['suburbanCommunity'],
                "UrbanCommunity" => $row['urbanCommunity'],
                "PrivacyPolicy" => $row['privacyPolicy']);
            
            // Add the request array to the requests array.
            array_push($requests, $request);
        }
        
        // Send a JSON encoded version of the requests array back to the Xamarin application.
        // The following two lines of code (from the Xamarin application) are responsible for decoding this JSON string:
        //
        //      HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent); // Get the HTTP response from the PHP file (this contains this encoded JSON array).
        //
        //      string responseContent = await response.Content.ReadAsStringAsync(); // Pull the encoded JSON array out of the HTTP response.
        //
        //      List<SignupRequestViewModel> rvmList = JsonConvert.DeserializeObject<List<SignupRequestViewModel>>(responseContent); // Deserialize (as in unencode) the JSON array and construct a List of SignupRequestViewModel objects from it.
        echo json_encode($requests);
    }
    
    catch(Exception $e)
    {
        echo $e->getMessage().PHP_EOL;
    }
}
?>