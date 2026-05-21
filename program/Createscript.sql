PRAGMA foreign_keys = ON;

-- 1. Basis-Informationen
CREATE TABLE IF NOT EXISTS DbInfo(
    Dbinfo_id INTEGER PRIMARY KEY AUTOINCREMENT,
    version INTEGER
);

-- 2. Stammdaten (Keine Abhängigkeiten)
CREATE TABLE IF NOT EXISTS orte (
    orte_id INTEGER PRIMARY KEY AUTOINCREMENT,
    plz INTEGER,
    name TEXT
);

CREATE TABLE IF NOT EXISTS nutzer (
    nutzer_id INTEGER PRIMARY KEY AUTOINCREMENT,
    vorname TEXT,
    nachname TEXT,
    email TEXT UNIQUE,
    guthaben NUMERIC(9,2),
    fuehrerschein_nr INTEGER
);

CREATE TABLE IF NOT EXISTS zahlungsmethoden (
    zm_id INTEGER PRIMARY KEY AUTOINCREMENT,
    typ TEXT
);

-- 3. Infrastruktur (Hängt von Orten ab)
CREATE TABLE IF NOT EXISTS stationen (
    stationen_id INTEGER PRIMARY KEY AUTOINCREMENT,
    fk_ort_id INTEGER,
    adresse TEXT,
    kapazitaet INTEGER,
    FOREIGN KEY (fk_ort_id) REFERENCES orte(orte_id)
);

-- 4. Fahrzeuge Basis (Hängt von Stationen ab)
CREATE TABLE IF NOT EXISTS e_fahrzeuge (
    efz_id INTEGER PRIMARY KEY AUTOINCREMENT,
    fk_stationen_id INTEGER,
    standort_lat NUMERIC(7,4),
    standort_lon NUMERIC(7,4),
    akkustand INTEGER,
    status TEXT,
    kilometerstand INTEGER,
    tarif NUMERIC(4,2),
    model TEXT,
    FOREIGN KEY (fk_stationen_id) REFERENCES stationen(stationen_id)
);

-- 5. Fahrzeug-Spezialisierungen (Hängen von e_fahrzeuge ab)
CREATE TABLE IF NOT EXISTS e_autos (
    fk_efz_id INTEGER PRIMARY KEY,
    sitzplaetze INTEGER,
    kennzeichen TEXT,
    FOREIGN KEY(fk_efz_id) REFERENCES e_fahrzeuge(efz_id)
);

CREATE TABLE IF NOT EXISTS e_bikes (
    fk_efz_id INTEGER PRIMARY KEY,
    hat_korb INTEGER,
    FOREIGN KEY (fk_efz_id) REFERENCES e_fahrzeuge(efz_id)
);

CREATE TABLE IF NOT EXISTS e_scooter (
    fk_efz_id INTEGER PRIMARY KEY,
    hoechstgeschwindigkeit INTEGER,
    FOREIGN KEY (fk_efz_id) REFERENCES e_fahrzeuge(efz_id)
);

-- 6. Transaktionsdaten (Hängt von fahrzeugen, nutzern und zahlungsmethoden ab)
CREATE TABLE IF NOT EXISTS buchungen (
    buchung_id INTEGER PRIMARY KEY AUTOINCREMENT,
    fk_efz_id INTEGER,
    fk_zahlungsmethoden INTEGER,
    fk_nutzer_id INTEGER,
    startzeit TEXT,
    endzeit TEXT,
    start_akku INTEGER,
    end_akku INTEGER,
    betrag NUMERIC(9,2),
    distanz NUMERIC(6,2),
    abgeschlossen INTEGER,
    status TEXT,
    FOREIGN KEY (fk_efz_id) REFERENCES e_fahrzeuge(efz_id),
    FOREIGN KEY (fk_nutzer_id) REFERENCES nutzer(nutzer_id),
    FOREIGN KEY (fk_zahlungsmethoden) REFERENCES zahlungsmethoden(zm_id)
);

INSERT OR IGNORE INTO DbInfo (Dbinfo_id, version) VALUES (1, 1);