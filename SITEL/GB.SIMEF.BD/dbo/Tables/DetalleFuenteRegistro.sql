CREATE TABLE [dbo].[DetalleFuenteRegistro] (
    [IdDetalleFuenteRegistro] INT           IDENTITY (1, 1) NOT NULL,
    [IdFuenteRegistro]        INT           NOT NULL,
    [NombreDestinatario]      VARCHAR (500) NOT NULL,
    [CorreoElectronico]       VARCHAR (300) NOT NULL,
    [IdUsuario]               INT           NOT NULL,
    [CorreoEnviado]           BIT           NULL,
    [Estado]                  BIT           NOT NULL,
    CONSTRAINT [PK_DetalleFuenteRegistro_1] PRIMARY KEY CLUSTERED ([IdDetalleFuenteRegistro] ASC),
    CONSTRAINT [FK_DetalleFuenteRegistro_FuenteRegistro] FOREIGN KEY ([IdFuenteRegistro]) REFERENCES [dbo].[FuenteRegistro] ([IdFuenteRegistro])
);

