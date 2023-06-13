CREATE TABLE [dbo].[RelacionCategoriaAtributo] (
    [IdRelacionCategoriaId]            INT          NOT NULL,
    [IdCategoriaDesagregacion]         VARCHAR (50) NOT NULL,
    [IdCategoriaDesagregacionAtributo] INT          NOT NULL,
    [IdDetalleCategoriaTextoAtributo]  INT          NOT NULL,
    CONSTRAINT [PK_RelacionCategoriaAtributo] PRIMARY KEY CLUSTERED ([IdRelacionCategoriaId] ASC, [IdCategoriaDesagregacion] ASC, [IdCategoriaDesagregacionAtributo] ASC, [IdDetalleCategoriaTextoAtributo] ASC),
    CONSTRAINT [FK_RelacionCategoriaAtributo_DetalleCategoriaTexto] FOREIGN KEY ([IdDetalleCategoriaTextoAtributo], [IdCategoriaDesagregacionAtributo]) REFERENCES [dbo].[DetalleCategoriaTexto] ([IdDetalleCategoriaTexto], [IdCategoriaDesagregacion]),
    CONSTRAINT [FK_RelacionCategoriaAtributo_RelacionCategoriaId] FOREIGN KEY ([IdRelacionCategoriaId], [IdCategoriaDesagregacion]) REFERENCES [dbo].[RelacionCategoriaId] ([IdRelacionCategoriaId], [IdCategoriaDesagregacion]) ON DELETE CASCADE
);

