CREATE TABLE [dbo].[DetalleAgrupacion] (
    [IdDetalleAgrupacion]   INT             IDENTITY (1, 1) NOT NULL,
    [IdOperador]            VARCHAR (20)    NOT NULL,
    [IdAgrupacion]          INT             NOT NULL,
    [DescDetalleAgrupacion] VARCHAR (250)   NOT NULL,
    [Borrado]               TINYINT         NOT NULL,
    [DescHexa]              VARBINARY (250) NULL,
    CONSTRAINT [PK_DETALLEAGRUPACION_1] PRIMARY KEY CLUSTERED ([IdDetalleAgrupacion] ASC, [IdOperador] ASC, [IdAgrupacion] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DETALLEA_REFERENCE_AGRUPACI] FOREIGN KEY ([IdAgrupacion]) REFERENCES [dbo].[Agrupacion] ([IdAgrupacion]),
    CONSTRAINT [FK_DETALLEA_REFERENCE_OPERADOR] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[Operador] ([IdOperador])
);


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[SCIPruebas]
   ON  [dbo].[DetalleAgrupacion]
   AFTER  INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	UPDATE DetalleAgrupacion SET DescHexa= Convert(varbinary(250),DescDetalleAgrupacion)

END
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'indica si esta borrado logicamente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del detalle de agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'DescDetalleAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a la agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del detalle agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion', @level2type = N'COLUMN', @level2name = N'IdDetalleAgrupacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene los detalles de agrupación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DetalleAgrupacion';

