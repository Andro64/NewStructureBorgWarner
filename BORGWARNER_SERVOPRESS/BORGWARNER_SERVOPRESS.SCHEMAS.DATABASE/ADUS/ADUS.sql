CREATE TABLE adu_ports (
  id int NOT NULL AUTO_INCREMENT,
  id_TypeWorkstation int DEFAULT NULL,
  IOCard varchar(20) NOT NULL,
  keySensor varchar(50) NOT NULL,
  label varchar(50) NOT NULL,
  id_ADU int NOT NULL,
  id_index int NOT NULL,
  id_routine varchar(10) NOT NULL,
  PRIMARY KEY (id)
);
-----------------------------------------------------------------------------------------------------------------------------------------------							

DELIMITER ||
CREATE PROCEDURE SP_GET_AMAOUNT_ADUS()
BEGIN
SELECT COUNT(*) AS total_ADUS
FROM (
    SELECT id_ADU
    FROM adu_ports WHERE id_TypeWorkstation = (SELECT id FROM workstation)
    GROUP BY id_ADU
) AS agrupacion; 
 END
 || 
 DELIMITER ;

CALL SP_GET_AMAOUNT_ADUS();       

-----------------------------------------------------------------------------------------------------------------------------------------------							

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_ADUPORTS`()
BEGIN

SELECT id,
    IOCard,
    keySensor,
    label,
    id_ADU,
    id_index,
    id_routine
FROM adu_ports WHERE id_TypeWorkstation = (SELECT id FROM workstation);

 END$$
DELIMITER ;

CALL SP_GET_ADUPORTS();