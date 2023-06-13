CREATE TABLE [dbo].[Bitacora] (
    [IdBitacora]    INT            IDENTITY (1, 1) NOT NULL,
    [Fecha]         DATETIME2 (7)  NOT NULL,
    [Usuario]       VARCHAR (300)  NOT NULL,
    [Pantalla]      VARCHAR (50)   NOT NULL,
    [Accion]        INT            NOT NULL,
    [Codigo]        VARCHAR (1000) NOT NULL,
    [ValorInicial]  TEXT           NULL,
    [ValorAnterior] TEXT           NULL,
    [ValorActual]   TEXT           NULL,
    CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([IdBitacora] ASC)
);

