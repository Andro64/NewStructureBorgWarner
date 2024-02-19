CREATE TABLE WorkStation (
    id INT PRIMARY KEY    
); 

insert into WorkStation value(1);

--------------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER $$
CREATE PROCEDURE SP_GET_WORKSTATION()
BEGIN

SELECT 	
		workstation.id,
        description
FROM workstation
LEFT JOIN type_workstation ON workstation.id = type_workstation.id;
 
 END$$
DELIMITER ;

CALL SP_GET_WORKSTATION();

--------------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER //

CREATE PROCEDURE SP_UPD_WORKSTATION(
	  IN p_id_TypeWorkstation INT	  
)
BEGIN	
SET SQL_SAFE_UPDATES = 0;
   	UPDATE workstation
        SET id = p_id_TypeWorkstation;        
END //

DELIMITER ;

CALL SP_UPD_WORKSTATION(1);

--------------------------------------------------------------------------------------------------------------------------------------------------



