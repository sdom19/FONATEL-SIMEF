CREATE TABLE [dbo].[Indicador] (
    [IdIndicador]               NVARCHAR (50)  NOT NULL,
    [IdTipoInd]                 INT            NOT NULL,
    [NombreIndicador]           VARCHAR (250)  NOT NULL,
    [DescIndicador]             VARCHAR (2000) NOT NULL,
    [Borrado]                   TINYINT        NOT NULL,
    [DefinicionIndicador]       VARCHAR (1000) NULL,
    [FuenteIndicador]           VARCHAR (1000) NULL,
    [NotaAlPie]                 VARCHAR (1000) NULL,
    [FechaUltimaModificacion]   DATE           NULL,
    [HoraUltimaModificacion]    TIME (7)       NULL,
    [UsuarioUltimaModificacion] INT            NULL,
    CONSTRAINT [PK_INDICADOR] PRIMARY KEY CLUSTERED ([IdIndicador] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_INDICADO_REFERENCE_TIPOINDI] FOREIGN KEY ([IdTipoInd]) REFERENCES [dbo].[TipoIndicador] ([IdTipoInd])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de indicadores', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Indicador';

