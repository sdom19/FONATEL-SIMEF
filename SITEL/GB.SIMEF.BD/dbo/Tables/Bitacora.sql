CREATE TABLE [dbo].[Bitacora] (
    [idBitacora] INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]      DATETIME2 (7) NOT NULL,
    [Usuario]    VARCHAR (300) NOT NULL,
    [Pantalla]   VARCHAR (50)  NOT NULL,
    [Accion]     INT           NOT NULL,
    [Codigo]     VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Bitacora] PRIMARY KEY CLUSTERED ([idBitacora] ASC)
);

