# 📝 Proyecto de Formularios Dinámicos

Este proyecto permite la creación y gestión dinámica de formularios y sus campos. La aplicación está dividida en dos partes: el **Backend** y el **Frontend**.

## Requisitos  
- **Backend (ASP.NET Core API)**: .NET 9.0, SQL Server  
- **Frontend (React)**: Node.js 20.13.1, npm (gestor de paquetes)  

## Ejecución  

```bash
# Clonar el repositorio
git clone https://github.com/fsteven96/Formularios.git

# Ejecutar el script SQL en el servidor de base de datos
Configurar la cadena de conexión en appsettings.json:
"ConnectionStrings": {
   "DefaultConnection": "Server=localhost;Database=FormulariosDB;User Id=sa;Password=tu_contraseña;"
 }

# Backend
cd Formularios/DynamicFormsAPI
dotnet restore
dotnet ef database update
dotnet run

# Frontend
cd Formularios/dynamic-forms-frontend
npm install
npm start



# Funcionalidades
Visualización de formularios – Lista de formularios creados con opciones para Agregar, editar, elimnar, vista previa y ver resultados.
Creación de formularios – Generación de nuevos formularios con distintos tipos de campos.
Edición de formularios – Modificación de formularios existentes.
Eliminacion de los formularios 
Ingreso de información – Captura de respuestas en los formularios creados.
Guardado de información – Almacenamiento de datos en SQL Server.
Consulta de respuestas – Visualización de respuestas ingresadas en los formularios.


## 📸 Evidencias  

### Listado de formularios  
![Listado de Formularios](evidencias/listado_formularios.png)  

### Creación de formularios  
![Creación de Formularios](evidencias/creacion_formulario.png)  

### Edición de formularios  
![Edición de Formularios](evidencias/edicion_formulario.png)  

### Creación de campos (inputs) de un formulario  
![Creación de Inputs](evidencias/creacion_inputs.png)  

### Edición de campos (inputs) de un formulario  
![Edición de Inputs](evidencias/edicion_inputs.png)  

### Ingreso de información en un formulario (extra)  
![Ingreso de Información](evidencias/ingreso_informacion.png)  

### Consulta de información de un formulario (extra)  
![Consulta de Información](evidencias/consulta_informacion.png)  
