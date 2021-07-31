<?php

	$password = $email = "";

	if(isset($_POST["password"]) && isset($_POST["email"]))
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

		$password = $_POST["password"];
		$email = $_POST["email"];

		$preparedStatement = $dbConnection->prepare("UPDATE `spot`.`Users` SET `password` = ? WHERE `email` = ?;");

		$preparedStatement->bind_param("ss", $password, $email);

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