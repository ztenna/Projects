# To do the following updates, i.e. before running them, 
# make sure you go to the edit tab and click on preferences in workbench,
# click sql editor and uncheck the safe update checkbox,
# and reconnect to server

UPDATE	vehicles V
SET		V.cid = null
WHERE	V.cid = 1;

UPDATE	vehicles V
SET		V.sid = null
WHERE	V.sid = 0;

UPDATE	vehicles V
SET		V.sale_date = null
WHERE	V.sale_date = 0000-00-00;

UPDATE	vehicles V
SET		V.commission = null
WHERE	V.commission = 0.00;