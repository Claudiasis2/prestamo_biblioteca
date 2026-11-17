USE [MaterialPrestamo_Test]
GO
/****** Object:  Table [dbo].[Ejemplar]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ejemplar](
	[IdEjemplar] [int] IDENTITY(1,1) NOT NULL,
	[UbicacionEstanteria] [nvarchar](50) NOT NULL,
	[FKMaterialBiblioteca] [int] NOT NULL,
	[FKEstado] [int] NOT NULL,
 CONSTRAINT [PK_Ejemplar] PRIMARY KEY CLUSTERED 
(
	[IdEjemplar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[NombreEmpleado] [nchar](20) NULL,
	[PrimerApellido] [nchar](20) NULL,
	[SegundoApellido] [nchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[IdEmpleado] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoEjemplar]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoEjemplar](
	[IdEstadoEjemplar] [int] IDENTITY(1,1) NOT NULL,
	[NombreEstado] [nchar](20) NULL,
 CONSTRAINT [PK_Estado] PRIMARY KEY CLUSTERED 
(
	[IdEstadoEjemplar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoPrestamo]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoPrestamo](
	[IdEstadoPrestamo] [int] IDENTITY(1,1) NOT NULL,
	[NombreEstado] [nchar](20) NULL,
 CONSTRAINT [PK_EstadoPrestamo] PRIMARY KEY CLUSTERED 
(
	[IdEstadoPrestamo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadoSolicitudPrestamo]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadoSolicitudPrestamo](
	[IdEstadoSolicitudPrestamo] [int] IDENTITY(1,1) NOT NULL,
	[NombreEstado] [nchar](20) NULL,
 CONSTRAINT [PK_EstadoSolicitudPrestamo] PRIMARY KEY CLUSTERED 
(
	[IdEstadoSolicitudPrestamo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialBiblioteca]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialBiblioteca](
	[IdMaterial] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [nvarchar](55) NOT NULL,
	[FKTipoMedio] [int] NOT NULL,
	[NombreDirector] [nchar](50) NULL,
	[ClasificacionEdad] [nchar](50) NULL,
	[DuracionMinutos] [int] NULL,
	[Isbn] [nchar](50) NULL,
	[Autor] [nchar](100) NULL,
	[NumeroPaginas] [int] NULL,
	[NombreArtista] [nchar](100) NULL,
	[AnioPublicacion] [smallint] NOT NULL,
 CONSTRAINT [PK_MaterialPrestamo] PRIMARY KEY CLUSTERED 
(
	[IdMaterial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prestamo]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prestamo](
	[IdPrestamo] [int] IDENTITY(1,1) NOT NULL,
	[FKSolicitudPrestamo] [int] NULL,
	[FechaLimiteDevolucion] [date] NOT NULL,
	[FechaSalida] [datetime] NOT NULL,
	[FechaDevolucionReal] [datetime] NULL,
	[FKEmpleado] [int] NOT NULL,
	[FKUsuario] [int] NOT NULL,
	[FKEjemplar] [int] NOT NULL,
	[FKEstadoPrestamo] [int] NOT NULL,
 CONSTRAINT [PK_Prestamo] PRIMARY KEY CLUSTERED 
(
	[IdPrestamo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SolicitudPrestamo]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SolicitudPrestamo](
	[IdSolicitud] [int] IDENTITY(1,1) NOT NULL,
	[FechaSolicitud] [datetime] NULL,
	[FKMaterialBiblioteca] [int] NOT NULL,
	[FKUsuario] [int] NOT NULL,
	[FKEmpleado] [int] NULL,
	[FKEstadoSolicitudPrestamo] [int] NOT NULL,
 CONSTRAINT [PK_SolicitudPrestamo] PRIMARY KEY CLUSTERED 
(
	[IdSolicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoMedio]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoMedio](
	[IdTipoMedio] [int] IDENTITY(1,1) NOT NULL,
	[NombreMedio] [nchar](20) NULL,
 CONSTRAINT [PK_TipoMedio] PRIMARY KEY CLUSTERED 
(
	[IdTipoMedio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 11/17/2026 12:12:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[NombreUsuario] [nchar](20) NOT NULL,
	[PrimerApellido] [nchar](20) NOT NULL,
	[SegundoApellido] [nchar](20) NULL,
	[Activo] [bit] NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Ejemplar]  WITH CHECK ADD  CONSTRAINT [FK_Ejemplar_Estado] FOREIGN KEY([FKEstado])
REFERENCES [dbo].[EstadoEjemplar] ([IdEstadoEjemplar])
GO
ALTER TABLE [dbo].[Ejemplar] CHECK CONSTRAINT [FK_Ejemplar_Estado]
GO
ALTER TABLE [dbo].[Ejemplar]  WITH CHECK ADD  CONSTRAINT [FK_Ejemplar_MaterialBiblioteca] FOREIGN KEY([FKMaterialBiblioteca])
REFERENCES [dbo].[MaterialBiblioteca] ([IdMaterial])
GO
ALTER TABLE [dbo].[Ejemplar] CHECK CONSTRAINT [FK_Ejemplar_MaterialBiblioteca]
GO
ALTER TABLE [dbo].[MaterialBiblioteca]  WITH CHECK ADD  CONSTRAINT [FK_MaterialBiblioteca_TipoMedio] FOREIGN KEY([FKTipoMedio])
REFERENCES [dbo].[TipoMedio] ([IdTipoMedio])
GO
ALTER TABLE [dbo].[MaterialBiblioteca] CHECK CONSTRAINT [FK_MaterialBiblioteca_TipoMedio]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_Ejemplar] FOREIGN KEY([FKEjemplar])
REFERENCES [dbo].[Ejemplar] ([IdEjemplar])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_Ejemplar]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_Empleado] FOREIGN KEY([FKEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_Empleado]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_EstadoPrestamo] FOREIGN KEY([FKEstadoPrestamo])
REFERENCES [dbo].[EstadoPrestamo] ([IdEstadoPrestamo])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_EstadoPrestamo]
GO
ALTER TABLE [dbo].[Prestamo]  WITH CHECK ADD  CONSTRAINT [FK_Prestamo_Usuario] FOREIGN KEY([FKUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Prestamo] CHECK CONSTRAINT [FK_Prestamo_Usuario]
GO
ALTER TABLE [dbo].[SolicitudPrestamo]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudPrestamo_Empleado] FOREIGN KEY([FKEmpleado])
REFERENCES [dbo].[Empleado] ([IdEmpleado])
GO
ALTER TABLE [dbo].[SolicitudPrestamo] CHECK CONSTRAINT [FK_SolicitudPrestamo_Empleado]
GO
ALTER TABLE [dbo].[SolicitudPrestamo]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudPrestamo_EstadoSolicitudPrestamo] FOREIGN KEY([FKEstadoSolicitudPrestamo])
REFERENCES [dbo].[EstadoSolicitudPrestamo] ([IdEstadoSolicitudPrestamo])
GO
ALTER TABLE [dbo].[SolicitudPrestamo] CHECK CONSTRAINT [FK_SolicitudPrestamo_EstadoSolicitudPrestamo]
GO
ALTER TABLE [dbo].[SolicitudPrestamo]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudPrestamo_MaterialBiblioteca] FOREIGN KEY([FKMaterialBiblioteca])
REFERENCES [dbo].[MaterialBiblioteca] ([IdMaterial])
GO
ALTER TABLE [dbo].[SolicitudPrestamo] CHECK CONSTRAINT [FK_SolicitudPrestamo_MaterialBiblioteca]
GO
ALTER TABLE [dbo].[SolicitudPrestamo]  WITH CHECK ADD  CONSTRAINT [FK_SolicitudPrestamo_Usuario] FOREIGN KEY([FKUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[SolicitudPrestamo] CHECK CONSTRAINT [FK_SolicitudPrestamo_Usuario]
GO

