<?php
// Include the PHP files that define objects or functions we need in this file.
include_once '../application_files/database.php';
include_once '../application_files/user.php';  
include_once '../application_files/utility_functions.php';
// In order to check if user is logged in, we have to have access to the $_SESSION variable.
// We can get this by calling the following function.
session_start();

// If the key "logged_in_user_email" is not set or is empty, then the user isn't logged in and shouldn't be able to get request data.
//if (!isset($_SESSION['logged_in_user_email']) || empty($_SESSION['logged_in_user_email']))
//{       
//    exit("get_request_data.php: error: User attempted to get request data but wasn't logged in.");
//}
// If the POST request doesn't have values for the keys "email" and "password", don't do anything.
if (isset($_POST['name']) && isset($_POST['email']) && isset($_POST['phoneNumber']) && isset($_POST['address']) && isset($_POST['school']) && isset($_POST['major']) 
    && isset($_POST['hearAboutSpot']) && isset($_POST['whySpotPresenter']) && isset($_POST['expWithStudents']) && isset($_POST['notAsPlanned']) 
    && isset($_POST['recommendationLetterName']) && isset($_POST['recommendationLetterEmail']) && isset($_POST['dietaryRestrictions']) && isset($_POST['monday']) 
    && isset($_POST['isNewApplicant']) && isset($_POST['isCurrentAmbassador']) && isset($_POST['tuesday']) && isset($_POST['wednesday']) && isset($_POST['thursday']) 
    && isset($_POST['friday']) && isset($_POST['fallBreak']) && isset($_POST['winterBreak']) && isset($_POST['springBreak']) && isset($_POST['availableVisitOther']) 
    && isset($_POST['haveLicense']) && isset($_POST['haveCar']) && isset($_POST['agreeToStatement']) && isset($_POST['agreeToDoComponents']) 
    && isset($_POST['canAttendTraining']) && isset($_POST['male']) && isset($_POST['female']) && isset($_POST['genderOther']) && isset($_POST['firstToCollege']) 
    && isset($_POST['ofOrigin']) && isset($_POST['africanAmerican']) && isset($_POST['americanIndianOrAlaskanNative']) && isset($_POST['pacificIslander']) 
    && isset($_POST['asian']) && isset($_POST['caucasian']) && isset($_POST['raceOther']) && isset($_POST['ruralCommunity']) && isset($_POST['suburbanCommunity']) 
    && isset($_POST['urbanCommunity']) && isset($_POST['privacyPolicy']))
{
	
    // Declare two variables that will contain the rows of data we want from the database.
    $name = $email = $phoneNumber = $address = $school = $major = $hearAboutSpot = $whySpotPresenter = $expWithStudents = $notAsPlanned = $recommendationLetterName =
        $recommendationLetterEmail = $dietaryRestrictions = $isNewApplicant = $isCurrentAmbassador = $monday = $tuesday = $wednesday = $thursday = $friday = $fallBreak = 
        $winterBreak = $springBreak = $availableVisitOther = $haveLicense = $haveCar = $agreeToStatement = $agreeToDoComponents = $canAttendTraining = $male = $female = 
        $genderOther = $firstToCollege = $ofOrigin = $africanAmerican = $americanIndianOrAlaskanNative = $pacificIslander = $asian = $caucasian = $raceOther = 
        $ruralCommunity = $suburbanCommunity = $urbanCommunity = $privacyPolicy = '';

    // Set those variables to the values contained in the POST array.
    $name = $_POST['name'];
    $email = $_POST['email'];
    $phoneNumber = (int) $_POST['phoneNumber'];
    $address = $_POST['address'];
    $school = $_POST['school'];
    $major = $_POST['major'];
    $hearAboutSpot = $_POST['hearAboutSpot'];
    $whySpotPresenter = $_POST['whySpotPresenter'];
    $expWithStudents = $_POST['expWithStudents'];
    $notAsPlanned = $_POST['notAsPlanned'];
    $recommendationLetterName = $_POST['recommendationLetterName'];
    $recommendationLetterEmail = $_POST['recommendationLetterEmail'];
    $dietaryRestrictions = $_POST['dietaryRestrictions'];
    $isNewApplicant = (int) $_POST['isNewApplicant'];
    $isCurrentAmbassador = (int) $_POST['isCurrentAmbassador'];
    $monday = (int) $_POST['monday'];
    $tuesday = (int) $_POST['tuesday'];
    $wednesday = (int) $_POST['wednesday'];
    $thursday = (int) $_POST['thursday'];
    $friday = (int) $_POST['friday'];
    $fallBreak = (int) $_POST['fallBreak'];
    $winterBreak = (int) $_POST['winterBreak'];
    $springBreak = (int) $_POST['springBreak'];
    $availableVisitOther = $_POST['availableVisitOther'];
    $haveLicense = (int) $_POST['haveLicense'];
    $haveCar = (int) $_POST['haveCar'];
    $agreeToStatement = (int) $_POST['agreeToStatement'];
    $agreeToDoComponents = (int) $_POST['agreeToDoComponents'];
    $canAttendTraining = (int) $_POST['canAttendTraining'];
    $male = (int) $_POST['male'];
    $female = (int) $_POST['female'];
    $genderOther = $_POST['genderOther'];
    $firstToCollege = (int) $_POST['firstToCollege'];
    $ofOrigin = (int) $_POST['ofOrigin'];
    $africanAmerican = (int) $_POST['africanAmerican'];
    $americanIndianOrAlaskanNative = (int) $_POST['americanIndianOrAlaskanNative'];
    $pacificIslander = (int) $_POST['pacificIslander'];
    $asian = (int) $_POST['asian'];
    $caucasian = (int) $_POST['caucasian'];
    $raceOther = $_POST['raceOther'];
    $ruralCommunity = (int) $_POST['ruralCommunity'];
    $suburbanCommunity = (int) $_POST['suburbanCommunity'];
    $urbanCommunity = (int) $_POST['urbanCommunity'];
    $privacyPolicy = $_POST['privacyPolicy'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $name = cleanseInput($name);
    $email = cleanseInput($email);
    $address = cleanseInput($address);
    $school = cleanseInput($school);
    $major = cleanseInput($major);
    $hearAboutSpot = cleanseInput($hearAboutSpot);
    $whySpotPresenter = cleanseInput($whySpotPresenter);
    $expWithStudents = cleanseInput($expWithStudents);
    $notAsPlanned = cleanseInput($notAsPlanned);
    $recommendationLetterName = cleanseInput($recommendationLetterName);
    $recommendationLetterEmail = cleanseInput($recommendationLetterEmail);
    $dietaryRestrictions = cleanseInput($dietaryRestrictions);
    $availableVisitOther = cleanseInput($availableVisitOther);
    $genderOther = cleanseInput($genderOther);
    $raceOther = cleanseInput($raceOther);
    $privacyPolicy = cleanseInput($privacyPolicy);
    
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
	
    // Set the user's email based on the value stored in current session.
    $user->email = 'testuseremail1@test.com';
	
	//Call the user.AddUser function
    $statement = $user->addPotentialUser($name, $email, $phoneNumber, $address, $school, $major, $hearAboutSpot, $whySpotPresenter, $expWithStudents, $notAsPlanned, 
        $recommendationLetterName, $recommendationLetterEmail, $dietaryRestrictions, $isNewApplicant, $isCurrentAmbassador, $monday, $tuesday, $wednesday, $thursday, 
        $friday, $fallBreak, $winterBreak, $springBreak, $availableVisitOther, $haveLicense, $haveCar, $agreeToStatement, $agreeToDoComponents, $canAttendTraining, 
        $male, $female, $genderOther, $firstToCollege, $ofOrigin, $africanAmerican, $americanIndianOrAlaskanNative, $pacificIslander, $asian, $caucasian, $raceOther, 
        $ruralCommunity, $suburbanCommunity, $urbanCommunity, $privacyPolicy);
	echo $statement;
	//close the PDO connection to the database. Yes, this is how it is closed according to the manual located
	//here. https://www.php.net/manual/en/pdo.connections.php Example#3 demonstrates closing a connection.
	$databaseConnection = null;
	
}

else
{
	
	echo "Not all parameters have been set for add_potential_user.php";
	
}
?>