Use MobilityProject;
GO

CREATE PROCEDURE VerfuegbareFahrzeugeAnzeigen
AS
BEGIN
	SELECT efz.efz_id, efz.model, efz.status, efz.akkustand, s.adresse, o.name AS ort
	FROM e_fahrzeuge efz
	INNER JOIN stationen s ON efz.fk_stationen_id = s.stationen_id
	INNER JOIN orte o ON s.fk_ort_id = o.orte_id
	WHERE efz.status = 'verfuegbar';
END;
GO


EXEC VerfuegbareFahrzeugeAnzeigen;