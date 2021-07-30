# Make sure you do the UpdateVehiclesTable before doing this

DELETE FROM	customers_engaged_with
WHERE		cid = 1;

DELETE FROM floor_salespeople
WHERE		sid = 0;

DELETE FROM	staff_works_in
WHERE		sid = 0;

DELETE FROM contacts
WHERE		id = 0;

DELETE FROM	contacts
WHERE		id = 1;

DELETE FROM addresses
WHERE		street = 'a';

DELETE FROM addresses
WHERE		street = 'b';