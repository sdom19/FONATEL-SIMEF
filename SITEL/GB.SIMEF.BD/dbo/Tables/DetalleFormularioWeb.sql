CREATE TABLE [dbo].[DetalleFormularioWeb] (
    [IdDetalleFormularioWeb] INT            IDENTITY (1, 1) NOT NULL,
    [IdFormularioWeb]        INT            NOT NULL,
    [IdIndicador]            INT            NOT NULL,
    [TituloHoja]             VARCHAR (300)  NOT NULL,
    [NotaInformante]         VARCHAR (3000) NULL,
    [NotaEncargado]          VARCHAR (3000) NULL,
    [Estado]                 BIT            NOT NULL,
    CONSTRAINT [PK_DetalleFormularioWeb] PRIMARY KEY CLUSTERED ([IdDetalleFormularioWeb] ASC),
    CONSTRAINT [FK_DetalleFormularioWeb_FormularioWeb1] FOREIGN KEY ([IdFormularioWeb]) REFERENCES [dbo].[FormularioWeb] ([IdFormularioWeb]),
    CONSTRAINT [FK_DetalleFormularioWeb_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

