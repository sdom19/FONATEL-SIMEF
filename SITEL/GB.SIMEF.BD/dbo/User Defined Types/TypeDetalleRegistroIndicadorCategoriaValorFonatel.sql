CREATE TYPE [dbo].[TypeDetalleRegistroIndicadorCategoriaValorFonatel] AS TABLE (
    [IdSolicitud]  INT            NOT NULL,
    [IdFormulario] INT            NOT NULL,
    [IdIndicador]  INT            NOT NULL,
    [idCategoria]  INT            NOT NULL,
    [NumeroFila]   INT            NOT NULL,
    [Valor]        VARCHAR (1000) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [IdFormulario] ASC, [IdIndicador] ASC, [idCategoria] ASC, [NumeroFila] ASC));

