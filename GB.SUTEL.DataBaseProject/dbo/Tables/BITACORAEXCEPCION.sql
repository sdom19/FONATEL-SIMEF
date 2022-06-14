CREATE TABLE [dbo].[BitacoraExcepcion] (
    [IdExcepcion]      UNIQUEIDENTIFIER NOT NULL,
    [NombreAplicacion] VARCHAR (60)     NOT NULL,
    [NombreServidor]   VARCHAR (60)     NOT NULL,
    [NombreUsuario]    VARCHAR (60)     NOT NULL,
    [NivelExcepcion]   VARCHAR (60)     NOT NULL,
    [Capa]             VARCHAR (60)     NOT NULL,
    [Mensaje]          VARCHAR (MAX)    NOT NULL,
    [Stacktrace]       VARCHAR (MAX)    NOT NULL,
    [InnerException]   VARCHAR (MAX)    NOT NULL,
    [Fecha]            DATETIME         CONSTRAINT [DF_BITACORAEXCEPCION_FECHA] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_BITACORAEXCEPCION] PRIMARY KEY CLUSTERED ([IdExcepcion] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha en que ocurrio la excepcion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'Fecha';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Detalla la excepción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'InnerException';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica donde se origino la exception', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'Stacktrace';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Mensaje de la excepción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'Mensaje';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Capa en la que se dio la excepción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'Capa';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nivel de la excepcion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'NivelExcepcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Usuario que provoco la excepcion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'NombreUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del servidor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'NombreServidor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la aplicación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'NombreAplicacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de la excepción', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion', @level2type = N'COLUMN', @level2name = N'IdExcepcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registra las excepciones', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraExcepcion';

