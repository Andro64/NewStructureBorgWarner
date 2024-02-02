CREATE TABLE connections (
  id int(11) NOT NULL AUTO_INCREMENT,
  id_type_device int(11) NOT NULL,
  ip varchar(45) NOT NULL,
  port_robot int(11) NOT NULL,
  id_type_connection int(11) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8;



DELIMITER $$
CREATE PROCEDURE SP_GET_connections()
BEGIN

SELECT 	connections.id,
		connections.id_type_device,
        type_device.description as des_type_device,
        ip,
        port_robot,
        id_type_connection,
        type_connection.description as des_type_connection
FROM connections
LEFT JOIN type_device ON connections.id_type_device = type_device.id 
LEFT JOIN type_connection ON connections.id_type_connection = type_connection.id;
 
 END$$
DELIMITER ;

INSERT INTO connections VALUES (1,3,'::1',5000,1),(2,3,'::1',4000,2),(3,3,'::1',4001,3),(4,3,'::1',4003,4),(5,2,'::1',4545,1),(6,1,'::1',502,1),(7,4,'::1',0,1),(8,5,'::1',0,1),(9,6,'::1',0,1),(10,5,'::1',0,5),(11,5,'::1',0,6),(12,4,'::1',23,7),(13,4,'::1',23,8);

CALL SP_GET_connections();