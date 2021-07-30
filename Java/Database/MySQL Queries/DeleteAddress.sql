DELIMITER $$
CREATE TRIGGER DeleteAddress
AFTER UPDATE ON contacts
FOR EACH ROW
BEGIN
	IF(SELECT	COUNT(*)
	   FROM		contacts C
       WHERE	C.street = Old.street AND C.zipcode = Old.zipcode) = 0 THEN
	DELETE FROM addresses
    WHERE street = Old.street AND zipcode = Old.zipcode;
    END IF;
END $$