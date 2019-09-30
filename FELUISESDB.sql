CREATE TABLE Client(
	idClientPK CHAR(9), 
	clientName VARCHAR(20) NOT NULL,
	clientLastName VARCHAR(20) NOT NULL,
	clientSecondLastName VARCHAR(20),
	company VARCHAR(20),
	tel VARCHAR(20) NOT NULL,
	email VARCHAR(30) NOT NULL,
	CONSTRAINT PK_Client PRIMARY KEY(idClientPK)
);

CREATE TABLE Project(
	idProjectPK INT, 
	projectName CHAR(20) NOT NULL UNIQUE,
	objective VARCHAR(256) NOT NULL,
	estimatedCost NUMERIC(20, 2), 
	realCost NUMERIC(20, 2) DEFAULT 0, 
	startingDate DATE,
	finishingDate DATE,
	budget NUMERIC(20, 2) CHECK (budget > 0),
	estimatedDuration INTEGER, --DERIVADO
	idClientFK CHAR(9) DEFAULT -1,
	CONSTRAINT PK_Project PRIMARY KEY(idProjectPK),
	CONSTRAINT FK_Project_Client FOREIGN KEY (idClientFK) REFERENCES Client(idClientPK)
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE,
	CONSTRAINT CK_Project_EstimatedCost CHECK (estimatedCost > 0),
	CONSTRAINT CK_Project_Budget CHECK (budget > 0)
);

CREATE TABLE Employee(
	idEmployeePK CHAR(9), 
	employeeName VARCHAR(20) NOT NULL,
	employeeLastName VARCHAR(20) NOT NULL,
	employeeSecondLastName VARCHAR(20),
	employeeBirthDate DATE,
	employeeHireDate DATE NOT NULL,
	developerFlag SMALLINT DEFAULT 0,
	tel VARCHAR(20) NOT NULL,
	email VARCHAR(30) NOT NULL,
	province VARCHAR(20),
	canton VARCHAR(20),
	district VARCHAR(20),
	exactDirection VARCHAR(35),
	pricePerHour NUMERIC(20,2),
	availability SMALLINT DEFAULT 0,
	CONSTRAINT PK_Employee  PRIMARY KEY(idEmployeePK)
);

CREATE TABLE Module(
	idProjectFKPK INT,
	idModulePK INT,
	name VARCHAR(30) NOT NULL,
	CONSTRAINT PK_Module PRIMARY KEY(idProjectFKPK,idModulePK),
	CONSTRAINT FK_Module_idProject FOREIGN KEY(idProjectFKPK) REFERENCES Project(idProjectPK)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

CREATE TABLE Requeriment(
	idProjectFKPK INT,
	idModuleFKPK INT DEFAULT -1,
	idRequerimentPK INT,
	idEmployeeFK CHAR(9) DEFAULT '?????????',
	estimatedDuration INT,
	realDuration INT,
	status SMALLINT DEFAULT 0,
	startingDate DATE NOT NULL,
	endDate DATE,
	complexity SMALLINT DEFAULT 0,
	CONSTRAINT PK_Requeriment PRIMARY KEY(idProjectFKPK, idModuleFKPK, idRequerimentPK),
	CONSTRAINT FK_Requeriment_Project FOREIGN KEY(idProjectFKPK,idModuleFKPK) REFERENCES Module(idProjectFKPK,idModulePK)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
	CONSTRAINT FK_Requeriment_Employee FOREIGN KEY(idEmployeeFK) REFERENCES Employee(idEmployeePK)
	ON DELETE SET DEFAULT
	ON UPDATE CASCADE
);

CREATE TABLE DeveloperKnowledge(
	idEmployeeFKPK CHAR(9), 
	devKnowledgePK VARCHAR(30),
	CONSTRAINT PK_DeveloperKnowledge PRIMARY KEY(idEmployeeFKPK,devKnowledgePK),
	CONSTRAINT FK_DeveloperKnowledge_Employee FOREIGN KEY(idEmployeeFKPK) REFERENCES Employee(idEmployeePK)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);

CREATE TABLE WorksIn(
    idEmployeeFKPK CHAR(9),
    idProjectFKPK INT,
    role INT DEFAULT 0,
    CONSTRAINT PK_WorksIn PRIMARY KEY(idEmployeeFKPK,idProjectFKPK),
    CONSTRAINT FK_WorksIn_Employee FOREIGN KEY(idEmployeeFKPK) REFERENCES Employee(idEmployeePK)
	ON DELETE CASCADE
	ON UPDATE CASCADE,
    CONSTRAINT FK_WorksIn_Project FOREIGN KEY(idProjectFKPK) REFERENCES Project(idProjectPK)
	ON DELETE CASCADE
	ON UPDATE CASCADE
);
