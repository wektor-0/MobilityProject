-- 1. Orte
INSERT INTO orte (plz, name) VALUES (6010, 'Kriens');
INSERT INTO orte (plz, name) VALUES (6006, 'Luzern');
INSERT INTO orte (plz, name) VALUES (6244, 'Nebikon');

-- 2. Stationen (Nutzt die IDs der Orte)
INSERT INTO stationen (fk_ort_id, adresse, kapazitaet) VALUES (1, 'Tulpenstrasse 9', 10);
INSERT INTO stationen (fk_ort_id, adresse, kapazitaet) VALUES (2, 'Alpenquaistrasse 2', 8);
INSERT INTO stationen (fk_ort_id, adresse, kapazitaet) VALUES (3, 'Bahnhofstrasse 6', 4);

-- 3. E-Autos (Zuerst Basisdaten, dann Spezialdaten)
-- Tesla (Fahrzeug ID 1)
INSERT INTO e_fahrzeuge (fk_stationen_id, standort_lat, standort_lon, akkustand, status, kilometerstand, tarif, model) 
VALUES (1, 52.5200, 13.4050, 85, 'bereit', 12500, 0.30, 'Tesla Model 3');
INSERT INTO e_autos (fk_efz_id, sitzplaetze, kennzeichen) VALUES (1, 5, 'B-EV-101');

-- VW ID.3 (Fahrzeug ID 2)
INSERT INTO e_fahrzeuge (fk_stationen_id, standort_lat, standort_lon, akkustand, status, kilometerstand, tarif, model) 
VALUES (2, 48.1351, 11.5820, 40, 'bereit', 8400, 0.25, 'VW ID.3');
INSERT INTO e_autos (fk_efz_id, sitzplaetze, kennzeichen) VALUES (2, 5, 'M-ID-202');

-- 4. E-Bikes (Fahrzeug ID 3)
INSERT INTO e_fahrzeuge (fk_stationen_id, standort_lat, standort_lon, akkustand, status, kilometerstand, tarif, model) 
VALUES (1, 52.5210, 13.4060, 100, 'bereit', 450, 0.10, 'VanMoof S3');
INSERT INTO e_bikes (fk_efz_id, hat_korb) VALUES (3, 1);

-- 5. E-Scooter (Fahrzeug ID 4)
INSERT INTO e_fahrzeuge (fk_stationen_id, standort_lat, standort_lon, akkustand, status, kilometerstand, tarif, model) 
VALUES (3, 52.5195, 13.4045, 60, 'bereit', 120, 0.15, 'Ninebot Max G30');
INSERT INTO e_scooter (fk_efz_id, hoechstgeschwindigkeit) VALUES (4, 20);

-- 6. Nutzer
INSERT INTO nutzer (vorname, nachname, email, guthaben, fuehrerschein_nr) 
VALUES ('Max', 'Mustermann', 'max@mustermann.de', 50.00, 111222333);
INSERT INTO nutzer (vorname, nachname, email, guthaben, fuehrerschein_nr) 
VALUES ('Lehrer', 'Schmidt', 'lara.s@web.de', 12.50, 444555666);

-- 7. Zahlungsmethoden
INSERT INTO zahlungsmethoden (typ) VALUES ('Kreditkarte');
INSERT INTO zahlungsmethoden (typ) VALUES ('Twint');
INSERT INTO zahlungsmethoden (typ) VALUES ('Guthaben');

-- 8. Buchungen
-- Max bucht Tesla (EFZ ID 1)
INSERT INTO buchungen (fk_efz_id, fk_nutzer_id, fk_zahlungsmethoden, startzeit, endzeit, start_akku, end_akku, betrag, distanz, abgeschlossen, status)
VALUES (1, 1, 2, '2024-06-10 14:00:00', '2024-06-10 15:30:00', 85, 70, 25.50, 45.2, 1, 'beendet');

-- Lara bucht VW (EFZ ID 2)
INSERT INTO buchungen (fk_efz_id, fk_nutzer_id, fk_zahlungsmethoden, startzeit, endzeit, start_akku, end_akku, betrag, distanz, abgeschlossen, status)
VALUES (2, 2, 1, '2024-06-11 09:00:00', '2024-06-11 09:15:00', 100, 95, 2.50, 4.0, 1, 'beendet');