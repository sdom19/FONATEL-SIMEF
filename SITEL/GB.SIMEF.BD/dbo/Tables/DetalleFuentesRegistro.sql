CREATE TABLE [dbo].[DetalleFuentesRegistro] (
    [idDetalleFuente]    INT           NOT NULL,
    [idFuente]           INT           NOT NULL,
    [NombreDestinatario] VARCHAR (300) NOT NULL,
    [CorreoElectronico]  VARCHAR (300) NOT NULL,
    [Estado]             BIT           NOT NULL,
    CONSTRAINT [PK_DetalleFuentesRegistro] PRIMARY KEY CLUSTERED ([idDetalleFuente] ASC, [idFuente] ASC),
    CONSTRAINT [FK_DetalleFuentesRegistro_FuentesRegistro] FOREIGN KEY ([idFuente]) REFERENCES [dbo].[FuentesRegistro] ([idFuente])
);

