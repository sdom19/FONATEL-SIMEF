CREATE TABLE [dbo].[GrupoIndicador] (
    [IdGrupoIndicador] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]           VARCHAR (60) NOT NULL,
    [Estado]           BIT          CONSTRAINT [DF_GrupoIndicador_Estado] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GrupoIndicador] PRIMARY KEY CLUSTERED ([IdGrupoIndicador] ASC)
);

