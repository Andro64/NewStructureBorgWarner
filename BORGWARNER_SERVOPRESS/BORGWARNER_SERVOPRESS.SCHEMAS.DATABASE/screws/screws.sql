CREATE TABLE screws (
  id INT AUTO_INCREMENT,
  id_screw int NOT NULL,
  encoder1 double NOT NULL,
  encoder2 double NOT NULL,
  tolerance double NOT NULL,
  id_model_screw int NOT NULL,
  text_position_X int NULL,
  text_position_Y int NULL,
  PRIMARY KEY (id,id_screw,id_model_screw)
) ;

INSERT INTO screws VALUES 	(1,1,96.9396,112.2992,3,1,340,-150),
							(2,2,92.6592,134.6468,3,1,0,-150),
							(3,3,94.002 ,119.5856,3,1,305, -70),
							(4,1,95.01  ,116.1404,3,2,340,-150),
							(5,4,52.9944,-79.906 ,3,1,0,-150),
							(6,5,52.998 ,-79.9192,3,1,305,-70);

SELECT * FROM screws;
-----------------------------------------------------------------------------------------------------------------------------------------------							

DELIMITER ||
CREATE PROCEDURE SP_GET_SCREWS()
BEGIN

SELECT 	 	screws.id,
			id_screw,
			encoder1,
			encoder2,
			tolerance,
			id_model_screw,
            models_screw.name_model,
            models_screw.quantity_screws,
            text_position_X,
            text_position_Y
FROM screws
LEFT JOIN models_screw ON screws.id_model_screw = models_screw.id
ORDER BY id_model_screw asc, id_screw  asc;
 
 END
 || 
 DELIMITER ;

CALL SP_GET_SCREWS();                            

-----------------------------------------------------------------------------------------------------------------------------------------------							


DELIMITER ||
CREATE PROCEDURE SP_GET_SCREWS_PAG(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	id,
		id_screw,
		encoder1,
		encoder2,
		tolerance,
		id_model_screw,
        text_position_X,
        text_position_Y
FROM screws
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END
 || 
 DELIMITER ;


 CALL SP_GET_SCREWS_PAG(1,5);          

 -----------------------------------------------------------------------------------------------------------------------------------------------							

 DELIMITER //

CREATE PROCEDURE SP_INS_UPD_SCREWS(
		IN p_id INT,
		IN p_id_screw INT,
		IN p_encoder1 DOUBLE,
		IN p_encoder2 DOUBLE,
		IN p_tolerance DOUBLE,
		IN p_id_model_screw INT,
        IN p_text_position_X INT,
		IN p_text_position_Y INT
)
BEGIN
	DECLARE screws_exist INT;
	SELECT COUNT(*) INTO screws_exist FROM screws WHERE id = p_id;
	
    IF screws_exist > 0 THEN
		UPDATE screws 
        SET id_screw = p_id_screw ,
			encoder1 = p_encoder1 ,
			encoder2 = p_encoder2 ,
			tolerance = p_tolerance ,
			id_model_screw= p_id_model_screw,
            text_position_X = p_text_position_X,
            text_position_Y = p_text_position_Y
        WHERE id = p_id;		
    ELSE
        INSERT INTO screws (id,id_screw,encoder1,encoder2,tolerance,id_model_screw,text_position_X,text_position_Y	) 
		VALUES (p_id ,
				p_id_screw ,			
				p_encoder1 ,
				p_encoder2 ,
				p_tolerance ,
				p_id_model_screw,
                p_text_position_X,
                p_text_position_Y);
    END IF;
END //

DELIMITER ;


CALL SP_INS_UPD_SCREWS(1,1,96.9396,112.2992,4,1,0,0);
CALL SP_INS_UPD_SCREWS(0,5,55.5555,555.5555,5,5,0,100);

-----------------------------------------------------------------------------------------------------------------------------------------------							
 DELIMITER //

CREATE PROCEDURE SP_DEL_SCREWS(
    IN p_id INT
)
BEGIN
    DELETE FROM screws WHERE id = p_id;
END //

DELIMITER ;


CALL SP_DEL_SCREWS(7);