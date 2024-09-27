-- Create Database
CREATE DATABASE EmployeeManagement;
GO

-- Use the Database
USE EmployeeManagement;
GO

-- Create Departments Table
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY, -- Primary Key, auto-increment
    DepartmentCode NVARCHAR(50) NOT NULL UNIQUE, -- Unique Department Code
    DepartmentName NVARCHAR(100) NOT NULL, -- Required Department Name
    CreatedDate DATETIME DEFAULT GETDATE(), -- Creation Date with default current timestamp
    ModifiedDate DATETIME DEFAULT GETDATE() -- Modification Date with default current timestamp
);
GO

-- Create Employees Table
CREATE TABLE Employees (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY, -- Primary Key, auto-increment
    FirstName NVARCHAR(100) NOT NULL, -- Required First Name
    LastName NVARCHAR(100) NOT NULL, -- Required Last Name
    EmailAddress NVARCHAR(100) NOT NULL UNIQUE, -- Required and unique Email Address
    DOB DATETIME NOT NULL, -- Required Date of Birth
    Age AS DATEDIFF(YEAR, DOB, GETDATE()), -- Computed column for Age
    Salary DECIMAL(18, 2) NOT NULL, -- Required Salary with precision 18 and scale 2
    DepartmentId INT NOT NULL, -- Foreign Key reference to Departments table
    CreatedDate DATETIME DEFAULT GETDATE(), -- Creation Date with default current timestamp
    ModifiedDate DATETIME DEFAULT GETDATE(), -- Modification Date with default current timestamp
    CONSTRAINT FK_Department FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId) -- Foreign Key constraint
);
GO

-----------------------------------------------------------

-- List all tables in the current database
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';

-- View structure of Departments table
EXEC sp_columns Departments;

-- View structure of Employees table
EXEC sp_columns Employees;

-----------------------------------------------------------

-- View data in Departments table
SELECT * FROM Departments;

-- View data in Employees table
SELECT * FROM Employees;

-----------------------------------------------------------

-- View foreign keys in the database
SELECT 
    f.name AS ForeignKey,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (f.referenced_object_id) AS ReferencedTable,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn
FROM 
    sys.foreign_keys AS f
INNER JOIN 
    sys.foreign_key_columns AS fc 
ON 
    f.object_id = fc.constraint_object_id;


INSERT INTO Employees (FirstName, LastName, EmailAddress, DOB, Salary, DepartmentId) 
VALUES ('John', 'Doe', 'john.doe@example.com', '1985-05-15', 50000.00, 1);

INSERT INTO Departments (DepartmentCode, DepartmentName) 
VALUES ('HR', 'Human Resources'),
       ('IT', 'Information Technology'),
       ('FIN', 'Finance');

SELECT * FROM Departments;
