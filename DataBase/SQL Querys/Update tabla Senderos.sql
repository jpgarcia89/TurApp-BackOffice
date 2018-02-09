/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [ID]
      ,[Desnivel]
      ,[Distancia]
      ,[AlturaMaxima]
  FROM [TurApp].[dbo].[Sendero]

UPDATE [TurApp].[dbo].[Sendero]
SET [Desnivel] = '313'
	, [Distancia]= '614.1'
	, [AlturaMaxima]= '1083'

ALTER TABLE [TurApp].[dbo].[Sendero]
ALTER COLUMN AlturaMaxima float

ALTER TABLE [TurApp].[dbo].[Sendero]
ALTER COLUMN [Distancia] float

ALTER TABLE [TurApp].[dbo].[Sendero]
ALTER COLUMN [AlturaMaxima] float



ALTER TABLE Sendero
ADD FOREIGN KEY (TipoDificultadFisicaID) REFERENCES TipoDificultadFisica(ID);

ALTER TABLE Sendero
ADD FOREIGN KEY (TipoDificultadTecnicaID) REFERENCES TipoDificultadTecnica(ID);