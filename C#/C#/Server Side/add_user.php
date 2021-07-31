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
// If the POST request doesn't have values for the keys "email" and "password", don't do anything.
/*else if (isset($_POST['email']) && isset($_POST['password']) && isset($_POST['firstName']) && isset($_POST['lastName']) && 
    isset($_POST['phoneNumber']) && isset($_POST['address']) && isset($_POST['school']) && isset($_POST['major']) && 
    isset($_POST['hearAboutSpot']) && isset($_POST['whySpotPresenter']) && isset($_POST['expWithStudents']) && isset($_POST['notAsPlanned']) && 
    isset($_POST['recommendationLetterName']) && isset($_POST['recomendationLetterEmail']) && isset($_POST['dietaryRestrictions']) && 
    isset($_POST['isAmbassador']) && isset($_POST['isAdmin']) && isset($_POST['availableToVisit']) && isset($_POST['haveLicense']) &&
    isset($_POST['haveCar']) && isset($_POST['canAttendTraining']) && isset($_POST['gender']) && isset($_POST['firstToCollege']) && 
    isset($_POST['ofOrigin']) && isset($_POST['race']) && isset($_POST['community']) && isset($_POST['privacyPolicy']))
*/else{
	
    // Declare two variables that will contain the rows of data we want from the database.
    $email = $password = $firstName = $lastName = $phoneNumber = $address = $school = $major = $hearAboutSpot = $whySpotPresenter = 
        $expWithStudents = $notAsPlanned = $recommendationLetterName = $recommendationLetterEmail = $dietaryRestrictions = 
        $isAmbassador = $isAdmin = $availableToVisit = $haveLicense = $haveCar = $canAttendTraining = $gender = $firstToCollege = 
        $ofOrigin = $race = $community = $privacyPolicy= '';

    // Set those variables to the values contained in the POST array.
    $email = $_POST['email'];
    $password = $_POST['password'];   
	$firstName = $_POST['firstName'];
	$lastName = $_POST['lastName'];
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
	$isAmbassador = (int) $_POST['isAmbassador'];
    $isAdmin = (int) $_POST['isAdmin'];
    $availableToVisit = $_POST['availableToVisit'];
    $haveLicense = $_POST['haveLicense'];
    $haveCar = $_POST['haveCar'];
    $canAttendTraining = $_POST['canAttendTraining'];
    $gender = $_POST['gender'];
    $firstToCollege = $_POST['firstToCollege'];
    $ofOrigin = $_POST['ofOrigin'];
    $race = $_POST['race'];
    $community = $_POST['community'];
    $privacyPolicy = $_POST['privacyPolicy'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $email = cleanseInput($email);
    $password = cleanseInput($password); 
	$firstName = cleanseInput($firstName);
    $lastName = cleanseInput($lastName);
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
    $availableToVisit = cleanseInput($availableToVisit);
    $haveLicense = cleanseInput($haveLicense);
    $haveCar = cleanseInput($haveCar);
    $canAttendTraining = cleanseInput($canAttendTraining);
    $gender = cleanseInput($gender);
    $firstToCollege = cleanseInput($firstToCollege);
    $ofOrigin = cleanseInput($ofOrigin);
    $race = cleanseInput($race);
    $community = cleanseInput($community);
    $privacyPolicy = cleanseInput($privacyPolicy);
	//$phoneNumber = cleanseInput($phoneNumber);
	//$isAmbassador = cleanseInput ($isAmbassador);
	//$isAdmin= cleanseInput ($isAdmin);

    
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
	
    // Set the user's email based on the value stored in current session.
    $user->email = $_SESSION['logged_in_user_email'];
	
	//Call the user.AddUser function
    $statement = $user->addUser($email, $password, $firstName, $lastName, $phoneNumber, $address, $school, $major, $hearAboutSpot,
    $whySpotPresenter, $expWithStudents, $notAsPlanned, $recommendationLetterName, $recommendationLetterEmail, $dietaryRestrictions, 
    $isAmbassador, $isAdmin, $availableToVisit, $haveLicense, $haveCar, $canAttendTraining, $gender, $firstToCollege, 
    $ofOrigin, $race, $community, $privacyPolicy);
	echo $statement;
	//close the PDO connection to the database. Yes, this is how it is closed according to the manual located
	//here. https://www.php.net/manual/en/pdo.connections.php Example#3 demonstrates closing a connection.
	$databaseConnection = null;
	
}

/*else
{
	
	echo "Not all parameters have been set for add_user.php";
	
}*/
?>