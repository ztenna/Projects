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
// The User class has two functions -- in this file, we're interested in the test_setData() function.
$user = new User($databaseConnection);

// Call the test_setData() function of the User class and store the query results in the "$statement_results" variable.
$statement_results = $user->test_setData();

// The following foreach loop echos the contents of the POST (which was sent by the Xamarin application) back to the Xamarin application in a formatted manner.
// NOTE: "PHP_EOL" serves the same purpose as a "new line" character.
foreach ($_POST as $key => $value)
{
    echo $key." => ";

    if (is_array($value))
    {
        foreach ($value as $arrayItem)
        {
            echo $arrayItem.PHP_EOL;
        }
    }
    
    else
    {
        echo $value.PHP_EOL;
    }
    
    echo PHP_EOL;
}
?>