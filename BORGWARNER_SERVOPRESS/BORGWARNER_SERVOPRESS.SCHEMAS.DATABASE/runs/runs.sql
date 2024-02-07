CREATE TABLE runs (
  id int NOT NULL AUTO_INCREMENT,
  partNumber varchar(100) NOT NULL,
  serial varchar(120) DEFAULT NULL,
  serial2 varchar(120) DEFAULT NULL,
  date timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  result varchar(20) NOT NULL,
  Screw1Torque varchar(20) DEFAULT NULL,
  Screw1Angle varchar(20) DEFAULT NULL,
  Screw2Torque varchar(20) DEFAULT NULL,
  Screw2Angle varchar(20) DEFAULT NULL,
  Screw3Torque varchar(20) DEFAULT NULL,
  Screw3Angle varchar(20) DEFAULT NULL,
  Screw4Torque varchar(20) DEFAULT NULL,
  Screw4Angle varchar(20) DEFAULT NULL,
  Screw5Torque varchar(20) DEFAULT NULL,
  Screw5Angle varchar(20) DEFAULT NULL,
  ultracappadinspect varchar(45) DEFAULT NULL,
  ultracapboardinspect varchar(45) DEFAULT NULL,
  insulatorinspect varchar(45) DEFAULT NULL,
  PRIMARY KEY (id),
  KEY fis_id_idx (id)
);
INSERT INTO models_screw VALUES (1,'modelo1','FUSION\r','modelo1','Model for testing cycle with FIS','3'),(2,'modelo2','serial','modelo2','Model for testing program with model selection with all the sensors in false.','2');

DELIMITER ||
CREATE PROCEDURE SP_GET_RUNS(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT id,
    partNumber,
    serial,
    serial2,
    date,
    result,
    Screw1Torque,
    Screw1Angle,
    Screw2Torque,
    Screw2Angle,
    Screw3Torque,
    Screw3Angle,
    Screw4Torque,
    Screw4Angle,
    Screw5Torque,
    Screw5Angle,
    ultracappadinspect,
    ultracapboardinspect,
    insulatorinspect
FROM runs
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END
 || 
 DELIMITER ;


CALL SP_GET_RUNS(1,10);

DELIMITER //

CREATE PROCEDURE SP_INS_UPD_RUNS(
		IN p_id int,
		IN p_partNumber varchar(100) ,
		IN p_serial varchar(120) ,
		IN p_serial2 varchar(120) ,
		IN p_date timestamp,
		IN p_result varchar(20) ,
		IN p_Screw1Torque varchar(20) ,
		IN p_Screw1Angle varchar(20) ,
		IN p_Screw2Torque varchar(20) ,
		IN p_Screw2Angle varchar(20) ,
		IN p_Screw3Torque varchar(20) ,
		IN p_Screw3Angle varchar(20) ,
		IN p_Screw4Torque varchar(20) ,
		IN p_Screw4Angle varchar(20) ,
		IN p_Screw5Torque varchar(20) ,
		IN p_Screw5Angle varchar(20) ,
		IN p_ultracappadinspect varchar(45) ,
		IN p_ultracapboardinspect varchar(45) ,
		IN p_insulatorinspect varchar(45) 
)
BEGIN
	DECLARE run_exist INT;
	SELECT COUNT(*) INTO run_exist FROM runs WHERE id = p_id;
	
    IF run_exist > 0 THEN
		UPDATE runs 
        SET id = p_id ,
			partNumber = p_partNumber  ,
			serial = p_serial  ,
			serial2 = p_serial2  ,
			date = p_date ,
			result = p_result ,
			Screw1Torque = p_Screw1Torque ,
			Screw1Angle = p_Screw1Angle ,
			Screw2Torque = p_Screw2Torque ,
			Screw2Angle = p_Screw2Angle ,
			Screw3Torque = p_Screw3Torque ,
			Screw3Angle = p_Screw3Angle ,
			Screw4Torque = p_Screw4Torque ,
			Screw4Angle = p_Screw4Angle ,
			Screw5Torque = p_Screw5Torque ,
			Screw5Angle = p_Screw5Angle ,
			ultracappadinspect = p_ultracappadinspect ,
			ultracapboardinspect = p_ultracapboardinspect ,
			insulatorinspect = p_insulatorinspect 
			WHERE id = p_id;	
    ELSE
        INSERT INTO runs (partNumber,serial,serial2,date,result,Screw1Torque,Screw1Angle,Screw2Torque,Screw2Angle,Screw3Torque,Screw3Angle,Screw4Torque,Screw4Angle,Screw5Torque,Screw5Angle,ultracappadinspect,ultracapboardinspect,insulatorinspect)
		VALUES(p_partNumber  ,
				p_serial  ,
				p_serial2  ,
				p_date ,
				p_result  ,
				p_Screw1Torque  ,
				p_Screw1Angle  ,
				p_Screw2Torque  ,
				p_Screw2Angle  ,
				p_Screw3Torque  ,
				p_Screw3Angle  ,
				p_Screw4Torque  ,
				p_Screw4Angle  ,
				p_Screw5Torque  ,
				p_Screw5Angle  ,
				p_ultracappadinspect  ,
				p_ultracapboardinspect  ,
				p_insulatorinspect ); 
    END IF;
END //

DELIMITER ;

CALL SP_INS_UPD_RUNS(0, 'test_model 3', NULL, NULL, '2021-07-06 15:44:58', 'Failed', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
CALL SP_INS_UPD_RUNS(35, 'test_model 3', NULL, NULL, '2021-07-06 15:44:58', 'Failed', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

DELIMITER //

CREATE PROCEDURE SP_DEL_RUNS(
    IN p_id INT
)
BEGIN
    DELETE FROM runs WHERE id = p_id;
END //

DELIMITER ;


CALL SP_DEL_RUNS(211);

