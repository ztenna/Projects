<?php
class User
{
    // The following variables are the properties of the User class.

    // This variable specifies a connection to an SQL database.
    // It is set when the User class is constructed (the constructor requires that a connection is passed to it).
    private $connection;

    // This variable stores the name of the SQL database table that holds user credentials.
    private $user_table_name = "Users";

    // This variable stores the name of the SQL database view that holds presentation information.
    private $presentations_view_name = "presentations";

    // The following two variables will contain the email/password combination of this user.
    // I suppose we will need to encrypt these somehow.
    public $email;
    private $password;

    // The constructor for this User class.
    public function __construct($databaseConnection)
    {
        $this->connection = $databaseConnection;
    }

    // This function gets data from a test table on an SQL database I set up.
    // This function returns this data.
    function test_getData()
    {
        // Define the query that should be executed on the database.
        // This query just gets all of the rows from my "test_user_table".
        $query = "SELECT * FROM spot.Users";

        try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
            $statement = $this->connection->prepare($query);

            // Execute the statement -- the results/database rows are stored in the same "$statement" variable.
            // This is what actually causes the query to be run on the database.
            $statement->execute();

            // Set the "fetch mode" so that, when the fetch() function is called, it will return an associative array.
            // What this means is that, if we call fetchAll(), it will return an array of key value pairs where the key will match the column name in the database.
            // The value(s) of a corresponding key are then defined by the values in the row cells corresponding to that column.
            $statement->setFetchMode(PDO::FETCH_ASSOC);

            //echo "PHP web service: user.php: test_getData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: test_getData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        // Return the results of the query to the file that called this function.
        return $statement;
    }

