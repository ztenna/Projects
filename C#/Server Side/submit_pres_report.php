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
    exit("get_presentation_request_data.php: error: User attempted to get request data but wasn't logged in.");
}

else
{
    // Declare two variables that will contain the rows of data we want from the database.
    $email = $name = $driver = $orgName = $contactPerson = $travelFeeRecDir = $travelFeeNoNeed = $travelFeeFollowUp = $presDate = $numUniversePres = $numWaterPres = $numStarPres = 
        $numPlanetPres = $numMarsPres = $numStationPres = $numTelescopePres = $numNANOGravPres = $numMentorPres = $numSolarActs = $numMoonActs = $numEngDesignActs = 
        $numRobArmActs = $numElectroWarActs = $numAlienActs = $numMoleculeActs = $numBucketActs = $numWatershedActs = $sameEverything = $anythingDiff = $firstTime = 
        $num_KTo3_Stu = $num_4To5_Stu = $num_6To8_Stu = $num_9To12_Stu = $numCollegeStu = $numTeachers = $otherAudience = $engagedRating = $appropriateRating = $askedQuestions = 
        $answerQuestions = $aspectsWorkBest = $presSuggestions = '';

    // Set those variables to the values contained in the POST array.
    $email = $_POST['email'];
    $name = $_POST['name'];
    $driver = $_POST['driver'];
    $orgName = $_POST['orgName'];
    $contactPerson = $_POST['contactPerson'];
    $travelFeeRecDir = (int) $_POST['travelFeeRecDir'];
    $travelFeeNoNeed = (int) $_POST['travelFeeNoNeed'];
    $travelFeeFollowUp = (int) $_POST['travelFeeFollowUp'];
    $presDate = $_POST['presDate'];
    $numUniversePres = (int) $_POST['numUniversePres'];
    $numWaterPres = (int) $_POST['numWaterPres'];
    $numStarPres = (int) $_POST['numStarPres'];
    $numPlanetPres = (int) $_POST['numPlanetPres'];
    $numMarsPres = (int) $_POST['numMarsPres'];
    $numStationPres = (int) $_POST['numStationPres'];
    $numTelescopePres = (int) $_POST['numTelescopePres'];
    $numNANOGravPres = (int) $_POST['numNANOGravPres'];
    $numMentorPres = (int) $_POST['numMentorPres'];
    $numSolarActs = (int) $_POST['numSolarActs'];
    $numMoonActs = (int) $_POST['numMoonActs'];
    $numEngDesignActs = (int) $_POST['numEngDesignActs'];
    $numRobArmActs = (int) $_POST['numRobArmActs'];
    $numElectroWarActs = (int) $_POST['numElectroWarActs'];
    $numAlienActs = (int) $_POST['numAlienActs'];
    $numMoleculeActs = (int) $_POST['numMoleculeActs'];
    $numBucketActs = (int) $_POST['numBucketActs'];
    $numWatershedActs = (int) $_POST['numWatershedActs'];
    $sameEverything = (int) $_POST['sameEverything'];
    $anythingDiff = $_POST['anythingDiff'];
    $firstTime = $_POST['firstTime'];
    $num_KTo3_Stu = (int) $_POST['num_KTo3_Stu'];
    $num_4To5_Stu = (int) $_POST['num_4To5_Stu'];
    $num_6To8_Stu = (int) $_POST['num_6To8_Stu'];
    $num_9To12_Stu = (int) $_POST['num_9To12_Stu'];
    $numCollegeStu = (int) $_POST['numCollegeStu'];
    $numTeachers = (int) $_POST['numTeachers'];
    $otherAudience = $_POST['otherAudience'];
    $engagedRating = (int) $_POST['engagedRating'];
    $appropriateRating = (int) $_POST['appropriateRating'];
    $askedQuestions = $_POST['askedQuestions'];
    $answerQuestions = $_POST['answerQuestions'];
    $aspectsWorkBest = $_POST['aspectsWorkBest'];
    $presSuggestions = $_POST['presSuggestions'];

    // Run the user input through the cleanseInput() function.
    // See utility_functions.php for a definition of cleanseInput().
    $email = cleanseInput($email);
    $name = cleanseInput($name);
    $driver = cleanseInput($driver);
    $orgName = cleanseInput($orgName);
    $contactPerson = cleanseInput($contactPerson);
    $presDate = cleanseInput($presDate);
    $anythingDiff = cleanseInput($anythingDiff);
    $firstTime = cleanseInput($firstTime);
    $otherAudience = cleanseInput($otherAudience);
    $askedQuestions = cleanseInput($askedQuestions);
    $answerQuestions = cleanseInput($answerQuestions);
    $aspectsWorkBest = cleanseInput($aspectsWorkBest);
    $presSuggestions = cleanseInput($presSuggestions);
    //echo "Done cleaning";
    
    // Declare and construct the database object.
    $database = new Database();
    
    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);
    
    
    // Set the user's email based on the value stored in current session.
    $user->email = $_SESSION['logged_in_user_email'];
    
    //Call the user.submitPresReport function
    $statement = $user->submitPresReport($email, $name, $driver, $orgName, $contactPerson, $travelFeeRecDir, $travelFeeNoNeed, $travelFeeFollowUp, $presDate, $numUniversePres, 
    $numWaterPres, $numStarPres, $numPlanetPres, $numMarsPres, $numStationPres, $numTelescopePres, $numNANOGravPres, $numMentorPres, $numSolarActs, $numMoonActs, 
    $numEngDesignActs, $numRobArmActs, $numElectroWarActs, $numAlienActs, $numMoleculeActs, $numBucketActs, $numWatershedActs, $sameEverything, $anythingDiff, $firstTime, 
    $num_KTo3_Stu, $num_4To5_Stu, $num_6To8_Stu, $num_9To12_Stu, $numCollegeStu, $numTeachers, $otherAudience, $engagedRating, $appropriateRating, $askedQuestions, 
    $answerQuestions, $aspectsWorkBest, $presSuggestions);
    echo $statement;
    //echo " Done in php";
    echo json_encode($statement);
    //close the PDO connection to the database. Yes, this is how it is closed according to the manual located
    //here. https://www.php.net/manual/en/pdo.connections.php Example#3 demonstrates closing a connection.
    $databaseConnection = null;
}
?>