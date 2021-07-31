<?php

	$presentationID = $teacherEmail = "";

	if(isset($_POST["presentationID"]) && isset($_POST["email"]))
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

		$presentationID = $_POST["presentationID"];
		$teacherEmail = $_POST["email"];

		$preparedStatement = $dbConnection->prepare("DELETE FROM `spot`.`Presentation_Requests` WHERE (`presentationID` = ?) and (`teacherEmail` = ?);");

		$preparedStatement->bind_param("is", $presentationID, $teacherEmail);

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