USE [Marcas]
GO
/****** Object:  Table [dbo].[Filtro]    Script Date: 22/04/2020 09:12:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Filtro](
	[FOLDER] [varchar](500) NULL,
	[FECHA] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 22/04/2020 09:12:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Folder](
	[FOLDER] [varchar](500) NULL,
	[FECHA] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Resultado_Comparacion]    Script Date: 22/04/2020 09:12:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Resultado_Comparacion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MARCA] [varchar](500) NULL,
	[FOLDER] [varchar](500) NULL,
	[RESULTADO] [int] NULL,
	[FECHA] [datetime] NULL,
	[CLASE] [int] NULL,
	[GACETA] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SPT_FILTRO]    Script Date: 22/04/2020 09:12:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Delgado
-- Create date: 12/03/2020
-- Description:	SP dispuesto para las operaciones al filtro de folders
-- =============================================
CREATE PROCEDURE [dbo].[SPT_FILTRO]
	
	@FOLDER VARCHAR(500) = null,
	@FECHA DATETIME = null,		
	@OPERACION TINYINT = 2
AS
BEGIN
	
	SET NOCOUNT ON;
-----------------------------------------------------
	--INSERTAR
-----------------------------------------------------
	
	IF @OPERACION = 0
	BEGIN
	
	INSERT INTO [dbo].[Filtro]
           ([FOLDER]
           ,[FECHA])
     VALUES
           (@FOLDER
           ,@FECHA)

	RETURN SCOPE_IDENTITY()

	END

-----------------------------------------------------
	--ELIMINAR TODOS LOS FILTROS
-----------------------------------------------------
	IF @OPERACION = 1
	BEGIN		
		DELETE [dbo].[Filtro] 
	END

-----------------------------------------------------
	--CONSULTAR TODOS
-----------------------------------------------------	
	IF @OPERACION = 2
	BEGIN		
		SELECT FOLDER,
				FECHA
		  FROM [dbo].[Filtro]  
	END

END


GO
/****** Object:  StoredProcedure [dbo].[SPT_RESULTADO_COMPARACION]    Script Date: 22/04/2020 09:12:10 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Delgado
-- Create date: 12/03/2020
-- Description:	SP dispuesto para las operaciones dedicadas a los resultados de las comparaciones 
-- =============================================
CREATE PROCEDURE [dbo].[SPT_RESULTADO_COMPARACION]
	@ID INT = null,
	@MARCA VARCHAR(500) = null,
	@FOLDER VARCHAR(500) = null,
	@RESULTADO INT = null,
	@FECHA DATETIME = null,
	@CLASE INT = NULL,
	@GACETA INT = NULL,
	@OPERACION TINYINT = 3
AS
BEGIN
	
	SET NOCOUNT ON;
-----------------------------------------------------
	--INSERTAR
-----------------------------------------------------
	
	IF @OPERACION = 0
	BEGIN
	
	INSERT INTO [dbo].[RESULTADO_COMPARACION]
           ([MARCA]
           ,[FOLDER]
           ,[RESULTADO]
           ,[FECHA]
		   ,[CLASE]
		   ,[GACETA])
     VALUES
           (@MARCA
           ,@FOLDER
           ,@RESULTADO
           ,@FECHA,@CLASE
		   ,@GACETA)

	RETURN SCOPE_IDENTITY()

	END

-----------------------------------------------------
	--ELIMINAR TODOS
-----------------------------------------------------
	IF @OPERACION = 1
	BEGIN		
		DELETE [dbo].[RESULTADO_COMPARACION]
	END

-----------------------------------------------------
	--CONSULTAR TODOS AGRUPADOS
-----------------------------------------------------	
	IF @OPERACION = 2
	BEGIN		
		SELECT
			  [MARCA],
			  [CLASE],
			  [GACETA]			  
		  FROM [dbo].[RESULTADO_COMPARACION]
		  GROUP BY MARCA,CLASE,GACETA

	END
-----------------------------------------------------
	--CONSULTAR TODOS
-----------------------------------------------------	
	IF @OPERACION = 3
	BEGIN		
		SELECT
			   [ID] 
			  ,[MARCA]
			  ,[FOLDER]
			  ,[RESULTADO]
			  ,[FECHA]
			  ,[CLASE]
			  ,[GACETA]
		  FROM [dbo].[RESULTADO_COMPARACION]
		   WHERE 
				CLASE = @CLASE
		   AND GACETA= @GACETA
		   AND MARCA LIKE @MARCA  

	END
END


GO
