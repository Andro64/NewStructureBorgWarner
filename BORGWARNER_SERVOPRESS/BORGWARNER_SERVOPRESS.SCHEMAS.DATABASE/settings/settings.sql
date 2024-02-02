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


