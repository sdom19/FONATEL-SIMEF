CREATE TABLE [dbo].[DetalleFormularioWeb] (
    [idDetalle]       INT            NOT NULL,
    [idFormulario]    INT            NOT NULL,
    [idIndicador]     INT            NOT NULL,
    [TituloHojas]     VARCHAR (300)  NOT NULL,
    [NotasInformante] VARCHAR (3000) NULL,
    [NotasEncargado]  VARCHAR (3000) NULL,
    [Estado]          BIT            NOT NULL,
    CONSTRAINT [PK_DetalleFormularioWeb] PRIMARY KEY CLUSTERED ([idDetalle] ASC),
    CONSTRAINT [FK_DetalleFormularioWeb_FormularioWeb1] FOREIGN KEY ([idFormulario]) REFERENCES [dbo].[FormularioWeb] ([idFormulario]),
    CONSTRAINT [FK_DetalleFormularioWeb_Indicador] FOREIGN KEY ([idIndicador]) REFERENCES [dbo].[Indicador] ([idIndicador])
);

