CREATE TABLE [dbo].[ClasificacionIndicador] (
    [IdClasificacionIndicador] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]                   VARCHAR (30) NOT NULL,
    [Estado]                   BIT          CONSTRAINT [DF_ClasificacionIndicador_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ClasificacionIndicador] PRIMARY KEY CLUSTERED ([IdClasificacionIndicador] ASC)
);

