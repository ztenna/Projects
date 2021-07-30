CREATE TABLE addresses (
    street			VARCHAR(37),
    city			VARCHAR(32) NOT NULL,
    state			CHAR(2) NOT NULL,
    zipcode			CHAR(5),
	PRIMARY KEY (street, zipcode)	);

CREATE TABLE contacts	(
	id				SMALLINT UNSIGNED PRIMARY KEY,
    name			VARCHAR(30) NOT NULL,
    street			VARCHAR(37) NOT NULL,
    zipcode			CHAR(5) NOT NULL,
    phone			CHAR(10),
    email			VARCHAR(52),
    FOREIGN KEY (street, zipcode) REFERENCES addresses (street, zipcode)	);   
    
CREATE TABLE locations	(
	lid				SMALLINT UNSIGNED PRIMARY KEY,
    street			VARCHAR(37) NOT NULL,
    zipcode			CHAR(5) NOT NULL,
    name			VARCHAR(30),
	FOREIGN KEY (street, zipcode) REFERENCES addresses (street, zipcode)	);
    
CREATE TABLE staff_works_in 	(
	sid				SMALLINT UNSIGNED PRIMARY KEY,
	since			DATE NOT NULL,
    lid				SMALLINT UNSIGNED,
    password		VARCHAR(11) NOT NULL,
    FOREIGN KEY (lid) REFERENCES locations (lid),
	FOREIGN KEY (sid) REFERENCES contacts (id)	);
       
CREATE TABLE floor_salespeople	(
	sid				SMALLINT UNSIGNED PRIMARY KEY,
    FOREIGN KEY (sid) REFERENCES staff_works_in (sid)		);
    
CREATE TABLE sales_manager	(
	sid				SMALLINT UNSIGNED PRIMARY KEY,
    FOREIGN KEY (sid) REFERENCES staff_works_in  (sid)		);
    
CREATE TABLE internet_salespeople	(
	sid				SMALLINT UNSIGNED PRIMARY KEY,
    FOREIGN KEY (sid) REFERENCES staff_works_in (sid)	);
    
CREATE TABLE customers_engaged_with	(
	cid				SMALLINT UNSIGNED PRIMARY KEY,
	sid				SMALLINT UNSIGNED,
	notes			VARCHAR(200),
    FOREIGN KEY (sid) REFERENCES floor_salespeople (sid),
	FOREIGN KEY (cid) REFERENCES contacts (id)	);
    
CREATE TABLE vehicles	(
	stock_num		MEDIUMINT UNSIGNED,
    lid				SMALLINT UNSIGNED,
    cid				SMALLINT UNSIGNED,
    sid				SMALLINT UNSIGNED,
    delivery_date	DATE,
    sale_date		DATE,
    commission		NUMERIC(7,2),
    make			VARCHAR(20) NOT NULL,
    model			VARCHAR(20) NOT NULL,
    year			YEAR(4) NOT NULL,
    price			NUMERIC(8,2) NOT NULL,
    status			ENUM('available', 'sold') NOT NULL,
    image			BLOB,
    hyperlink		VARCHAR(61) NOT NULL,
    PRIMARY KEY (stock_num, lid),
	FOREIGN KEY (lid) REFERENCES locations (lid) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (cid) REFERENCES customers_engaged_with (cid),
    FOREIGN KEY (sid) REFERENCES staff_works_in (sid)		);
 
CREATE TABLE timestamps		(
	time_date		DATETIME PRIMARY KEY	);
    
CREATE TABLE visit_info		(
	cid				SMALLINT UNSIGNED,
    stock_num		MEDIUMINT UNSIGNED,
    lid				SMALLINT UNSIGNED,
    time_date		DATETIME NOT NULL,
    PRIMARY KEY (cid, stock_num, lid),
    FOREIGN KEY (cid) REFERENCES customers_engaged_with (cid),
    FOREIGN KEY (time_date) REFERENCES timestamps (time_date),
    FOREIGN KEY (stock_num, lid) REFERENCES vehicles (stock_num, lid)	);