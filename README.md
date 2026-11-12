# Proyecto
Material de prestamo (Segunda fase)

### Equipo
Claudia, Francisco y Jordi

### Fecha última actualización
12/11/2026

### Diseño
Enlace para Figma: https://www.figma.com/design/UWQk5lDH94CnLBX69yl5fe/PrestamoBiblioteca?node-id=0-1&t=eEGrRcWuSFJ4yIzW-1

# Carga inicial
Para hacer una carga de los datos:
En una base de datos vacía, cargar el fichero migracions.sql y ejecutar el script 

# Casos de uso

### GENERAL
Login de acceso que diferenciará internamente entre si su nivel es usuario o administrador para darle acceso a unas páginas u otras de la web.

### USUARIO
**Ver catálogo**
El usuario podrá ver todo el catálogo de materiales para solicitar, así como su estado (en préstamo, disponible…)
**Solicitar**
El usuario podrá solicitar el préstamo de un material a partir del catálogo.
**Reservar**
En un futuro ha de poder reservar un material prestado.
**Darse de alta*+
En caso de no tener acceso ha de poder darse de alta con los datos mas básicos: mail, nombre, teléfono de contacto y contraseña.
**Editar datos**
Poder modificar alguno de sus datos de contactos. Tantos los básicos como si en un futuro se amplían.
**Ver Prestamos**
Ha de poder consultar que prestamos se le han realizado y la fecha de vencimiento de los mismos.

###  ADMINISTRADOR
(Mismas funciones del usuario)
Ha de poder realizar todas las acciones que realiza el usuario, así como modificar algunos de sus datos si fuera necesario
**Gestionar Material (CRUD)**
Es el encargado de dar de alta, baja y modificar un material y sus estados.
**Gestionar Usuarios**
Es el encargado de dar de baja y modificar, si fuera necesario un usuario, y sus estados.
**Ver préstamos**
Puede ver la totalidad de los préstamos realizados sus estados y sus fechas de vencimientos.
**Enviar recordatorios**
Ha de tener una vía de comunicación con el usuario, unilateral.
