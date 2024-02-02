CREATE TABLE screws (
  id INT AUTO_INCREMENT,
  id_screw int NOT NULL,
  encoder1 double NOT NULL,
  encoder2 double NOT NULL,
  tolerance double NOT NULL,
  id_model_screw int NOT NULL,
  PRIMARY KEY (id,id_screw,id_model_screw)
) ;

INSERT INTO screws VALUES (1,1,96.9396,112.2992,3,1),(2,2,92.6592,134.6468,3,1),(3,3,94.002,119.5856,3,1),(4,1,95.01,116.1404,3,2),(5,4,52.9944,-79.906,3,1),(6,5,52.998,-79.9192,3,1);

DELIMITER ||
CREATE PROCEDURE SP_GET_SCREWS()
BEGIN

SELECT 	 	screws.id,
			id_screw,
			encoder1,
			encoder2,
			tolerance,
			id_model_screw,
            models_screw.name_model,
            models_screw.quantity_screws
FROM screws
LEFT JOIN models_screw ON screws.id_model_screw = models_screw.id
ORDER BY id_model_screw asc, id_screw  asc;
 
 END
 || 
 DELIMITER ;
 
CALL SP_GET_SCREWS();
