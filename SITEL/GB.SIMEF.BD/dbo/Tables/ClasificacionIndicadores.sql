CREATE TABLE [dbo].[ClasificacionIndicadores] (
    [idClasificacion] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]          VARCHAR (30) NOT NULL,
    [Estado]          BIT          CONSTRAINT [DF_ClasificacionIndicadores_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ClasificacionIndicadores] PRIMARY KEY CLUSTERED ([idClasificacion] ASC)
);

