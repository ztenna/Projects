<?php
class Database
{
    // The following variables are the properties of the Database class.
    // $server_name, $database_name, $database_connection_username, and $database_connection_password are used to connect to a specific SQL database with a specific connection (in this case the connection is "test1").
    private $server_name = "";
    private $database_name = "";
    private $database_connection_username = "";
    private $database_connection_password = "";

    // This data member will contain the connection to the SQL database.
    public $connection;

    // A function that attempts to create a connection to the SQL database.
    // If this function is successful, it will return the connection.
    public function getConnection()
    {
        $server_name = $this->getValue("ServerName");
        $database_name = $this->getValue("DatabaseName");
        $database_connection_username = $this->getValue("Username");
		$database_connection_password = $this->getValue("Password");

        try
        {
            // Create the connection.
            $this->connection = new PDO("mysql:host=" . $this->server_name . ";dbname=" . $this->database_name, $database_connection_username, $database_connection_password);

            // Set ERRMODE attribute of the PDO such that, if something goes wrong, it will throw an Exception.
            $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

            // Echo a message if the connection was successfully created.
            // NOTE: if you are trying to parse a JSON object (on the application) or return some data in a specific format, you should leave this (and other success messages) commented out.
            // If you don't, you'll most likely encounter problems parsing the data that gets sent back to the application (you'll have both the success message(s) and the data).
            //echo "PHP web service: database.php: getConnection(): Connected successfully".PHP_EOL;
        }

        // Catch the Exception that is thrown if the connection creation was unsuccessful and echo an error message.
        // NOTE: the error message should be visible in the Debug console of Visual Studio.
        catch(PDOException $e)
        {
            echo "PHP web service: database.php: getConnection(): Connection failed: " . $e->getMessage().PHP_EOL;
        }

        return $this->connection;
    }

	function getValue($key)
	{
		$ini_array = parse_ini_file("../database.properties");
		$value =  $ini_array[$key];
		return $value;
	}

}
?>