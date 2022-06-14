CREATE TABLE [dbo].[ServicioDefinicion] (
    [IdIndicador] NVARCHAR (50) NOT NULL,
    [Tipo]        INT           NULL,
    PRIMARY KEY CLUSTERED ([IdIndicador] ASC),
    FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);

