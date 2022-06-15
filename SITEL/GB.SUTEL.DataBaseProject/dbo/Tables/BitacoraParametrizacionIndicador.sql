CREATE TABLE [dbo].[BitacoraParametrizacionIndicador] (
    [IdBitacoraParametrizacionIndicador] INT           IDENTITY (1, 1) NOT NULL,
    [IdIndicador]                        NVARCHAR (50) NOT NULL,
    [IdServicio]                         INT           NOT NULL,
    [Visualiza]                          BIT           NOT NULL,
    [AnnoPorOperador]                    INT           NOT NULL,
    [MesPorOperador]                     INT           NOT NULL,
    [AnnoPorTotal]                       INT           NOT NULL,
    [MesPorTotal]                        INT           NOT NULL,
    [FechaPublicacion]                   DATE          NOT NULL,
    [HoraPublicacion]                    TIME (7)      NOT NULL,
    [UsuarioPublicador]                  VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_BitacoraParametrizacionIndicador] PRIMARY KEY CLUSTERED ([IdBitacoraParametrizacionIndicador] ASC),
    CONSTRAINT [FK_BitacoraParametrizacionIndicadores_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK_BitacoraParametrizacionIndicadores_Servicio] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[Servicio] ([IdServicio])
);

