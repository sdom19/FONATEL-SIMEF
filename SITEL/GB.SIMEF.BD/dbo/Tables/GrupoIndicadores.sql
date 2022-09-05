CREATE TABLE [dbo].[GrupoIndicadores] (
    [idGrupo] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]  VARCHAR (60) NOT NULL,
    [Estado]  BIT          CONSTRAINT [DF_GrupoIndicadores_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GrupoIndicadores] PRIMARY KEY CLUSTERED ([idGrupo] ASC)
);

