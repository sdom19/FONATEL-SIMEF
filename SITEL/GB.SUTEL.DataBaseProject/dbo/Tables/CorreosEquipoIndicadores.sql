CREATE TABLE [dbo].[CorreosEquipoIndicadores] (
    [IdCorreosGrupoIndicadores] INT            IDENTITY (1, 1) NOT NULL,
    [CorreosGrupoIndicadores]   VARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_CorreosGrupoIndicadores] PRIMARY KEY CLUSTERED ([IdCorreosGrupoIndicadores] ASC)
);

