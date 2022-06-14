CREATE TABLE [dbo].[RolAccionPantalla] (
    [IdRol]      INT NOT NULL,
    [IdPantalla] INT NOT NULL,
    [IdAccion]   INT NOT NULL,
    CONSTRAINT [PK_ROLACCIONPANTALLA] PRIMARY KEY CLUSTERED ([IdRol] ASC, [IdPantalla] ASC, [IdAccion] ASC),
    CONSTRAINT [FK_ROLACCIO_REFERENCE_ACCION] FOREIGN KEY ([IdAccion]) REFERENCES [dbo].[Accion] ([IdAccion]),
    CONSTRAINT [FK_ROLACCIO_REFERENCE_PANTALLA] FOREIGN KEY ([IdPantalla]) REFERENCES [dbo].[Pantalla] ([IdPantalla]),
    CONSTRAINT [FK_ROLACCIO_REFERENCE_ROL] FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Rol] ([IdRol])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de accion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolAccionPantalla', @level2type = N'COLUMN', @level2name = N'IdAccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la pantalla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolAccionPantalla', @level2type = N'COLUMN', @level2name = N'IdPantalla';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de rol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolAccionPantalla', @level2type = N'COLUMN', @level2name = N'IdRol';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación de la acción con el rol y la pantalla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RolAccionPantalla';

