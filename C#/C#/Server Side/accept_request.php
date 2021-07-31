<?php

	$ambassadorEmail = $teacherEmail = $presentationID = "";

	if(isset($_POST["ambassadorEmail"]) && isset($_POST["teacherEmail"]) && isset($_POST["presentationID"]))
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

		$ambassadorEmail = $_POST["ambassadorEmail"];
		$teacherEmail = $_POST["teacherEmail"];
		$presentationID = $_POST["presentationID"];

		$preparedStatement = $dbConnection->prepare("UPDATE `spot`.`Presentations` SET `ambassadorEmail` = ? WHERE (`presentationID` = ?) and (`teacherEmail` = ?);");

		$preparedStatement->bind_param("sis", $ambassadorEmail, $presentationID, $teacherEmail);

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