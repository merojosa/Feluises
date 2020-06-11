CREATE PROCEDURE SP_GetRequirements

	@idClient CHAR(9),
	@idProject INT

AS
	SELECT	R.idProjectFKPK, R.idModuleFKPK, R.idRequerimentPK, 
			R.status, R.startingDate, R.endDate
	FROM Requeriment R, Project P, Module M
	WHERE	P.idClientFK = @idClient AND P.idProjectPK = @idProject AND
			P.idProjectPK = M.idProjectFKPK AND M.idModulePK = R.idModuleFKPK