-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema frizerski_salon
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema frizerski_salon
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `frizerski_salon` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci ;
USE `frizerski_salon` ;

-- -----------------------------------------------------
-- Table `frizerski_salon`.`kategorije_usluga`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`kategorije_usluga` (
  `kategorija_id` INT NOT NULL AUTO_INCREMENT,
  `naziv` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`kategorija_id`),
  UNIQUE INDEX `naziv` (`naziv` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`usluge`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`usluge` (
  `usluga_id` INT NOT NULL AUTO_INCREMENT,
  `sifra` VARCHAR(20) NOT NULL,
  `naziv` VARCHAR(100) NOT NULL,
  `opis` TEXT NULL DEFAULT NULL,
  `cena` DECIMAL(10,2) NOT NULL,
  `trajanje_minuta` INT NOT NULL,
  `kategorija_id` INT NOT NULL,
  `aktivan` TINYINT(1) NULL DEFAULT '1',
  PRIMARY KEY (`usluga_id`),
  UNIQUE INDEX `sifra` (`sifra` ASC) VISIBLE,
  INDEX `kategorija_id` (`kategorija_id` ASC) VISIBLE,
  CONSTRAINT `usluge_ibfk_1`
    FOREIGN KEY (`kategorija_id`)
    REFERENCES `frizerski_salon`.`kategorije_usluga` (`kategorija_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 14
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`korisnici`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`korisnici` (
  `korisnik_id` INT NOT NULL AUTO_INCREMENT,
  `korisnicko_ime` VARCHAR(50) NOT NULL,
  `ime` VARCHAR(50) NOT NULL,
  `prezime` VARCHAR(50) NOT NULL,
  `lozinka_hash` VARCHAR(255) NOT NULL,
  `uloga` ENUM('admin', 'radnik') NOT NULL,
  `preferirana_tema` VARCHAR(30) NULL DEFAULT 'svetla',
  `jezik_koda` VARCHAR(10) NULL DEFAULT 'sr',
  PRIMARY KEY (`korisnik_id`),
  UNIQUE INDEX `korisnicko_ime` (`korisnicko_ime` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`istorija_cena`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`istorija_cena` (
  `istorija_id` INT NOT NULL AUTO_INCREMENT,
  `usluga_id` INT NOT NULL,
  `stara_cena` DECIMAL(10,2) NULL DEFAULT NULL,
  `nova_cena` DECIMAL(10,2) NULL DEFAULT NULL,
  `promenio_korisnik_id` INT NOT NULL,
  `vreme_promene` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`istorija_id`),
  INDEX `usluga_id` (`usluga_id` ASC) VISIBLE,
  INDEX `promenio_korisnik_id` (`promenio_korisnik_id` ASC) VISIBLE,
  CONSTRAINT `istorija_cena_ibfk_1`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `frizerski_salon`.`usluge` (`usluga_id`),
  CONSTRAINT `istorija_cena_ibfk_2`
    FOREIGN KEY (`promenio_korisnik_id`)
    REFERENCES `frizerski_salon`.`korisnici` (`korisnik_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`klijenti`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`klijenti` (
  `klijent_id` INT NOT NULL AUTO_INCREMENT,
  `puno_ime` VARCHAR(100) NOT NULL,
  `telefon` VARCHAR(20) NULL DEFAULT NULL,
  `email` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`klijent_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 17
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`log_aktivnosti`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`log_aktivnosti` (
  `log_id` INT NOT NULL AUTO_INCREMENT,
  `korisnik_id` INT NOT NULL,
  `akcija` VARCHAR(100) NOT NULL,
  `opis` TEXT NULL DEFAULT NULL,
  `vreme` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`log_id`),
  INDEX `korisnik_id` (`korisnik_id` ASC) VISIBLE,
  CONSTRAINT `log_aktivnosti_ibfk_1`
    FOREIGN KEY (`korisnik_id`)
    REFERENCES `frizerski_salon`.`korisnici` (`korisnik_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`rezervacije`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`rezervacije` (
  `rezervacija_id` INT NOT NULL AUTO_INCREMENT,
  `klijent_id` INT NULL DEFAULT NULL,
  `dodeljen_korisnik_id` INT NOT NULL,
  `datum_vreme` DATETIME NOT NULL,
  `status` ENUM('zakazano', 'u_toku', 'zavrseno', 'otkazano') NULL DEFAULT 'zakazano',
  `kreirano` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`rezervacija_id`),
  INDEX `klijent_id` (`klijent_id` ASC) VISIBLE,
  INDEX `dodeljen_korisnik_id` (`dodeljen_korisnik_id` ASC) VISIBLE,
  CONSTRAINT `rezervacije_ibfk_1`
    FOREIGN KEY (`klijent_id`)
    REFERENCES `frizerski_salon`.`klijenti` (`klijent_id`),
  CONSTRAINT `rezervacije_ibfk_2`
    FOREIGN KEY (`dodeljen_korisnik_id`)
    REFERENCES `frizerski_salon`.`korisnici` (`korisnik_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 14
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`racuni`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`racuni` (
  `racun_id` INT NOT NULL AUTO_INCREMENT,
  `broj_racuna` VARCHAR(20) NOT NULL,
  `rezervacija_id` INT NOT NULL,
  `datum_izdavanja` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `ukupan_iznos` DECIMAL(10,2) NOT NULL,
  `izdao_korisnik_id` INT NULL DEFAULT '0',
  `klijent_id` INT NULL DEFAULT NULL,
  PRIMARY KEY (`racun_id`),
  UNIQUE INDEX `broj_racuna` (`broj_racuna` ASC) VISIBLE,
  INDEX `rezervacija_id` (`rezervacija_id` ASC) VISIBLE,
  INDEX `izdao_korisnik_id` (`izdao_korisnik_id` ASC) VISIBLE,
  INDEX `klijent_id` (`klijent_id` ASC) VISIBLE,
  CONSTRAINT `racuni_ibfk_1`
    FOREIGN KEY (`rezervacija_id`)
    REFERENCES `frizerski_salon`.`rezervacije` (`rezervacija_id`),
  CONSTRAINT `racuni_ibfk_2`
    FOREIGN KEY (`izdao_korisnik_id`)
    REFERENCES `frizerski_salon`.`korisnici` (`korisnik_id`),
  CONSTRAINT `racuni_ibfk_3`
    FOREIGN KEY (`klijent_id`)
    REFERENCES `frizerski_salon`.`klijenti` (`klijent_id`))
ENGINE = InnoDB
AUTO_INCREMENT = 20
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`sistemske_postavke`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`sistemske_postavke` (
  `kljuc_postavke` VARCHAR(50) NOT NULL,
  `vrednost_postavke` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`kljuc_postavke`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`stavke_racuna`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`stavke_racuna` (
  `stavka_id` INT NOT NULL AUTO_INCREMENT,
  `racun_id` INT NOT NULL,
  `usluga_id` INT NOT NULL,
  `naziv_usluge` VARCHAR(100) NOT NULL,
  `cena_usluge` DECIMAL(10,2) NOT NULL,
  `kolicina` INT NULL DEFAULT '1',
  PRIMARY KEY (`stavka_id`),
  INDEX `racun_id` (`racun_id` ASC) VISIBLE,
  INDEX `usluga_id` (`usluga_id` ASC) VISIBLE,
  CONSTRAINT `stavke_racuna_ibfk_1`
    FOREIGN KEY (`racun_id`)
    REFERENCES `frizerski_salon`.`racuni` (`racun_id`)
    ON DELETE CASCADE,
  CONSTRAINT `stavke_racuna_ibfk_2`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `frizerski_salon`.`usluge` (`usluga_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


-- -----------------------------------------------------
-- Table `frizerski_salon`.`usluge_rezervacije`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `frizerski_salon`.`usluge_rezervacije` (
  `rezervacija_id` INT NOT NULL,
  `usluga_id` INT NOT NULL,
  PRIMARY KEY (`rezervacija_id`, `usluga_id`),
  INDEX `usluga_id` (`usluga_id` ASC) VISIBLE,
  CONSTRAINT `usluge_rezervacije_ibfk_1`
    FOREIGN KEY (`rezervacija_id`)
    REFERENCES `frizerski_salon`.`rezervacije` (`rezervacija_id`)
    ON DELETE CASCADE,
  CONSTRAINT `usluge_rezervacije_ibfk_2`
    FOREIGN KEY (`usluga_id`)
    REFERENCES `frizerski_salon`.`usluge` (`usluga_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
