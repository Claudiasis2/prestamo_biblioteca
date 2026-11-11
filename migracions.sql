CREATE DATABASE "MaterialPrestamo";
USE "MaterialPrestamo";

-- Volcando estructura para tabla MaterialPrestamo.Cd
CREATE TABLE IF NOT EXISTS "Cd" (
	"NombreArtista" NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"DuracionMinutos" SMALLINT,
	"FKMaterialBiblioteca" INT,
	CONSTRAINT "FK_Cd_MaterialBiblioteca" FOREIGN KEY ("FKMaterialBiblioteca") REFERENCES "dbo"."MaterialBiblioteca" ("IdMaterial") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.Cd: 50 rows
INSERT INTO "Cd" ("NombreArtista", "DuracionMinutos", "FKMaterialBiblioteca") VALUES
	(N'AC/DC', 42, 130),
	(N'AC/DC', 41, 131),
	(N'Adele', 48, 115),
	(N'Alejandra Guzmán', 40, 117),
	(N'Alice in Chains', 57, 140),
	(N'Amy Winehouse', 35, 105),
	(N'Arctic Monkeys', 41, 120),
	(N'Babasónicos', 45, 148),
	(N'Bruce Springsteen', 46, 111),
	(N'Charly García', 33, 141),
	(N'Charly García', 35, 142),
	(N'Daft Punk', 74, 102),
	(N'Daft Punk', 61, 123),
	(N'Daft Punk', 74, 124),
	(N'Eagles', 43, 109),
	(N'Fleetwood Mac', 40, 110),
	(N'Gustavo Cerati', 59, 116),
	(N'Gustavo Cerati', 52, 119),
	(N'Juanes', 44, 149),
	(N'Juanes', 47, 150),
	(N'Led Zeppelin', 42, 128),
	(N'Led Zeppelin', 82, 129),
	(N'Madonna', 51, 112),
	(N'Metallica', 54, 132),
	(N'Metallica', 47, 133),
	(N'Michael Jackson', 48, 125),
	(N'Michael Jackson', 42, 126),
	(N'Michael Jackson', 77, 127),
	(N'Michael Jackson', 42, 103),
	(N'Nirvana', 42, 106),
	(N'Pearl Jam', 53, 137),
	(N'Pearl Jam', 46, 138),
	(N'Pink Floyd', 43, 104),
	(N'Pink Floyd', 81, 108),
	(N'Queen', 43, 107),
	(N'R.E.M.', 48, 136),
	(N'Radiohead', 50, 121),
	(N'Radiohead', 42, 122),
	(N'Radiohead', 53, 113),
	(N'Red Hot Chili Peppers', 56, 114),
	(N'Shakira', 41, 118),
	(N'Soda Stereo', 41, 143),
	(N'Soda Stereo', 39, 144),
	(N'Soda Stereo', 42, 145),
	(N'Soda Stereo', 47, 146),
	(N'Soda Stereo', 53, 147),
	(N'Soundgarden', 70, 139),
	(N'The Beatles', 47, 101),
	(N'U2', 50, 134),
	(N'U2', 55, 135);

-- Volcando estructura para tabla MaterialPrestamo.Dvd
CREATE TABLE IF NOT EXISTS "Dvd" (
	"NombreDirector" NVARCHAR(50) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"ClasificacionEdad" NVARCHAR(50) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"DuracionMinutos" SMALLINT DEFAULT NULL,
	"FKMaterialBiblioteca" INT,
	CONSTRAINT "FK_Dvd_MaterialBiblioteca" FOREIGN KEY ("FKMaterialBiblioteca") REFERENCES "dbo"."MaterialBiblioteca" ("IdMaterial") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.Dvd: 50 rows
INSERT INTO "Dvd" ("NombreDirector", "ClasificacionEdad", "DuracionMinutos", "FKMaterialBiblioteca") VALUES
	(N'Andrew Stanton', N'G', 98, 92),
	(N'Andrew Stanton', N'G', 100, 94),
	(N'Bob Persichetti', N'PG', 117, 96),
	(N'Bong Joon-ho', N'R', 132, 69),
	(N'Christopher Nolan', N'R', 180, 70),
	(N'Christopher Nolan', N'PG-13', 130, 77),
	(N'Christopher Nolan', N'R', 113, 78),
	(N'Christopher Nolan', N'PG-13', 148, 51),
	(N'Christopher Nolan', N'PG-13', 152, 52),
	(N'Christopher Nolan', N'PG-13', 169, 54),
	(N'Damien Chazelle', N'R', 106, 81),
	(N'Damien Chazelle', N'PG-13', 128, 82),
	(N'David Fincher', N'R', 139, 59),
	(N'Denis Villeneuve', N'PG-13', 155, 71),
	(N'Denis Villeneuve', N'R', 164, 72),
	(N'Francis Ford Coppola', N'R', 147, 87),
	(N'Francis Ford Coppola', N'R', 175, 62),
	(N'Frank Darabont', N'R', 142, 75),
	(N'George Lucas', N'PG', 121, 73),
	(N'Hayao Miyazaki', N'PG', 125, 67),
	(N'Irvin Kershner', N'PG', 124, 74),
	(N'James Cameron', N'R', 137, 80),
	(N'James Cameron', N'PG-13', 194, 63),
	(N'James Cameron', N'PG-13', 162, 57),
	(N'Joel y Ethan Coen', N'R', 122, 100),
	(N'John Lasseter', N'G', 81, 91),
	(N'Jonathan Demme', N'R', 118, 65),
	(N'Lana y Lilly Wachowski', N'R', 136, 53),
	(N'Lee Unkrich', N'PG', 105, 68),
	(N'Martin Scorsese', N'R', 114, 86),
	(N'Martin Scorsese', N'R', 146, 76),
	(N'Martin Scorsese', N'R', 151, 99),
	(N'Mel Gibson', N'R', 178, 89),
	(N'Pete Docter', N'PG', 96, 93),
	(N'Pete Docter', N'PG', 95, 95),
	(N'Peter Jackson', N'PG-13', 201, 58),
	(N'Peter Weir', N'PG', 103, 97),
	(N'Quentin Tarantino', N'R', 154, 56),
	(N'Quentin Tarantino', N'R', 153, 84),
	(N'Quentin Tarantino', N'R', 165, 85),
	(N'Ridley Scott', N'R', 117, 79),
	(N'Ridley Scott', N'R', 155, 61),
	(N'Robert Zemeckis', N'PG', 116, 64),
	(N'Robert Zemeckis', N'PG-13', 142, 60),
	(N'Roger Allers', N'G', 88, 90),
	(N'Steven Spielberg', N'PG-13', 141, 98),
	(N'Steven Spielberg', N'PG-13', 127, 55),
	(N'Steven Spielberg', N'R', 195, 66),
	(N'Steven Spielberg', N'R', 169, 88),
	(N'Wes Anderson', N'R', 99, 83);

-- Volcando estructura para tabla MaterialPrestamo.Ejemplar
CREATE TABLE IF NOT EXISTS "Ejemplar" (
	"IdEjemplar" INT,
	"UbicacionEstanteria" NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"FKMaterialBiblioteca" INT,
	"FKEstado" INT,
	PRIMARY KEY ("IdEjemplar"),
	CONSTRAINT "FK_Ejemplar_MaterialBiblioteca" FOREIGN KEY ("FKMaterialBiblioteca") REFERENCES "dbo"."MaterialBiblioteca" ("IdMaterial") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_Ejemplar_Estado" FOREIGN KEY ("FKEstado") REFERENCES "dbo"."Estado" ("IdEstado") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.Ejemplar: 30 rows
INSERT INTO "Ejemplar" ("IdEjemplar", "UbicacionEstanteria", "FKMaterialBiblioteca", "FKEstado") VALUES
	(1, N'Estante A-01', 1, 1),
	(2, N'Estante A-01', 1, 2),
	(3, N'Estante A-01', 1, 1),
	(4, N'Estante A-02', 2, 1),
	(5, N'Estante A-02', 2, 1),
	(6, N'Estante A-03', 3, 1),
	(7, N'Estante A-03', 3, 2),
	(8, N'Estante A-04', 4, 1),
	(9, N'Estante A-04', 4, 2),
	(10, N'Estante A-05', 5, 1),
	(11, N'Estante A-05', 6, 3),
	(12, N'Estante A-06', 7, 1),
	(13, N'Estante A-06', 8, 1),
	(14, N'Estante A-07', 9, 1),
	(15, N'Estante A-07', 10, 1),
	(16, N'Estante A-08', 11, 1),
	(17, N'Estante A-08', 12, 1),
	(18, N'Estante A-09', 13, 1),
	(19, N'Estante A-09', 14, 1),
	(20, N'Estante A-10', 15, 1),
	(21, N'Estante B-01', 51, 1),
	(22, N'Estante B-01', 51, 2),
	(23, N'Estante B-02', 52, 1),
	(24, N'Estante B-02', 53, 1),
	(25, N'Estante B-03', 54, 1),
	(26, N'Estante B-04', 58, 3),
	(27, N'Estante C-01', 101, 1),
	(28, N'Estante C-01', 101, 2),
	(29, N'Estante C-02', 102, 1),
	(30, N'Estante C-03', 104, 1);

-- Volcando estructura para tabla MaterialPrestamo.Empleado
CREATE TABLE IF NOT EXISTS "Empleado" (
	"NombreEmpleado" NCHAR(20) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"PrimerApellido" NCHAR(20) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"SegundoApellido" NCHAR(20) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Email" NVARCHAR(255) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"IdEmpleado" INT,
	PRIMARY KEY ("IdEmpleado")
);

-- Volcando datos para la tabla MaterialPrestamo.Empleado: 7 rows
INSERT INTO "Empleado" ("NombreEmpleado", "PrimerApellido", "SegundoApellido", "Email", "IdEmpleado") VALUES
	(N'Paco                ', N'Martínez            ', NULL, N'pmartinez@materialprestamo.com', 1),
	(N'Juan                ', N'Pérez               ', N'Suárez              ', N'jperez@materialprestamo.com', 2),
	(N'Carmen              ', N'Casas               ', N'Blanquerna          ', N'ccasas@materialprestamo.com', 3),
	(N'Francisco           ', N'López               ', NULL, N'flopez@materialprestamo.com', 4),
	(N'Míriam              ', N'Zapatero            ', N'Ruiz                ', N'mzapatero@materialprestamo.com', 5),
	(N'Jose Manuel         ', N'Gracia              ', NULL, N'jmgracia@materialprestamo.com', 6),
	(N'                    ', N'                    ', N'                    ', N'', 7);

-- Volcando estructura para tabla MaterialPrestamo.Estado
CREATE TABLE IF NOT EXISTS "Estado" (
	"IdEstado" INT,
	"NombreEstado" NCHAR(20) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	PRIMARY KEY ("IdEstado")
);

-- Volcando datos para la tabla MaterialPrestamo.Estado: 7 rows
INSERT INTO "Estado" ("IdEstado", "NombreEstado") VALUES
	(1, N'Disponible          '),
	(2, N'Presatdo            '),
	(3, N'En mantenimiento    '),
	(4, N'Pendiente           '),
	(5, N'Aprobado            '),
	(6, N'Rechazadp           '),
	(7, N'Cancelado           ');

-- Volcando estructura para función MaterialPrestamo.fn_diagramobjects
DELIMITER //

	CREATE FUNCTION dbo.fn_diagramobjects() 
	RETURNS int
	WITH EXECUTE AS N'dbo'
	AS
	BEGIN
		declare @id_upgraddiagrams		int
		declare @id_sysdiagrams			int
		declare @id_helpdiagrams		int
		declare @id_helpdiagramdefinition	int
		declare @id_creatediagram	int
		declare @id_renamediagram	int
		declare @id_alterdiagram 	int 
		declare @id_dropdiagram		int
		declare @InstalledObjects	int

		select @InstalledObjects = 0

		select 	@id_upgraddiagrams = object_id(N'dbo.sp_upgraddiagrams'),
			@id_sysdiagrams = object_id(N'dbo.sysdiagrams'),
			@id_helpdiagrams = object_id(N'dbo.sp_helpdiagrams'),
			@id_helpdiagramdefinition = object_id(N'dbo.sp_helpdiagramdefinition'),
			@id_creatediagram = object_id(N'dbo.sp_creatediagram'),
			@id_renamediagram = object_id(N'dbo.sp_renamediagram'),
			@id_alterdiagram = object_id(N'dbo.sp_alterdiagram'), 
			@id_dropdiagram = object_id(N'dbo.sp_dropdiagram')

		if @id_upgraddiagrams is not null
			select @InstalledObjects = @InstalledObjects + 1
		if @id_sysdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 2
		if @id_helpdiagrams is not null
			select @InstalledObjects = @InstalledObjects + 4
		if @id_helpdiagramdefinition is not null
			select @InstalledObjects = @InstalledObjects + 8
		if @id_creatediagram is not null
			select @InstalledObjects = @InstalledObjects + 16
		if @id_renamediagram is not null
			select @InstalledObjects = @InstalledObjects + 32
		if @id_alterdiagram  is not null
			select @InstalledObjects = @InstalledObjects + 64
		if @id_dropdiagram is not null
			select @InstalledObjects = @InstalledObjects + 128
		
		return @InstalledObjects 
	END
	//


-- Volcando estructura para tabla MaterialPrestamo.Libro
CREATE TABLE IF NOT EXISTS "Libro" (
	"Isbn" VARCHAR(50) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Autor" NVARCHAR(50) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"NumeroPagina" SMALLINT DEFAULT NULL,
	"FKMaterialBiblioteca" INT,
	CONSTRAINT "FK_Libro_MaterialBiblioteca" FOREIGN KEY ("FKMaterialBiblioteca") REFERENCES "dbo"."MaterialBiblioteca" ("IdMaterial") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.Libro: 50 rows
INSERT INTO "Libro" ("Isbn", "Autor", "NumeroPagina", "FKMaterialBiblioteca") VALUES
	(N'978-0060850524', N'Aldous Huxley', 288, 22),
	(N'978-0061120084', N'Harper Lee', 336, 23),
	(N'978-0140048452', N'Graham Greene', 304, 49),
	(N'978-0140268867', N'Homero', 541, 25),
	(N'978-0140275360', N'Homero', 683, 26),
	(N'978-0141439518', N'Jane Austen', 432, 5),
	(N'978-0156013987', N'Antoine de Saint-Exupéry', 96, 3),
	(N'978-0261102217', N'J.R.R. Tolkien', 310, 21),
	(N'978-0307387264', N'Gabriel García Márquez', 368, 15),
	(N'978-0307474728', N'Gabriel García Márquez', 496, 1),
	(N'978-0307474773', N'Isabel Allende', 496, 13),
	(N'978-0307474780', N'Isabel Allende', 304, 29),
	(N'978-0316769487', N'J.D. Salinger', 224, 24),
	(N'978-0451524935', N'George Orwell', 328, 4),
	(N'978-1400034710', N'Gabriel García Márquez', 120, 9),
	(N'978-1451673319', N'Ray Bradbury', 256, 7),
	(N'978-8408043644', N'Carlos Ruiz Zafón', 576, 17),
	(N'978-8420464527', N'Mario Vargas Llosa', 528, 50),
	(N'978-8420471839', N'Mario Vargas Llosa', 448, 10),
	(N'978-8420471846', N'Mario Vargas Llosa', 704, 32),
	(N'978-8424922405', N'Miguel de Cervantes', 1080, 2),
	(N'978-8426412345', N'Umberto Eco', 632, 42),
	(N'978-8432204562', N'Patrick Süskind', 255, 41),
	(N'978-8433966636', N'Roberto Bolaño', 608, 14),
	(N'978-8437600819', N'Rómulo Gallegos', 440, 34),
	(N'978-8437601113', N'José Eustasio Rivera', 384, 39),
	(N'978-8437601236', N'Jorge Isaacs', 384, 35),
	(N'978-8437602345', N'Alejo Carpentier', 336, 37),
	(N'978-8437602352', N'Alejo Carpentier', 144, 38),
	(N'978-8437603452', N'Carlos Fuentes', 320, 31),
	(N'978-8437604183', N'Juan Rulfo', 130, 8),
	(N'978-8437604381', N'Ernesto Sabato', 160, 27),
	(N'978-8437604398', N'Ernesto Sabato', 512, 28),
	(N'978-8437604572', N'Julio Cortázar', 600, 6),
	(N'978-8437605678', N'Mario Benedetti', 224, 44),
	(N'978-8437605685', N'Mario Benedetti', 112, 45),
	(N'978-8437606789', N'Julio Cortázar', 144, 46),
	(N'978-8437606796', N'Julio Cortázar', 192, 47),
	(N'978-8437607892', N'José Donoso', 544, 48),
	(N'978-8445071793', N'J.R.R. Tolkien', 576, 20),
	(N'978-8447355037', N'Fiódor Dostoyevski', 608, 18),
	(N'978-8497592857', N'Jorge Luis Borges', 224, 11),
	(N'978-8497592864', N'Jorge Luis Borges', 208, 12),
	(N'978-8497593250', N'José Saramago', 448, 16),
	(N'978-8497593267', N'José Saramago', 368, 43),
	(N'978-8497595155', N'Franz Kafka', 128, 19),
	(N'978-9681603014', N'Octavio Paz', 352, 33),
	(N'978-9681608408', N'Carlos Fuentes', 68, 30),
	(N'978-9978801234', N'Juan León Mera', 240, 36),
	(N'978-9978805678', N'Jorge Icaza', 208, 40);

-- Volcando estructura para tabla MaterialPrestamo.MaterialBiblioteca
CREATE TABLE IF NOT EXISTS "MaterialBiblioteca" (
	"IdMaterial" INT,
	"Titulo" NVARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"AnioPublicacion" SMALLINT,
	PRIMARY KEY ("IdMaterial")
);

-- Volcando datos para la tabla MaterialPrestamo.MaterialBiblioteca: 150 rows
INSERT INTO "MaterialBiblioteca" ("IdMaterial", "Titulo", "AnioPublicacion") VALUES
	(1, N'Cien Años de Soledad', 1967),
	(2, N'Don Quijote de la Mancha', 1605),
	(3, N'El Principito', 1943),
	(4, N'1984', 1949),
	(5, N'Orgullo y Prejuicio', 1813),
	(6, N'Rayuela', 1963),
	(7, N'Fahrenheit 451', 1953),
	(8, N'Pedro Páramo', 1955),
	(9, N'Crónica de una Muerte Anunciada', 1981),
	(10, N'La Ciudad y los Perros', 1963),
	(11, N'Ficciones', 1944),
	(12, N'El Aleph', 1949),
	(13, N'La Casa de los Espíritus', 1982),
	(14, N'Los Detectives Salvajes', 1998),
	(15, N'El Amor en los Tiempos del Cólera', 1985),
	(16, N'Ensayo sobre la Ceguera', 1995),
	(17, N'La Sombra del Viento', 2001),
	(18, N'Crimen y Castigo', 1866),
	(19, N'La Metamorfosis', 1915),
	(20, N'El Señor de los Anillos: La Comunidad del Anillo', 1954),
	(21, N'El Hobbit', 1937),
	(22, N'Un Mundo Feliz', 1932),
	(23, N'Matar a un Ruiseñor', 1960),
	(24, N'El Guardián entre el Centeno', 1951),
	(25, N'La Odisea', -800),
	(26, N'La Ilíada', -750),
	(27, N'El Túnel', 1948),
	(28, N'Sobre Héroes y Tumbas', 1961),
	(29, N'Cuentos de Eva Luna', 1989),
	(30, N'Aura', 1962),
	(31, N'La Muerte de Artemio Cruz', 1962),
	(32, N'Conversación en La Catedral', 1969),
	(33, N'El Laberinto de la Soledad', 1950),
	(34, N'Doña Bárbara', 1929),
	(35, N'María', 1867),
	(36, N'Cumandá', 1879),
	(37, N'Los Pasos Perdidos', 1953),
	(38, N'El Reino de este Mundo', 1949),
	(39, N'La Vorágine', 1924),
	(40, N'Huasipungo', 1934),
	(41, N'El Perfume', 1985),
	(42, N'El Nombre de la Rosa', 1980),
	(43, N'Ensayo sobre la Lucidez', 2004),
	(44, N'La Tregua', 1960),
	(45, N'Pedro y el Capitán', 1979),
	(46, N'Historias de Cronopios y de Famas', 1962),
	(47, N'Bestiario', 1951),
	(48, N'El Obsceno Pájaro de la Noche', 1970),
	(49, N'El Factor Humano', 1978),
	(50, N'La Fiesta del Chivo', 2000),
	(51, N'Inception (El Origen)', 2010),
	(52, N'The Dark Knight', 2008),
	(53, N'The Matrix', 1999),
	(54, N'Interstellar', 2014),
	(55, N'Jurassic Park', 1993),
	(56, N'Pulp Fiction', 1994),
	(57, N'Avatar', 2009),
	(58, N'El Señor de los Anillos: El Retorno del Rey', 2003),
	(59, N'Fight Club', 1999),
	(60, N'Forrest Gump', 1994),
	(61, N'Gladiator', 2000),
	(62, N'The Godfather', 1972),
	(63, N'Titanic', 1997),
	(64, N'Back to the Future', 1985),
	(65, N'The Silence of the Lambs', 1991),
	(66, N'Schindler List', 1993),
	(67, N'Spirited Away (El Viaje de Chihiro)', 2001),
	(68, N'Coco', 2017),
	(69, N'Parasite', 2019),
	(70, N'Oppenheimer', 2023),
	(71, N'Dune', 2021),
	(72, N'Blade Runner 2049', 2017),
	(73, N'Star Wars: Una Nueva Esperanza', 1977),
	(74, N'Star Wars: El Imperio Contraataca', 1980),
	(75, N'The Shawshank Redemption', 1994),
	(76, N'Goodfellas', 1990),
	(77, N'The Prestige', 2006),
	(78, N'Memento', 2000),
	(79, N'Alien: El Octavo Pasajero', 1979),
	(80, N'Aliens: El Regreso', 1986),
	(81, N'Whiplash', 2014),
	(82, N'La La Land', 2016),
	(83, N'The Grand Budapest Hotel', 2014),
	(84, N'Inglourious Basterds', 2009),
	(85, N'Django Unchained', 2012),
	(86, N'Taxi Driver', 1976),
	(87, N'Apocalypse Now', 1979),
	(88, N'Saving Private Ryan', 1998),
	(89, N'Braveheart', 1995),
	(90, N'El Rey León', 1994),
	(91, N'Toy Story', 1995),
	(92, N'Wall-E', 2008),
	(93, N'Up', 2009),
	(94, N'Buscando a Nemo', 2003),
	(95, N'Intensamente (Inside Out)', 2015),
	(96, N'Spider-Man: Into the Spider-Verse', 2018),
	(97, N'The Truman Show', 1998),
	(98, N'Catch Me If You Can', 2002),
	(99, N'The Departed', 2006),
	(100, N'No Country for Old Men', 2007),
	(101, N'Abbey Road', 1969),
	(102, N'Random Access Memories', 2013),
	(103, N'Thriller', 1982),
	(104, N'The Dark Side of the Moon', 1973),
	(105, N'Back to Black', 2006),
	(106, N'Nevermind', 1991),
	(107, N'A Night at the Opera', 1975),
	(108, N'The Wall', 1979),
	(109, N'Hotel California', 1976),
	(110, N'Rumours', 1977),
	(111, N'Born in the U.S.A.', 1984),
	(112, N'Like a Prayer', 1989),
	(113, N'OK Computer', 1997),
	(114, N'Californication', 1999),
	(115, N'21', 2011),
	(116, N'Bocanada', 1999),
	(117, N'Reina de Corazones', 1991),
	(118, N'Dónde Están los Ladrones?', 1998),
	(119, N'Fuerza Natural', 2009),
	(120, N'AM', 2013),
	(121, N'Kid A', 2000),
	(122, N'In Rainbows', 2007),
	(123, N'Discovery', 2001),
	(124, N'Homework', 1997),
	(125, N'Bad', 1987),
	(126, N'Off the Wall', 1979),
	(127, N'Dangerous', 1991),
	(128, N'Led Zeppelin IV', 1971),
	(129, N'Physical Graffiti', 1975),
	(130, N'Back in Black', 1980),
	(131, N'Highway to Hell', 1979),
	(132, N'Master of Puppets', 1986),
	(133, N'Ride the Lightning', 1984),
	(134, N'The Joshua Tree', 1987),
	(135, N'Achtung Baby', 1991),
	(136, N'Automatic for the People', 1992),
	(137, N'Ten', 1991),
	(138, N'Vs.', 1993),
	(139, N'Superunknown', 1994),
	(140, N'Dirt', 1992),
	(141, N'Clics Modernos', 1983),
	(142, N'Piano Bar', 1984),
	(143, N'Canción Animal', 1990),
	(144, N'Signos', 1986),
	(145, N'Doble Vida', 1988),
	(146, N'Dynamo', 1992),
	(147, N'Sueño Stereo', 1995),
	(148, N'Infame', 2003),
	(149, N'Un Día Normal', 2002),
	(150, N'Mi Sangre', 2004);

-- Volcando estructura para tabla MaterialPrestamo.Prestamo
CREATE TABLE IF NOT EXISTS "Prestamo" (
	"IdPrestamo" INT,
	"FKSolicitudPrestamo" INT DEFAULT NULL,
	"FechaLimiteDevolucion" DATE,
	"FechaSalida" DATETIME,
	"FechaDevolucionReal" DATETIME DEFAULT NULL,
	"FKEmpleado" INT,
	"FKUsuario" INT,
	"FKEjemplar" INT,
	"FKEstado" INT,
	PRIMARY KEY ("IdPrestamo"),
	CONSTRAINT "FK_Prestamo_Usuario" FOREIGN KEY ("FKUsuario") REFERENCES "dbo"."Usuario" ("IdUsuario") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_Prestamo_Ejemplar" FOREIGN KEY ("FKEjemplar") REFERENCES "dbo"."Ejemplar" ("IdEjemplar") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_Prestamo_Estado" FOREIGN KEY ("FKEstado") REFERENCES "dbo"."Estado" ("IdEstado") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_Prestamo_Empleado" FOREIGN KEY ("FKEmpleado") REFERENCES "dbo"."Empleado" ("IdEmpleado") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_Prestamo_SolicitudPrestamo" FOREIGN KEY ("FKSolicitudPrestamo") REFERENCES "dbo"."SolicitudPrestamo" ("IdSolicitud") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.Prestamo: 5 rows
INSERT INTO "Prestamo" ("IdPrestamo", "FKSolicitudPrestamo", "FechaLimiteDevolucion", "FechaSalida", "FechaDevolucionReal", "FKEmpleado", "FKUsuario", "FKEjemplar", "FKEstado") VALUES
	(1, 1, N'2026-11-15', N'2026-11-01 10:00:00.000', NULL, 1, 1, 2, 2),
	(2, 2, N'2026-11-16', N'2026-11-02 11:00:00.000', NULL, 2, 2, 7, 2),
	(3, 3, N'2026-11-17', N'2026-11-03 11:30:00.000', NULL, 1, 3, 9, 2),
	(4, 4, N'2026-11-18', N'2026-11-04 15:00:00.000', NULL, 2, 1, 22, 2),
	(5, 5, N'2026-11-12', N'2026-11-05 17:00:00.000', N'2026-11-09 14:00:00.000', 1, 2, 28, 1);

-- Volcando estructura para tabla MaterialPrestamo.SolicitudPrestamo
CREATE TABLE IF NOT EXISTS "SolicitudPrestamo" (
	"IdSolicitud" INT,
	"FechaSolicitud" DATETIME DEFAULT NULL,
	"FKMaterialBiblioteca" INT,
	"FKUsuario" INT,
	"FKEstado" INT,
	PRIMARY KEY ("IdSolicitud"),
	CONSTRAINT "FK_SolicitudPrestamo_MaterialBiblioteca" FOREIGN KEY ("FKMaterialBiblioteca") REFERENCES "dbo"."MaterialBiblioteca" ("IdMaterial") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_SolicitudPrestamo_Usuario" FOREIGN KEY ("FKUsuario") REFERENCES "dbo"."Usuario" ("IdUsuario") ON UPDATE NO_ACTION ON DELETE NO_ACTION,
	CONSTRAINT "FK_SolicitudPrestamo_Estado" FOREIGN KEY ("FKEstado") REFERENCES "dbo"."Estado" ("IdEstado") ON UPDATE NO_ACTION ON DELETE NO_ACTION
);

-- Volcando datos para la tabla MaterialPrestamo.SolicitudPrestamo: 9 rows
INSERT INTO "SolicitudPrestamo" ("IdSolicitud", "FechaSolicitud", "FKMaterialBiblioteca", "FKUsuario", "FKEstado") VALUES
	(1, N'2026-11-01 09:30:00.000', 1, 1, 5),
	(2, N'2026-11-02 10:15:00.000', 3, 2, 5),
	(3, N'2026-11-03 11:00:00.000', 4, 3, 5),
	(4, N'2026-11-04 14:20:00.000', 51, 1, 5),
	(5, N'2026-11-05 16:45:00.000', 101, 2, 5),
	(6, N'2026-11-08 12:00:00.000', 2, 1, 4),
	(7, N'2026-11-09 09:15:00.000', 52, 2, 4),
	(8, N'2026-11-09 15:30:00.000', 102, 3, 6),
	(9, N'2026-11-10 08:00:00.000', 5, 1, 7);

-- Volcando estructura para tabla MaterialPrestamo.Usuario
CREATE TABLE IF NOT EXISTS "Usuario" (
	"IdUsuario" INT,
	"Email" NVARCHAR(255) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"NombreUsuario" NCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"PrimerApellido" NCHAR(20) COLLATE SQL_Latin1_General_CP1_CI_AS,
	"SegundoApellido" NCHAR(20) DEFAULT NULL COLLATE SQL_Latin1_General_CP1_CI_AS,
	"Activo" BIT,
	"Password" NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS,
	PRIMARY KEY ("IdUsuario")
);
 
-- Volcando datos para la tabla MaterialPrestamo.Usuario: 18 rows
INSERT INTO "Usuario" ("IdUsuario", "Email", "NombreUsuario", "PrimerApellido", "SegundoApellido", "Activo", "Password") VALUES
	(1, N'pgutierrez@gmail.com', N'Pedro               ', N'Gutierrez           ', N'                    ', 1, N'nada'),
	(2, N'mgarcia@gmail.com', N'Maria               ', N'Garcia              ', N'Lopez               ', 1, N'nada'),
	(3, N'jmartinez@gmail.com', N'Javier              ', N'Martinez            ', N'Perez               ', 1, N'nada'),
	(4, N'lfernandez@gmail.com', N'Laura               ', N'Fernandez           ', N'Sanchez             ', 1, N'nada'),
	(5, N'pnavarro@gmail.com', N'Pablo               ', N'Navarro             ', N'                    ', 1, N'nada'),
	(6, N'asmartin@gmail.com', N'Ana                 ', N'Martin              ', N'Ruiz                ', 1, N'nada'),
	(7, N'dlopez@gmail.com', N'Daniel              ', N'Lopez               ', N'Moreno              ', 1, N'nada'),
	(10, N'rromero@gmail.com', N'Roberto             ', N'Romero              ', N'Diaz                ', 1, N'nada'),
	(11, N'elgarcia@gmail.com', N'Elena               ', N'Garcia              ', N'Castro              ', 1, N'nada'),
	(12, N'mmoreno@gmail.com', N'Maria               ', N'Moreno              ', N'                    ', 1, N'nada'),
	(13, N'lmartin@yahoo.com', N'Laura               ', N'Martin              ', N'Perez               ', 1, N'nada'),
	(14, N'jfernandez@barcelona.cat', N'Javier              ', N'Fernandez           ', N'Sanchez             ', 1, N'nada'),
	(15, N'pruiz@outlook.com', N'Pablo               ', N'Ruiz                ', N'Navarro             ', 1, N'nada'),
	(16, N'elopez@gmail.com', N'Elena               ', N'Lopez               ', N'                    ', 1, N'nada'),
	(17, N'ctorres@yahoo.com', N'Carlos              ', N'Torres              ', N'                    ', 1, N'nada'),
	(19, N'iromero@barcelona.cat', N'Isabel              ', N'Romero              ', N'Diaz                ', 1, N'nada'),
	(20, N'agarcia@gmail.com', N'Ana                 ', N'Garcia              ', N'Castro              ', 1, N'nada'),
	(21, N'hola@gmail.com', N'Pedro               ', N'Martínez            ', N'                    ', 1, N'1332312');

