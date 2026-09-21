IF DB_ID(N'Veterinarska_Ambulanta') IS NULL
    CREATE DATABASE Veterinarska_Ambulanta;
GO

USE Veterinarska_Ambulanta;
GO

IF OBJECT_ID(N'dbo.Pregled', N'U') IS NULL
AND OBJECT_ID(N'dbo.Termin', N'U') IS NULL
AND OBJECT_ID(N'dbo.Pacijent', N'U') IS NULL
AND OBJECT_ID(N'dbo.Lek', N'U') IS NULL
AND OBJECT_ID(N'dbo.Veterinar', N'U') IS NULL
BEGIN
    CREATE TABLE Veterinar (
        IDVeterinara INT IDENTITY(1,1) PRIMARY KEY,
        Ime NVARCHAR(50) NOT NULL,
        Prezime NVARCHAR(50) NOT NULL,
        JMBG NVARCHAR(13) NOT NULL,
        BrojTelefona NVARCHAR(20) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        isDeleted BIT NOT NULL DEFAULT 0
    );

    CREATE TABLE Pacijent (
        IDPacijenta INT IDENTITY(1,1) PRIMARY KEY,
        Ime NVARCHAR(50) NOT NULL,
        Vlasnik NVARCHAR(50) NOT NULL,
        BrojCipa NVARCHAR(15) NOT NULL,
        BrojTelefona NVARCHAR(20) NOT NULL,
        Pol NVARCHAR(5) NOT NULL,
        Alergije NVARCHAR(200) NULL,
        Sterilisan BIT NULL,
        TezinaKg INT NOT NULL,
        IDVeterinara INT NOT NULL,
        CONSTRAINT FK_Pacijent_Veterinar FOREIGN KEY (IDVeterinara) REFERENCES Veterinar(IDVeterinara)
    );

    CREATE TABLE Termin (
        IDTermina INT IDENTITY(1,1) PRIMARY KEY,
        Datum DATE NOT NULL,
        Vreme TIME NOT NULL,
        VrstaUsluge NVARCHAR(100) NOT NULL,
        IDPacijenta INT NOT NULL,
        IDVeterinara INT NOT NULL,
        CONSTRAINT FK_Termin_Pacijent FOREIGN KEY (IDPacijenta) REFERENCES Pacijent(IDPacijenta),
        CONSTRAINT FK_Termin_Veterinar FOREIGN KEY (IDVeterinara) REFERENCES Veterinar(IDVeterinara)
    );

    CREATE TABLE Lek (
        IDLeka INT IDENTITY(1,1) PRIMARY KEY,
        Naziv NVARCHAR(100) NOT NULL,
        Proizvodjac NVARCHAR(100) NOT NULL,
        Jacina NVARCHAR(50) NOT NULL,
        Doziranje NVARCHAR(100) NOT NULL,
        isDeleted BIT NOT NULL DEFAULT 0
    );

    CREATE TABLE Pregled (
        IDPregleda INT IDENTITY(1,1) PRIMARY KEY,
        Izvestaj NVARCHAR(MAX) NOT NULL,
        IDTermina INT NOT NULL,
        IDLeka INT NOT NULL,
        CONSTRAINT FK_Pregled_Termin FOREIGN KEY (IDTermina) REFERENCES Termin(IDTermina),
        CONSTRAINT FK_Pregled_Lek FOREIGN KEY (IDLeka) REFERENCES Lek(IDLeka)
    );
END
GO
