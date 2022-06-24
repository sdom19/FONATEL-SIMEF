CREATE TABLE [dbo].[BitacoraSync] (
    [IdProcess]  UNIQUEIDENTIFIER CONSTRAINT [DF_BITACORASYNC_idProcess] DEFAULT (newid()) NOT NULL,
    [DateSync]   DATETIME         CONSTRAINT [DF_BITACORASYNC_date] DEFAULT (getdate()) NOT NULL,
    [Status]     CHAR (1)         NOT NULL,
    [RowsInsert] INT              CONSTRAINT [DF_BITACORASYNC_rowsInsert] DEFAULT ((0)) NOT NULL,
    [RowsUpdate] INT              CONSTRAINT [DF_BITACORASYNC_rowsUpdate] DEFAULT ((0)) NOT NULL,
    [Error]      VARCHAR (1024)   NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'error que fue provocado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'Error';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'fila actualizada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'RowsUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de la fila insertada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'RowsInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'estatus de la excepción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'decha de la sincronización', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'DateSync';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del proceso', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync', @level2type = N'COLUMN', @level2name = N'IdProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Sinconización de bitacora', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraSync';

