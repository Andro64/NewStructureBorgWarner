CREATE TABLE models_screw (
  id int(11) NOT NULL AUTO_INCREMENT,
  partNumber varchar(50) DEFAULT NULL,
  serial varchar(50) DEFAULT NULL,
  name_model varchar(50) NOT NULL,
  description varchar(300) DEFAULT NULL,
  quantity_screws int(11) DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;


INSERT INTO models_screw VALUES (1,'modelo1','FUSION\r','modelo1','Model for testing cycle with FIS','3'),(2,'modelo2','serial','modelo2','Model for testing program with model selection with all the sensors in false.','2');

DELIMITER ||
CREATE PROCEDURE SP_GET_MODELS_SCREWS(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	 	id,
			partNumber,
			serial,
			name_model,
			description,
			quantity_screws  
FROM models_screw
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END
 || 
 DELIMITER ;


CALL SP_GET_MODELS_SCREWS();

DELIMITER //

CREATE PROCEDURE SP_INS_UPD_MODELS_SCREWS(
	  IN p_id INT,
	  IN p_partNumber VARCHAR(50),
	  IN p_serial VARCHAR(50),
	  IN p_name_model VARCHAR(50),
	  IN p_description VARCHAR(300),
	  IN p_quantity_screws INT(11)
)
BEGIN
	DECLARE model_exist INT;
	SELECT COUNT(*) INTO model_exist FROM models_screw WHERE id = p_id;
	
    IF model_exist > 0 THEN
		UPDATE models_screw 
        SET partNumber = p_partNumber,
			serial = p_serial,
			name_model = p_name_model,
			description = p_description,
			quantity_screws = p_quantity_screws
        WHERE id = p_id;		
    ELSE
        INSERT INTO models_screw (partNumber,serial,name_model,description,quantity_screws) 
		VALUES (p_partNumber,
				p_serial,
				p_name_model,
				p_description,
				p_quantity_screws);
    END IF;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE SP_DEL_MODELS_SCREWS(
    IN p_id INT
)
BEGIN
    DELETE FROM models_screw WHERE id = p_id;
END //

DELIMITER ;


CALL df4187_sta9.INS_UPD_MODELS_SCREWS(1,'modelo1','fusioon','modelo1','Model for testing cycle with FIS',3);
CALL df4187_sta9.INS_UPD_MODELS_SCREWS(0,'modelo3','serial','prueba1','Modelo de prueba 1',5);

