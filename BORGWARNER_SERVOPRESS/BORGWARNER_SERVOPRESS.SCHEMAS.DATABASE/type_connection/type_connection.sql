CREATE TABLE type_connection (
    id INT AUTO_INCREMENT PRIMARY KEY,
    description VARCHAR(45) UNIQUE NOT NULL
); 

INSERT INTO type_connection (description) VALUES ('Main');
INSERT INTO type_connection (description) VALUES ('Auxiliary');
INSERT INTO type_connection (description) VALUES ('InputDevices');
INSERT INTO type_connection (description) VALUES ('Emergency');

INSERT INTO type_connection (description) VALUES ('Scan_1');
INSERT INTO type_connection (description) VALUES ('Scan_2');
INSERT INTO type_connection (description) VALUES ('CognexD900');
INSERT INTO type_connection (description) VALUES ('CognexD2800');
