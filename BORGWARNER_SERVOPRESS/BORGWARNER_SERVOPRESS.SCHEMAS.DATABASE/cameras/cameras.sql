CREATE TABLE type_camera (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description VARCHAR(45) UNIQUE NOT NULL
);

INSERT INTO type_camera (description) VALUES ('CognexD900');
INSERT INTO type_camera (description) VALUES ('CognexD2800');

-----------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE command_camera (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    id_type_camera INT NOT NULL,
    id_connections_by_workstation INT NOT NULL,
    path_image VARCHAR(255) NOT NULL,
    command_user VARCHAR(45) NOT NULL,
    command_setstring VARCHAR(45) NOT NULL,
    command_setevent VARCHAR(45) NOT NULL,
    command_getvalue_test VARCHAR(45) NOT NULL,
    command_getvalue_real VARCHAR(45) NOT NULL,
    command_getjob VARCHAR(45) NOT NULL,
    command_setjob VARCHAR(45) NOT NULL,
    command_getvalue_test_1 VARCHAR(45) NOT NULL,
    command_getvalue_real_1 VARCHAR(45) NOT NULL,
    command_getvalue_test_2 VARCHAR(45) NOT NULL,
    command_getvalue_real_2 VARCHAR(45) NOT NULL
);

INSERT INTO command_camera (id_type_camera,id_connections_by_workstation,path_image,command_user,command_setstring,command_setevent,command_getvalue_test,command_getvalue_real,
							command_getjob,command_setjob,command_getvalue_test_1,command_getvalue_real_1,command_getvalue_test_2,command_getvalue_real_2) 
VALUES 	(2,45,'C:\Users\MyUser\Desktop\COGNEX\Cognex_0','admin','SSMath_1.B4','SE8','GVMath_1.C4','GVMath_1.B4','','','','','',''),
		(2,46,'C:\Users\MyUser\Desktop\COGNEX\Cognex_1\Conector2Good','admin','SSMath_1.B6','SE8','GVMath_1.D5','GVMath_1.D4','','','','','',''),
		(1,44,'C:\Users\MyUser\Desktop\COGNEX\Cognex_2\Conector3Good','admin','SSInput','SE8','GVCarnada','GVOutput','','','','','','');


-----------------------------------------------------------------------------------------------------------------------------------------------

DELIMITER $$
CREATE PROCEDURE SP_GET_COMMAND_CAMERAS()
BEGIN

SELECT 	command_camera.id,
		type_connection.id as id_type_connection,
		type_connection.description as des_type_connection,
        type_camera.id as id_type_camara,        
        type_camera.description as des_type_camara,        
		id_connections_by_workstation,
        connections_by_workstation.ip as ip,
        connections_by_workstation.port_robot as port,
        path_image,
		command_user,
        command_setstring,
        command_setevent,
        command_getvalue_test,
        command_getvalue_real,
		command_getjob,
        command_setjob,
        command_getvalue_test_1,
        command_getvalue_real_1,
        command_getvalue_test_2,
        command_getvalue_real_2
FROM command_camera
LEFT JOIN connections_by_workstation ON command_camera.id_connections_by_workstation = connections_by_workstation.id
LEFT JOIN type_camera ON type_camera.id = command_camera.id_type_camera
LEFT JOIN type_device ON connections_by_workstation.id_type_device = type_device.id 
LEFT JOIN type_connection ON connections_by_workstation.id_type_connection = type_connection.id;
 
 END$$
DELIMITER ;

-----------------------------------------------------------------------------------------------------------------------------------------------
