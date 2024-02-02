CREATE TABLE users (
  name varchar(45) DEFAULT NULL,
  lastName varchar(45) DEFAULT NULL,
  username varchar(45) DEFAULT NULL,
  password varchar(45) DEFAULT NULL,
  profile varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO users VALUES ('Andro','Bas','Androide','12345','admin'),('Emanuel','Jot','Reader','56789','otro');

DELIMITER $$
CREATE FUNCTION CheckPassword(username_p VARCHAR(8), password_p VARCHAR(20)) RETURNS tinyint(1)
    READS SQL DATA
BEGIN
    RETURN EXISTS (SELECT username FROM users WHERE username = username_p AND password = password_p);
END$$
DELIMITER ;
