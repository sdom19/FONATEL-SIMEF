CREATE TABLE [dbo].[DetalleFuentesRegistro] (
    [IdDetalleFuente]    INT           IDENTITY (1, 1) NOT NULL,
    [IdFuente]           INT           NOT NULL,
    [NombreDestinatario] VARCHAR (300) NOT NULL,
    [CorreoElectronico]  VARCHAR (300) NOT NULL,
    [Estado]             BIT           NOT NULL,
    CONSTRAINT [PK_DetalleFuentesRegistro_1] PRIMARY KEY CLUSTERED ([IdDetalleFuente] ASC),
    CONSTRAINT [FK_DetalleFuentesRegistro_FuentesRegistro] FOREIGN KEY ([IdFuente]) REFERENCES [dbo].[FuentesRegistro] ([IdFuente])
);



