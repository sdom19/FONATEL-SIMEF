CREATE TYPE [TypeDetalleRegistroIndicadorCategoriaValorFonatel] AS TABLE(
	[IdSolicitud] [int] NOT NULL,
	[IdFormulario] [int] NOT NULL,
	[IdIndicador] [int] NOT NULL,
	[idCategoria] [int] NOT NULL,
	[NumeroFila] [int] NOT NULL,
	[Valor] [varchar](1000) NOT NULL,
 PRIMARY KEY([IdSolicitud],[IdFormulario],[IdIndicador],[idCategoria],[NumeroFila])
)