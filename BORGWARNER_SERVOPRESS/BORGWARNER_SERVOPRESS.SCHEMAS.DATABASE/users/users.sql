CREATE TABLE users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name varchar(45) DEFAULT NULL,
  lastName varchar(45) DEFAULT NULL,
  username varchar(45) DEFAULT NULL,
  password varchar(45) DEFAULT NULL,
  id_profile int DEFAULT NULL
);


INSERT INTO users(name,lastName,username,password,id_profile) VALUES ('Andro','Bas','Androide','12345',1),('Emanuel','Jot','Reader','56789',2);

DELIMITER $$
CREATE FUNCTION CheckPassword(username_p VARCHAR(8), password_p VARCHAR(20)) RETURNS tinyint(1)
    READS SQL DATA
BEGIN
    RETURN EXISTS (SELECT username FROM users WHERE username = username_p AND password = password_p);
END$$
DELIMITER ;


CREATE TABLE profile (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description VARCHAR(45) UNIQUE NOT NULL
); 

INSERT INTO profile (description) VALUES ('Admin');
INSERT INTO profile (description) VALUES ('Usuario');



DELIMITER ||
CREATE PROCEDURE SP_GET_USERS_PAG(IN page_p INT, IN size_p INT)
BEGIN
    DECLARE page INT;
    DECLARE _size INT;
    SET page = page_p;
    SET _size = size_p;

    SET @qry_string = CONCAT("
        SELECT  users.id as id_users,
                name,
                lastName,
                username,
                password,
                id_profile,
                profile.description as profile_description
        FROM users
        LEFT JOIN profile ON users.id_profile = profile.id
        LIMIT ", ((page - 1) * _size), ",", _size);
    PREPARE qry FROM @qry_string;
    EXECUTE qry;
    DEALLOCATE PREPARE qry;
END ||
DELIMITER ;

 
 CALL SP_GET_USERS_PAG(1,10);


 DELIMITER //

CREATE PROCEDURE SP_INS_UPD_USERS(
		IN p_id INT,
		IN p_name varchar(45),
		IN p_lastName varchar(45),
		IN p_username varchar(45),
		IN p_password varchar(45),
		IN p_id_profile int
)
BEGIN
	DECLARE users_exist INT;
	SELECT COUNT(*) INTO users_exist FROM users WHERE id = p_id;
	
    IF users_exist > 0 THEN
		UPDATE users 
        SET name = p_name, 
			lastName= p_lastName, 
			username = p_username, 
			password = p_password, 
			id_profile= p_id_profile
        WHERE id = p_id;		
    ELSE
        INSERT INTO users (name,lastName,username,password,id_profile) 
		VALUES (p_name, 
				p_lastName, 
				p_username, 
				p_password, 
				p_id_profile);
    END IF;
END //

DELIMITER ;

 CALL SP_INS_UPD_USERS(0,'AndroIDE','Bas','Androide','12345',1)
CALL SP_INS_UPD_USERS(3,'AndroPE','Bas','Androide','12345',1)

CREATE PROCEDURE SP_DEL_USERS(
    IN p_id INT
)
BEGIN
    DELETE FROM users WHERE id = p_id;
END //

DELIMITER ;


CALL SP_DEL_USERS(3);


DELIMITER $$
CREATE PROCEDURE SP_GET_PROFILES()
BEGIN
SELECT id,
    description
FROM profile;

 END$$
DELIMITER ;

CALL SP_GET_PROFILES()


DELIMITER //

CREATE PROCEDURE SP_VALIDATE_USER(
    IN p_username VARCHAR(255),
    IN p_password VARCHAR(255)
)
BEGIN
    DECLARE user_count INT;

    -- Verificar si el usuario y la contraseña coinciden en la base de datos
    SELECT COUNT(*) INTO user_count
    FROM users
    WHERE username = p_username AND password = p_password;

    -- Si se encuentra un usuario válido, devuelve sus datos
    IF user_count > 0 THEN
       SELECT  users.id as id_users,
                name,
                lastName,
                username,
                password,
                id_profile,
                profile.description as profile_description
        FROM users
        LEFT JOIN profile ON users.id_profile = profile.id
        WHERE username = p_username AND password = p_password;
    END IF;
END //

DELIMITER ;


CALL SP_VALIDATE_USER('Android','12345');