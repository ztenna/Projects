CREATE DEFINER = CURRENT_USER TRIGGER `test`.`vehicles_AFTER_UPDATE` AFTER UPDATE ON `vehicles` FOR EACH ROW
BEGIN
		IF Old.stock_num = New.stock_num AND Old.lid = New.lid THEN
        UPDATE vehicles V
        SET V.status = 'sold' AND V.commission = V.price * 0.05
        WHERE New.stock_num = Old.stock_num AND New.lid = Old.lid;
        END IF;
END
