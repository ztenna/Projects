DELIMITER $$
CREATE TRIGGER VehicleSoldTrigger
BEFORE UPDATE ON vehicles
FOR EACH ROW
BEGIN
      IF New.sale_date IS NOT NULL AND New.cid IS NOT NULL AND New.sid IS NOT NULL THEN
        SET New.commission = New.price * 0.05, New.status = 'sold';
        END IF;
END $$

UPDATE vehicles 
SET cid = 217, sid = 144, sale_date = '2019-11-21' 
WHERE stock_num = 100002 AND lid = 4;

UPDATE vehicles 
SET sale_date = NULL, cid = NULL, sid = NULL, commission = NULL, status = 'available' 
WHERE stock_num = 100002;

DROP TRIGGER test.VehicleSoldTrigger;