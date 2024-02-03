DELIMITER ||
CREATE PROCEDURE SP_GET_TOTALREG_BY_TABLES()
BEGIN
DECLARE num_reg_by_page INT;
DROP TEMPORARY TABLE IF EXISTS totalreg_by_tables_temp;
CREATE TEMPORARY TABLE totalreg_by_tables_temp (    
    name_table VARCHAR(50),
    total_reg INT    
);
SELECT CAST( value_setting as SIGNED) INTO num_reg_by_page FROM settings WHERE setting = 'GRID_Number_Reg_by_Page';

INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'connections',COUNT(*) as 'TotalReg'  FROM connections;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'fis_history',COUNT(*) as 'TotalReg'  FROM fis_history;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'models_screw',COUNT(*) as 'TotalReg' FROM models_screw;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'screws',COUNT(*) as 'TotalReg'  FROM screws;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'settings',COUNT(*) as 'TotalReg'  FROM settings;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'type_connection',COUNT(*) as 'TotalReg'  FROM type_connection;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'type_device',COUNT(*) as 'TotalReg'  FROM type_device;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'users',COUNT(*) as 'TotalReg'  FROM users;

SELECT name_table, total_reg,CEIL( total_reg / num_reg_by_page) as pages FROM totalreg_by_tables_temp;

 END
 || 
 DELIMITER ;
 
 CALL SP_GET_TOTALREG_BY_TABLES();