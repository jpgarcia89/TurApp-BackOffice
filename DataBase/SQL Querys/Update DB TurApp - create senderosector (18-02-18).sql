USE [TurApp]
GO

/****** Object:  Table [dbo].[SenderoSector]    Script Date: 18/02/2018 10:34:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SenderoSector](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[RutaZipMapa] [nvarchar](400) NULL,
	[PesoZipMapa] [nvarchar](50) NULL,
	[NombreDepartamento] [nvarchar](100) NULL,
 CONSTRAINT [PK_SenderoSector] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


declare @id int
---------------- RIVADAVIA ----------------
INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('SIERRA DE MARQUESADO - Sector 3 Marías', '', '', 'RIVADAVIA');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id
--select * from [SenderoSector] Where id=@id

INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('SIERRA DE MARQUESADO - Sector LAS COLORADAS (FAUNISITICO)', '', '', 'RIVADAVIA');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id

INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('QUEBRADA DE ZONDA', '', '', 'RIVADAVIA');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id

---------------- ZONDA ----------------
INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('CERRO BLANCO', '', '', 'ZONDA');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id

INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('DIQUE PUNTA NEGRA', '', '', 'ZONDA');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id

---------------- ULLUM ----------------
INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('LAS TAPIAS', '', '', 'ULLUM');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id


---------------- CAUCETE ----------------
INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('DIFUNTA CORREA - Sierra de Pie de Palo', '', '', 'CAUCETE');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id


---------------- VALLE FERTIL ----------------
INSERT INTO [SenderoSector] ([Nombre], [RutaZipMapa], [PesoZipMapa], [NombreDepartamento])
VALUES ('SIERRA DE VALLE FERTIL', '', '', 'VALLE FERTIL');
set @id = SCOPE_IDENTITY()
UPDATE [SenderoSector] SET [RutaZipMapa] = '/Content/Sector/'+CAST(@id AS varchar)+'/Mapa/sectorMapa_'+CAST(@id AS varchar)+'.zip' 
Where id=@id



ALTER TABLE [Sendero]
ADD [SenderoSectorID] [int] NULL;

UPDATE Sendero
SET SenderoSectorID = 1

ALTER TABLE [Sendero]
ALTER COLUMN [SenderoSectorID] [int] NOT NULL;


ALTER TABLE [dbo].[Sendero]  WITH CHECK ADD  CONSTRAINT [FK_Sendero_SenderoSector] FOREIGN KEY([SenderoSectorID])
REFERENCES [dbo].[SenderoSector] ([ID])
GO

ALTER TABLE [dbo].[Sendero] CHECK CONSTRAINT [FK_Sendero_SenderoSector]
GO