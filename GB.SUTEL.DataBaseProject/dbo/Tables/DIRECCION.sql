CREATE TABLE [dbo].[Direccion] (
    [IdDireccion] INT          NOT NULL,
    [Nombre]      VARCHAR (20) NOT NULL,
    [Correo]      VARCHAR (60) NOT NULL,
    CONSTRAINT [PK_DIRECCION] PRIMARY KEY CLUSTERED ([IdDireccion] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Correo de responsables de la dirección (separados por punto y coma(;))', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Direccion', @level2type = N'COLUMN', @level2name = N'Correo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre de la dirección', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Direccion', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la dirección', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Direccion', @level2type = N'COLUMN', @level2name = N'IdDireccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de direcciones de la Sutel registradas en el sistema', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Direccion';

