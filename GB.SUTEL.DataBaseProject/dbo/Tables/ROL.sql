CREATE TABLE [dbo].[Rol] (
    [IdRol]     INT           IDENTITY (1, 1) NOT NULL,
    [NombreRol] VARCHAR (250) NULL,
    CONSTRAINT [PK_ROL] PRIMARY KEY CLUSTERED ([IdRol] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del rol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rol', @level2type = N'COLUMN', @level2name = N'NombreRol';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'codigo de rol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rol', @level2type = N'COLUMN', @level2name = N'IdRol';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Roles de usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rol';