    // This function will insert data from the POST associative array superglobal (superglobal means that you can access its contents from any scope).
    function test_setData()
    {
        // Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
        $query = "INSERT INTO spot.Users (username, password, extra_info) VALUES ('".$_POST["Name"]."', '".$_POST["OrganizationName"]."', '".$_POST["Email"]."')";

        try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
            $statement = $this->connection->prepare($query);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $statement->execute();

            //echo "PHP web service: user.php: test_setData(): Successfully set data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: test_setData(): Failed to set data" . $e->getMessage().PHP_EOL;
        }
    }

    function getRequestData($maxNumRowsToGet, $startRowOffset)
    {
        // Get data from the starting row (where starting row = row 0 + the value stored in $startRowOffset) and limit the number of rows returned by the value in $maxNumRowsToGet.
        $query = "SELECT * FROM spot.Presentations LIMIT ".$maxNumRowsToGet." OFFSET ".$startRowOffset;

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getRequestData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getRequestData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    function getSignupRequestData($maxNumRowsToGet, $startRowOffset)
    {
        // Get data from the starting row (where starting row = row 0 + the value stored in $startRowOffset) and limit the number of rows returned by the value in $maxNumRowsToGet.
        $query = "SELECT * FROM spot.Potential_Users LIMIT ".$maxNumRowsToGet." OFFSET ".$startRowOffset;

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getSignupRequestData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getSignupRequestData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    function getPresentationRequestData($maxNumRowsToGet, $startRowOffset)
    {
        // Get data from the starting row (where starting row = row 0 + the value stored in $startRowOffset) and limit the number of rows returned by the value in $maxNumRowsToGet.
        $query = "SELECT * FROM spot.Presentation_Requests LIMIT ".$maxNumRowsToGet." OFFSET ".$startRowOffset;

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getPresentationRequestData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getPresentationRequestData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    function getPresentationReportData($maxNumRowsToGet, $startRowOffset)
    {
        // Get data from the starting row (where starting row = row 0 + the value stored in $startRowOffset) and limit the number of rows returned by the value in $maxNumRowsToGet.
        $query = "SELECT * FROM spot.Presentation_Reports LIMIT ".$maxNumRowsToGet." OFFSET ".$startRowOffset;

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getPresentationReportData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getPresentationReportData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    function getCurrentUserData($maxNumRowsToGet, $startRowOffset)
    {
        // Get data from the starting row (where starting row = row 0 + the value stored in $startRowOffset) and limit the number of rows returned by the value in $maxNumRowsToGet.
        $query = "SELECT * FROM spot.Users LIMIT ".$maxNumRowsToGet." OFFSET ".$startRowOffset;

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getCurrentUserData(): Successfully got data".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getCurrentUserData(): Failed to get data" . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    // This function checks (via a query to the SQL database) whether the email/password combination contained in this User instance exists in the SQL spot.users table.
    // It will return the query results to the PHP file that called this function.
    function login()
    {
        $query = "SELECT email, password, isAdministrator FROM spot.".$this->user_table_name." WHERE email='".$this->email."' AND password='".$this->password."'";
        // The above query is equivalent to this following example query assuming email == "testuseremail1@test.com" and password == "password1":
        // SELECT email, password FROM spot.users WHERE email='testuseremail1@test.com' AND password='password1'

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: login(): Successfully logged in".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: login(): PDOException: " . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }

    function setPassword($password)
    {
        $this->password = $password;
    }

	//This function will query the database for the remaining user information, and return the query results

	function getUserData()
    {
        $query = "SELECT email,first_name,last_name,phone_number,IsAmbassador,isAdministrator FROM spot.Users WHERE email='".$this->email."'";
        // The above query is equivalent to this following example query assuming email == "testuseremail1@test.com" and password == "password1":
        // SELECT email,first_name,last_name,phone_number,IsAmbassador,isAdministrator WHERE email=$_SESSION['logged_in_user_email']

        try
        {
            // Prepare and execute the query.
            // The query results are stored in the $statement variable.
            $statement = $this->connection->prepare($query);

            $statement->execute();

            //echo "PHP web service: user.php: getUserData(): Successfully gotUserData".PHP_EOL;
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: getUserData(): PDOException: " . $e->getMessage().PHP_EOL;
        }

        return $statement;
    }
    function addPotentialUser($name, $email, $phoneNumber, $address, $school, $major, $hearAboutSpot, $whySpotPresenter, $expWithStudents, $notAsPlanned, 
        $recommendationLetterName, $recommendationLetterEmail, $dietaryRestrictions,$isNewApplicant, $isCurrentAmbassador, $monday, $tuesday, $wednesday, 
        $thursday, $friday, $fallBreak, $winterBreak, $springBreak, $availableVisitOther, $haveLicense, $haveCar, $agreeToStatement, $agreeToDoComponents, 
        $canAttendTraining, $male, $female, $genderOther, $firstToCollege, $ofOrigin, $africanAmerican, $americanIndianOrAlaskanNative, $pacificIslander, 
        $asian, $caucasian, $raceOther, $ruralCommunity, $suburbanCommunity, $urbanCommunity, $privacyPolicy)
	{
		// Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
		try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
			$preparedStatement = $this->connection->prepare("INSERT INTO spot.Potential_Users (`name`, `email`, `phoneNumber`, `address`, `school`, `major`, `hearAboutSpot`, `whySpotPresenter`, `expWithStudents`, `notAsPlanned`, `recommendationLetterName`, `recommendationLetterEmail`, `dietaryRestrictions`, `isNewApplicant`, `isCurrentAmbassador`, `monday`, `tuesday`, `wednesday`, `thursday`, `friday`, `fallBreak`, `winterBreak`, `springBreak`, `availableVisitOther`, `haveLicense`, `haveCar`, `agreeToStatement`, `agreeToDoComponents`, `canAttendTraining`, `male`, `female`, `genderOther`, `firstToCollege`, `ofOrigin`, `africanAmerican`, `americanIndianOrAlaskanNative`, `pacificIslander`, `asian`, `caucasian`, `raceOther`, `ruralCommunity`, `suburbanCommunity`, `urbanCommunity`, `privacyPolicy`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");
			$preparedStatement->bindParam(1, $name, PDO::PARAM_STR);
			$preparedStatement->bindParam(2, $email, PDO::PARAM_STR);
			$preparedStatement->bindParam(3, $phoneNumber, PDO::PARAM_INT);
			$preparedStatement->bindParam(4, $address, PDO::PARAM_STR);
			$preparedStatement->bindParam(5, $school, PDO::PARAM_STR);
			$preparedStatement->bindParam(6, $major, PDO::PARAM_STR);
            $preparedStatement->bindParam(7, $hearAboutSpot, PDO::PARAM_STR);
            $preparedStatement->bindParam(8, $whySpotPresenter, PDO::PARAM_STR);
            $preparedStatement->bindParam(9, $expWithStudents, PDO::PARAM_STR);
            $preparedStatement->bindParam(10, $notAsPlanned, PDO::PARAM_STR);
            $preparedStatement->bindParam(11, $recommendationLetterName, PDO::PARAM_STR);
            $preparedStatement->bindParam(12, $recommendationLetterEmail, PDO::PARAM_STR);
            $preparedStatement->bindParam(13, $dietaryRestrictions, PDO::PARAM_STR);
            $preparedStatement->bindParam(14, $isNewApplicant, PDO::PARAM_INT);
            $preparedStatement->bindParam(15, $isCurrentAmbassador, PDO::PARAM_INT);
            $preparedStatement->bindParam(16, $monday, PDO::PARAM_INT);
            $preparedStatement->bindParam(17, $tuesday, PDO::PARAM_INT);
            $preparedStatement->bindParam(18, $wednesday, PDO::PARAM_INT);
            $preparedStatement->bindParam(19, $thursday, PDO::PARAM_INT);
            $preparedStatement->bindParam(20, $friday, PDO::PARAM_INT);
            $preparedStatement->bindParam(21, $fallBreak, PDO::PARAM_INT);
            $preparedStatement->bindParam(22, $winterBreak, PDO::PARAM_INT);
            $preparedStatement->bindParam(23, $springBreak, PDO::PARAM_INT);
            $preparedStatement->bindParam(24, $availableVisitOther, PDO::PARAM_STR);
            $preparedStatement->bindParam(25, $haveLicense, PDO::PARAM_INT);
            $preparedStatement->bindParam(26, $haveCar, PDO::PARAM_INT);
            $preparedStatement->bindParam(27, $agreeToStatement, PDO::PARAM_INT);
            $preparedStatement->bindParam(28, $agreeToDoComponents, PDO::PARAM_INT);
            $preparedStatement->bindParam(29, $canAttendTraining, PDO::PARAM_INT);
            $preparedStatement->bindParam(30, $male, PDO::PARAM_INT);
            $preparedStatement->bindParam(31, $female, PDO::PARAM_INT);
            $preparedStatement->bindParam(32, $genderOther, PDO::PARAM_STR);
            $preparedStatement->bindParam(33, $firstToCollege, PDO::PARAM_INT);
            $preparedStatement->bindParam(34, $ofOrigin, PDO::PARAM_INT);
            $preparedStatement->bindParam(35, $africanAmerican, PDO::PARAM_INT);
            $preparedStatement->bindParam(36, $americanIndianOrAlaskanNative, PDO::PARAM_INT);
            $preparedStatement->bindParam(37, $pacificIslander, PDO::PARAM_INT);
            $preparedStatement->bindParam(38, $asian, PDO::PARAM_INT);
            $preparedStatement->bindParam(39, $caucasian, PDO::PARAM_INT);
            $preparedStatement->bindParam(40, $raceOther, PDO::PARAM_STR);
            $preparedStatement->bindParam(41, $ruralCommunity, PDO::PARAM_INT);
            $preparedStatement->bindParam(42, $suburbanCommunity, PDO::PARAM_INT);
            $preparedStatement->bindParam(43, $urbanCommunity, PDO::PARAM_INT);
            $preparedStatement->bindParam(44, $privacyPolicy, PDO::PARAM_STR);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $preparedStatement->execute();
			$preparedStatement->closeCursor();
			echo "User has been created";
           return "Success";
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: addPotentialUser(): Failed to add potential user" . $e->getMessage().PHP_EOL;
			return "Failed";
        }
	}
	function addUser($email, $password, $firstName, $lastName, $phoneNumber, $address, $school, $major, $hearAboutSpot,
        $whySpotPresenter, $expWithStudents, $notAsPlanned, $recommendationLetterName, $recommendationLetterEmail, $dietaryRestrictions, 
        $isAmbassador, $isAdmin, $availableToVisit, $haveLicense, $haveCar, $canAttendTraining, $gender, $firstToCollege, 
        $ofOrigin, $race, $community, $privacyPolicy)
	{
		// Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
		try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
			$preparedStatement = $this->connection->prepare("INSERT INTO spot.Users (`email`, `password`, `first_name`, `last_name`, `phone_number`, `address`, `school`, `major`, `hearAboutSpot`, `whySpotPresenter`, `expWithStudents`, `notAsPlanned`, `recommendationLetterName`, `recommendationLetterEmail`, `dietaryRestrictions`, `isAmbassador`, `isAdministrator`, `availableToVisit`, `haveLicense`, `haveCar`, `canAttendTraining`, `gender`, `firstToCollege`, `ofOrigin`, `race`, `community`, `privacyPolicy`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

			$preparedStatement->bindParam(1, $email, PDO::PARAM_STR);
			$preparedStatement->bindParam(2, $password, PDO::PARAM_STR);
			$preparedStatement->bindParam(3, $firstName, PDO::PARAM_STR);
			$preparedStatement->bindParam(4, $lastName, PDO::PARAM_STR);
            $preparedStatement->bindParam(5, $phoneNumber, PDO::PARAM_INT);
            $preparedStatement->bindParam(6, $address, PDO::PARAM_STR);
            $preparedStatement->bindParam(7, $school, PDO::PARAM_STR);
            $preparedStatement->bindParam(8, $major, PDO::PARAM_STR);
            $preparedStatement->bindParam(9, $hearAboutSpot, PDO::PARAM_STR);
            $preparedStatement->bindParam(10, $whySpotPresenter, PDO::PARAM_STR);
            $preparedStatement->bindParam(11, $expWithStudents, PDO::PARAM_STR);
            $preparedStatement->bindParam(12, $notAsPlanned, PDO::PARAM_STR);
            $preparedStatement->bindParam(13, $recommendationLetterName, PDO::PARAM_STR);
            $preparedStatement->bindParam(14, $recommendationLetterEmail, PDO::PARAM_STR);
            $preparedStatement->bindParam(15, $dietaryRestrictions, PDO::PARAM_STR);
			$preparedStatement->bindParam(16, $isAmbassador, PDO::PARAM_INT);
            $preparedStatement->bindParam(17, $isAdmin, PDO::PARAM_INT);
            $preparedStatement->bindParam(18, $availableToVisit, PDO::PARAM_STR);
            $preparedStatement->bindParam(19, $haveLicense, PDO::PARAM_STR);
            $preparedStatement->bindParam(20, $haveCar, PDO::PARAM_STR);
            $preparedStatement->bindParam(21, $canAttendTraining, PDO::PARAM_STR);
            $preparedStatement->bindParam(22, $gender, PDO::PARAM_STR);
            $preparedStatement->bindParam(23, $firstToCollege, PDO::PARAM_STR);
            $preparedStatement->bindParam(24, $ofOrigin, PDO::PARAM_STR);
            $preparedStatement->bindParam(25, $race, PDO::PARAM_STR);
            $preparedStatement->bindParam(26, $community, PDO::PARAM_STR);
            $preparedStatement->bindParam(27, $privacyPolicy, PDO::PARAM_STR);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $preparedStatement->execute();
			$preparedStatement->closeCursor();
			echo "User has been created";
           return "Success";
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: addUser(): Failed to add user" . $e->getMessage().PHP_EOL;
			return "Failed";
        }
    }
    function submitPresRequest($name, $orgName, $email, $phoneNumber, $altPhoneNumber, $contactDayTime, $orgStreet, $orgCity, $orgState, $orgZip, $anyPres, 
    $invisiblePres, $mentorPres, $starPres, $waterPres, $planetPres, $marsPres, $stationPres, $telescopePres, $NANOGravPres, $presRotations, $noHandsOn, $buildMolecule,
    $designAlien, $pocketSolSys, $sizeUpMoon, $telescopeDesign, $roboticArm, $electroWar, $buildWatershed, $dropInBucket, $gradeLevels, $numberStudents, $proposedDateTime,
    $projectSupplies, $speakerSupplies, $compSupplies, $cordSupplies, $micSupplies, $otherSupplies, $travelFee, $agreeStatement, $concerns, $collegePres, $skypePres, $altOtherPres)
	{

        date_default_timezone_set("America/New_York");
        $timeDateCreated = date("Y-m-d H:i:s");

        $d = strtotime($proposedDateTime);
        //echo $d;
        $proposedDateTime = date("Y-m-d H:i:s", $d);
        //echo $proposedDateTime;
        
		// Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
		try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
			$preparedStatement = $this->connection->prepare("INSERT INTO spot.Presentation_Requests (`timeDateCreated`, `name`, `orgName`, `teacherEmail`, `phoneNumber`, `altPhoneNumber`, `contactDayTime`, `orgStreet`, `orgCity`, `orgState`, `orgZip`, `anyPres`, `invisiblePres`, `mentorPres`, `starPres`, `waterPres`, `planetPres`, `marsPres`, `stationPres`, `telescopePres`, `NANOGravPres`, `presRotations`, `noHandsOn`, `buildMolecule`, `designAlien`, `pocketSolSys`, `sizeUpMoon`, `telescopeDesign`, `roboticArm`, `electroWar`, `buildWatershed`, `dropInBucket`, `gradeLevels`, `numberStudents`, `proposedDateTime`, `projectSupplies`, `speakerSupplies`, `compSupplies`, `cordSupplies`, `micSupplies`, `otherSupplies`, `travelFee`, `agreeStatement`, `concerns`, `collegePres`, `skypePres`, `altOtherPres`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");
            $preparedStatement->bindParam(1, $timeDateCreated, PDO::PARAM_STR);
            $preparedStatement->bindParam(2, $name, PDO::PARAM_STR);
            $preparedStatement->bindParam(3, $orgName, PDO::PARAM_STR);
            $preparedStatement->bindParam(4, $email, PDO::PARAM_STR);
            $preparedStatement->bindParam(5, $phoneNumber, PDO::PARAM_INT);
            $preparedStatement->bindParam(6, $altPhoneNumber, PDO::PARAM_INT);
			$preparedStatement->bindParam(7, $contactDayTime, PDO::PARAM_STR);
			$preparedStatement->bindParam(8, $orgStreet, PDO::PARAM_STR);
			$preparedStatement->bindParam(9, $orgCity, PDO::PARAM_STR);
            $preparedStatement->bindParam(10, $orgState, PDO::PARAM_STR);
            $preparedStatement->bindParam(11, $orgZip, PDO::PARAM_INT);
            $preparedStatement->bindParam(12, $anyPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(13, $invisiblePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(14, $mentorPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(15, $starPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(16, $waterPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(17, $planetPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(18, $marsPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(19, $stationPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(20, $telescopePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(21, $NANOGravPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(22, $presRotations, PDO::PARAM_INT);
            $preparedStatement->bindParam(23, $noHandsOn, PDO::PARAM_INT);
            $preparedStatement->bindParam(24, $buildMolecule, PDO::PARAM_INT);
            $preparedStatement->bindParam(25, $designAlien, PDO::PARAM_INT);
            $preparedStatement->bindParam(26, $pocketSolSys, PDO::PARAM_INT);
            $preparedStatement->bindParam(27, $sizeUpMoon, PDO::PARAM_INT);
            $preparedStatement->bindParam(28, $telescopeDesign, PDO::PARAM_INT);
            $preparedStatement->bindParam(29, $roboticArm, PDO::PARAM_INT);
            $preparedStatement->bindParam(30, $electroWar, PDO::PARAM_INT);
            $preparedStatement->bindParam(31, $buildWatershed, PDO::PARAM_INT);
            $preparedStatement->bindParam(32, $dropInBucket, PDO::PARAM_INT);
            $preparedStatement->bindParam(33, $gradeLevels, PDO::PARAM_STR);
            $preparedStatement->bindParam(34, $numberStudents, PDO::PARAM_INT);
            $preparedStatement->bindParam(35, $proposedDateTime, PDO::PARAM_STR);
            $preparedStatement->bindParam(36, $projectSupplies, PDO::PARAM_INT);
            $preparedStatement->bindParam(37, $speakerSupplies, PDO::PARAM_INT);
            $preparedStatement->bindParam(38, $compSupplies, PDO::PARAM_INT);
            $preparedStatement->bindParam(39, $cordSupplies, PDO::PARAM_INT);
            $preparedStatement->bindParam(40, $micSupplies, PDO::PARAM_INT);
            $preparedStatement->bindParam(41, $otherSupplies, PDO::PARAM_STR);
            $preparedStatement->bindParam(42, $travelFee, PDO::PARAM_INT);
            $preparedStatement->bindParam(43, $agreeStatement, PDO::PARAM_INT);
            $preparedStatement->bindParam(44, $concerns, PDO::PARAM_STR);
            $preparedStatement->bindParam(45, $collegePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(46, $skypePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(47, $altOtherPres, PDO::PARAM_STR);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $preparedStatement->execute();
			$preparedStatement->closeCursor();
			echo "Presentation has been created";
           return "Success";
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: submitPresRequest(): Failed to add presentation" . $e->getMessage().PHP_EOL;
			return "Failed";
        }
    }
    function acceptPresentationRequest($timeDateCreated, $name, $orgName, $teacherEmail, $ambassadorEmail, $otherAmbassadorEmails,
        $phoneNumber, $altPhoneNumber, $contactDayTime, $presLocation, $requestedPres, $presRotations, $handsOnActivities, $gradeLevels,
        $numberStudents, $proposedDateTime, $supplies, $travelFee, $agreeStatement, $concerns, $altPres)
	{   
        // Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
        try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
			$preparedStatement = $this->connection->prepare("INSERT INTO spot.Presentations (`timeDateCreated`, `name`, `orgName`, `teacherEmail`, `ambassadorEmail`, `otherAmbassadorEmails`, `phoneNumber`, `altPhoneNumber`, `contactDayTime`, `presLocation`, `requestedPres`, `presRotations`, `handsOnActivities`, `gradeLevels`, `numberStudents`, `proposedDateTime`, `supplies`, `travelFee`, `agreeStatement`, `concerns`, `altPres`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

			$preparedStatement->bindParam(1, $timeDateCreated, PDO::PARAM_STR);
			$preparedStatement->bindParam(2, $name, PDO::PARAM_STR);
			$preparedStatement->bindParam(3, $orgName, PDO::PARAM_STR);
            $preparedStatement->bindParam(4, $teacherEmail, PDO::PARAM_STR);
            $preparedStatement->bindParam(5, $ambassadorEmail, PDO::PARAM_STR);
            $preparedStatement->bindParam(6, $otherAmbassadorEmails, PDO::PARAM_STR);
            $preparedStatement->bindParam(7, $phoneNumber, PDO::PARAM_INT);
            $preparedStatement->bindParam(8, $altPhoneNumber, PDO::PARAM_INT);
            $preparedStatement->bindParam(9, $contactDayTime, PDO::PARAM_STR);
            $preparedStatement->bindParam(10, $presLocation, PDO::PARAM_STR);
            $preparedStatement->bindParam(11, $requestedPres, PDO::PARAM_STR);
            $preparedStatement->bindParam(12, $presRotations, PDO::PARAM_STR);
            $preparedStatement->bindParam(13, $handsOnActivities, PDO::PARAM_STR);
            $preparedStatement->bindParam(14, $gradeLevels, PDO::PARAM_STR);
            $preparedStatement->bindParam(15, $numberStudents, PDO::PARAM_INT);
            $preparedStatement->bindParam(16, $proposedDateTime, PDO::PARAM_STR);
			$preparedStatement->bindParam(17, $supplies, PDO::PARAM_STR);
            $preparedStatement->bindParam(18, $travelFee, PDO::PARAM_STR);
            $preparedStatement->bindParam(19, $agreeStatement, PDO::PARAM_STR);
            $preparedStatement->bindParam(20, $concerns, PDO::PARAM_STR);
            $preparedStatement->bindParam(21, $altPres, PDO::PARAM_STR);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $preparedStatement->execute();
			$preparedStatement->closeCursor();
			echo "Presentation request has been accepted";
           return "Success";
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: acceptPresentationRequest(): Failed to accept presentation request" . $e->getMessage().PHP_EOL;
			return "Failed";
        }
    }
	function checkAcceptance($presentationID)
	{
		$query = "SELECT ambassadorEmail FROM spot.Presentations WHERE presentationID = '". $presentationID."'";

		try
		{

			$statement = $this->connection->prepare($query);

            $statement->execute();

			//echo "PHP web service: user.php: checkAcceptance(): checked acceptance".PHP_EOL;
		}
		catch(PDOException $e)
		{
			echo "PHP web service: user.php: checkAcceptance(): PDOException: " . $e->getMessage().PHP_EOL;
		}
		return $statement;
	}

	function accept_request($teacher_email, $presentationID, $ambassador_email)
	{
		try
		{
			$query = "UPDATE `spot`.`Presentations` SET `ambassadorEmail` = '".$ambassadorEmail."' WHERE (`presentationID` = "."'".$presentationID."'".") AND (`teacherEmail` = '".$teacher_email."');";
			echo "$query =".$query;
			#UPDATE `spot`.`presentations` SET `ambassador_email` = "aa" WHERE (`presentationID` = '1') AND (`teacher_email` = "testuseremail1@test.com");
			$preparedStatement = $this->connection->prepare($query);

			#$preparedStatement->bind_param("sss", $ambassador_email, $presentationID, $teacher_email);

			$preparedStatement->execute();
		#	$preparedStatement->close();

			//echo "PHP web service: user.php: accept_request(): accepted request".PHP_EOL;

			return "Successfully updated ambassador_email";
		}
		catch(PDOException $e)
		{
			echo "PHP web servie: user.php: accept_request(): ". $e->getMessage().PHP_EOL;
			return "Failed to update ambassador_email";
		}
    }
    function submitPresReport($email, $name, $driver, $orgName, $contactPerson, $travelFeeRecDir, $travelFeeNoNeed, $travelFeeFollowUp, $presDate, $numUniversePres, 
    $numWaterPres, $numStarPres, $numPlanetPres, $numMarsPres, $numStationPres, $numTelescopePres, $numNANOGravPres, $numMentorPres, $numSolarActs, $numMoonActs, 
    $numEngDesignActs, $numRobArmActs, $numElectroWarActs, $numAlienActs, $numMoleculeActs, $numBucketActs, $numWatershedActs, $sameEverything, $anythingDiff, $firstTime, 
    $num_KTo3_Stu, $num_4To5_Stu, $num_6To8_Stu, $num_9To12_Stu, $numCollegeStu, $numTeachers, $otherAudience, $engagedRating, $appropriateRating, $askedQuestions, 
    $answerQuestions, $aspectsWorkBest, $presSuggestions)
	{
        $d = strtotime($presDate);
        //echo "d: " . $d;
        $presDate = date("Y-m-d", $d);
        //echo " presDate: " . $presDate;
        
		// Define the query that should be executed on the database.
        // NOTE: the "." indicates a concatenation -- I am concatenating values from the POST superglobal into the query string.
		try
        {
            // Prepare the query by calling the "prepare()" function of the PDO object "connection".
            // This prepare() function returns an executable "statement" object.
			$preparedStatement = $this->connection->prepare("INSERT INTO spot.Presentation_Reports (`email`, `name`, `driver`, `orgName`, `contactPerson`, `travelFeeRecDir`, `travelFeeNoNeed`, `travelFeeFollowUp`, `presDate`, `numUniversePres`, `numWaterPres`, `numStarPres`, `numPlanetPres`, `numMarsPres`, `numStationPres`, `numTelescopePres`, `numNANOGravPres`, `numMentorPres`, `numSolarActs`, `numMoonActs`, `numEngDesignActs`, `numRobArmActs`, `numElectroWarActs`, `numAlienActs`, `numMoleculeActs`, `numBucketActs`, `numWatershedActs`, `sameEverything`, `anythingDiff`, `firstTime`, `num_KTo3_Stu`, `num_4To5_Stu`, `num_6To8_Stu`, `num_9To12_Stu`, `numCollegeStu`, `numTeachers`, `otherAudience`, `engagedRating`, `appropriateRating`, `askedQuestions`, `answerQuestions`, `aspectsWorkBest`, `presSuggestions`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");
            $preparedStatement->bindParam(1, $email, PDO::PARAM_STR);
            $preparedStatement->bindParam(2, $name, PDO::PARAM_STR);
            $preparedStatement->bindParam(3, $driver, PDO::PARAM_STR);
            $preparedStatement->bindParam(4, $orgName, PDO::PARAM_STR);
            $preparedStatement->bindParam(5, $contactPerson, PDO::PARAM_STR);
            $preparedStatement->bindParam(6, $travelFeeRecDir, PDO::PARAM_INT);
            $preparedStatement->bindParam(7, $travelFeeNoNeed, PDO::PARAM_INT);
			$preparedStatement->bindParam(8, $travelFeeFollowUp, PDO::PARAM_INT);
			$preparedStatement->bindParam(9, $presDate, PDO::PARAM_STR);
			$preparedStatement->bindParam(10, $numUniversePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(11, $numWaterPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(12, $numStarPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(13, $numPlanetPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(14, $numMarsPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(15, $numStationPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(16, $numTelescopePres, PDO::PARAM_INT);
            $preparedStatement->bindParam(17, $numNANOGravPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(18, $numMentorPres, PDO::PARAM_INT);
            $preparedStatement->bindParam(19, $numSolarActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(20, $numMoonActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(21, $numEngDesignActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(22, $numRobArmActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(23, $numElectroWarActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(24, $numAlienActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(25, $numMoleculeActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(26, $numBucketActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(27, $numWatershedActs, PDO::PARAM_INT);
            $preparedStatement->bindParam(28, $sameEverything, PDO::PARAM_INT);
            $preparedStatement->bindParam(29, $anythingDiff, PDO::PARAM_STR);
            $preparedStatement->bindParam(30, $firstTime, PDO::PARAM_STR);
            $preparedStatement->bindParam(31, $num_KTo3_Stu, PDO::PARAM_INT);
            $preparedStatement->bindParam(32, $num_4To5_Stu, PDO::PARAM_INT);
            $preparedStatement->bindParam(33, $num_6To8_Stu, PDO::PARAM_INT);
            $preparedStatement->bindParam(34, $num_9To12_Stu, PDO::PARAM_INT);
            $preparedStatement->bindParam(35, $numCollegeStu, PDO::PARAM_INT);
            $preparedStatement->bindParam(36, $numTeachers, PDO::PARAM_INT);
            $preparedStatement->bindParam(37, $otherAudience, PDO::PARAM_STR);
            $preparedStatement->bindParam(38, $engagedRating, PDO::PARAM_INT);
            $preparedStatement->bindParam(39, $appropriateRating, PDO::PARAM_INT);
            $preparedStatement->bindParam(40, $askedQuestions, PDO::PARAM_STR);
            $preparedStatement->bindParam(41, $answerQuestions, PDO::PARAM_STR);
            $preparedStatement->bindParam(42, $aspectsWorkBest, PDO::PARAM_STR);
            $preparedStatement->bindParam(43, $presSuggestions, PDO::PARAM_STR);

            // Execute the statement -- this causes the values from the POST to be inserted into the SQL database.
            // To be sure that this statement actually did something, you should look at the database table you attempted to insert data into.
            $preparedStatement->execute();
			$preparedStatement->closeCursor();
			//echo " Presentation Report has been submitted";
           return "Success";
        }

        catch(PDOException $e)
        {
            echo "PHP web service: user.php: submitPresReport(): Failed to add presentation report" . $e->getMessage().PHP_EOL;
			return "Failed";
        }
    }

	function search($stringToSearchFor)
	{
		try
		{
			$preparedStatement = "SELECT * FROM `spot`.`Presentations` WHERE (`teach_email` LIKE '%?%') OR (`amabassador_email` LIKE '%?%') OR (`contactEmail` LIKE '%?%') OR (`contact_email` LIKE '%?%') OR (`notes` LIKE '%?%') OR (`time_date_created` LIKE '%?%') OR (`organization_name` LIKE '%?%') OR (`grade_level` LIKE '%?%') OR (`number_of_presentations` LIKE '%?%')
OR (`number_of_students_persentation` LIKE '%?%') OR (`subject_requested` LIKE '%?%') OR (`concerns` LIKE '%?%') OR (`preferred_days` LIKE '%?%') OR (`contact_times` LIKE '%?%')  OR (`hands_on_activities` LIKE '%?%')  OR (`can_pay_fee` LIKE '%?%') OR (`legal_agreement` LIKE '%?%')  OR (`supplies` LIKE '%?%') OR (`propose_time_date` LIKE '%?%')
 OR (`organization_street_address` LIKE '%?%')  OR (`organization_zip` LIKE '%?%')  OR (`organization_city` LIKE '%?%')  OR (`organization_state` LIKE '%?%')  OR (`contactTimes` LIKE '%?%')  OR (`handsOnActivites` LIKE '%?%')  OR (`canPayFee` LIKE '%?%') OR (`legalAgreement` LIKE '%?%')";

 			//echo "PHP web service: user.php: search(): Successfully searched".PHP_EOL;
		}
		catch(PDOException $e)
		{
			echo "PHP web servie: user.php: search(): ". $e->getMessage().PHP_EOL;
		}
	}
}
?>