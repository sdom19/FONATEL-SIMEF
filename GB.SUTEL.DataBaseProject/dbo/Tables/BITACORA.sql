CREATE TABLE [dbo].[Bitacora] (
    [IdBitacora]       INT            IDENTITY (1, 1) NOT NULL,
    [Pantalla]         VARCHAR (60)   NOT NULL,
    [Accion]           INT            NOT NULL,
    [Usuario]          VARCHAR (30)   NOT NULL,
    [Descripcion]      NVARCHAR (100) NOT NULL,
    [FechaBitacora]    DATETIME       NOT NULL,
    [RegistroAnterior] NVARCHAR (MAX) NOT NULL,
    [RegistroNuevo]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_BITACORA] PRIMARY KEY CLUSTERED ([IdBitacora] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_BITACORA_ACCION] FOREIGN KEY ([Accion]) REFERENCES [dbo].[Accion] ([IdAccion])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'contiene la información del registro nuevo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'RegistroNuevo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene el valor de los registros anteriores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'RegistroAnterior';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha en que se realizó la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'FechaBitacora';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Describe la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica el usuario que realizo la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'Usuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Describe la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'Accion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica la pantalla a la cual se realizó la acción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'Pantalla';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID de la bitacora', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora', @level2type = N'COLUMN', @level2name = N'IdBitacora';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registra las acciones que se realizan en el sistema', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bitacora';

