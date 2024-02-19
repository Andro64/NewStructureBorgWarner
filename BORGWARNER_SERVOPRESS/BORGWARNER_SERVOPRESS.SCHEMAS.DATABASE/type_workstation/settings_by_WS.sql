CREATE TABLE settings_by_Workstation (
    id_TypeWorkstation INT NOT NULL,
    id INT AUTO_INCREMENT PRIMARY KEY,
    setting VARCHAR(45) NOT NULL,
    value_setting VARCHAR(255) NOT NULL    
);


INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'TYPE_WORKSTATION', '6');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'Path_LOG', 'D:\\Repo\\Logs\\');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'GRID_Number_Reg_by_Page','2');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'ADU_SERIAL_1','E21531'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'ADU_SERIAL_2','E21534'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'ADU_SERIAL_3','E21535'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'FTP_Camara_Path','C:\\Users\\PC\\Desktop\\FTP-2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'fis_process','CMI'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'fis_station','sta2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'last_model','modelo1'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'Number_Screws_Process','3'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (1,'Model_Screw','2'); 
					
					
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'TYPE_WORKSTATION', '6');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'Path_LOG', 'D:\\Repo\\Logs\\');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'GRID_Number_Reg_by_Page','2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'ADU_SERIAL_1','E21531'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'ADU_SERIAL_2','E21534'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'ADU_SERIAL_3','E21535'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'Number_Screws_Process','3'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (2,'Model_Screw','2'); 
					
					
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'TYPE_WORKSTATION', '6');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'Path_LOG', 'D:\\Repo\\Logs\\');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'GRID_Number_Reg_by_Page','2');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'ADU_SERIAL_1','E21531'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'ADU_SERIAL_2','E21534'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'ADU_SERIAL_3','E21535'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'Number_Screws_Process','3'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (3,'Model_Screw','2'); 
					
					
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'TYPE_WORKSTATION', '6');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'Path_LOG', 'D:\\Repo\\Logs\\');
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'GRID_Number_Reg_by_Page','2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'ADU_SERIAL_1','E21531'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'ADU_SERIAL_2','E21534'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'ADU_SERIAL_3','E21535'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'FTP_Camara_Path','C:\\Users\\PC\\Desktop\\FTP-2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'fis_process','CMI'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'fis_station','sta2'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'last_model','modelo1'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'Number_Screws_Process','3'); 
INSERT INTO settings_by_Workstation (id_TypeWorkstation, setting, value_setting) VALUES (6,'Model_Screw','2'); 


-----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER ||
CREATE PROCEDURE SP_GET_SETTINGS_BY_WS(IN p_id_TypeWorkstation INT)
BEGIN

SELECT 	id_TypeWorkstation,
		id,
		setting, 
        value_setting
FROM settings_by_Workstation WHERE id_TypeWorkstation = p_id_TypeWorkstation;
 
 END
 || 
 DELIMITER ;

  CALL SP_GET_SETTINGS_BY_WS(1);

  -----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER //

CREATE PROCEDURE SP_GET_SETTINGS_PAG_BY_WS(IN p_id_TypeWorkstation INT, IN page_p INT, IN size_p INT)
BEGIN
    DECLARE _page INT;
    DECLARE _size INT;
    DECLARE id_TypeWorkstation INT;
    
    SET _page = page_p;
    SET _size = size_p;
    SET id_TypeWorkstation = p_id_TypeWorkstation;

    SET @qry_string = CONCAT('
        SELECT 
            id_TypeWorkstation,
            id,
            setting,
            value_setting
        FROM settings_by_Workstation
        WHERE id_TypeWorkstation = ', id_TypeWorkstation, '
        LIMIT ', (_page - 1) * _size, ',', _size
    );

    PREPARE stmt FROM @qry_string;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END//

DELIMITER ;

  CALL SP_GET_SETTINGS_PAG_BY_WS(1,1,10);
 
-----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER //

CREATE PROCEDURE SP_INS_UPD_SETTINGS_BY_WS(
	  IN p_id_TypeWorkstation INT,
	  IN p_id INT,
	  IN p_setting VARCHAR(50),
	  IN p_value_setting VARCHAR(50)
)
BEGIN
	DECLARE settings_exist INT;
	SELECT COUNT(*) INTO settings_exist FROM settings_by_Workstation WHERE id = p_id;
	
    IF settings_exist > 0 THEN
		UPDATE settings_by_Workstation 
        SET setting = p_setting,
			value_setting = p_value_setting
        WHERE id = p_id;		
    ELSE
        INSERT INTO settings_by_Workstation (id_TypeWorkstation,setting,value_setting) 
		VALUES (p_id_TypeWorkstation,
				p_setting,
				p_value_setting);
    END IF;
END //

DELIMITER ;

  CALL SP_INS_UPD_SETTINGS_BY_WS(2,0,'prueba','1');
  CALL SP_INS_UPD_SETTINGS_BY_WS(2,43,'prueba','10');
  CALL SP_GET_SETTINGS_PAG_BY_WS(2,1,20);

-----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER //

CREATE PROCEDURE SP_DEL_SETTINGS_BY_WS(IN p_id_TypeWorkstation INT, IN p_id INT)
BEGIN
    DELETE FROM settings_by_Workstation WHERE id = p_id;
END //

DELIMITER ;


CALL SP_DEL_SETTINGS_BY_WS(2,43);
CALL SP_GET_SETTINGS_PAG_BY_WS(2,1,20);

