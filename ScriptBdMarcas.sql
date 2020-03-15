--Script para crear la tabla MARCAS y el SP  [SPT_MARCAS]
USE [Marcas]
GO
/****** Object:  Table [dbo].[Marcas]    Script Date: 14/03/2020 06:54:35 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Marcas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MARCA] [varchar](500) NULL,
	[FONEMA] [varchar](500) NULL,
	[NOGACETA] [int] NULL,
	[FGACETA] [date] NULL,
	[CODIGO_CLASE] [int] NULL,
	[FPRESENTA] [date] NULL,
	[NOPUB] [int] NULL,
	[NOEXP] [int] NULL,
	[SOLICITANT] [varchar](500) NULL,
	[CODIGO_PAIS] [int] NULL,
	[APODERADO] [varchar](500) NULL,
	[TIPO] [varchar](500) NULL,
	[FDIGITACIO] [date] NULL
) ON [PRIMARY]

GO


SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SPT_MARCAS]    Script Date: 14/03/2020 06:54:35 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Delgado
-- Create date: 12/03/2020
-- Description:	SP dispuesto para las operaciones dedicadas a las marcas 
-- =============================================
CREATE PROCEDURE [dbo].[SPT_MARCAS]
	@ID INT = null,
	@MARCA VARCHAR(500) = null,
	@FONEMA VARCHAR(500) = null,
	@NOGACETA int = null,
	@FGACETA DATE = null,
	@CODIGO_CLASE VARCHAR(70) = null,
	@FPRESENTA DATE= null,
	@NOPUB INT = NULL,	
	@NOEXP INT = NULL,	
	@SOLICITANT VARCHAR(500) = NULL,
	@CODIGO_PAIS VARCHAR(500) = NULL,	
	@APODERADO VARCHAR(500) = NULL,	
	@TIPO VARCHAR(500) = NULL,	
	@FDIGITACIO DATE = NULL,	
	@OPERACION TINYINT = 3
AS
BEGIN
	
	SET NOCOUNT ON;
-----------------------------------------------------
	--INSERTAR
-----------------------------------------------------
	
	IF @OPERACION = 0
	BEGIN
	
	INSERT INTO [dbo].[Marcas]
           ([MARCA]
           ,[FONEMA]
           ,[NOGACETA]
           ,[FGACETA]
           ,[CODIGO_CLASE]
           ,[FPRESENTA]
           ,[NOPUB]
           ,[NOEXP]
           ,[SOLICITANT]
           ,[CODIGO_PAIS]
           ,[APODERADO]
           ,[TIPO]
           ,[FDIGITACIO])
     VALUES
           (@MARCA
           ,@FONEMA
           ,@NOGACETA
           ,@FGACETA
           ,@CODIGO_CLASE
           ,@FPRESENTA
           ,@NOPUB 
           ,@NOEXP 
           ,@SOLICITANT
           ,@CODIGO_PAIS 
           ,@APODERADO
           ,@TIPO 
           ,@FDIGITACIO)

	RETURN SCOPE_IDENTITY()

	END

-----------------------------------------------------
	--ELIMINAR TODOS
-----------------------------------------------------
	IF @OPERACION = 1
	BEGIN		
		DELETE MARCAS 
	END

-----------------------------------------------------
	--CONSULTAR UNO POR USUARIO
-----------------------------------------------------

	IF @OPERACION = 2
	BEGIN
		SELECT [MARCA]
			  ,[FONEMA]
			  ,[NOGACETA]
			  ,[FGACETA]
			  ,[CODIGO_CLASE]
			  ,[FPRESENTA]
			  ,[NOPUB]
			  ,[NOEXP]
			  ,[SOLICITANT]
			  ,[CODIGO_PAIS]
			  ,[APODERADO]
			  ,[TIPO]
			  ,[FDIGITACIO]
		  FROM [dbo].[Marcas]
		  WHERE MARCA LIKE @MARCA  
	END
-----------------------------------------------------
	--CONSULTAR TODOS
-----------------------------------------------------	
	IF @OPERACION = 3
	BEGIN		
		SELECT [MARCA]
			  ,[FONEMA]
			  ,[NOGACETA]
			  ,[FGACETA]
			  ,[CODIGO_CLASE]
			  ,[FPRESENTA]
			  ,[NOPUB]
			  ,[NOEXP]
			  ,[SOLICITANT]
			  ,[CODIGO_PAIS]
			  ,[APODERADO]
			  ,[TIPO]
			  ,[FDIGITACIO]
		  FROM [dbo].[Marcas]  
	END

END

GO
