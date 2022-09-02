CREATE TABLE [dbo].[DetalleRegistroIndicadorVariable] (
    [IdDetalleRegistroindicador] INT NOT NULL,
    [IdRegistroIndicador]        INT NOT NULL,
    [idIndicador]                INT NULL,
    [idDetalleIndicador]         INT NOT NULL,
    CONSTRAINT [PK_DetalleRegistroIndicador2] PRIMARY KEY CLUSTERED ([IdDetalleRegistroindicador] ASC),
    CONSTRAINT [FK_DetalleRegistroIndicadorVariable_DetalleIndicadorVariables1] FOREIGN KEY ([idDetalleIndicador], [idIndicador]) REFERENCES [dbo].[DetalleIndicadorVariables] ([idDetalleIndicador], [idIndicador]),
    CONSTRAINT [FK_DetalleRegistroIndicadorVariable_RegistroIndicador] FOREIGN KEY ([IdRegistroIndicador], [idIndicador]) REFERENCES [dbo].[RegistroIndicador] ([idRegistroIndicador], [idIndicador])
);

