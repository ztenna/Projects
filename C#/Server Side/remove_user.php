<?php

	$email = "";

	if(isset($_POST["email"]))
	{
		$server = getValue("ServerName");
        $dbName = getValue("DatabaseName");
        $user = getValue("Username");
		$pass = getValue("Password");
        $dbConnection = new mysqli($server, $user, $pass, $dbName);

        // Check connection to database
        if ($dbConnection->connect_error) {
            die("Connection Failed: ".$dbConnection->connect_error);
            exit();
        }

		$email = $_POST["email"];

		$preparedStatement = $dbConnection->prepare("DELETE FROM `spot`.`Users` WHERE (`email` = ?);");

		$preparedStatement->bind_param("s", $email);

		$preparedStatement->execute();

		echo $dbConnection->error;

		$preparedStatement->close();
        mysqli_close($dbConnection);

	}

	function getValue($key)
	{
		$ini_array = parse_ini_file("../database.properties");
		$value =  $ini_array[$key];
		return $value;
	}

?>