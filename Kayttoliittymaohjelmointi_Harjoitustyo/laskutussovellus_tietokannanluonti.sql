-- Käyttöliittymäohjelmointi-harjoitustyö Tietokannan luominen Anna-Sofia Hiltunen 2205419 16.3.2023
-- luodaan tietokanta ja taulut
DROP DATABASE IF EXISTS laskutus;
CREATE DATABASE laskutus;
USE laskutus;

CREATE TABLE Tuotteet
(
  TuoteID INT(20) NOT NULL AUTO_INCREMENT,
  Tuotenimi VARCHAR(50) NOT NULL,
  Yksikko VARCHAR(15) NOT NULL,
  Yksikkohinta FLOAT NOT NULL,
  PRIMARY KEY (TuoteID)
);

CREATE TABLE Asiakkaat
(
  AsiakasID INT(20) NOT NULL AUTO_INCREMENT,
  Nimi VARCHAR(100) NOT NULL,
  Katuosoite VARCHAR(100) NOT NULL,
  Postinumero VARCHAR(10) NOT NULL,
  Kaupunki VARCHAR(50) NOT NULL,
  PRIMARY KEY (AsiakasID)
);

CREATE TABLE Laskut
(
  LaskuID INT(20) NOT NULL AUTO_INCREMENT,
  AsiakasID INT(20) NOT NULL,
  Paivays DATE NOT NULL,
  Erapaiva DATE NOT NULL,
  Lisatiedot VARCHAR(200) NULL,
  PRIMARY KEY (LaskuID),
  FOREIGN KEY (AsiakasID) REFERENCES Asiakkaat(AsiakasID)
);

CREATE TABLE laskurivit -- tuotteet voi muuttua tai poistua niin samat tiedot tallennetaan joka laskuriville mitä tuotteet taulussa
(
  LaskuID INT(20) NOT NULL,
  LaskuriviID INT(50) NOT NULL AUTO_INCREMENT,
  Tuotenimi VARCHAR(50) NOT NULL,
  TuotteidenMaara FLOAT(8,1) NOT NULL,
  Yksikko VARCHAR(15) NOT NULL,
  Yksikkohinta FLOAT NOT NULL,
  PRIMARY KEY (LaskuriviID),
  FOREIGN KEY (LaskuID) REFERENCES Laskut(LaskuID)
);

-- lisätään tauluihin tietoja
INSERT INTO tuotteet(Tuotenimi,Yksikko,Yksikkohinta)
	VALUES('Parketti','m2',89.50),
	('Alusmateriaali','m2',2.38),
	('Liima','l',17.63),
	('Maali','l',13.69),
	('Maalarinteippi','m',0.11),
	('Kalusteovi','kpl',23.5),
	('Kalustevedin','kpl',3.98),
	('Sarana','kpl',3.99);

INSERT INTO asiakkaat(Nimi,Katuosoite,Postinumero,Kaupunki)
	VALUES('Yritys Oy','Esimerkkitie 1','80100','Joensuu'),
	('Erkki Esimerkki','Esimerkkitie 2','80100','Joensuu'),
	('Osakeyhtio Oy','Esimerkkitie 3','80100','Joensuu');
	
INSERT INTO laskut(Erapaiva,Lisatiedot,Paivays,AsiakasID)
	VALUES("2023-03-29",'Lattiaremontti',"2023-02-27",1),
	("2023-03-30",'Seinien maalaus',"2023-02-28",1),
	("2023-04-04",'Keittioremontti, kaappien ovien vaihto',"2023-03-05",2),
	("2023-04-07",'Lattiaremontti',"2023-03-08",3),
	("2023-04-09",'Seinien maalaus',"2023-03-10",3),
	("2023-04-16",NULL,"2023-03-17",1);
	
INSERT INTO laskurivit(LaskuID,Tuotenimi,TuotteidenMaara,Yksikko,Yksikkohinta)
	VALUES(1,'Tyo',10,'t',60),
	(1,'Parketti',46,'m2',89.5),
	(1,'Alusmateriaali',46,'m2',2.38),
	(1,'Liima',10,'l',17.63),
	(2,'Tyo',4,'t',40),
	(2,'Maali',16,'l',13.69),
	(2,'Maalarinteippi',75,'m',0.11),
	(3,'Tyo',3.5,'t',60),
	(3,'Kalusteovi',8,'kpl',23.5),
	(3,'Kalustevedin',8,'kpl',3.98),
	(3,'Sarana',16,'kpl',3.99),
	(4,'Tyo',9,'t',60),
	(4,'Parketti',42,'m2',89.5),
	(4,'Alusmateriaali',42,'m2',2.38),
	(4,'Liima',9,'l',17.63),
	(5,'Tyo',5,'t',40),
	(5,'Maali',22,'l',13.69),
	(5,'Maalarinteippi',84.5,'m',0.11),
	(6,'Maali',30,'l',13.69),
	(6,'Maalarinteippi',160,'m',0.11);