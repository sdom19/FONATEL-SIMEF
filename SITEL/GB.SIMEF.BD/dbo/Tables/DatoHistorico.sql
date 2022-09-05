CREATE TABLE [dbo].[DatoHistorico] (
    [IdHistorico]    INT          IDENTITY (1, 1) NOT NULL,
    [NombrePrograma] VARCHAR (30) NOT NULL,
    [CantidadFilas]  INT          NOT NULL,
    [Fecha]          DATE         NOT NULL,
    [FechaCarga]     DATE         NOT NULL,
    [Estado]         BIT          NOT NULL,
    CONSTRAINT [PK_DatoHistorico] PRIMARY KEY CLUSTERED ([IdHistorico] ASC)
);

