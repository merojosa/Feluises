/* -- Cuando se crea un proyecto se agrega un módulo 'No asignado'
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
CREATE PROC USP_EmployeeRequeriments @employee nvarchar(9), @project int 
AS
BEGIN
	SELECT *
	FROM Requeriment R
	WHERE R.idProjectFKPK=@project AND R.idEmployeeFK=@employee;
END
*/

/*
CREATE PROC USP_CloseProject @project int 
AS
BEGIN
	UPDATE Employee 
	SET availability = 0
	WHERE idEmployeePK IN
	(
		SELECT W.idEmployeeFKPK
		FROM WorksIn W 
		WHERE W.idProjectFKPK=@project
	)
	DECLARE @closed smallint
	SELECT @closed = 0
	UPDATE Project
	SET status=@closed
	WHERE idProjectPK=@project
END
*/



/*
-- Falta terminar este.
CREATE TRIGGER TR_duracionEstimada
ON Project
FOR INSERT, UPDATE --SE PRODRÍA CAMBIAR LA DURACIÓN DEL PROYECTO
AS --((DATEDIFF(d, E.HireDate, O.OrderDate))/365 = 4)
	DECLARE @duracion DATE
	SELECT @duracion = (DATEDIFF(d, I.finishingDate, I.startingDate))
	FROM inserted I
*/



--Trigger para cambiar la disponibilidad de un empleado una vez que este se agregue a un equipo
/*
CREATE TRIGGER TR_disponibilidadEmpleado
ON WorksIn
FOR INSERT
AS
	DECLARE @num INT
	SELECT @num = (SELECT I.idEmployeeFKPK
					FROM inserted I)
	UPDATE Employee
	SET availability = 1 --Se pone en ocupado
	WHERE idEmployeePK = @num ;
*/


--Trigger para borrar modulos y poner requerimientos en default, revisar
/*
CREATE TRIGGER TR_modificarModuloDelete
ON Module
AFTER DELETE
AS
BEGIN
	UPDATE Requeriment
	SET idProjectFKPK = (SELECT TOP 1 D.idProjectFKPK, 
						FROM DELETED D),
						idModuleFKPK = -1
	WHERE idProjectFKPK = -1
END
GO
*/



