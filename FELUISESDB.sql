CREATE TABLE CLIENT(
	idClient CHAR(9), 
	clientName VARCHAR(20) NOT NULL,
	clientLastName VARCHAR(20) NOT NULL,
	clientSecondLastName VARCHAR(20),
	company VARCHAR(20),
	Tel VARCHAR(20) NOT NULL,
	email VARCHAR(30) NOT NULL,
	CONSTRAINT CLIENT_PK PRIMARY KEY(idClient)
);

CREATE TABLE PROYECT(
	idProyect INT PRIMARY KEY, 
	proyectName CHAR(20) NOT NULL UNIQUE,
	objective VARCHAR(256) NOT NULL,
	estimatedCost NUMERIC(20, 2), 
	realCost NUMERIC(20, 2) DEFAULT 0, 
	startingDate DATE,
	finishingDate DATE,
	budget NUMERIC(20, 2) CHECK (budget > 0),
	estimatedDuration INTEGER, --DERIVADO
	idClient CHAR(9) DEFAULT -1,
	CONSTRAINT FK_Client FOREIGN KEY (idClient) REFERENCES CLIENT(idClient)
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE,
	CONSTRAINT CK_estimatedCost  CHECK (estimatedCost > 0),
	CONSTRAINT CK_budget  CHECK (budget > 0)
);

CREATE TABLE EMPLOYEE(
	idEmployee CHAR(9) PRIMARY KEY, 
	employeeName VARCHAR(20) NOT NULL,
	employeeLastName VARCHAR(20) NOT NULL,
	employeeSecondLastName VARCHAR(20),
	employeeBirthDate DATE,
	employeeHireDate DATE NOT NULL,
	province VARCHAR(20),
	canton VARCHAR(20),
	district VARCHAR(20),
	exactDirection VARCHAR(35),
	leaderFlag BIT DEFAULT 0,
	developerFlag BIT DEFAULT 1,
	idLeaderSupervise CHAR(9) DEFAULT -1,
	idProyect INT DEFAULT -1,
	CONSTRAINT FK_leader FOREIGN KEY (idLeaderSupervise) REFERENCES EMPLOYEE(idEmployee)
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE,
	CONSTRAINT FK_Proyect FOREIGN KEY (idProyect) REFERENCES PROYECT(idProyect)
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE
);

CREATE TABLE MODULE(
	idProyect INT,
	idModule INT,
	Name VARCHAR(30) NOT NULL,
	CONSTRAINT MODULE_PK PRIMARY KEY(idProyect,idModule),
	CONSTRAINT IDPROYECT_PK FOREIGN KEY(idProyect) REFERENCES PROYECT(idProyect)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

CREATE TABLE DEVELOPER_KNOWLEDGE(
	idEmployee CHAR(9), 
	devKnowledge VARCHAR(30),
	CONSTRAINT DEVKNOWLEDGE_PK PRIMARY KEY(idEmployee,devKnowledge),
	CONSTRAINT EMPLOYEE_FK FOREIGN KEY(idEmployee) REFERENCES EMPLOYEE(idEmployee)
	ON DELETE CASCADE ON UPDATE CASCADE
);



