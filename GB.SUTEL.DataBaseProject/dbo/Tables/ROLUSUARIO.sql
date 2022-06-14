CREATE TABLE [dbo].[RolUsuario] (
    [IdRol]     INT NOT NULL,
    [IdUsuario] INT NOT NULL,
    CONSTRAINT [PK_ROLUSUARIO] PRIMARY KEY CLUSTERED ([IdRol] ASC, [IdUsuario] ASC),
    CONSTRAINT [FK_ROLUSUAR_REFERENCE_ROL] FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Rol] ([IdRol]),
    CONSTRAINT [FK_ROLUSUAR_REFERENCE_USUARIO] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolUsuario', @level2type = N'COLUMN', @level2name = N'IdUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del rol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolUsuario', @level2type = N'COLUMN', @level2name = N'IdRol';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación del rol de usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolUsuario';

