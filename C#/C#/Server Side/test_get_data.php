<?php
// Include the database.php file wherein the Database class is defined.
include_once '../application_files/database.php';

// Include the user.php file wherein the User class is defined.
include_once '../application_files/user.php';  

// Include the utility_functions.php files wherein several utility functions are defined.
include_once '../application_files/utility_functions.php';

// Construct the Database object.
$database = new Database();

// Call the getConnection() function of the Database class.
// This function returns a connection to the SQL database if the connection was successfully created.
// The database that Database class connects to is defined in the data members of the Database class.
$databaseConnection = $database->getConnection();

// Construct a new User object from the connection to the SQL database.
// The User class has two functions -- in this file, we're interested in the test_getData() function.
$user = new User($databaseConnection);

// Call the test_getData() function of the User class and store the query results in the "$statement_results" variable.
$statement_results = $user->test_getData();

// When calling the echo function (or print_r or var_dump), the results of those function calls are sent back to the *thing* that called this php page.
// This means that since the Xamarin application called this file, whatever is echoed will be sent back to the Xamarin application.
// So, by calling the echoKeyValuePair() function below, I am sending the key value pairs from the $statement_results variable back to the application.
foreach($statement_results->fetchAll() as $key => $value)
{   
    // The following function is defined in the "utility_functions.php" file.
    // This function call is what causes the data of the query to be sent back to the Xamarin application.
    echoKeyValuePair($key, $value);
}
?>