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

-----------------------------------------------------------------------------------------------------------------------------------------------							

DROP PROCEDURE IF EXISTS SP_GET_SCREW_PROGRAM;
DELIMITER //
CREATE PROCEDURE SP_GET_SCREW_PROGRAM(
		IN p_rework BOOL,
		IN p_debug BOOL,
		IN p_removeScrew BOOL		
)
BEGIN
	DECLARE model_screw_id INT;
    DECLARE ProgramaAtornillador VARCHAR(255);
      
    SELECT  models_screw.id INTO model_screw_id
	      /*models_screw.name_model as models_screw_name_model,
			models_screw.id as models_screw_id INTO model_screw_id*/
    FROM settings_by_workstation 
    LEFT JOIN models_screw ON models_screw.id = settings_by_workstation.value_setting
    WHERE id_TypeWorkstation = (SELECT id FROM workstation) AND setting = "Model_Screw";
      
    IF model_screw_id = 1 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    ELSEIF model_screw_id = 3 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '11';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '15';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    ELSEIF model_screw_id = 2 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    END IF;
    
    SELECT ProgramaAtornillador;
    
END //

DELIMITER ;

CALL SP_GET_SCREW_PROGRAM(false,false,true);

-----------------------------------------------------------------------------------------------------------------------------------------------							


CREATE TABLE programs_screws (
  id INT AUTO_INCREMENT PRIMARY KEY,
  id_model_screw int NOT NULL,
  screwing VARCHAR(4) NOT NULL,
  rescrewing VARCHAR(4) NOT NULL,
  unscrewing VARCHAR(4) NOT NULL,
  simulated VARCHAR(4) NOT NULL  
) ;

INSERT INTO programs_screws VALUES 	(1,43,'01','01','10','25'),
									(2,1 ,'01','01','10','25'),
                                    (3,2 ,'11','15','25','10');
select * from programs_screws;                                    

-----------------------------------------------------------------------------------------------------------------------------------------------							

DROP PROCEDURE IF EXISTS SP_GET_SCREW_PROGRAMS;

DELIMITER //
CREATE PROCEDURE SP_GET_SCREW_PROGRAMS()
BEGIN
	SELECT 
		programs_screws.id,
		id_model_screw,
		screwing,
		rescrewing,
		unscrewing,
		simulated 
    FROM settings_by_workstation
    LEFT JOIN programs_screws  ON programs_screws.id_model_screw = settings_by_workstation.value_setting
    WHERE id_TypeWorkstation = (SELECT id FROM workstation) AND setting = "Model_Screw";
      
END //

DELIMITER ;

CALL SP_GET_SCREW_PROGRAMS();