<?php
// Include the PHP files that define objects or functions we need in this file.
include_once '../application_files/database.php';
include_once '../application_files/user.php';  
include_once '../application_files/utility_functions.php';
// In order to check if user is logged in, we have to have access to the $_SESSION variable.
// We can get this by calling the following function.
session_start();

// If the POST request doesn't have values for the keys "email" and "password", don't do anything.
if (isset($_POST['name']) && isset($_POST['orgName']) && isset($_POST['email']) && isset($_POST['phoneNumber']) && isset($_POST['altPhoneNumber']) 
    && isset($_POST['contactDayTime']) && isset($_POST['orgStreet']) && isset($_POST['orgCity']) && isset($_POST['orgState']) && isset($_POST['orgZip']) 
    && isset($_POST['anyPres']) && isset($_POST['invisiblePres']) && isset($_POST['mentorPres']) && isset($_POST['starPres']) && isset($_POST['waterPres']) 
    && isset($_POST['planetPres']) && isset($_POST['marsPres']) && isset($_POST['stationPres']) && isset($_POST['telescopePres']) && isset($_POST['NANOGravPres']) 
    && isset($_POST['presRotations']) && isset($_POST['noHandsOn']) && isset($_POST['buildMolecule']) && isset($_POST['designAlien']) && isset($_POST['pocketSolSys']) 
    && isset($_POST['sizeUpMoon']) && isset($_POST['telescopeDesign']) && isset($_POST['roboticArm']) && isset($_POST['electroWar']) && isset($_POST['buildWatershed']) 
    && isset($_POST['dropInBucket']) && isset($_POST['gradeLevels']) && isset($_POST['numberStudents']) && isset($_POST['proposedDateTime']) && isset($_POST['projectSupplies']) 
    && isset($_POST['speakerSupplies']) && isset($_POST['compSupplies']) && isset($_POST['cordSupplies']) && isset($_POST['micSupplies']) && isset($_POST['otherSupplies']) 
    && isset($_POST['otherSupplies']) && isset($_POST['travelFee']) && isset($_POST['agreeStatement']) && isset($_POST['concerns']) && isset($_POST['collegePres'])
    && isset($_POST['skypePres']) && isset($_POST['altOtherPres']))
{
	
    // Declare two variables that will contain the rows of data we want from the database.
    $name = $orgName = $email = $phoneNumber = $altPhoneNumber = $contactDayTime = $orgStreet = $orgCity = $orgState = $orgZip = $anyPres = $invisiblePres = $mentorPres =
        $starPres = $waterPres = $planetPres = $marsPres = $stationPres = $telescopePres = $NANOGravPres = $presRotations = $noHandsOn = $buildMolecule = $designAlien = 
        $pocketSolSys = $sizeUpMoon = $telescopeDesign = $roboticArm = $electroWar = $buildWatershed = $dropInBucket = $gradeLevels = $numberStudents = $proposedDateTime = 
        $projectSupplies = $speakerSupplies = $compSupplies = $cordSupplies = $micSupplies = $otherSupplies = $travelFee = $agreeStatement = $concerns = $collegePres = 
        $skypePres = $altOtherPres = '';

    // Set those variables to the values contained in the POST array.
    $name = $_POST['name'];
    $orgName = $_POST['orgName'];
    $email = $_POST['email'];
    $phoneNumber = (int) $_POST['phoneNumber'];
    $altPhoneNumber = (int) $_POST['altPhoneNumber'];
    $contactDayTime = $_POST['contactDayTime'];
    $orgStreet = $_POST['orgStreet'];
    $orgCity = $_POST['orgCity'];
    $orgState = $_POST['orgState'];
    $orgZip = (int) $_POST['orgZip'];
    $anyPres = (int) $_POST['anyPres'];
    $invisiblePres = (int) $_POST['invisiblePres'];
    $mentorPres = (int) $_POST['mentorPres'];
    $starPres = (int) $_POST['starPres'];
    $waterPres = (int) $_POST['waterPres'];
    $planetPres = (int) $_POST['planetPres'];
    $marsPres = (int) $_POST['marsPres'];
    $stationPres = (int) $_POST['stationPres'];
    $telescopePres = (int) $_POST['telescopePres'];
    $NANOGravPres = (int) $_POST['NANOGravPres'];
    $presRotations = (int) $_POST['presRotations'];
    $noHandsOn = (int) $_POST['noHandsOn'];
    $buildMolecule = (int) $_POST['buildMolecule'];
    $designAlien = (int) $_POST['designAlien'];
    $pocketSolSys = (int) $_POST['pocketSolSys'];
    $sizeUpMoon = (int) $_POST['sizeUpMoon'];
    $telescopeDesign = (int) $_POST['telescopeDesign'];
    $roboticArm = (int) $_POST['roboticArm'];
    $electroWar = (int) $_POST['electroWar'];
    $buildWatershed = (int) $_POST['buildWatershed'];
    $dropInBucket = (int) $_POST['dropInBucket'];
    $gradeLevels = $_POST['gradeLevels'];
    $numberStudents = (int) $_POST['numberStudents'];
    $proposedDateTime = $_POST['proposedDateTime'];
    $projectSupplies = (int) $_POST['projectSupplies'];
    $speakerSupplies = (int) $_POST['speakerSupplies'];
    $compSupplies = (int) $_POST['compSupplies'];
    $cordSupplies = (int) $_POST['cordSupplies'];
    $micSupplies = (int) $_POST['micSupplies'];
    $otherSupplies = $_POST['otherSupplies'];
    $travelFee = (int) $_POST['travelFee'];
    $agreeStatement = (int) $_POST['agreeStatement'];
    $concerns = $_POST['concerns'];
    $collegePres = (int) $_POST['collegePres'];
    $skypePres = (int) $_POST['skypePres'];
    $altOtherPres = $_POST['altOtherPres'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $name = cleanseInput($name);
    $orgName = cleanseInput($orgName);
    $email = cleanseInput($email);
    $contactDayTime = cleanseInput($contactDayTime);
    $orgStreet = cleanseInput($orgStreet);
    $orgCity = cleanseInput($orgCity);
    $orgState = cleanseInput($orgState);
    $gradeLevels = cleanseInput($gradeLevels);
    $proposedDateTime = cleanseInput($proposedDateTime);
    $otherSupplies = cleanseInput($otherSupplies);
    $concerns = cleanseInput($concerns);
    $altOtherPres = cleanseInput($altOtherPres);
    
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
	
    // Set the user's email based on the value stored in current session.
    $user->email = 'testuseremail1@test.com';
	
	//Call the user.AddUser function
    $statement = $user->submitPresRequest($name, $orgName, $email, $phoneNumber, $altPhoneNumber, $contactDayTime, $orgStreet, $orgCity, $orgState, $orgZip, $anyPres, 
    $invisiblePres, $mentorPres, $starPres, $waterPres, $planetPres, $marsPres, $stationPres, $telescopePres, $NANOGravPres, $presRotations, $noHandsOn, $buildMolecule,
    $designAlien, $pocketSolSys, $sizeUpMoon, $telescopeDesign, $roboticArm, $electroWar, $buildWatershed, $dropInBucket, $gradeLevels, $numberStudents, $proposedDateTime,
    $projectSupplies, $speakerSupplies, $compSupplies, $cordSupplies, $micSupplies, $otherSupplies, $travelFee, $agreeStatement, $concerns, $collegePres, $skypePres, $altOtherPres);
	echo $statement;
	//close the PDO connection to the database. Yes, this is how it is closed according to the manual located
	//here. https://www.php.net/manual/en/pdo.connections.php Example#3 demonstrates closing a connection.
	$databaseConnection = null;
	
}

else
{
	
	echo "Not all parameters have been set for submit_pres_request.php";
	
}
?>