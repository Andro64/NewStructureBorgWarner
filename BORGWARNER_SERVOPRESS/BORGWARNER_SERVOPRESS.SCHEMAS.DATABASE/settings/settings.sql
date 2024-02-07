CREATE TABLE settings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    setting VARCHAR(45) NOT NULL,
    value_setting VARCHAR(255) NOT NULL    
);


INSERT INTO settings (setting, value_setting) VALUES ('ADU_SERIAL_1','E21531'); 
INSERT INTO settings (setting, value_setting) VALUES ('ADU_SERIAL_2','E21534'); 
INSERT INTO settings (setting, value_setting) VALUES ('ADU_SERIAL_3','E21535'); 

INSERT INTO settings (setting, value_setting) VALUES ('FTP_Camara_Path','C:\\Users\\PC\\Desktop\\FTP-2'); 
INSERT INTO settings (setting, value_setting) VALUES ('fis_process','CMI'); 
INSERT INTO settings (setting, value_setting) VALUES ('fis_station','sta2'); 
INSERT INTO settings (setting, value_setting) VALUES ('last_model','modelo1'); 

INSERT INTO settings (setting, value_setting) VALUES ('Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX'); 

INSERT INTO settings (setting, value_setting) VALUES ('Number_Screws_Process','3'); 
INSERT INTO settings (setting, value_setting) VALUES ('Model_Screw','2'); 
INSERT INTO settings (setting, value_setting) VALUES ('GRID_Number_Reg_by_Page','2'); 

DELIMITER ||
CREATE PROCEDURE SP_GET_SETTINGS()
BEGIN

SELECT 	id,
		setting, 
        value_setting
FROM settings;
 
 END
 || 
 DELIMITER ;

  CALL SP_GET_SETTINGS();


  DELIMITER ||
CREATE PROCEDURE SP_GET_SETTINGS_PAG(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	id,
		setting, 
        value_setting
FROM settings
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END
 || 
 DELIMITER ;


  CALL SP_GET_SETTINGS_PAG(1,10);


  DELIMITER //

CREATE PROCEDURE SP_INS_UPD_SETTINGS(
	  IN p_id INT,
	  IN p_setting VARCHAR(50),
	  IN p_value_setting VARCHAR(50)
)
BEGIN
	DECLARE settings_exist INT;
	SELECT COUNT(*) INTO settings_exist FROM settings WHERE id = p_id;
	
    IF settings_exist > 0 THEN
		UPDATE settings 
        SET setting = p_setting,
			value_setting = p_value_setting
        WHERE id = p_id;		
    ELSE
        INSERT INTO models_screw (setting,value_setting) 
		VALUES (p_setting,
				p_value_setting);
    END IF;
END //

DELIMITER ;


 DELIMITER //

CREATE PROCEDURE SP_INS_UPD_SETTINGS(
	  IN p_id INT,
	  IN p_setting VARCHAR(50),
	  IN p_value_setting VARCHAR(50)
)
BEGIN
	DECLARE settings_exist INT;
	SELECT COUNT(*) INTO settings_exist FROM settings WHERE id = p_id;
	
    IF settings_exist > 0 THEN
		UPDATE settings 
        SET setting = p_setting,
			value_setting = p_value_setting
        WHERE id = p_id;		
    ELSE
        INSERT INTO settings (setting,value_setting) 
		VALUES (p_setting,
				p_value_setting);
    END IF;
END //

DELIMITER ;
  
  
  CALL SP_INS_UPD_SETTINGS(0,'prueba','1');
  CALL SP_INS_UPD_SETTINGS(11,'GRID_Number_Reg_by_Page','10');



    DELIMITER //

CREATE PROCEDURE SP_DEL_SETTINGS(
    IN p_id INT
)
BEGIN
    DELETE FROM settings WHERE id = p_id;
END //

DELIMITER ;


CALL SP_DEL_SETTINGS(12);


