-- Vedran Biđin 0036478158
-- Završni rad - baza podataka
-- Sustav za evidenciju vremena studija

CREATE TABLE Predmet
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Ime NVARCHAR(100) NOT NULL,
	Godina NVARCHAR(20),
	ECTS INT CHECK (ECTS BETWEEN 0 and 180),
	CONSTRAINT UQ_Predmet UNIQUE (Ime, Godina)
);

CREATE TABLE TipAktivnosti
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Ime NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Opterecenje
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	PredmetID INT NOT NULL FOREIGN KEY REFERENCES Predmet(ID),
	TipAktivnostiID INT NOT NULL FOREIGN KEY REFERENCES TipAktivnosti(ID),
	Iznos FLOAT CHECK (Iznos BETWEEN 0 and 180)
);

CREATE TABLE Aktivnost
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Ime NVARCHAR(100) NOT NULL,
	PredmetID INT NOT NULL FOREIGN KEY REFERENCES Predmet(ID),
	TipAktivnostiID INT NOT NULL FOREIGN KEY REFERENCES TipAktivnosti(ID),
	Termin DATETIME, 
	Trajanje INT, -- u minutama
);

CREATE TABLE Korisnik
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	KorisnickoIme NVARCHAR(100) NOT NULL UNIQUE,
	Lozinka BINARY(128) NOT NULL,
	Email NVARCHAR(100) NOT NULL,
	Uloga INT NOT NULL CHECK (Uloga BETWEEN 0 and 2)
);

CREATE TABLE Pretplata
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	KorisnikID INT NOT NULL FOREIGN KEY REFERENCES Korisnik(ID),
	PredmetID INT NOT NULL FOREIGN KEY REFERENCES Predmet(ID),
);

CREATE TABLE Evidencija
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Trajanje INT NOT NULL, -- u minutama
	DatumUnosa DATETIME NOT NULL,
	KorisnikID INT NOT NULL FOREIGN KEY REFERENCES Korisnik(ID),
	AktivnostID INT NOT NULL FOREIGN KEY REFERENCES Aktivnost(ID)
);
