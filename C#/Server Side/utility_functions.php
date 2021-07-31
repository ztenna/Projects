<?php

// This function returns a "login response" array which is used in the login.php file.
// The first argument must be a boolean (otherwise an exception is thrown).
// The third argument is optional -- it is set to an empty string by default.
function createLoginResponse($status, $message, $email = "")
{
    if (!is_bool($status))
        throw new Exception("createLoginResponse(): error: the argument variable \$status was not a boolean.");
    
    else
    {
        return array(
            "Status" => $status,
            "Message" => $message,
            "Email" => $email);
    }
}

// This is a recursive function that is intended to echo the contents of a associative (as in key value pair) array where each value could be another array.
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

// This is a recursive function that is intended to echo the contents of normal array.
// If one of the array items is another array, the function will call itself on that array.
// This function is identical to the "echoContents" array except that it adds a new line to the end of the echoed data.
function echoContentsWithEOL($data)
{
    if (is_array($data))
    {
        foreach ($data as $subItem)
        {
            echoContents($subItem);
        }
    }
    
    else
    {      
        echo $data.PHP_EOL;
    }
}    

// This is a recursive function that is intended to echo the contents of normal array.
// If one of the array items is another array, the function will call itself on that array.
// This function is identical to the "echoContentsWithEOL" array except that it does not add a new line to the end of the echoed data.
function echoContents($data)
{
    if (is_array($data))
    {
        foreach ($data as $subItem)
        {
            echoContents($subItem);
        }
    }
    
    else
    {      
        echo $data;
    }
}

// This is a recursive function that is intended to be used to clean user input.
// If one of the array items is another array, the function will call itself on that array.
// The argument array is passed by reference and three functions are called called on the items in the array.
// See this documentation for what these function do:
// http://php.net/manual/en/function.trim.php
// http://php.net/manual/en/function.stripslashes.php
// http://php.net/manual/en/function.htmlspecialchars.php
function cleanseInput(&$data)
{
    if (is_array($data))
    {
        foreach ($data as &$subItem)
        {
            $subItem = cleanseInput($subItem);
        }
    }
    
    else
    {           
        $data = trim($data, "';#");
        $data = stripslashes($data);
        $data = htmlspecialchars($data);
    }
    
    return $data;
} 
?>