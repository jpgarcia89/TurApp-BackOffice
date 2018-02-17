USE [TurApp]
GO

/****** Object:  Table [dbo].[TipoPuntoInteres]    Script Date: 16/02/2018 06:31:00 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TipoPuntoInteres](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TipoPuntoInteres] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/******  Inserts into Table [dbo].[TipoPuntoInteres]    Script Date: 16/02/2018 06:31:00 p.m. ******/
INSERT INTO [dbo].[TipoPuntoInteres] ([Descripcion])
     VALUES ('Inicio')
GO

INSERT INTO [dbo].[TipoPuntoInteres] ([Descripcion])
     VALUES ('Fin')
GO

INSERT INTO [dbo].[TipoPuntoInteres] ([Descripcion])
     VALUES ('Mirador')
GO

INSERT INTO [dbo].[TipoPuntoInteres] ([Descripcion])
     VALUES ('Cumbre')
GO








/****** Object:  Table [dbo].[SenderoPuntoInteres]    Script Date: 16/02/2018 06:31:19 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SenderoPuntoInteres](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SenderoID] [int] NOT NULL,
	[TipoPuntoInteresID] [int] NOT NULL,
	[Latitud] [nvarchar](50) NOT NULL,
	[Longitud] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_SenderoPuntoInteres] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SenderoPuntoInteres]  WITH CHECK ADD  CONSTRAINT [FK_SenderoPuntoInteres_Sendero] FOREIGN KEY([SenderoID])
REFERENCES [dbo].[Sendero] ([ID])
GO

ALTER TABLE [dbo].[SenderoPuntoInteres] CHECK CONSTRAINT [FK_SenderoPuntoInteres_Sendero]
GO

ALTER TABLE [dbo].[SenderoPuntoInteres]  WITH CHECK ADD  CONSTRAINT [FK_SenderoPuntoInteres_TipoPuntoInteres] FOREIGN KEY([TipoPuntoInteresID])
REFERENCES [dbo].[TipoPuntoInteres] ([ID])
GO

ALTER TABLE [dbo].[SenderoPuntoInteres] CHECK CONSTRAINT [FK_SenderoPuntoInteres_TipoPuntoInteres]
GO









/****** Object:  Table [dbo].[Sendero].[Descripcion]    Script Date: 16/02/2018 06:31:19 p.m. ******/

ALTER TABLE [TurApp].[dbo].[Sendero]
ALTER COLUMN [Descripcion] nvarchar(max)