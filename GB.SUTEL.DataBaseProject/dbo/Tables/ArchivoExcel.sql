CREATE TABLE [dbo].[ArchivoExcel] (
    [IdArchivoExcel]                INT              IDENTITY (1, 1) NOT NULL,
    [IdSolicitudIndicador]          UNIQUEIDENTIFIER NOT NULL,
    [IdOperador]                    VARCHAR (20)     NOT NULL,
    [ArchivoExcelGenerado]          BIT              CONSTRAINT [DF_ArchivoExcel_ArchivoExcelGenerado] DEFAULT ((0)) NOT NULL,
    [ArchivoExcelBytes]             VARBINARY (MAX)  NULL,
    [FechaHoraSolicitud]            DATETIME         NOT NULL,
    [FechaHoraGeneracionAutomatica] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([IdArchivoExcel] ASC) WITH (FILLFACTOR = 90)
);

