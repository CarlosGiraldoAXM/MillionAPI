/********************************************************************************************
    SCRIPT DE BASE DE DATOS: MillionDB
    Autor: Carlos Giraldo
    Fecha: 06/10/2025
    Descripción:
        Crea la base de datos MillionDB con sus tablas, relaciones y datos de ejemplo
        para ejecutar el proyecto MillionAPI.
********************************************************************************************/

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MillionDB')
BEGIN
    CREATE DATABASE MillionDB;
END
GO

USE MillionDB;
GO
--------------------------------------------------------------------------------------------
-- TABLA: Owner
--------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[Owner]    Script Date: 6/10/2025 10:03:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Owner](
	[IdOwner] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Address] [nvarchar](250) NULL,
	[Photo] [varbinary](max) NULL,
	[Birthday] [date] NULL,
 CONSTRAINT [PK_Owner] PRIMARY KEY CLUSTERED 
(
	[IdOwner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--------------------------------------------------------------------------------------------
-- TABLA: Property
--------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[Property]    Script Date: 6/10/2025 10:03:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[IdProperty] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Address] [nvarchar](250) NULL,
	[Price] [decimal](18, 2) NULL,
	[CodeInternal] [nvarchar](50) NULL,
	[Year] [int] NULL,
	[IdOwner] [int] NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[IdProperty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------------------------------------------------------------------
-- TABLA: PropertyImage
--------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[PropertyImage]    Script Date: 6/10/2025 10:03:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyImage](
	[IdPropertyImage] [int] IDENTITY(1,1) NOT NULL,
	[IdProperty] [int] NULL,
	[File] [varbinary](max) NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_PropertyImage] PRIMARY KEY CLUSTERED 
(
	[IdPropertyImage] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--------------------------------------------------------------------------------------------
-- TABLA: PropertyTrace
--------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[PropertyTrace]    Script Date: 6/10/2025 10:03:35 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyTrace](
	[IdPropertyTrace] [int] IDENTITY(1,1) NOT NULL,
	[DateSale] [datetime] NULL,
	[Name] [nvarchar](200) NULL,
	[Value] [decimal](18, 2) NULL,
	[Tax] [decimal](18, 2) NULL,
	[IdProperty] [int] NULL,
 CONSTRAINT [PK_PropertyTrace] PRIMARY KEY CLUSTERED 
(
	[IdPropertyTrace] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Property]  WITH CHECK ADD  CONSTRAINT [FK_Property_Owner] FOREIGN KEY([IdOwner])
REFERENCES [dbo].[Owner] ([IdOwner])
GO
ALTER TABLE [dbo].[Property] CHECK CONSTRAINT [FK_Property_Owner]
GO
ALTER TABLE [dbo].[PropertyImage]  WITH CHECK ADD  CONSTRAINT [FK_PropertyImage_Property] FOREIGN KEY([IdProperty])
REFERENCES [dbo].[Property] ([IdProperty])
GO
ALTER TABLE [dbo].[PropertyImage] CHECK CONSTRAINT [FK_PropertyImage_Property]
GO
ALTER TABLE [dbo].[PropertyTrace]  WITH CHECK ADD  CONSTRAINT [FK_PropertyTrace_Property] FOREIGN KEY([IdProperty])
REFERENCES [dbo].[Property] ([IdProperty])
GO
ALTER TABLE [dbo].[PropertyTrace] CHECK CONSTRAINT [FK_PropertyTrace_Property]
GO
