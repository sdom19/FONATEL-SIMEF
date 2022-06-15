CREATE TABLE [dbo].[EstadoSolicitud] (
    [IdEstado] INT           NOT NULL,
    [Nombre]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ESTADOSOLICITUD] PRIMARY KEY CLUSTERED ([IdEstado] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'nombre del estado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EstadoSolicitud', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del estado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EstadoSolicitud', @level2type = N'COLUMN', @level2name = N'IdEstado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica el estado de la solicitud para los operadores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EstadoSolicitud';

