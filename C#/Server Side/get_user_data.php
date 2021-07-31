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
    exit("get_user_data.php: error: User data attempted to load but user is not logged in.");
}

// Otherwise user session is created and we can pull data from them
else
{
    // Declare and construct the database object.
    $database = new Database();

    // Get a connection to the database.
    $databaseConnection = $database->getConnection();

    // Construct a User based on the connection to the database.
    $user = new User($databaseConnection);

    // Set the user's email based on the value stored in current session.
    $user->email = $_SESSION['logged_in_user_email'];

    // Call the getUserData() function.
    $statement = $user->getUserData();

    // Set the fetch mode to an associative array.
    $statement->setFetchMode(PDO::FETCH_ASSOC);

	$row = $statement->fetch();
    // Construct an array that represents a user from the query result.
    // NOTE: the name of the key (for example the key "Email") MUST match the name of the property of the USer class back in the Xamarin application.
    // This is because, when the JsonConvert.DeserializeObject() function is called in the application, it will look for keys in this array that correspond to properties of the User object.
    // If the array key matches the property of the object, then it will set the property of the object to the value associated with the matching key.
    $user_data = array("Email" => $row['email'],
        "FirstName" => $row['first_name'],
        "LastName" => $row['last_name'],
        "PrimaryPhoneNumber" => $row['phone_number'],
		"IsAmbassador" => $row['IsAmbassador'],
		"IsAdmin" => $row['isAdministrator']);

    // Send a JSON encoded version of the requests array back to the Xamarin application.
    // The following two lines of code (from the Xamarin application) are responsible for decoding this JSON string:
    //
    //      HttpResponseMessage response = await client.PostAsync(uri, formUrlEncodedContent); // Get the HTTP response from the PHP file (this contains this encoded JSON array).
    //
    //      string responseContent = await response.Content.ReadAsStringAsync(); // Pull the encoded JSON array out of the HTTP response.
    //
    //      user = JsonConvert.DeserializeObject<User>(responseContent); // Deserialize (as in unencode) the JSON array and construct a User
    echo json_encode($user_data);
}
?>