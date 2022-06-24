-- =============================================
-- Author:		Walter Montes
-- Create date: 3-4-15
-- Description:	Registra una excepcion a la base de datos
-- =============================================
CREATE PROCEDURE [dbo].[RegistrarExcepcion] 
	@IDEXCEPCION uniqueidentifier,
	@NOMBREAPLICACION varchar(60),
	@NOMBRESERVIDOR [varchar](60),
	@NombreUsuario [varchar](60),
	@NIVELEXCEPCION [varchar](60),
	@CAPA [varchar](60),
	@MENSAJE [varchar](max),
	@STACKTRACE [varchar](max),
	@INNEREXCEPTION [varchar](max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[BITACORAEXCEPCION]
           ([IDEXCEPCION]
           ,[NOMBREAPLICACION]
           ,[NOMBRESERVIDOR]
           ,[NombreUsuario]
           ,[NIVELEXCEPCION]
           ,[CAPA]
           ,[MENSAJE]
           ,[STACKTRACE]
           ,[INNEREXCEPTION])
     VALUES
           (@IDEXCEPCION,
			@NOMBREAPLICACION,
			@NOMBRESERVIDOR,
			@NombreUsuario,
			@NIVELEXCEPCION ,
			@CAPA,
			@MENSAJE,
			@STACKTRACE,
			@INNEREXCEPTION)
END

