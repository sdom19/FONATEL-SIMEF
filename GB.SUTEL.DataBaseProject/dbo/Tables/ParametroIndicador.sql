CREATE TABLE [dbo].[ParametroIndicador] (
    [IdParametroIndicador]    INT           IDENTITY (1, 1) NOT NULL,
    [IdIndicador]             NVARCHAR (50) NOT NULL,
    [Visualiza]               BIT           NOT NULL,
    [AnnoPorOperador]         INT           NOT NULL,
    [MesPorOperador]          INT           NOT NULL,
    [AnnoPorTotal]            INT           NOT NULL,
    [MesPorTotal]             INT           NOT NULL,
    [Publicar]                BIT           NOT NULL,
    [FechaUltimaPublicacion]  DATE          NOT NULL,
    [HoraUltimaPublicacion]   TIME (7)      NOT NULL,
    [UsuarioUltimoPublicador] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_ParametroIndicador] PRIMARY KEY CLUSTERED ([IdParametroIndicador] ASC),
    CONSTRAINT [FK_ParametroIndicador_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


GO
CREATE TRIGGER TRG_ParametroToBitacora 
	ON dbo.ParametroIndicador
	AFTER INSERT, UPDATE AS
BEGIN
	INSERT INTO dbo.BitacoraParametrizacionIndicador
	SELECT IdIndicador, (SELECT TOP 1 IdServicio FROM ServicioIndicador as SI WHERE SI.IdIndicador = IdIndicador) , Visualiza, AnnoPorOperador, MesPorOperador, AnnoPorTotal, MesPorTotal, FechaUltimaPublicacion, HoraUltimaPublicacion, UsuarioUltimoPublicador
	FROM INSERTED
END