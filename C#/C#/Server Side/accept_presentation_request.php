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
    exit("accept_presentation_request.php: error: User attempted to accept presentation request but wasn't logged in.");
}

else
{
	
    // Declare two variables that will contain the rows of data we want from the database.
    $timeDateCreated = $name = $orgName = $teacherEmail = $ambassadorEmail = $otherAmbassadorEmails = $phoneNumber = $altPhoneNumber = 
        $contactDayTime = $presLocation = $requestedPres = $presRotations = $handsOnActivities = $gradeLevels = $numberStudents = 
        $proposedDateTime = $supplies = $travelFee = $agreeStatement = $concerns = $altPres = '';

    // Set those variables to the values contained in the POST array.
    $timeDateCreated = $_POST['timeDateCreated'];   
	$name = $_POST['name'];
	$orgName = $_POST['orgName'];
    $teacherEmail = $_POST['teacherEmail'];
    $ambassadorEmail = $_POST['ambassadorEmail'];
    $otherAmbassadorEmails = $_POST['otherAmbassadorEmails'];
    $phoneNumber = (int) $_POST['phoneNumber'];
    $altPhoneNumber = (int) $_POST['altPhoneNumber'];
    $contactDayTime = $_POST['contactDayTime'];
    $presLocation = $_POST['presLocation'];
    $requestedPres = $_POST['requestedPres'];
    $presRotations = (int) $_POST['presRotations'];
    $handsOnActivities = $_POST['handsOnActivities'];
    $gradeLevels = $_POST['gradeLevels'];
    $numberStudents = (int) $_POST['numberStudents'];
    $proposedDateTime = $_POST['proposedDateTime'];
	$supplies = $_POST['supplies'];
    $travelFee = $_POST['travelFee'];
    $agreeStatement = $_POST['agreeStatement'];
    $concerns = $_POST['concerns'];
    $altPres = $_POST['altPres'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $timeDateCreated = cleanseInput($timeDateCreated);
    $name = cleanseInput($name); 
	$orgName = cleanseInput($orgName);
    $teacherEmail = cleanseInput($teacherEmail);
    $ambassadorEmail = cleanseInput($ambassadorEmail);
    $otherAmbassadorEmails = cleanseInput($otherAmbassadorEmails);
    $contactDayTime = cleanseInput($contactDayTime);
    $presLocation = cleanseInput($presLocation);
    $requestedPres = cleanseInput($requestedPres);
    $handsOnActivities = cleanseInput($handsOnActivities);
    $gradeLevels = cleanseInput($gradeLevels);
    $proposedDateTime = cleanseInput($proposedDateTime);
    $supplies = cleanseInput($supplies);
    $travelFee = cleanseInput($travelFee);
    $agreeStatement = cleanseInput($agreeStatement);
    $concerns = cleanseInput($concerns);
    $altPres = cleanseInput($altPres);
    
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
	
    // Set the user's email based on the value stored in current session.
    $user->email = $_SESSION['logged_in_user_email'];
	
	//Call the user.AddUser function
    $statement = $user->acceptPresentationRequest($timeDateCreated, $name, $orgName, $teacherEmail, $ambassadorEmail, $otherAmbassadorEmails,
        $phoneNumber, $altPhoneNumber, $contactDayTime, $presLocation, $requestedPres, $presRotations, $handsOnActivities, $gradeLevels,
        $numberStudents, $proposedDateTime, $supplies, $travelFee, $agreeStatement, $concerns, $altPres);
	echo $statement;
	//close the PDO connection to the database. Yes, this is how it is closed according to the manual located
	//here. https://www.php.net/manual/en/pdo.connections.php Example#3 demonstrates closing a connection.
	$databaseConnection = null;
	
}
?>