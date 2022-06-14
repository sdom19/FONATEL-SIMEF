CREATE TABLE [dbo].[OperadorSync] (
    [IdOperador]     VARCHAR (20)  NOT NULL,
    [NombreOperador] VARCHAR (150) NOT NULL,
    [Estado]         VARCHAR (30)  NULL,
    CONSTRAINT [PK_OPERADOR_SYNC_1] PRIMARY KEY CLUSTERED ([IdOperador] ASC)
);

