CREATE TABLE type_device (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description VARCHAR(45) UNIQUE NOT NULL
); 

INSERT INTO type_device (description) VALUES ('ErgoArm');
INSERT INTO type_device (description) VALUES ('Screw');
INSERT INTO type_device (description) VALUES ('RobotEpson');
INSERT INTO type_device (description) VALUES ('Camara');
INSERT INTO type_device (description) VALUES ('Scanner');
INSERT INTO type_device (description) VALUES ('FIS');
