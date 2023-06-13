CREATE TABLE [dbo].[DetalleIndicadorVariable] (
    [IdDetalleIndicadorVariable] INT            IDENTITY (1, 1) NOT NULL,
    [IdIndicador]                INT            NOT NULL,
    [NombreVariable]             VARCHAR (300)  NOT NULL,
    [Descripcion]                VARCHAR (3000) NOT NULL,
    [Estado]                     BIT            NOT NULL,
    CONSTRAINT [PK_DetalleIndicador] PRIMARY KEY CLUSTERED ([IdDetalleIndicadorVariable] ASC, [IdIndicador] ASC),
    CONSTRAINT [FK_DetalleIndicadorValores_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

