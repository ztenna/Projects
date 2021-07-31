<?php
// This test PHP file verifies that the chosen hashing algorithm from Xamarin provides the same hashed result as the PHP algorithm used in this file.
// This PHP file is currently called by the "testPasswordHashing()" function of the RestService class on the Xamarin application.

echo PHP_EOL;
echo "var_dump(\$_POST):".PHP_EOL;

var_dump($_POST);

echo PHP_EOL;
echo "------------------------------------------------------------------------------------------------".PHP_EOL;
echo PHP_EOL;

// Get the email/password combination out of the POST request and store them in variables.
$email = $_POST['email'];
$password = $_POST['password'];
$xamarin_app_hashed_password = $_POST['hashedPassword'];

echo "Unhashed email/password combination:".PHP_EOL;
echoKeyValuePair($email, $password);

echo PHP_EOL;

echo "Xamarin hashed email/password combination:".PHP_EOL;

// Echo the email and the Xamarin hashed password back to the application.
echoKeyValuePair($email, $xamarin_app_hashed_password);

echo PHP_EOL;

// Hash the password with the MD5 algorithm and, via the boolean "false", force the function to return the result as a lowercase hexadecimal digits..
// For information on the following hash() function, see: https://www.php.net/manual/en/function.hash.php
$php_hashed_password = hash("md5", $password, false);

echo "PHP hashed email/password combination:".PHP_EOL;

// Echo the email and the PHP hashed password back to the application.
echoKeyValuePair($email, $php_hashed_password);

echo PHP_EOL;

// If the two password hashes (one from the Xamarin app and the other from this PHP file) are identical, echo an appropriate message.
if ($php_hashed_password === $xamarin_app_hashed_password) // If you want to know what === is, see: https://www.php.net/manual/en/language.operators.comparison.php
{
    echo "\$php_hashed_password and \$xamarin_app_hashed_password are identical.".PHP_EOL;
}

// Otherwise, if they are not identical, echo another corresponding message.
else
{
    echo "\$php_hashed_password and \$xamarin_app_hashed_password are NOT identical.".PHP_EOL;
}

//----------------------------------------------------------------------------------------------//

// Utility function to quickly echo key/value pairs.
function echoKeyValuePair($key, $value)
{
    if (is_array($value))
    {
        foreach ($value as $subItem)
        {
            echoKeyValuePair($key, $subItem);
        }
    }
    
    else
    {      
        echo $key." => ".$value.PHP_EOL;
    }
} 
?>