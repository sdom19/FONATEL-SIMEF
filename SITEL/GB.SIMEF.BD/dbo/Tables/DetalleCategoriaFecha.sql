CREATE TABLE [dbo].[DetalleCategoriaFecha] (
    [IdDetalleCategoriaFecha]  INT  IDENTITY (1, 1) NOT NULL,
    [IdCategoriaDesagregacion] INT  NOT NULL,
    [FechaMinima]              DATE NOT NULL,
    [FechaMaxima]              DATE NOT NULL,
    [Estado]                   BIT  NOT NULL,
    CONSTRAINT [PK_DetalleCategoriaFecha_1] PRIMARY KEY CLUSTERED ([IdDetalleCategoriaFecha] ASC),
    CONSTRAINT [FK_DetalleCategoriaFecha_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion])
);

