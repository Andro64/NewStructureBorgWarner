CREATE DATABASE  IF NOT EXISTS `bw_database` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `bw_database`;
-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: bw_database
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `command_camera`
--

DROP TABLE IF EXISTS `command_camera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `command_camera` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_type_camera` int NOT NULL,
  `id_connections_by_workstation` int NOT NULL,
  `path_image` varchar(255) NOT NULL,
  `path_image_show_errors` varchar(255) NOT NULL,
  `command_user` varchar(45) NOT NULL,
  `command_setstring` varchar(45) NOT NULL,
  `command_setevent` varchar(45) NOT NULL,
  `command_getvalue_test` varchar(45) NOT NULL,
  `command_getvalue_real` varchar(45) NOT NULL,
  `command_getjob` varchar(45) NOT NULL,
  `command_setjob` varchar(45) NOT NULL,
  `command_getvalue_test_1` varchar(45) NOT NULL,
  `command_getvalue_real_1` varchar(45) NOT NULL,
  `command_getvalue_test_2` varchar(45) NOT NULL,
  `command_getvalue_real_2` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `command_camera`
--

LOCK TABLES `command_camera` WRITE;
/*!40000 ALTER TABLE `command_camera` DISABLE KEYS */;
INSERT INTO `command_camera` VALUES (1,2,45,'C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_1\\Conector2Good','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_1\\Conector2Bad','admin','SSMath_1.B4','SE8','GVMath_1.C4','GVMath_1.B4','','','','','',''),(2,2,46,'C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_2\\Conector3Good','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_2\\Conector3Bad','admin','SSMath_1.B6','SE8','GVMath_1.D5','GVMath_1.D4','','','','','',''),(3,1,44,'C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0','admin','SSInput','SE8','GVCarnada','GVOutput','','','','','','');
/*!40000 ALTER TABLE `command_camera` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `connections`
--

DROP TABLE IF EXISTS `connections`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `connections` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_type_device` int NOT NULL,
  `ip` varchar(45) NOT NULL,
  `port_robot` int NOT NULL,
  `id_type_connection` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `connections`
--

LOCK TABLES `connections` WRITE;
/*!40000 ALTER TABLE `connections` DISABLE KEYS */;
/*!40000 ALTER TABLE `connections` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `connections_by_workstation`
--

DROP TABLE IF EXISTS `connections_by_workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `connections_by_workstation` (
  `id_TypeWorkstation` int NOT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  `id_type_device` int NOT NULL,
  `ip` varchar(45) NOT NULL,
  `port_robot` int NOT NULL,
  `id_type_connection` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `connections_by_workstation`
--

LOCK TABLES `connections_by_workstation` WRITE;
/*!40000 ALTER TABLE `connections_by_workstation` DISABLE KEYS */;
INSERT INTO `connections_by_workstation` VALUES (6,33,3,'::1',5000,1),(6,34,3,'::1',4000,2),(6,35,3,'::1',4001,3),(6,36,3,'::1',4003,4),(6,37,2,'192.168.1.41',4545,1),(6,38,1,'192.168.1.31',502,1),(6,41,6,'10.34.126.22',24539,1),(6,42,5,'192.168.1.21',9004,5),(6,43,5,'192.168.1.22',8000,6),(6,44,4,'192.168.1.11',23,7),(6,45,4,'192.168.1.12',23,8),(6,46,4,'192.168.1.13',23,9),(12,47,3,'::1',5000,1),(12,48,3,'::1',4000,2),(12,49,3,'::1',4001,3),(12,50,3,'::1',4003,4),(12,51,2,'192.168.1.41',4545,1),(12,52,1,'192.168.1.31',502,1),(12,53,6,'10.34.126.22',24539,1),(12,54,5,'192.168.1.21',9004,5),(12,55,5,'192.168.1.22',8000,6),(12,56,4,'192.168.1.11',23,7),(12,57,4,'192.168.1.12',23,8),(12,58,4,'192.168.1.13',23,9);
/*!40000 ALTER TABLE `connections_by_workstation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fis_history`
--

DROP TABLE IF EXISTS `fis_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fis_history` (
  `id` int NOT NULL AUTO_INCREMENT,
  `model` varchar(50) NOT NULL,
  `to_fis` varchar(100) NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `from_fis` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fis_history`
--

LOCK TABLES `fis_history` WRITE;
/*!40000 ALTER TABLE `fis_history` DISABLE KEYS */;
INSERT INTO `fis_history` VALUES (1,'4206086640150046','BREQ|id=420608664015004|process=CMI|station=sta12','2024-03-03 01:06:55','BCNF|id=420608664015004|status=FAIL|msg=FailedToGetUnitStats\n'),(2,'4203640532080059','BREQ|id=420364053208005|process=CMI|station=sta12','2024-03-03 01:10:34','BCNF|id=420364053208005|status=FAIL|msg=FailedToGetUnitStats\n'),(3,'420548494043JA08','BREQ|id=420548494043JA0|process=PIM_S12_01|station=PIM_S12_AL01','2024-03-03 02:07:10','BCNF|id=420548494043JA0|status=FAIL|msg=FailedToGetUnitStats\n'),(4,'420608664039JA18','BREQ|id=420608664039JA1|process=PIM_S12_01|station=PIM_S12_AL01','2024-03-03 02:08:15','BCNF|id=420608664039JA1|status=FAIL|msg=FailedToGetUnitStats\n'),(5,'420608664039JA18','BREQ|id=420608664039JA1|process=PIM_S12_01|station=PIM_S12_AL01','2024-03-03 02:08:41','BCNF|id=420608664039JA1|status=FAIL|msg=FailedToGetUnitStats\n'),(6,'420608664039JA18','BREQ|id=420608664039JA18|process=PIM_S12_01|station=PIM_S12_AL01','2024-03-03 02:11:28','BCNF|id=420608664039JA18|status=FAIL|msg=RouteCheck\n'),(7,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:32:27','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(8,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:37:18','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(9,'4203640532080036\r','BREQ|id=4203640532080036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:40:27','BCNF|id=4203640532080036|status=FAIL|msg=RouteCheck\n'),(10,'42036363B\r','BREQ|id=42036363B|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:43:32','BCNF|id=42036363B|status=FAIL|msg=FailedToGetUnitStats\n'),(11,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:55:55','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(12,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:57:21','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(13,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 00:57:54','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(14,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:24:20','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(15,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:27:38','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(16,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:27:50','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(17,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:35:57','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(18,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:36:02','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(19,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:41:41','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(20,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:42:01','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(21,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:52:51','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(22,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 01:53:20','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(23,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 02:09:24','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(24,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 02:09:44','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(25,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 02:29:29','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(26,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 02:31:32','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(27,'4204169340380036\r','BREQ|id=4204169340380036|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 02:33:36','BCNF|id=4204169340380036|status=PASS|model=42041693\n'),(28,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 17:58:30','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(29,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 17:59:08','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(30,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:07:35','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(31,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:07:59','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(32,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:08:22','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(33,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:21:17','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(34,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:21:26','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(35,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:25:16','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(36,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:25:22','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(37,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:25:46','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(38,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:37:58','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(39,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:40:10','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(40,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:40:12','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(41,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:42:41','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(42,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:42:42','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(43,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:44:49','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(44,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:45:02','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(45,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:48:05','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(46,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:48:40','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(47,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:53:56','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(48,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 18:56:43','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(49,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 19:00:26','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(50,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 19:00:42','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(51,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 19:17:19','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(52,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 19:18:33','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(53,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 23:18:30','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(54,'4204169340380038\r','BREQ|id=4204169340380038|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 23:18:51','BCNF|id=4204169340380038|status=PASS|model=42041693\n'),(55,'4204169340380038','BREQ|id=420416934038003|process=PIM_S12|station=PIM_S12_AL01','2024-03-13 23:44:39','BCNF|id=420416934038003|status=FAIL|msg=FailedToGetUnitStats\n'),(56,'4205484940111111\r','BREQ|id=4205484940111111|process=PIM_S12|station=PIM_S12_AL01','2024-03-22 01:54:54','BCNF|id=4205484940111111|status=FAIL|msg=RouteCheck\n');
/*!40000 ALTER TABLE `fis_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loggeddata`
--

DROP TABLE IF EXISTS `loggeddata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loggeddata` (
  `runID` int NOT NULL,
  `positions` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`runID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loggeddata`
--

LOCK TABLES `loggeddata` WRITE;
/*!40000 ALTER TABLE `loggeddata` DISABLE KEYS */;
/*!40000 ALTER TABLE `loggeddata` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `models`
--

DROP TABLE IF EXISTS `models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `models` (
  `id` int NOT NULL AUTO_INCREMENT,
  `partNumber` varchar(50) DEFAULT NULL,
  `serial` varchar(50) DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  `description` varchar(300) DEFAULT NULL,
  `screws` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `models`
--

LOCK TABLES `models` WRITE;
/*!40000 ALTER TABLE `models` DISABLE KEYS */;
/*!40000 ALTER TABLE `models` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `models_screw`
--

DROP TABLE IF EXISTS `models_screw`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `models_screw` (
  `id` int NOT NULL AUTO_INCREMENT,
  `partNumber` varchar(50) DEFAULT NULL,
  `serial` varchar(50) DEFAULT NULL,
  `name_model` varchar(50) NOT NULL,
  `description` varchar(300) DEFAULT NULL,
  `quantity_screws` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `models_screw`
--

LOCK TABLES `models_screw` WRITE;
/*!40000 ALTER TABLE `models_screw` DISABLE KEYS */;
INSERT INTO `models_screw` VALUES (1,'modelo1','FUSION\r','modelo1','Model for testing cycle with FIS',3),(2,'modelo2','serial','modelo2','Model for testing program with model selection with all the sensors in false.',2),(43,'400V PIM/250KW','400V PIM/250KW','400V PIM/250KW','Modelo PIM',2);
/*!40000 ALTER TABLE `models_screw` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profile`
--

DROP TABLE IF EXISTS `profile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profile` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profile`
--

LOCK TABLES `profile` WRITE;
/*!40000 ALTER TABLE `profile` DISABLE KEYS */;
INSERT INTO `profile` VALUES (1,'Admin'),(2,'Usuario');
/*!40000 ALTER TABLE `profile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `programs_screws`
--

DROP TABLE IF EXISTS `programs_screws`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `programs_screws` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_model_screw` int NOT NULL,
  `screwing` varchar(4) NOT NULL,
  `rescrewing` varchar(4) NOT NULL,
  `unscrewing` varchar(4) NOT NULL,
  `simulated` varchar(4) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programs_screws`
--

LOCK TABLES `programs_screws` WRITE;
/*!40000 ALTER TABLE `programs_screws` DISABLE KEYS */;
INSERT INTO `programs_screws` VALUES (1,43,'01','01','10','25'),(2,1,'01','01','10','25'),(3,2,'11','15','25','10');
/*!40000 ALTER TABLE `programs_screws` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `runs`
--

DROP TABLE IF EXISTS `runs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `runs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `partNumber` varchar(100) NOT NULL,
  `serial` varchar(120) DEFAULT NULL,
  `serial2` varchar(120) DEFAULT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `result` varchar(20) NOT NULL,
  `Screw1Torque` varchar(20) DEFAULT NULL,
  `Screw1Angle` varchar(20) DEFAULT NULL,
  `Screw2Torque` varchar(20) DEFAULT NULL,
  `Screw2Angle` varchar(20) DEFAULT NULL,
  `Screw3Torque` varchar(20) DEFAULT NULL,
  `Screw3Angle` varchar(20) DEFAULT NULL,
  `Screw4Torque` varchar(20) DEFAULT NULL,
  `Screw4Angle` varchar(20) DEFAULT NULL,
  `Screw5Torque` varchar(20) DEFAULT NULL,
  `Screw5Angle` varchar(20) DEFAULT NULL,
  `ultracappadinspect` varchar(45) DEFAULT NULL,
  `ultracapboardinspect` varchar(45) DEFAULT NULL,
  `insulatorinspect` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fis_id_idx` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=214 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `runs`
--

LOCK TABLES `runs` WRITE;
/*!40000 ALTER TABLE `runs` DISABLE KEYS */;
/*!40000 ALTER TABLE `runs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `screws`
--

DROP TABLE IF EXISTS `screws`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `screws` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_screw` int NOT NULL,
  `encoder1` double NOT NULL,
  `encoder2` double NOT NULL,
  `tolerance` double NOT NULL,
  `id_model_screw` int NOT NULL,
  `text_position_X` int DEFAULT NULL,
  `text_position_Y` int DEFAULT NULL,
  PRIMARY KEY (`id`,`id_screw`,`id_model_screw`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `screws`
--

LOCK TABLES `screws` WRITE;
/*!40000 ALTER TABLE `screws` DISABLE KEYS */;
INSERT INTO `screws` VALUES (1,1,40.4,-9.8,5,43,450,25),(2,2,43.7172,-43.0516,5,43,120,-55);
/*!40000 ALTER TABLE `screws` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `settings`
--

DROP TABLE IF EXISTS `settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settings` (
  `id` int NOT NULL AUTO_INCREMENT,
  `setting` varchar(45) NOT NULL,
  `value_setting` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `settings`
--

LOCK TABLES `settings` WRITE;
/*!40000 ALTER TABLE `settings` DISABLE KEYS */;
/*!40000 ALTER TABLE `settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `settings_by_workstation`
--

DROP TABLE IF EXISTS `settings_by_workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `settings_by_workstation` (
  `id_TypeWorkstation` int NOT NULL,
  `id` int NOT NULL AUTO_INCREMENT,
  `setting` varchar(45) NOT NULL,
  `value_setting` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `settings_by_workstation`
--

LOCK TABLES `settings_by_workstation` WRITE;
/*!40000 ALTER TABLE `settings_by_workstation` DISABLE KEYS */;
INSERT INTO `settings_by_workstation` VALUES (1,46,'TYPE_WORKSTATION','6'),(1,47,'Path_LOG','D:\\Repo\\Logs\\'),(1,48,'GRID_Number_Reg_by_Page','2'),(1,49,'ADU_SERIAL_1','E21531'),(1,50,'ADU_SERIAL_2','E21534'),(1,51,'ADU_SERIAL_3','E21535'),(1,52,'FTP_Camara_Path','C:\\Users\\PC\\Desktop\\FTP-2'),(1,53,'fis_process','CMI'),(1,54,'fis_station','sta2'),(1,55,'last_model','modelo1'),(1,56,'Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX'),(1,57,'Number_Screws_Process','3'),(1,58,'Model_Screw','2'),(2,59,'TYPE_WORKSTATION','6'),(2,60,'Path_LOG','D:\\Repo\\Logs\\'),(2,61,'GRID_Number_Reg_by_Page','2'),(2,62,'ADU_SERIAL_1','E21531'),(2,63,'ADU_SERIAL_2','E21534'),(2,64,'ADU_SERIAL_3','E21535'),(2,65,'Number_Screws_Process','3'),(2,66,'Model_Screw','2'),(3,67,'TYPE_WORKSTATION','6'),(3,68,'Path_LOG','D:\\Repo\\Logs\\'),(3,69,'GRID_Number_Reg_by_Page','2'),(3,70,'ADU_SERIAL_1','E21531'),(3,71,'ADU_SERIAL_2','E21534'),(3,72,'ADU_SERIAL_3','E21535'),(3,73,'Number_Screws_Process','3'),(3,74,'Model_Screw','2'),(6,75,'TYPE_WORKSTATION','6'),(6,76,'Path_LOG','C:\\Repo\\Logs\\'),(6,77,'GRID_Number_Reg_by_Page','10'),(6,78,'ADU_SERIAL_1','E21927'),(6,79,'ADU_SERIAL_2','E21709'),(6,80,'ADU_SERIAL_3','E21880'),(6,81,'FTP_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0'),(6,82,'fis_process','PIM_S12'),(6,83,'fis_station','PIM_S12_AL01'),(6,84,'last_model','400V PIM/250KW'),(6,85,'Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0'),(6,86,'Number_Screws_Process','2'),(6,87,'Model_Screw','43'),(6,89,'EneableFIS','0'),(12,90,'TYPE_WORKSTATION','6'),(12,91,'Path_LOG','C:\\Repo\\Logs\\'),(12,92,'GRID_Number_Reg_by_Page','10'),(12,93,'ADU_SERIAL_1','E21927'),(12,94,'ADU_SERIAL_2','E21709'),(12,95,'ADU_SERIAL_3','E21880'),(12,96,'FTP_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0'),(12,97,'fis_process','PIM_S12'),(12,98,'fis_station','PIM_S12_AL01'),(12,99,'last_model','400V PIM/250KW'),(12,100,'Cognex_Camara_Path','C:\\Users\\MyUser\\Desktop\\COGNEX\\Cognex_0'),(12,101,'Number_Screws_Process','2'),(12,102,'Model_Screw','43'),(12,103,'EneableFIS','0'),(12,105,'ErgoArm_HomeEncoder1','43'),(12,106,'ErgoArm_HomeEncoder2','-42'),(12,107,'ErgoArm_HomeTolerancia','5');
/*!40000 ALTER TABLE `settings_by_workstation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_camera`
--

DROP TABLE IF EXISTS `type_camera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_camera` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_camera`
--

LOCK TABLES `type_camera` WRITE;
/*!40000 ALTER TABLE `type_camera` DISABLE KEYS */;
INSERT INTO `type_camera` VALUES (2,'CognexD2800'),(1,'CognexD900');
/*!40000 ALTER TABLE `type_camera` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_connection`
--

DROP TABLE IF EXISTS `type_connection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_connection` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_connection`
--

LOCK TABLES `type_connection` WRITE;
/*!40000 ALTER TABLE `type_connection` DISABLE KEYS */;
INSERT INTO `type_connection` VALUES (2,'Auxiliary'),(7,'Camara_1'),(8,'Camara_2'),(9,'Camara_3'),(4,'Emergency'),(3,'InputDevices'),(1,'Main'),(5,'Scan_1'),(6,'Scan_2');
/*!40000 ALTER TABLE `type_connection` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_device`
--

DROP TABLE IF EXISTS `type_device`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_device` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_device`
--

LOCK TABLES `type_device` WRITE;
/*!40000 ALTER TABLE `type_device` DISABLE KEYS */;
INSERT INTO `type_device` VALUES (4,'Camara'),(1,'ErgoArm'),(6,'FIS'),(3,'RobotEpson'),(5,'Scanner'),(2,'Screw');
/*!40000 ALTER TABLE `type_device` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_robot`
--

DROP TABLE IF EXISTS `type_robot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_robot` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_robot`
--

LOCK TABLES `type_robot` WRITE;
/*!40000 ALTER TABLE `type_robot` DISABLE KEYS */;
/*!40000 ALTER TABLE `type_robot` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_workstation`
--

DROP TABLE IF EXISTS `type_workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_workstation` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `description` (`description`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_workstation`
--

LOCK TABLES `type_workstation` WRITE;
/*!40000 ALTER TABLE `type_workstation` DISABLE KEYS */;
INSERT INTO `type_workstation` VALUES (6,'WS Automatica Tipo 1'),(7,'WS Automatica Tipo 2'),(8,'WS Automatica Tipo 3'),(9,'WS Automatica Tipo 4'),(10,'WS Automatica Tipo 5'),(11,'WS Automatica Tipo 6'),(1,'WS Manual Tipo 1'),(12,'WS Manual Tipo 12'),(3,'WS Manual Tipo 17A'),(4,'WS Manual Tipo 17B'),(5,'WS Manual Tipo 17C'),(2,'WS Manual Tipo 2');
/*!40000 ALTER TABLE `type_workstation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `lastName` varchar(45) DEFAULT NULL,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(45) DEFAULT NULL,
  `id_profile` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (3,'Andro','Bas','Androide','12345',1),(4,'Emanuel','DeRose','Reader','56789',2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `workstation`
--

DROP TABLE IF EXISTS `workstation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `workstation` (
  `id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workstation`
--

LOCK TABLES `workstation` WRITE;
/*!40000 ALTER TABLE `workstation` DISABLE KEYS */;
INSERT INTO `workstation` VALUES (12);
/*!40000 ALTER TABLE `workstation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'bw_database'
--
/*!50003 DROP FUNCTION IF EXISTS `CheckPassword` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `CheckPassword`(username_p VARCHAR(8), password_p VARCHAR(20)) RETURNS tinyint(1)
    READS SQL DATA
BEGIN
    RETURN EXISTS (SELECT username FROM df4187_sta9.users WHERE username = username_p AND password = password_p);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `INS_UPD_MODELS_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `INS_UPD_MODELS_SCREWS`(
	  IN p_id INT,
	  IN p_partNumber VARCHAR(50),
	  IN p_serial VARCHAR(50),
	  IN p_name_model VARCHAR(50),
	  IN p_description VARCHAR(300),
	  IN p_quantity_screws INT(11)
)
BEGIN
	DECLARE model_exist INT;
	SELECT COUNT(*) INTO model_exist FROM models_screw WHERE id = p_id;
	
    IF model_exist > 0 THEN
		UPDATE models_screw 
        SET partNumber = p_partNumber,
			serial = p_serial,
			name_model = p_name_model,
			description = p_description,
			quantity_screws = p_quantity_screws
        WHERE id = p_id;		
    ELSE
        INSERT INTO models_screw (partNumber,serial,name_model,description,quantity_screws) 
		VALUES (p_partNumber,
				p_serial,
				p_name_model,
				p_description,
				p_quantity_screws);
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_MODELS_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_MODELS_SCREWS`(
    IN p_id INT
)
BEGIN
    DELETE FROM models_screw WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_RUNS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_RUNS`(
    IN p_id INT
)
BEGIN
    DELETE FROM runs WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_SCREWS`(
    IN p_id INT
)
BEGIN
    DELETE FROM screws WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_SETTINGS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_SETTINGS`(
    IN p_id INT
)
BEGIN
    DELETE FROM settings WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_SETTINGS_BY_WS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_SETTINGS_BY_WS`(IN p_id_TypeWorkstation INT, IN p_id INT)
BEGIN
    DELETE FROM settings_by_Workstation WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_DEL_USERS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_DEL_USERS`(
    IN p_id INT
)
BEGIN
    DELETE FROM users WHERE id = p_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_COMMAND_CAMERAS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_COMMAND_CAMERAS`()
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
        path_image_show_errors,
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
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_connections` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_connections`()
BEGIN

SELECT 	connections.id,
		connections.id_type_device,
        type_device.description as des_type_device,
        ip,
        port_robot,
        id_type_connection,
        type_connection.description as des_type_connection
FROM connections
LEFT JOIN type_device ON connections.id_type_device = type_device.id 
LEFT JOIN type_connection ON connections.id_type_connection = type_connection.id;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_CONNECTIONS_BY_WS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_CONNECTIONS_BY_WS`(IN p_id_TypeWorkstation INT)
BEGIN

SELECT 	id_TypeWorkstation,
		connections_by_workstation.id,
		connections_by_workstation.id_type_device,
        type_device.description as des_type_device,
        ip,
        port_robot,
        id_type_connection,
        type_connection.description as des_type_connection
FROM connections_by_workstation
LEFT JOIN type_device ON connections_by_workstation.id_type_device = type_device.id 
LEFT JOIN type_connection ON connections_by_workstation.id_type_connection = type_connection.id
WHERE id_TypeWorkstation = p_id_TypeWorkstation;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_CREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_CREWS`(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("select id, tornillo, Encoder1, Encoder2, Tolerancia, id_partNumber, id_Name from screws as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_MODELS_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_MODELS_SCREWS`()
BEGIN
SELECT 	 	id,
			partNumber,
			serial,
			name_model,
			description,
			quantity_screws  
FROM models_screw;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_MODELS_SCREWS_PAG` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_MODELS_SCREWS_PAG`(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	 	id,
			partNumber,
			serial,
			name_model,
			description,
			quantity_screws  
FROM models_screw
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_PROFILES` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_PROFILES`()
BEGIN
SELECT id,
    description
FROM profile;

 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_ROBOT_CONNECTIONS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_ROBOT_CONNECTIONS`()
BEGIN

SELECT 	robot_connections.id,
		robot_connections.id_type_device,
        type_device.description as des_type_device,
        ip,
        port_robot,
        id_type_connection,
        type_connection.description as des_type_connection
FROM robot_connections
LEFT JOIN type_device ON robot_connections.id_type_device = type_device.id 
LEFT JOIN type_connection ON robot_connections.id_type_connection = type_connection.id;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_RUNS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_RUNS`(IN page_p INT, IN size_p INT)
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
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SCREWS`()
BEGIN

SELECT 	 	screws.id,
			id_screw,
			encoder1,
			encoder2,
			tolerance,
			id_model_screw,
            models_screw.name_model,
            models_screw.quantity_screws,
            text_position_X,
            text_position_Y
FROM screws
LEFT JOIN models_screw ON screws.id_model_screw = models_screw.id
ORDER BY id_model_screw asc, id_screw  asc;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SCREWS_PAG` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SCREWS_PAG`(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	id,
		id_screw,
		encoder1,
		encoder2,
		tolerance,
		id_model_screw,
        text_position_X,
        text_position_Y
FROM screws
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SCREW_PROGRAM` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SCREW_PROGRAM`(
		IN p_rework BOOL,
		IN p_debug BOOL,
		IN p_removeScrew BOOL		
)
BEGIN
	DECLARE model_screw_id INT;
    DECLARE ProgramaAtornillador VARCHAR(255);
      
    SELECT  models_screw.id INTO model_screw_id
	      /*models_screw.name_model as models_screw_name_model,
			models_screw.id as models_screw_id INTO model_screw_id*/
    FROM settings_by_workstation 
    LEFT JOIN models_screw ON models_screw.id = settings_by_workstation.value_setting
    WHERE id_TypeWorkstation = (SELECT id FROM workstation) AND setting = "Model_Screw";
      
    IF model_screw_id = 1 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    ELSEIF model_screw_id = 3 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '11';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '15';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    ELSEIF model_screw_id = 43 THEN
        IF NOT p_rework AND NOT p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_rework AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '01';
        ELSEIF p_debug AND NOT p_removeScrew THEN
            SET ProgramaAtornillador = '25';
        ELSEIF p_removeScrew THEN
            SET ProgramaAtornillador = '10';
        END IF;
    END IF;
    
    SELECT ProgramaAtornillador;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SCREW_PROGRAMS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SCREW_PROGRAMS`()
BEGIN
	SELECT 
		programs_screws.id,
		id_model_screw,
		screwing,
		rescrewing,
		unscrewing,
		simulated 
    FROM settings_by_workstation
    LEFT JOIN programs_screws  ON programs_screws.id_model_screw = settings_by_workstation.value_setting
    WHERE id_TypeWorkstation = (SELECT id FROM workstation) AND setting = "Model_Screw";
      
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SETTINGS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SETTINGS`()
BEGIN

SELECT 	id,
		setting, 
        value_setting
FROM settings;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SETTINGS_BY_WS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SETTINGS_BY_WS`(IN p_id_TypeWorkstation INT)
BEGIN

SELECT 	id_TypeWorkstation,
		id,
		setting, 
        value_setting
FROM settings_by_Workstation WHERE id_TypeWorkstation = p_id_TypeWorkstation;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SETTINGS_PAG` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SETTINGS_PAG`(IN page_p INT, IN size_p INT)
BEGIN
set @page = page_p;
set @_size = size_p;

set @qry_string = concat("
SELECT 	id,
		setting, 
        value_setting
FROM settings
as s limit ",(@page - 1) * @_size,",",@_size);
prepare qry from @qry_string;
 execute qry;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_SETTINGS_PAG_BY_WS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_SETTINGS_PAG_BY_WS`(IN p_id_TypeWorkstation INT, IN page_p INT, IN size_p INT)
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_TOTALREG_BY_TABLES` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_TOTALREG_BY_TABLES`()
BEGIN
DECLARE num_reg_by_page INT;
DROP TEMPORARY TABLE IF EXISTS totalreg_by_tables_temp;
CREATE TEMPORARY TABLE totalreg_by_tables_temp (    
    name_table VARCHAR(50),
    total_reg INT    
);

DROP TEMPORARY TABLE IF EXISTS totalreg_by_tables_temp2;
CREATE TEMPORARY TABLE totalreg_by_tables_temp2 (    
    name_table VARCHAR(50),
    total_reg INT,
    pages INT
);
SELECT CAST( value_setting as SIGNED) INTO num_reg_by_page FROM settings_by_Workstation WHERE setting = 'GRID_Number_Reg_by_Page' AND id_TypeWorkstation = (SELECT id FROM workstation);

INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'connections',COUNT(*) as 'TotalReg'  FROM connections_by_workstation WHERE id_TypeWorkstation = (SELECT id FROM workstation);
/*INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'connections',COUNT(*) as 'TotalReg'  FROM connections;*/
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'fis_history',COUNT(*) as 'TotalReg'  FROM fis_history;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'models_screw',COUNT(*) as 'TotalReg' FROM models_screw;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'screws',COUNT(*) as 'TotalReg'  FROM screws;
/*INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'settings',COUNT(*) as 'TotalReg'  FROM settings;*/
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'settings',COUNT(*) as 'TotalReg'   FROM settings_by_Workstation WHERE id_TypeWorkstation = (SELECT id FROM workstation);
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'type_connection',COUNT(*) as 'TotalReg'  FROM type_connection;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'type_device',COUNT(*) as 'TotalReg'  FROM type_device;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'users',COUNT(*) as 'TotalReg'  FROM users;
INSERT INTO totalreg_by_tables_temp (name_table,total_reg) SELECT 'runs',COUNT(*) as 'TotalReg'  FROM runs;

INSERT INTO totalreg_by_tables_temp2 (name_table,total_reg,pages)
SELECT name_table, total_reg,CAST(CEIL( total_reg / num_reg_by_page) as SIGNED) as pages FROM totalreg_by_tables_temp;

SELECT name_table, total_reg,pages FROM totalreg_by_tables_temp2;
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_TYPE_WORKSTATION` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_TYPE_WORKSTATION`()
BEGIN

SELECT 	id,
		description
FROM type_workstation;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_TYPE_WORKSTATION_BY_ID` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_TYPE_WORKSTATION_BY_ID`(IN p_id INT)
BEGIN	
SELECT 	id,
		description
FROM type_workstation
WHERE id = p_id;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_USERS_PAG` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_USERS_PAG`(IN page_p INT, IN size_p INT)
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_GET_WORKSTATION` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_GET_WORKSTATION`()
BEGIN

SELECT 	
		workstation.id,
        description
FROM workstation
LEFT JOIN type_workstation ON workstation.id = type_workstation.id;
 
 END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_MODELS_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_MODELS_SCREWS`(
	  IN p_id INT,
	  IN p_partNumber VARCHAR(50),
	  IN p_serial VARCHAR(50),
	  IN p_name_model VARCHAR(50),
	  IN p_description VARCHAR(300),
	  IN p_quantity_screws INT(11)
)
BEGIN
	DECLARE model_exist INT;
	SELECT COUNT(*) INTO model_exist FROM models_screw WHERE id = p_id;
	
    IF model_exist > 0 THEN
		UPDATE models_screw 
        SET partNumber = p_partNumber,
			serial = p_serial,
			name_model = p_name_model,
			description = p_description,
			quantity_screws = p_quantity_screws
        WHERE id = p_id;		
    ELSE
        INSERT INTO models_screw (partNumber,serial,name_model,description,quantity_screws) 
		VALUES (p_partNumber,
				p_serial,
				p_name_model,
				p_description,
				p_quantity_screws);
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_RUNS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_RUNS`(
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_SCREWS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_SCREWS`(
		IN p_id INT,
		IN p_id_screw INT,
		IN p_encoder1 DOUBLE,
		IN p_encoder2 DOUBLE,
		IN p_tolerance DOUBLE,
		IN p_id_model_screw INT,
        IN p_text_position_X INT,
		IN p_text_position_Y INT
)
BEGIN
	DECLARE screws_exist INT;
	SELECT COUNT(*) INTO screws_exist FROM screws WHERE id = p_id;
	
    IF screws_exist > 0 THEN
		UPDATE screws 
        SET id_screw = p_id_screw ,
			encoder1 = p_encoder1 ,
			encoder2 = p_encoder2 ,
			tolerance = p_tolerance ,
			id_model_screw= p_id_model_screw,
            text_position_X = p_text_position_X,
            text_position_Y = p_text_position_Y
        WHERE id = p_id;		
    ELSE
        INSERT INTO screws (id,id_screw,encoder1,encoder2,tolerance,id_model_screw,text_position_X,text_position_Y	) 
		VALUES (p_id ,
				p_id_screw ,			
				p_encoder1 ,
				p_encoder2 ,
				p_tolerance ,
				p_id_model_screw,
                p_text_position_X,
                p_text_position_Y);
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_SETTINGS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_SETTINGS`(
	  IN p_id INT,
	  IN p_setting VARCHAR(50),
	  IN p_value_setting VARCHAR(50)
)
BEGIN
	DECLARE settings_exist INT;
	SELECT COUNT(*) INTO settings_exist FROM settings WHERE id = p_id;
	
    IF settings_exist > 0 THEN
		UPDATE settings 
        SET setting = p_setting,
			value_setting = p_value_setting
        WHERE id = p_id;		
    ELSE
        INSERT INTO settings (setting,value_setting) 
		VALUES (p_setting,
				p_value_setting);
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_SETTINGS_BY_WS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_SETTINGS_BY_WS`(
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_INS_UPD_USERS` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_INS_UPD_USERS`(
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_UPD_WORKSTATION` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_UPD_WORKSTATION`(
	  IN p_id_TypeWorkstation INT	  
)
BEGIN	
SET SQL_SAFE_UPDATES = 0;
   	UPDATE workstation
        SET id = p_id_TypeWorkstation;        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SP_VALIDATE_USER` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `SP_VALIDATE_USER`(
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-04-30 17:35:03
