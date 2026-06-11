INSERT INTO orte (plz, name) VALUES (6010, 'Kriens'), (6006, 'Luzern'), (6244, 'Nebikon'), (6003, 'Luzern'), (6048, 'Horw'), (6014, 'Littau');

INSERT INTO stationen (fk_ort_id, adresse, kapazitaet) VALUES 
(1, 'Tulpenstrasse 9', 10), (2, 'Alpenquaistrasse 2', 8), (3, 'Bahnhofstrasse 6', 4),
(4, 'Hirschengraben 43', 15), (5, 'Kantonsstrasse 12', 6), (2, 'Zentralstrasse 5', 12);

INSERT INTO e_fahrzeuge (efz_id, fk_stationen_id, standort_lat, standort_lon, akkustand, status, kilometerstand, tarif, model) VALUES 
(1, 1, 47.0342, 8.2789, 85, 'bereit', 12500, 0.30, 'Tesla Model 3'),
(2, 2, 47.0498, 8.3162, 40, 'bereit', 8400, 0.25, 'VW ID.3'),
(3, 1, 47.0345, 8.2792, 100, 'bereit', 450, 0.10, 'VanMoof S3'),
(4, 3, 47.1311, 7.9782, 60, 'bereit', 120, 0.15, 'Ninebot Max G30'),
(5, 4, 47.0502, 8.3034, 95, 'bereit', 3100, 0.35, 'Audi Q4 e-tron'),
(6, 4, 47.0501, 8.3031, 12, 'laden', 18900, 0.28, 'Renault Zoe'),
(7, 5, 47.0163, 8.3105, 100, 'bereit', 85, 0.12, 'Stromer ST3'),
(8, 6, 47.0489, 8.3078, 5, 'laden', 890, 0.08, 'Canyon Precede:ON'),
(9, 2, 47.0495, 8.3159, 80, 'bereit', 340, 0.15, 'Xiaomi Pro 5'),
(10, 6, 47.0491, 8.3075, 45, 'bereit', 510, 0.15, 'Voiager 4');

INSERT INTO e_autos (fk_efz_id, sitzplaetze, kennzeichen) VALUES 
(1, 5, 'LU-101-U'), (2, 5, 'LU-202-U'), (5, 5, 'LU-505-U'), (6, 4, 'LU-606-U');

INSERT INTO e_bikes (fk_efz_id, hat_korb) VALUES 
(3, 1), (7, 0), (8, 1);

INSERT INTO e_scooter (fk_efz_id, hoechstgeschwindigkeit) VALUES 
(4, 20), (9, 20), (10, 22);

INSERT INTO nutzer (vorname, nachname, email, guthaben, fuehrerschein_nr) VALUES 
('Max', 'Mustermann', 'max@mustermann.de', 50.00, '111222333');

INSERT INTO zahlungsmethoden (typ) VALUES ('Kreditkarte'), ('Twint'), ('Guthaben');
