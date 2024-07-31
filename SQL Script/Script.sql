CREATE DATABASE PruebaFinanzauto;
use PruebaFinanzauto;


Create Table Students (
	StudentsId INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
	DateOfBirth Date NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Phone VARCHAR(15) NOT NULL,
	Adress VARCHAR(64) NOT NULL,
);


Create Table Teachers (
	TeacherId INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Phone VARCHAR(15) NOT NULL,
	Department VARCHAR(15) NOT NULL,
	Adress VARCHAR(64) NOT NULL,
);


Create Table Courses (
	CoursesID INT IDENTITY(1,1) PRIMARY KEY,
	CorseName VARCHAR(100) NOT NULL,
	Credits INT NOT NULL,
	TeacherId INT,
	CONSTRAINT Fk_Courses_Teachers FOREIGN KEY (TeacherId) REFERENCES Teachers(TeacherId)
);



Create Table Grade (
	GradeID INT IDENTITY(1,1) PRIMARY KEY,
	StudentsId INT,
	CoursesID INT NOT NULL,
	Grade VARCHAR(100) NOT NULL,
	CONSTRAINT Fk_Greade_Students FOREIGN KEY (StudentsId) REFERENCES Students(StudentsId),
	CONSTRAINT Fk_Greade_Courses FOREIGN KEY (CoursesID) REFERENCES Courses(CoursesID)
);


CREATE TABLE Usuarios (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    ClaveHash NVARCHAR(255) NOT NULL
);



CREATE INDEX idx_Students_FistName ON Students(FirstName);
CREATE INDEX idx_Teachers_FistName ON Teachers(FirstName);
CREATE INDEX idx_Courses_CorseName ON Courses(CorseName);
CREATE INDEX idx_Grade_Grade ON Grade(Grade);


-- Insertar datos en la tabla Students
INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, Phone, Adress)
VALUES 
    ('John', 'Doe', '2000-01-15', 'john.doe@example.com', '123-456-7890', '123 Main St'),
    ('Jane', 'Smith', '1998-03-22', 'jane.smith@example.com', '098-765-4321', '456 Elm St'),
    ('Emily', 'Johnson', '2001-07-30', 'emily.johnson@example.com', '555-123-4567', '789 Oak St');



	-- Insertar datos en la tabla Teachers
INSERT INTO Teachers (FirstName, LastName, Email, Phone, Department, Adress)
VALUES 
    ('Alice', 'Brown', 'alice.brown@example.com', '111-222-3333', 'Mathematics', '101 Maple Ave'),
    ('Bob', 'Davis', 'bob.davis@example.com', '444-555-6666', 'Physics', '202 Birch Rd'),
    ('Carol', 'Wilson', 'carol.wilson@example.com', '777-888-9999', 'Chemistry', '303 Pine St');



	-- Insertar datos en la tabla Courses
INSERT INTO Courses (CorseName, Credits, TeacherId)
VALUES 
    ('Calculus I', 4, 1),  -- TeacherId 1 corresponds to Alice Brown
    ('Quantum Mechanics', 3, 2),  -- TeacherId 2 corresponds to Bob Davis
    ('Organic Chemistry', 4, 3);  -- TeacherId 3 corresponds to Carol Wilson



	-- Insertar datos en la tabla Grade
INSERT INTO Grade (StudentsId, CoursesID, Grade)
VALUES 
    (1, 1, 'A'),  -- StudentId 1 received an A in CourseId 1 (Calculus I)
    (2, 2, 'B'),  -- StudentId 2 received a B in CourseId 2 (Quantum Mechanics)
    (3, 3, 'A-');  -- StudentId 3 received an A- in CourseId 3 (Organic Chemistry)
