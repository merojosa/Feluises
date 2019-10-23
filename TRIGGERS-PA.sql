/*
CREATE TRIGGER TR_moduloNoAsignado
ON Project
FOR INSERT
AS
	DECLARE @numerito INT
	DECLARE @nombre VARCHAR(30)
	SELECT @numerito = (SELECT I.idProjectPK
						FROM inserted I)
	SELECT @nombre = 'No asignado'
	SET IDENTITY_INSERT Module ON
	INSERT INTO Module(idProjectFKPK, idModulePK, name)
			VALUES (@numerito,
					-1,
					@nombre)
	SET IDENTITY_INSERT Module OFF
*/
/*
CREATE TRIGGER TR_duracionEstimada
ON Project
FOR INSERT, UPDATE --SE PRODRÍA CAMBIAR LA DURACIÓN DEL PROYECTO
AS --((DATEDIFF(d, E.HireDate, O.OrderDate))/365 = 4)
	DECLARE @duracion DATE
	SELECT @duracion = (DATEDIFF(d, I.deadline, I.startingDate)/365 =
	FROM inserted I
*/

SELECT *
FROM Project P


DECLARE @PRB INT

SELECT @PRB = (DATEDIFF(d, P.startingDate, P.finishingDate))
FROM Project P

--ALTER TABLE Project ADD deadline DATE



