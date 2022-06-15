CREATE TABLE [dbo].[BitacoraArchivosMifCsv] (
    [IdBitacora]         INT           IDENTITY (1, 1) NOT NULL,
    [IdArchivo]          INT           NULL,
    [FechaModificacion]  DATETIME      NOT NULL,
    [Accion]             INT           NOT NULL,
    [Usuario]            VARCHAR (20)  NOT NULL,
    [Maquina]            VARCHAR (20)  NOT NULL,
    [TipoArchivo]        VARCHAR (3)   NOT NULL,
    [AccionExitosa]      BIT           NOT NULL,
    [DetalleTransaccion] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_BitacoraArchivosMifCsv] PRIMARY KEY CLUSTERED ([IdBitacora] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BitacoraArchivosMifCsv_Accion] FOREIGN KEY ([Accion]) REFERENCES [dbo].[Accion] ([IdAccion])
);

