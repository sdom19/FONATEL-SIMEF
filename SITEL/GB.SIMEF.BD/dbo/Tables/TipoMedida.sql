CREATE TABLE [dbo].[TipoMedida] (
    [idMedida] INT          NOT NULL,
    [Nombre]   VARCHAR (50) NOT NULL,
    [Estado]   BIT          NOT NULL,
    CONSTRAINT [PK_TipoMedida] PRIMARY KEY CLUSTERED ([idMedida] ASC)
);

