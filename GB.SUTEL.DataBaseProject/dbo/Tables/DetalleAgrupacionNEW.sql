CREATE TABLE [dbo].[DetalleAgrupacionNEW] (
    [IdDetalleAgrupacion]   INT             IDENTITY (1, 1) NOT NULL,
    [IdOperador]            VARCHAR (20)    NOT NULL,
    [IdAgrupacion]          INT             NOT NULL,
    [DescDetalleAgrupacion] VARCHAR (250)   NOT NULL,
    [Borrado]               TINYINT         NOT NULL,
    [DescHexa]              VARBINARY (250) NULL
);

