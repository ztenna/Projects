<?php
// This entire PHP file is just for testing. You can call this file if you want to see the contents of the POST request.
// First var_dump() is called on the POST -- this will display detailed information.
// Second a separating line is printed.
// Third the key/value pairs of the POST are echoed via a call to the echoKeyValuePair() function defined in this file.
// The third operation provides less detailed but easier to read information.

var_dump($_POST);

echo "------------------------------------------------------------------------------------------------".PHP_EOL;

foreach ($_POST as $key => $value)
{
    echokeyValuePair($key, $value);
}

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