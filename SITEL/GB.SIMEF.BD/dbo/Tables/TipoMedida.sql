CREATE TABLE [dbo].[TipoMedida] (
    [IdMedida] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]   VARCHAR (50) NOT NULL,
    [Estado]   BIT          NOT NULL,
    CONSTRAINT [PK_TipoMedida] PRIMARY KEY CLUSTERED ([IdMedida] ASC)
);



