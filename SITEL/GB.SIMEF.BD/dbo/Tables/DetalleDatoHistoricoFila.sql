CREATE TABLE [dbo].[DetalleDatoHistoricoFila] (
    [IdDetalleDato]    INT          IDENTITY (1, 1) NOT NULL,
    [IdDetalleColumna] INT          NOT NULL,
    [NumeroFila]       INT          NOT NULL,
    [Atributo]         VARCHAR (50) NOT NULL,
    [NumeroColumna]    INT          NOT NULL,
    CONSTRAINT [PK_DetalleDatoHistoricoFila] PRIMARY KEY CLUSTERED ([IdDetalleDato] ASC)
);

