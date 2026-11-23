-- =============================================================================
-- Script 01: Creación de Base de Datos y Esquema Relacional BiblioCampusDB
-- Motor: Microsoft SQL Server (.\SQLEXPRESS)
-- Equivalente exacto a la migración EF Core 10.0.15 (20261120180000_Inicial)
-- =============================================================================
-- NOTA: Si ejecutas 'Update-Database' desde la Consola del Administrador de
-- Paquetes en Visual Studio Community 2026, EF Core creará esta base de datos
-- y sus tablas automáticamente. Usa este script solo si deseas crear el esquema
-- manualmente en SQL Server Management Studio (SSMS).
-- =============================================================================

IF DB_ID(N'BiblioCampusDB') IS NULL
BEGIN
    CREATE DATABASE [BiblioCampusDB];
END
GO

USE [BiblioCampusDB];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

-- 1. Tabla Categorias (5 atributos)
IF OBJECT_ID(N'[dbo].[Categorias]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Categorias] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Codigo] nvarchar(10) NOT NULL,
        [Nombre] nvarchar(80) NOT NULL,
        [Descripcion] nvarchar(250) NOT NULL,
        [UbicacionEstante] nvarchar(40) NOT NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_Categorias_Codigo] ON [dbo].[Categorias] ([Codigo]);
END
GO

-- 2. Tabla Autores (5 atributos)
IF OBJECT_ID(N'[dbo].[Autores]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Autores] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Nombres] nvarchar(80) NOT NULL,
        [Apellidos] nvarchar(80) NOT NULL,
        [Nacionalidad] nvarchar(60) NOT NULL,
        [CorreoContacto] nvarchar(120) NOT NULL,
        CONSTRAINT [PK_Autores] PRIMARY KEY ([Id])
    );
END
GO

-- 3. Tabla Estudiantes (5 atributos)
IF OBJECT_ID(N'[dbo].[Estudiantes]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Estudiantes] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Cedula] nvarchar(10) NOT NULL,
        [NombreCompleto] nvarchar(120) NOT NULL,
        [CorreoElectronico] nvarchar(120) NOT NULL,
        [Carrera] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Estudiantes] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_Estudiantes_Cedula] ON [dbo].[Estudiantes] ([Cedula]);
    CREATE UNIQUE INDEX [IX_Estudiantes_CorreoElectronico] ON [dbo].[Estudiantes] ([CorreoElectronico]);
END
GO

-- 4. Tabla Libros (5 atributos: incluye FK AutorId y FK CategoriaId)
IF OBJECT_ID(N'[dbo].[Libros]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Libros] (
        [Id] int NOT NULL IDENTITY(1,1),
        [Titulo] nvarchar(150) NOT NULL,
        [Isbn] nvarchar(20) NOT NULL,
        [AutorId] int NOT NULL,
        [CategoriaId] int NOT NULL,
        CONSTRAINT [PK_Libros] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Libros_Autores_AutorId] FOREIGN KEY ([AutorId])
            REFERENCES [dbo].[Autores] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Libros_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId])
            REFERENCES [dbo].[Categorias] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Libros_AutorId] ON [dbo].[Libros] ([AutorId]);
    CREATE INDEX [IX_Libros_CategoriaId] ON [dbo].[Libros] ([CategoriaId]);
    CREATE UNIQUE INDEX [IX_Libros_Isbn] ON [dbo].[Libros] ([Isbn]);
END
GO

-- 5. Tabla Prestamos (5 atributos: incluye FK EstudianteId y FK LibroId)
IF OBJECT_ID(N'[dbo].[Prestamos]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Prestamos] (
        [Id] int NOT NULL IDENTITY(1,1),
        [FechaPrestamo] datetime2 NOT NULL,
        [Estado] nvarchar(20) NOT NULL,
        [EstudianteId] int NOT NULL,
        [LibroId] int NOT NULL,
        CONSTRAINT [PK_Prestamos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Prestamos_Estudiantes_EstudianteId] FOREIGN KEY ([EstudianteId])
            REFERENCES [dbo].[Estudiantes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Prestamos_Libros_LibroId] FOREIGN KEY ([LibroId])
            REFERENCES [dbo].[Libros] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_Prestamos_EstudianteId] ON [dbo].[Prestamos] ([EstudianteId]);
    CREATE INDEX [IX_Prestamos_LibroId] ON [dbo].[Prestamos] ([LibroId]);
END
GO

IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20261120180000_Inicial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261120180000_Inicial', N'10.0.15');
END
GO
