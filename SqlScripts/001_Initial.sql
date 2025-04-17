CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Age INT NOT NULL,
    Workplace NVARCHAR(100),
    Phone NVARCHAR(20)
);

CREATE TABLE CreditProducts (
    Id INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(50) NOT NULL,
    InterestRate DECIMAL(5,2) NOT NULL
);

CREATE TABLE CreditApplications (
    Id INT PRIMARY KEY IDENTITY,
    LoanPurpose NVARCHAR(200) NOT NULL,
    LoanAmount DECIMAL(18,2) NOT NULL,
    ClientIncome DECIMAL(18,2) NOT NULL,
    CreditProductId INT FOREIGN KEY REFERENCES CreditProducts(Id)
);

CREATE TABLE Calls (
    Id INT PRIMARY KEY IDENTITY,
    ScheduledDate DATETIME NOT NULL,
    Result NVARCHAR(MAX),
    IsProcessed BIT DEFAULT 0
);