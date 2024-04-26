CREATE TABLE type_WorkStation (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description VARCHAR(45) UNIQUE NOT NULL
); 

INSERT INTO type_WorkStation (description) VALUES ('WS Manual Tipo 1');
INSERT INTO type_WorkStation (description) VALUES ('WS Manual Tipo 2');
INSERT INTO type_WorkStation (description) VALUES ('WS Manual Tipo 17A');
INSERT INTO type_WorkStation (description) VALUES ('WS Manual Tipo 17B');
INSERT INTO type_WorkStation (description) VALUES ('WS Manual Tipo 17C');
INSERT INTO type_WorkStation (description) VALUES ('WS Automatica Tipo 1');
INSERT INTO type_WorkStation (description) VALUES ('WS Automatica Tipo 2');
INSERT INTO type_WorkStation (description) VALUES ('WS Automatica Tipo 3');
INSERT INTO type_WorkStation (description) VALUES ('WS Automatica Tipo 4');


DELIMITER ||
CREATE PROCEDURE SP_GET_TYPE_WORKSTATION()
BEGIN

SELECT 	id,
		description
FROM type_workstation;
 
 END
 || 
 DELIMITER ;
 
 CALL SP_GET_TYPE_WORKSTATION();



  DELIMITER ||
CREATE PROCEDURE SP_GET_TYPE_WORKSTATION_BY_ID(IN p_id INT)
BEGIN	
SELECT 	id,
		description
FROM type_workstation
WHERE id = p_id;
 
 END
 || 
 DELIMITER ;

  DELIMITER ||
CREATE PROCEDURE SP_GET_TYPE_WORKSTATION_BY_ID(IN p_id INT)
BEGIN	
SELECT 	id,
		description
FROM type_workstation
WHERE id = p_id;
 
 END
 || 
 DELIMITER ;


 CALL SP_GET_TYPE_WORKSTATION_BY_ID(5);