# BiblioCampus — Sistema de Gestión de Biblioteca Universitaria (ASP.NET Core MVC · .NET 10)

**Materia:** Aplicaciones Web — Semana 4, Tarea 1  
**Tecnologías:** ASP.NET Core MVC (.NET 10.0), C#, Entity Framework Core `10.0.15`, SQL Server (`.\SQLEXPRESS`), Scaffolding (`Microsoft.VisualStudio.Web.CodeGeneration.Design 10.0.3`), Razor Views (`.cshtml`) y Tailwind CSS via CDN.  
**IDE objetivo:** Visual Studio Community 2026 (formato de solución `.slnx`).

---

## 1. Requisitos Previos

- **Visual Studio Community 2026** con la carga de trabajo **Desarrollo de ASP.NET y web** y SDK de **.NET 10.0**.
- **Microsoft SQL Server Express** (`.\SQLEXPRESS`) activo con el usuario `cris` (o ajuste `appsettings.json` a sus credenciales locales).
- **SQL Server Management Studio (SSMS)** opcional para inspeccionar tablas y ejecutar el script de datos semilla.

---

## 2. Cómo Abrir y Ejecutar en Visual Studio Community 2026

1. Extraiga la carpeta `BiblioCampus` en su equipo.
2. Haga doble clic en **`BiblioCampus.slnx`** para abrir la solución en **Visual Studio Community 2026**.
3. Espere unos segundos a que Visual Studio restaure automáticamente los paquetes NuGet (`EF Core 10.0.15` y `CodeGeneration.Design 10.0.3`). Si no lo hace automáticamente, haga clic derecho sobre la solución y elija **Restaurar paquetes NuGet**.
4. Verifique la cadena de conexión en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "cn": "server=.\\SQLEXPRESS;pwd=12345;uid=cris;database=BiblioCampusDB;TrustServerCertificate=True"
   }
   ```

---

## 3. Migración de Base de Datos en Visual Studio Community (SIN usar `dotnet` CLI)

Abra la **Consola del Administrador de Paquetes** desde el menú superior de Visual Studio:  
`Herramientas` → `Administrador de paquetes NuGet` → `Consola del Administrador de paquetes` (`Package Manager Console`).

### Opción A — Usar la migración `Inicial` ya incluida en el proyecto (Recomendado)
Ejecute únicamente:
```powershell
Update-Database
```
Esto creará automáticamente la base de datos **`BiblioCampusDB`** en `.\SQLEXPRESS` con las 5 tablas (`Categorias`, `Autores`, `Libros`, `Estudiantes`, `Prestamos`), llaves primarias, llaves foráneas (`ON DELETE NO ACTION` / `Restrict`) e índices únicos.

### Opción B — Generar la migración desde cero en su equipo
Si desea demostrar al profesor el proceso completo de creación de la migración:
1. Elimine la carpeta `Migrations/` del Explorador de Soluciones.
2. En la **Consola del Administrador de Paquetes**, ejecute:
```powershell
Add-Migration Inicial
Update-Database
```

### Cargar datos iniciales de prueba (Opcional)
Abra el archivo `Scripts/02_DatosSemilla_BiblioCampusDB.sql` en SSMS (o en el Explorador de objetos de SQL Server de Visual Studio) y ejecútelo sobre `BiblioCampusDB`.

---

## 4. Ejecutar la Aplicación

Presione **`F5`** (o clic en el botón **https** en la barra superior de Visual Studio Community 2026). El navegador abrirá automáticamente el Panel Principal de **BiblioCampus** con acceso a los 5 módulos CRUD generados con arquitectura de Scaffolding.

---

## 5. Cómo Replicar el Scaffolding en Visual Studio Community 2026

Si desea agregar un nuevo controlador o regenerar uno existente con el asistente visual:
1. Clic derecho sobre la carpeta `Controllers` → `Agregar` → `Nuevo elemento con scaffolding...`.
2. Seleccione **Controlador de MVC con vistas que usa Entity Framework**.
3. Elija la clase de modelo (por ejemplo, `Libro (BiblioCampus.Models)`).
4. En **Clase de contexto de datos**, elija `BiblioCampusDbContext (BiblioCampus.Models)`.
5. Marque **Generar vistas**, **Hacer referencia a bibliotecas de scripts** y **Usar una página de diseño**.
6. Haga clic en **Agregar**.
