
use PruebaFinanzauto;
--Stored PRocedure

------------------STORED PROCEDURE ESTUDENTS----------------
CREATE PROCEDURE sp_InsertStudent
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @DateOfBirth DATE,
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @Adress VARCHAR(64)
AS
BEGIN
    INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, Phone, Adress)
    VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @Phone, @Adress);
END;
GO

CREATE PROCEDURE sp_UpdateStudent
    @StudentsId INT,
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @DateOfBirth DATE,
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @Adress VARCHAR(64)
AS
BEGIN
    UPDATE Students
    SET FirstName = @FirstName,
        LastName = @LastName,
        DateOfBirth = @DateOfBirth,
        Email = @Email,
        Phone = @Phone,
        Adress = @Adress
    WHERE StudentsId = @StudentsId;
END;
GO

CREATE PROCEDURE sp_DeleteStudent
    @StudentsId INT
AS
BEGIN
    DELETE FROM Students
    WHERE StudentsId = @StudentsId;
END;
GO


CREATE PROCEDURE sp_GetStudent
    @StudentsId INT
AS
BEGIN
    SELECT * FROM Students
    WHERE StudentsId = @StudentsId;
END;
GO


CREATE PROCEDURE sp_GetAllStudents
AS
BEGIN
    SELECT * FROM Students;
END;
GO


------------------STORED PROCEDURE TEACHER----------------

-- Procedimiento para insertar un registro en Teachers
CREATE PROCEDURE sp_InsertTeacher
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @Department VARCHAR(15),
    @Adress VARCHAR(64)
AS
BEGIN
    INSERT INTO Teachers (FirstName, LastName, Email, Phone, Department, Adress)
    VALUES (@FirstName, @LastName, @Email, @Phone, @Department, @Adress);
END;
GO

-- Procedimiento para actualizar un registro en Teachers
CREATE PROCEDURE sp_UpdateTeacher
    @TeacherId INT,
    @FirstName VARCHAR(100),
    @LastName VARCHAR(100),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @Department VARCHAR(15),
    @Adress VARCHAR(64)
AS
BEGIN
    UPDATE Teachers
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Phone = @Phone,
        Department = @Department,
        Adress = @Adress
    WHERE TeacherId = @TeacherId;
END;
GO

-- Procedimiento para eliminar un registro de Teachers
CREATE PROCEDURE sp_DeleteTeacher
    @TeacherId INT
AS
BEGIN
    DELETE FROM Teachers
    WHERE TeacherId = @TeacherId;
END;
GO

-- Procedimiento para obtener un registro de Teachers
CREATE PROCEDURE sp_GetTeacher
    @TeacherId INT
AS
BEGIN
    SELECT * FROM Teachers
    WHERE TeacherId = @TeacherId;
END;
GO

-- Procedimiento para obtener todos los registros de Teachers
CREATE PROCEDURE sp_GetAllTeachers
AS
BEGIN
    SELECT * FROM Teachers;
END;
GO


------------------STORED PROCEDURE COURSES----------------

-- Procedimiento para insertar un registro en Courses
CREATE PROCEDURE sp_InsertCourse
    @CourseName VARCHAR(100),
    @Credits INT,
    @TeacherId INT
AS
BEGIN
    INSERT INTO Courses (CorseName, Credits, TeacherId)
    VALUES (@CourseName, @Credits, @TeacherId);
END;
GO

-- Procedimiento para actualizar un registro en Courses
CREATE PROCEDURE sp_UpdateCourse
    @CourseId INT,
    @CourseName VARCHAR(100),
    @Credits INT,
    @TeacherId INT
AS
BEGIN
    UPDATE Courses
    SET CorseName = @CourseName,
        Credits = @Credits,
        TeacherId = @TeacherId
    WHERE CoursesID = @CourseId;
END;
GO

-- Procedimiento para eliminar un registro de Courses
CREATE PROCEDURE sp_DeleteCourse
    @CourseId INT
AS
BEGIN
    DELETE FROM Courses
    WHERE CoursesID = @CourseId;
END;
GO

-- Procedimiento para obtener un registro de Courses
CREATE PROCEDURE sp_GetCourse
    @CourseId INT
AS
BEGIN
    SELECT * FROM Courses
    WHERE CoursesID = @CourseId;
END;
GO

-- Procedimiento para obtener todos los registros de Courses
CREATE PROCEDURE sp_GetAllCourses
AS
BEGIN
    SELECT * FROM Courses;
END;
GO


------------------STORED PROCEDURE GRADE----------------

-- Procedimiento para insertar un registro en Grade
CREATE PROCEDURE sp_InsertGrade
    @StudentsId INT,
    @CoursesID INT,
    @Grade VARCHAR(100)
AS
BEGIN
    INSERT INTO Grade (StudentsId, CoursesID, Grade)
    VALUES (@StudentsId, @CoursesID, @Grade);
END;
GO

-- Procedimiento para actualizar un registro en Grade
CREATE PROCEDURE sp_UpdateGrade
    @GradeID INT,
    @StudentsId INT,
    @CoursesID INT,
    @Grade VARCHAR(100)
AS
BEGIN
    UPDATE Grade
    SET StudentsId = @StudentsId,
        CoursesID = @CoursesID,
        Grade = @Grade
    WHERE GradeID = @GradeID;
END;
GO

-- Procedimiento para eliminar un registro de Grade
CREATE PROCEDURE sp_DeleteGrade
    @GradeID INT
AS
BEGIN
    DELETE FROM Grade
    WHERE GradeID = @GradeID;
END;
GO

-- Procedimiento para obtener un registro de Grade
CREATE PROCEDURE sp_GetGrade
    @GradeID INT
AS
BEGIN
    SELECT * FROM Grade
    WHERE GradeID = @GradeID;
END;
GO

-- Procedimiento para obtener todos los registros de Grade
CREATE PROCEDURE sp_GetAllGrades
AS
BEGIN
    SELECT * FROM Grade;
END;
GO
