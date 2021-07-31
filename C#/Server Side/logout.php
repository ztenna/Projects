<?php
// Include the PHP files that define objects or functions we need in this file.
include_once '../application_files/database.php';
include_once '../application_files/user.php';  
include_once '../application_files/utility_functions.php';

// In order to log the user out, we have to have access to the $_SESSION variable.
// We can get this by calling the following function.
session_start();

// This conditional echos one of two messages based on if the user was logged in or not when this PHP file was executed.
// This is for debugging.
if (!isset($_SESSION['logged_in_user_email']) || empty($_SESSION['logged_in_user_email']))
{
    echo("logout.php: error: User attempted to logout but wasn't logged in -- session destroyed.");
}

else
{
    echo("logout.php: User successfully logged out -- session destroyed.");
}

// Destroy the user's current session -- this "logs them out" by destroying the key "logged_in_user_email" and its associated value.
// See: http://php.net/manual/en/function.session-destroy.php
session_destroy();
?>