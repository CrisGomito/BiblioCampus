-- =============================================================================
-- Script 02: Datos Semilla de Prueba para BiblioCampusDB
-- Ejecutar DESPUÉS de correr Update-Database en Visual Studio Community 2026
-- =============================================================================

USE [BiblioCampusDB];
GO

-- 1. Categorías
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categorias])
BEGIN
    INSERT INTO [dbo].[Categorias] ([Codigo], [Nombre], [Descripcion], [UbicacionEstante]) VALUES
    (N'ING-01', N'Ingeniería de Software', N'Arquitectura, patrones de diseño, desarrollo web ASP.NET Core y pruebas automatizadas.', N'Pasillo A - Estante 01'),
    (N'BDD-02', N'Bases de Datos', N'Diseño relacional, normalización, SQL Server, Entity Framework Core y optimización.', N'Pasillo A - Estante 04'),
    (N'IAE-03', N'Inteligencia Artificial', N'Aprendizaje automático, redes neuronales, minería de datos y visión computacional.', N'Pasillo B - Estante 02'),
    (N'RED-04', N'Redes y Ciberseguridad', N'Protocolos TCP/IP, seguridad defensiva, criptografía aplicada y administración de servidores.', N'Pasillo C - Estante 01');
END
GO

-- 2. Autores
IF NOT EXISTS (SELECT 1 FROM [dbo].[Autores])
BEGIN
    INSERT INTO [dbo].[Autores] ([Nombres], [Apellidos], [Nacionalidad], [CorreoContacto]) VALUES
    (N'Robert Cecil', N'Martin', N'Estadounidense', N'rmartin@cleancoder.edu'),
    (N'Martin', N'Fowler', N'Británica', N'mfowler@architecture.org'),
    (N'Abraham', N'Silberschatz', N'Israelí', N'silberschatz@db-systems.edu'),
    (N'Andrew Stuart', N'Tanenbaum', N'Estadounidense', N'tanenbaum@cs-networks.edu'),
    (N'Stuart Jonathan', N'Russell', N'Británica', N'srussell@ai-research.edu');
END
GO

-- 3. Estudiantes
IF NOT EXISTS (SELECT 1 FROM [dbo].[Estudiantes])
BEGIN
    INSERT INTO [dbo].[Estudiantes] ([Cedula], [NombreCompleto], [CorreoElectronico], [Carrera]) VALUES
    (N'1724589631', N'Mateo Sebastián Paredes Villacís', N'mateo.paredes@campus.edu.ec', N'Ingeniería en Software'),
    (N'0918456237', N'Camila Fernanda Cevallos Morales', N'camila.cevallos@campus.edu.ec', N'Ingeniería en Tecnologías de la Información'),
    (N'1804529173', N'Diego Alejandro Espinoza Torres', N'diego.espinoza@campus.edu.ec', N'Ciencia de Datos'),
    (N'1719843205', N'Valentina Sofía Salazar Quintero', N'valentina.salazar@campus.edu.ec', N'Ingeniería en Software'),
    (N'1003764892', N'Nicolás Andrés Herrera Zambrano', N'nicolas.herrera@campus.edu.ec', N'Ciberseguridad');
END
GO

-- 4. Libros (referenciando Autores y Categorias)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Libros])
BEGIN
    DECLARE @AutorMartin INT = (SELECT TOP 1 Id FROM [dbo].[Autores] WHERE Apellidos = N'Martin');
    DECLARE @AutorFowler INT = (SELECT TOP 1 Id FROM [dbo].[Autores] WHERE Apellidos = N'Fowler');
    DECLARE @AutorSilber INT = (SELECT TOP 1 Id FROM [dbo].[Autores] WHERE Apellidos = N'Silberschatz');
    DECLARE @AutorTanen INT = (SELECT TOP 1 Id FROM [dbo].[Autores] WHERE Apellidos = N'Tanenbaum');
    DECLARE @AutorRussell INT = (SELECT TOP 1 Id FROM [dbo].[Autores] WHERE Apellidos = N'Russell');

    DECLARE @CatIng INT = (SELECT TOP 1 Id FROM [dbo].[Categorias] WHERE Codigo = N'ING-01');
    DECLARE @CatBdd INT = (SELECT TOP 1 Id FROM [dbo].[Categorias] WHERE Codigo = N'BDD-02');
    DECLARE @CatIa INT = (SELECT TOP 1 Id FROM [dbo].[Categorias] WHERE Codigo = N'IAE-03');
    DECLARE @CatRed INT = (SELECT TOP 1 Id FROM [dbo].[Categorias] WHERE Codigo = N'RED-04');

    INSERT INTO [dbo].[Libros] ([Titulo], [Isbn], [AutorId], [CategoriaId]) VALUES
    (N'Clean Code: Manual de Estilo para Desarrollo Ágil', N'978-0-13-235088-4', @AutorMartin, @CatIng),
    (N'Patterns of Enterprise Application Architecture', N'978-0-32-112742-6', @AutorFowler, @CatIng),
    (N'Fundamentos de Bases de Datos (7ma Ed.)', N'978-0-07-802215-9', @AutorSilber, @CatBdd),
    (N'Redes de Computadoras (6ta Ed.)', N'978-0-13-752321-4', @AutorTanen, @CatRed),
    (N'Inteligencia Artificial: Un Enfoque Moderno', N'978-0-13-461099-3', @AutorRussell, @CatIa);
END
GO

-- 5. Préstamos (referenciando Estudiantes y Libros)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Prestamos])
BEGIN
    DECLARE @EstMateo INT = (SELECT TOP 1 Id FROM [dbo].[Estudiantes] WHERE Cedula = N'1724589631');
    DECLARE @EstCamila INT = (SELECT TOP 1 Id FROM [dbo].[Estudiantes] WHERE Cedula = N'0918456237');
    DECLARE @EstDiego INT = (SELECT TOP 1 Id FROM [dbo].[Estudiantes] WHERE Cedula = N'1804529173');
    DECLARE @EstValen INT = (SELECT TOP 1 Id FROM [dbo].[Estudiantes] WHERE Cedula = N'1719843205');

    DECLARE @LibClean INT = (SELECT TOP 1 Id FROM [dbo].[Libros] WHERE Isbn = N'978-0-13-235088-4');
    DECLARE @LibBdd INT = (SELECT TOP 1 Id FROM [dbo].[Libros] WHERE Isbn = N'978-0-07-802215-9');
    DECLARE @LibIa INT = (SELECT TOP 1 Id FROM [dbo].[Libros] WHERE Isbn = N'978-0-13-461099-3');
    DECLARE @LibRedes INT = (SELECT TOP 1 Id FROM [dbo].[Libros] WHERE Isbn = N'978-0-13-752321-4');

    INSERT INTO [dbo].[Prestamos] ([FechaPrestamo], [Estado], [EstudianteId], [LibroId]) VALUES
    ('2026-11-15', N'Activo', @EstMateo, @LibClean),
    ('2026-11-12', N'Devuelto', @EstCamila, @LibBdd),
    ('2026-11-04', N'Atrasado', @EstDiego, @LibIa),
    ('2026-11-18', N'Activo', @EstValen, @LibRedes);
END
GO
