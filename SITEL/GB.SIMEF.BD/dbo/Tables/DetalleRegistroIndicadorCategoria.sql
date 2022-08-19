CREATE TABLE [dbo].[DetalleRegistroIndicadorCategoria] (
    [IdDetalleRegistroindicador] INT NOT NULL,
    [IdRegistroIndicador]        INT NOT NULL,
    [idIndicador]                INT NULL,
    [idDetalleIndicador]         INT NOT NULL,
    CONSTRAINT [PK_DetalleRegistroIndicadorCategoria] PRIMARY KEY CLUSTERED ([IdDetalleRegistroindicador] ASC),
    CONSTRAINT [FK_DetalleRegistroIndicadorCategoria_DetalleIndicadorCategoria] FOREIGN KEY ([idDetalleIndicador], [idIndicador]) REFERENCES [dbo].[DetalleIndicadorCategoria] ([idDetalleIndicador], [idIndicador]),
    CONSTRAINT [FK_DetalleRegistroIndicadorCategoria_RegistroIndicador] FOREIGN KEY ([IdRegistroIndicador], [idIndicador]) REFERENCES [dbo].[RegistroIndicador] ([idRegistroIndicador], [idIndicador])
);

