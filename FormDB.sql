CREATE TABLE Formularios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(255) NOT NULL,
    Descripcion NVARCHAR(1000) NULL
);


CREATE TABLE Campos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormularioId INT FOREIGN KEY REFERENCES Formularios(Id),
    Nombre NVARCHAR(255) NOT NULL,
    Tipo NVARCHAR(50) NOT NULL, -- Ejemplo: "text", "number", "date", etc.
    Requerido BIT NOT NULL,
    Posicion INT -- Puede ser usado para ordenar los campos en el formulario
);

CREATE TABLE Respuestas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormularioId INT FOREIGN KEY REFERENCES Formularios(Id) ON DELETE CASCADE,
    CampoId INT FOREIGN KEY REFERENCES Campos(Id) ON DELETE CASCADE,
    Valor NVARCHAR(MAX) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

