<?php
		
	$teacherEmail = $presentationID = "";
	
	if(isset($_POST["teacherEmail"]) && isset($_POST["presentationID"]))
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
		
		$teacherEmail = $_POST["teacherEmail"];
		$presentationID = $_POST["presentationID"];
		
		$preparedStatement = $dbConnection->prepare("UPDATE `spot`.`Presentations` SET `ambassador_email` = NULL WHERE (`presentationID` = ?) and (`teacher_email` = ?);");
				
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