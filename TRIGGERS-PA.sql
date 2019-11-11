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

/*
CREATE PROC USP_EmployeeRequerimentsComplete @employee nvarchar(9), @project int 
AS
BEGIN
	SELECT *
	FROM Project P 
	JOIN Module M
	ON P.idProjectPK = M.idProjectFKPK
	JOIN Requeriment R
	ON M.idModulePK = R.idModuleFKPK AND M.idProjectFKPK = R.idProjectFKPK
	WHERE R.idProjectFKPK=@project AND R.idEmployeeFK=@employee;
END
*/

/*
CREATE TRIGGER TR_interrumpirProyecto
ON Project
AFTER UPDATE
AS
BEGIN
	IF( UPDATE(status) )
	BEGIN
		DECLARE @status smallint
		SELECT @status = (SELECT TOP 1 I.status FROM INSERTED I)
		IF( @status = 2 OR @status = 4) --caso de que fue interrumpido
		BEGIN
			UPDATE Requeriment
			SET status = @status
			WHERE idProjectFKPK = (SELECT TOP 1 I.idProjectPK FROM INSERTED I)
			AND status = 1 
		END
		IF( @status = 4 )
		BEGIN
			DECLARE @project int
			SELECT @project = (SELECT TOP 1 I.idProjectPK FROM INSERTED I)
			UPDATE Employee 
			SET availability = 0
			WHERE idEmployeePK IN
			(
				SELECT W.idEmployeeFKPK
				FROM WorksIn W 
				WHERE W.idProjectFKPK=@project
			)
		END
	END
END
*/



/*
Trigger de integridad referencial al borrar modulos.
CREATE TRIGGER TR_Default_Module
ON Module
AFTER DELETE
AS
BEGIN
	DECLARE @idAnterior INT
	DECLARE @projectId INT
	DECLARE @CantidadModulos INT
	SELECT @idAnterior = (SELECT idModulePK FROM Deleted) --Obtiene el id del modulo eliminado
	SELECT @projectId = (SELECT idProjectFKPK FROM Deleted) --Obtiene el id del proyecto al que pertenecía el módulo eliminado
	SELECT @CantidadModulos = (	SELECT COUNT(Module.idProjectFKPK) AS cantidad --Obtiene la cantidad de módulos que actualmente pertenecen a ese proyecto
								FROM Module JOIN Project
								ON Project.idProjectPK = Module.idProjectFKPK
								WHERE @projectId = Project.idProjectPK
								GROUP BY Module.idProjectFKPK)
	IF(@idAnterior != -1) --Si no era el módulo no asignado entonces asigna los requerimientos al no asignado
	BEGIN
		UPDATE Requeriment
		SET idModuleFKPK = -1
		WHERE idModuleFKPK = @idAnterior
	END
	IF(@idAnterior = -1 AND @CantidadModulos = 0) --Si era el no asignado y era el único módulo que queda entonces elimina los requerimientos.
	BEGIN
		DELETE FROM Requeriment
		WHERE idModuleFKPK = @idAnterior
	END
	ELSE --Si era el no asignado pero no era el único que queda hace un rollback (no permite borrarlo)
	BEGIN
		ROLLBACK
	END
END;
*/

/*
CREATE TRIGGER TR_Requeriment_is_alive
ON Requeriment
AFTER DELETE
AS
BEGIN
	DECLARE @idProjectFKPK INT, @idModuleFKPK INT, @idRequerimentPK INT, @idEmployee VARCHAR(9)
	DECLARE @estimatedDuration INT, @realduration INT, @status SMALLINT, @startingDate Date
	DECLARE @endDate Date, @complexity INT, @objective VARCHAR(50)
	SELECT  @idProjectFKPK = idProjectFKPK, @idModuleFKPK = idModuleFKPK, @idRequerimentPK = idRequerimentPK, @idEmployee = idEmployeeFK,
			@estimatedDuration = estimatedDuration, @realduration = realDuration, @status = status, @startingDate = startingDate, @endDate = endDate,
			@complexity = complexity, @objective = objective
	FROM deleted
	IF(@idModuleFKPK != -1)
	BEGIN
		INSERT INTO Requeriment(idProjectFKPK, idModuleFKPK, idEmployeeFK, estimatedDuration, realDuration, status, startingDate, endDate, complexity,objective)
		VALUES(@idProjectFKPK, -1, @idEmployee, @estimatedDuration, @realduration, @status, @startingDate, @endDate, @complexity, @objective)
	END
END
*/