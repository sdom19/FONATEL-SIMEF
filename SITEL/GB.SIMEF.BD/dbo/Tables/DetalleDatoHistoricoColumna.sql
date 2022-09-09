CREATE TABLE [dbo].[DetalleDatoHistoricoColumna] (
    [IdDetalleDato] INT          IDENTITY (1, 1) NOT NULL,
    [IdHistorico]   INT          NOT NULL,
    [NumeroColumna] INT          NOT NULL,
    [Nombre]        VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DetalleDatoHistorico] PRIMARY KEY CLUSTERED ([IdDetalleDato] ASC),
    CONSTRAINT [FK_DetalleDatoHistorico_DatoHistorico] FOREIGN KEY ([IdHistorico]) REFERENCES [dbo].[DatoHistorico] ([IdHistorico])
);

