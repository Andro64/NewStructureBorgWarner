CREATE TABLE connections_by_workstation (
  id_TypeWorkstation INT NOT NULL,
  id int(11) NOT NULL AUTO_INCREMENT,
  id_type_device int(11) NOT NULL,
  ip varchar(45) NOT NULL,
  port_robot int(11) NOT NULL,
  id_type_connection int(11) NOT NULL,
  PRIMARY KEY (id)
) 

INSERT INTO connections_by_workstation 
(id_TypeWorkstation,id_type_device,ip,port_robot,id_type_connection)
VALUES 	(1,3,'::1',5000,1),
		(1,3,'::1',4000,2),
		(1,3,'::1',4001,3),
		(1,3,'::1',4003,4),
		(1,2,'::1',4545,1),
		(1,1,'::1',502,1),
		(1,4,'::1',0,1),
		(1,5,'::1',0,1),
		(1,6,'::1',0,1),
		(1,5,'::1',0,5),
		(1,5,'::1',0,6),
		(1,4,'::1',23,7),
		(1,4,'::1',23,8);
                                
INSERT INTO connections_by_workstation 
(id_TypeWorkstation,id_type_device,ip,port_robot,id_type_connection)
VALUES 	(2,3,'::1',5000,1),								
		(2,1,'::1',502,1),
		(2,4,'::1',0,1),
		(2,5,'::1',0,5),                                
		(2,4,'::1',23,7),
		(2,4,'::1',23,8);
                                
INSERT INTO connections_by_workstation 
(id_TypeWorkstation,id_type_device,ip,port_robot,id_type_connection)
VALUES 	(6,3,'::1',5000,1),
		(6,3,'::1',4000,2),
		(6,3,'::1',4001,3),
		(6,3,'::1',4003,4),
		(6,2,'::1',4545,1),
		(6,1,'::1',502,1),
		(6,4,'::1',0,1),
		(6,5,'::1',0,1),
		(6,6,'::1',0,1),
		(6,5,'::1',0,5),
		(6,5,'::1',0,6),
		(6,4,'::1',23,7),
		(6,4,'::1',23,8); 
        

-----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER $$
CREATE PROCEDURE SP_GET_CONNECTIONS_BY_WS(IN p_id_TypeWorkstation INT)
BEGIN

SELECT 	id_TypeWorkstation,
		connections_by_workstation.id,
		connections_by_workstation.id_type_device,
        type_device.description as des_type_device,
        ip,
        port_robot,
        id_type_connection,
        type_connection.description as des_type_connection
FROM connections_by_workstation
LEFT JOIN type_device ON connections_by_workstation.id_type_device = type_device.id 
LEFT JOIN type_connection ON connections_by_workstation.id_type_connection = type_connection.id
WHERE id_TypeWorkstation = p_id_TypeWorkstation;
 
 END$$
DELIMITER ;


CALL SP_GET_CONNECTIONS_BY_WS(2);
-----------------------------------------------------------------------------------------------------------------------------------------------
