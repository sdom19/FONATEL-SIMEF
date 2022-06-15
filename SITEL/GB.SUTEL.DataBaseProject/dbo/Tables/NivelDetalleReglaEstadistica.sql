CREATE TABLE [dbo].[NivelDetalleReglaEstadistica] (
    [IdNivelDetalleReglaEstadistica]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [IdConstructorCriterioDetalleAgrupacion] UNIQUEIDENTIFIER NOT NULL,
    [ValorLimiteSuperior]                    NVARCHAR (50)    NOT NULL,
    [ValorLimiteInferior]                    NVARCHAR (50)    NOT NULL,
    [IdNivelDetalle]                         INT              NOT NULL,
    [IdGenerico]                             INT              NOT NULL,
    [Borrado]                                TINYINT          NOT NULL,
    CONSTRAINT [PK_NivelDetalleReglaEstadistica] PRIMARY KEY CLUSTERED ([IdNivelDetalleReglaEstadistica] ASC),
    CONSTRAINT [FK_NivelDetalleReglaEstadistica_TipoNivelDetalle] FOREIGN KEY ([IdNivelDetalle]) REFERENCES [dbo].[TipoNivelDetalle] ([IdNivelDetalle]),
    CONSTRAINT [FK_ReglaEstadistica_ConstructorCriterioDetalleAgrupacion] FOREIGN KEY ([IdConstructorCriterioDetalleAgrupacion]) REFERENCES [dbo].[ConstructorCriterioDetalleAgrupacion] ([IdConstructorCriterio])
);

