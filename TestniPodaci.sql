-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema frizerski_salon
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `frizerski_salon` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci ;
USE `frizerski_salon` ;

-- -----------------------------------------------------
-- Table `kategorije_usluga`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `kategorije_usluga` (
  `kategorija_id` INT NOT NULL AUTO_INCREMENT,
  `naziv` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`kategorija_id`),
  UNIQUE INDEX `naziv` (`naziv` ASC)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `usluge`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `usluge` (
  `usluga_id` INT NOT NULL AUTO_INCREMENT,
  `sifra` VARCHAR(20) NOT NULL,
  `naziv` VARCHAR(100) NOT NULL,
  `opis` TEXT NULL DEFAULT NULL,
  `cena` DECIMAL(10,2) NOT NULL,
  `trajanje_minuta` INT NOT NULL,
  `kategorija_id` INT NOT NULL,
  `aktivan` TINYINT(1) NULL DEFAULT '1',
  PRIMARY KEY (`usluga_id`),
  UNIQUE INDEX `sifra` (`sifra` ASC),
  INDEX `kategorija_id` (`kategorija_id` ASC),
  CONSTRAINT `usluge_ibfk_1`
    FOREIGN KEY (`kategorija_id`)
    REFERENCES `kategorije_usluga` (`kategorija_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `korisnici`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `korisnici` (
  `korisnik_id` INT NOT NULL AUTO_INCREMENT,
  `korisnicko_ime` VARCHAR(50) NOT NULL,
  `ime` VARCHAR(50) NOT NULL,
  `prezime` VARCHAR(50) NOT NULL,
  `lozinka_hash` VARCHAR(255) NOT NULL,
  `uloga` ENUM('admin', 'radnik') NOT NULL,
  `preferirana_tema` VARCHAR(30) NULL DEFAULT 'svetla',
  `jezik_koda` VARCHAR(10) NULL DEFAULT 'sr',
  PRIMARY KEY (`korisnik_id`),
  UNIQUE INDEX `korisnicko_ime` (`korisnicko_ime` ASC)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `istorija_cena`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `istorija_cena` (
  `istorija_id` INT NOT NULL AUTO_INCREMENT,
  `usluga_id` INT NOT NULL,
  `stara_cena` DECIMAL(10,2) NULL DEFAULT NULL,
  `nova_cena` DECIMAL(10,2) NULL DEFAULT NULL,
  `promenio_korisnik_id` INT NOT NULL,
  `vreme_promene` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`istorija_id`),
  INDEX `usluga_id` (`usluga_id` ASC),
  INDEX `promenio_korisnik_id` (`promenio_korisnik_id` ASC),
  CONSTRAINT `istorija_cena_ibfk_1`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `usluge` (`usluga_id`),
  CONSTRAINT `istorija_cena_ibfk_2`
    FOREIGN KEY (`promenio_korisnik_id`)
    REFERENCES `korisnici` (`korisnik_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `klijenti`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `klijenti` (
  `klijent_id` INT NOT NULL AUTO_INCREMENT,
  `puno_ime` VARCHAR(100) NOT NULL,
  `telefon` VARCHAR(20) NULL DEFAULT NULL,
  `email` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`klijent_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `log_aktivnosti`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `log_aktivnosti` (
  `log_id` INT NOT NULL AUTO_INCREMENT,
  `korisnik_id` INT NOT NULL,
  `akcija` VARCHAR(100) NOT NULL,
  `opis` TEXT NULL DEFAULT NULL,
  `vreme` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`log_id`),
  INDEX `korisnik_id` (`korisnik_id` ASC),
  CONSTRAINT `log_aktivnosti_ibfk_1`
    FOREIGN KEY (`korisnik_id`)
    REFERENCES `korisnici` (`korisnik_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `rezervacije`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `rezervacije` (
  `rezervacija_id` INT NOT NULL AUTO_INCREMENT,
  `klijent_id` INT NULL DEFAULT NULL,
  `dodeljen_korisnik_id` INT NOT NULL,
  `datum_vreme` DATETIME NOT NULL,
  `status` ENUM('zakazano', 'u_toku', 'zavrseno', 'otkazano') NULL DEFAULT 'zakazano',
  `kreirano` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`rezervacija_id`),
  INDEX `klijent_id` (`klijent_id` ASC),
  INDEX `dodeljen_korisnik_id` (`dodeljen_korisnik_id` ASC),
  CONSTRAINT `rezervacije_ibfk_1`
    FOREIGN KEY (`klijent_id`)
    REFERENCES `klijenti` (`klijent_id`),
  CONSTRAINT `rezervacije_ibfk_2`
    FOREIGN KEY (`dodeljen_korisnik_id`)
    REFERENCES `korisnici` (`korisnik_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `racuni`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `racuni` (
  `racun_id` INT NOT NULL AUTO_INCREMENT,
  `broj_racuna` VARCHAR(20) NOT NULL,
  `rezervacija_id` INT NOT NULL,
  `datum_izdavanja` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `ukupan_iznos` DECIMAL(10,2) NOT NULL,
  `izdao_korisnik_id` INT NULL DEFAULT '0',
  `klijent_id` INT NULL DEFAULT NULL,
  PRIMARY KEY (`racun_id`),
  UNIQUE INDEX `broj_racuna` (`broj_racuna` ASC),
  INDEX `rezervacija_id` (`rezervacija_id` ASC),
  INDEX `izdao_korisnik_id` (`izdao_korisnik_id` ASC),
  INDEX `klijent_id` (`klijent_id` ASC),
  CONSTRAINT `racuni_ibfk_1`
    FOREIGN KEY (`rezervacija_id`)
    REFERENCES `rezervacije` (`rezervacija_id`),
  CONSTRAINT `racuni_ibfk_2`
    FOREIGN KEY (`izdao_korisnik_id`)
    REFERENCES `korisnici` (`korisnik_id`),
  CONSTRAINT `racuni_ibfk_3`
    FOREIGN KEY (`klijent_id`)
    REFERENCES `klijenti` (`klijent_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `sistemske_postavke`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sistemske_postavke` (
  `kljuc_postavke` VARCHAR(50) NOT NULL,
  `vrednost_postavke` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`kljuc_postavke`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `stavke_racuna`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stavke_racuna` (
  `stavka_id` INT NOT NULL AUTO_INCREMENT,
  `racun_id` INT NOT NULL,
  `usluga_id` INT NOT NULL,
  `naziv_usluge` VARCHAR(100) NOT NULL,
  `cena_usluge` DECIMAL(10,2) NOT NULL,
  `kolicina` INT NULL DEFAULT '1',
  PRIMARY KEY (`stavka_id`),
  INDEX `racun_id` (`racun_id` ASC),
  INDEX `usluga_id` (`usluga_id` ASC),
  CONSTRAINT `stavke_racuna_ibfk_1`
    FOREIGN KEY (`racun_id`)
    REFERENCES `racuni` (`racun_id`)
    ON DELETE CASCADE,
  CONSTRAINT `stavke_racuna_ibfk_2`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `usluge` (`usluga_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

-- -----------------------------------------------------
-- Table `usluge_rezervacije`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `usluge_rezervacije` (
  `rezervacija_id` INT NOT NULL,
  `usluga_id` INT NOT NULL,
  PRIMARY KEY (`rezervacija_id`, `usluga_id`),
  INDEX `usluga_id` (`usluga_id` ASC),
  CONSTRAINT `usluge_rezervacije_ibfk_1`
    FOREIGN KEY (`rezervacija_id`)
    REFERENCES `rezervacije` (`rezervacija_id`)
    ON DELETE CASCADE,
  CONSTRAINT `usluge_rezervacije_ibfk_2`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `usluge` (`usluga_id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;

---------------------------------------------------------
-- TEST DATA (INSERT INTO)
---------------------------------------------------------

INSERT INTO kategorije_usluga (naziv) VALUES
('Šišanje'),
('Bojanje'),
('Njega kose');

INSERT INTO usluge (sifra, naziv, opis, cena, trajanje_minuta, kategorija_id, aktivan) VALUES
('S001', 'Muško šišanje', 'Klasično muško šišanje', 10.00, 20, 1, 1),
('S002', 'Žensko šišanje', 'Šišanje ženske kose', 15.00, 30, 1, 1),
('S003', 'Farbanje kratke kose', 'Osnovno farbanje', 25.00, 60, 2, 1),
('S004', 'Farbanje duge kose', 'Farbanje i tretiranje duge kose', 40.00, 90, 2, 1),
('S005', 'Maska za kosu', 'Hranjiva maska', 8.00, 15, 3, 1);

INSERT INTO korisnici (korisnicko_ime, ime, prezime, lozinka_hash, uloga)
VALUES
('admin', 'Ivana', 'Admin', 'admin123', 'admin'),
('marija', 'Marija', 'Kovač', 'lozinka1', 'radnik'),
('ana', 'Ana', 'Petrović', 'lozinka2', 'radnik');

INSERT INTO klijenti (puno_ime, telefon, email) VALUES
('Jelena Marković', '066123456', 'jelena@gmail.com'),
('Petar Savić', '065987654', 'petar@gmail.com'),
('Maja Ilić', '060112233', 'maja@gmail.com');

INSERT INTO rezervacije (klijent_id, dodeljen_korisnik_id, datum_vreme, status) VALUES
(1, 2, '2025-01-10 10:00:00', 'zakazano'),
(2, 3, '2025-01-10 11:00:00', 'u_toku'),
(3, 2, '2025-01-11 12:00:00', 'zavrseno');

INSERT INTO usluge_rezervacije (rezervacija_id, usluga_id) VALUES
(1, 1),
(2, 3),
(2, 5),
(3, 2);

INSERT INTO racuni (broj_racuna, rezervacija_id, ukupan_iznos, izdao_korisnik_id, klijent_id)
VALUES
('R-001', 3, 15.00, 2, 3);

INSERT INTO stavke_racuna (racun_id, usluga_id, naziv_usluge, cena_usluge, kolicina) VALUES
(1, 2, 'Žensko šišanje', 15.00, 1);

INSERT INTO istorija_cena (usluga_id, stara_cena, nova_cena, promenio_korisnik_id)
VALUES
(1, 9.00, 10.00, 1);

INSERT INTO log_aktivnosti (korisnik_id, akcija, opis) VALUES
(1, 'login', 'Administrator se prijavio'),
(2, 'kreiranje rezervacije', 'Napravio rezervaciju za Jelenu Marković');

INSERT INTO sistemske_postavke (kljuc_postavke, vrednost_postavke) VALUES
('tema', 'svetla'),
('jezik', 'sr');

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
