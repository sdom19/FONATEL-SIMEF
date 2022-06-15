CREATE TABLE [dbo].[ServicioOperador] (
    [IdeServicio] INT          NOT NULL,
    [IdOperador]  VARCHAR (20) NOT NULL,
    [Borrado]     TINYINT      NULL,
    [Verificar]   BIT          NULL,
    CONSTRAINT [PK_SERVICIOOPERADOR] PRIMARY KEY CLUSTERED ([IdeServicio] ASC, [IdOperador] ASC),
    CONSTRAINT [FK_SERVICIOOPERADOR_OPERADOR] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[Operador] ([IdOperador]),
    CONSTRAINT [FK_SERVICIOOPERADOR_SERVICIO] FOREIGN KEY ([IdeServicio]) REFERENCES [dbo].[Servicio] ([IdServicio])
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indicador por el operador para saber si ofrece el servicio seleccionado por SUTEL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioOperador', @level2type = N'COLUMN', @level2name = N'Verificar';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioOperador', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioOperador', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioOperador', @level2type = N'COLUMN', @level2name = N'IdeServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación del servicio con el operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioOperador';

