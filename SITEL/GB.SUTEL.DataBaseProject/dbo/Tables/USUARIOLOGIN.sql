CREATE TABLE [dbo].[UsuarioLogin] (
    [IdUsuario]           INT      NOT NULL,
    [PrimerLogueo]        BIT      CONSTRAINT [DF__USUARIOLO__PRIME__7B5B524B] DEFAULT ((1)) NOT NULL,
    [Intentos]            TINYINT  CONSTRAINT [DF__USUARIOLO__INTEN__7C4F7684] DEFAULT ((3)) NOT NULL,
    [UltimoLogueoFallido] DATETIME NULL,
    [UltimoLogueoExitoso] DATETIME NULL,
    CONSTRAINT [PK__USUARIOL__98242AA93BC3EEA1] PRIMARY KEY CLUSTERED ([IdUsuario] ASC),
    CONSTRAINT [FK_USUARIOLOGIN_USUARIO] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha de logueado exitoso', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin', @level2type = N'COLUMN', @level2name = N'UltimoLogueoExitoso';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha del ultimo logueado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin', @level2type = N'COLUMN', @level2name = N'UltimoLogueoFallido';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Cantidad de intentos fallidos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin', @level2type = N'COLUMN', @level2name = N'Intentos';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si es el primer logueado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin', @level2type = N'COLUMN', @level2name = N'PrimerLogueo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'codigo usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin', @level2type = N'COLUMN', @level2name = N'IdUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registra los intentos de logueo de los usuarios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'UsuarioLogin';

