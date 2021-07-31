<?php
// Include the PHP files that define objects or functions we need in this file.
include_once '../application_files/database.php'; 
include_once '../application_files/utility_functions.php';
// In order to check if user is logged in, we have to have access to the $_SESSION variable.
// We can get this by calling the following function.
session_start();

// If the key "logged_in_user_email" is not set or is empty, then the user isn't logged in and shouldn't be able to get request data.
if (!isset($_SESSION['logged_in_user_email']) || empty($_SESSION['logged_in_user_email']))
{       
    exit("edit_presentation_request.php: error: User attempted to edit presentation request but wasn't logged in.");
}

else
{
    $server = getValue("ServerName");
    $dbName = getValue("DatabaseName");
    $user = getValue("Username");
    $pass = getValue("Password");
    $dbConnection = new mysqli($server, $user, $pass, $dbName);

    // Check connection to database
    if ($dbConnection->connect_error) {
        die("Connection Failed: ".$dbConnection->connect_error);
        exit();
    }
	
    // Declare two variables that will contain the rows of data we want from the database.
    $presentationID = $name = $orgName = $teacherEmail = $ambassadorEmail = $otherAmbassadorEmails = $phoneNumber = $altPhoneNumber = 
        $contactDayTime = $presLocation = $requestedPres = $presRotations = $handsOnActivities = $gradeLevels = $numberStudents = 
        $proposedDateTime = $supplies = $travelFee = $concerns = $altPres = '';

    // Set those variables to the values contained in the POST array.
    $presentationID = (int) $_POST['presentationID']; 
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
    $concerns = $_POST['concerns'];
    $altPres = $_POST['altPres'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $name = cleanseInput($name); 
	$orgName = cleanseInput($orgName);
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
    $concerns = cleanseInput($concerns);
    $altPres = cleanseInput($altPres);    
    
    $preparedStatement = $dbConnection->prepare("UPDATE `spot`.`Presentations` SET `name` = ?, `orgName` = ?, `ambassadorEmail` = ?, `otherAmbassadorEmails` = ?, `phoneNumber` = ?, `altPhoneNumber` = ?, `contactDayTime` = ?, `presLocation` = ?, `requestedPres` = ?, `presRotations` = ?, `handsOnActivities` = ?, `gradeLevels` = ?, `numberStudents` = ?, `proposedDateTime` = ?, `supplies` = ?, `travelFee` = ?, `concerns` = ?, `altPres` = ? WHERE (`presentationID` = ?) and (`teacherEmail` = ?);");

    $preparedStatement->bind_param("ssssiisssississsssis", $name, $orgName, $ambassadorEmail, $otherAmbassadorEmails, $phoneNumber, $altPhoneNumber, $contactDayTime, $presLocation, $requestedPres, $presRotations, $handsOnActivities, $gradeLevels, $numberStudents, $proposedDateTime, $supplies, $travelFee, $concerns, $altPres, $presentationID, $teacherEmail);

    $preparedStatement->execute();

    echo $dbConnection->error;

    $preparedStatement->close();
    mysqli_close($dbConnection);	
}

function getValue($key)
{
    $ini_array = parse_ini_file("../database.properties");
    $value =  $ini_array[$key];
    return $value;
}
?>