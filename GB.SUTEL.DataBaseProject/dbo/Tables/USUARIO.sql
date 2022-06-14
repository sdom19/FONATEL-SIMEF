CREATE TABLE [dbo].[Usuario] (
    [IdUsuario]      INT           IDENTITY (1, 1) NOT NULL,
    [IdOperador]     VARCHAR (20)  CONSTRAINT [df_IDOPERADOR] DEFAULT (NULL) NULL,
    [AccesoUsuario]  VARCHAR (60)  NOT NULL,
    [NombreUsuario]  VARCHAR (60)  NOT NULL,
    [Contrasena]     VARCHAR (128) NULL,
    [CorreoUsuario]  VARCHAR (60)  NOT NULL,
    [UsuarioInterno] TINYINT       NOT NULL,
    [Activo]         TINYINT       NOT NULL,
    [Borrado]        TINYINT       NOT NULL,
    [Mercado]        BIT           NULL,
    [Calidad]        BIT           NULL,
    [FONATEL]        BIT           NULL,
    [FechaRegistro]  DATETIME      CONSTRAINT [DF_Usuario_FechaRegistro] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_USUARIO] PRIMARY KEY CLUSTERED ([IdUsuario] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_USUARIO_REFERENCE_OPERADOR] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[Operador] ([IdOperador])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si el usuario esta activo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'Activo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si el usuario es interno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'UsuarioInterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Correo del usuario externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'CorreoUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contraseña del usuario externo. El interno va por AC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'Contrasena';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'NombreUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del usuario para accesar al sistema', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'AccesoUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario', @level2type = N'COLUMN', @level2name = N'IdUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'lista de usuarios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Usuario';

